using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Affili.ProductData.Common;
using Affilinet.Business.ImageImport.Entity;
using Affilinet.Exceptions;
using Affilinet.Business.ImageImport.Common;
using Affilinet.Business.ImageImport.DAO;
using Microsoft.SqlServer.Dts.Runtime;
using Utilities = Affilinet.Business.ImageImport.Common.Utilities;
using CommonUtilities = Affili.ProductData.Common.Utilities;
namespace Affilinet.Business.ImageImport
{
    /// <summary>
    /// Delete the Images from the file system and from the db
    /// </summary>
    public class ImageDeleteManager
    {
        #region Private Instances
        private ImageDAO _imgDao;
        private Application _app;
        private Package _deletePackage;
        private Variables _vars;
        private int _imagesWithDeleteError;
        private string _imagesWithDeleteErrorMes = string.Empty;

        /// <summary>
        /// This object instance is used to create thread synchronisation.
        /// </summary>
        protected Object ThisLock = new Object();
        protected Object HealthCheckLock = new Object();
        #endregion

        public ImageDeleteManager()
        {
            _imgDao = new ImageDAO();
        }

        public void DeleteFromFs()
        {
            try
            {
                CommonUtilities.CreateInfoLog("SsisManager.DeleteFromFs() Start DeleteFromFs()", 0);
                lock (HealthCheckLock)
                {
                    CommonUtilities.WriteHealthCheckTimeStamp(WinServiceHealthCheckFileTypesEnum.ImageDeleteService);    
                }
                

                // check the time window
                Utilities.ReloadConfigSection();
                DateTime imgDelStartTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " +
                                                           Utilities.GetAppSettingValue(
                                                               Constants.AppSettings.ImageDeleteFromFsStartTime));
                DateTime imgDelEndTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " +
                                                         Utilities.GetAppSettingValue(
                                                             Constants.AppSettings.ImageDeleteFromFsEndTime));

                if ((DateTime.Now.CompareTo(imgDelStartTime) >= 0 && DateTime.Now.CompareTo(imgDelEndTime) <= 0))
                {
                    StartDeleteFromFs();
                }
            }
            catch (Exception ex)
            {
                new AffiliGenericException("Error in SsisManager.DeleteFromFs()", ex).CreateLog();
            } 
        }

        private void StartDeleteFromFs()
        {
            CommonUtilities.CreateInfoLog("ImageDeleteManager.StartImageDeleteFromFs() ", 0);
            string prodProgId = string.Empty;
            string imageTimeStamp = string.Empty;
            // reset
            _imagesWithDeleteError = 0;
            _imagesWithDeleteErrorMes = string.Empty;
            try
            {
                // Get one program to delete the images from the filesystem
                DataTable dtNextProg = _imgDao.GetProdProgForImageDeleteFromFS();

                if (dtNextProg != null && dtNextProg.Rows != null && dtNextProg.Rows.Count > 0)
                {
                    if (dtNextProg.Rows[0][Constants.ProdImageFilesCache.ProdProgId] != DBNull.Value)
                        prodProgId = dtNextProg.Rows[0][Constants.ProdImageFilesCache.ProdProgId].ToString();

                    if (dtNextProg.Rows[0][Constants.ProdImageFilesCache.ImageImportTimeStamp] != DBNull.Value)
                        imageTimeStamp = dtNextProg.Rows[0][Constants.ProdImageFilesCache.ImageImportTimeStamp].ToString();
                }

                if (!string.IsNullOrEmpty(prodProgId) || !string.IsNullOrEmpty(imageTimeStamp))
                {
                    GetImagesFromCsv(prodProgId, imageTimeStamp);

                    // write counter of not deleted Images to ReportLog and the last errormessage and ImageId
                    if (_imagesWithDeleteError > 0)
                    {
                        // objReportLog.ImageDeleteError = "Count of files with error on delete from filesystem " + ImagesWithDeleteError + ". Last exception message: " + ImagesWithDeleteErrorMes;
                        CommonUtilities.CreateWarningLog(
                            "ImageDeleteManager.StartImageDeleteFromFs() ImagesWithDeleteError: Count of files with error on delete from filesystem " +
                            _imagesWithDeleteError + " Last exception message: " + _imagesWithDeleteErrorMes, 0);
                    }

                }
                else
                {
                    // get broken images from db and delete from FS
                    StartBrokenImageProcessing();
                }


            }
            catch (AffiliAbortRequestedException)
            {
                _imgDao.UpdateImageProgramFilesCacheDeleteStatus(prodProgId, imageTimeStamp, ProdImageFilesCacheDeleteStatusEnum.error);
                throw;
            }
            catch (AffiliGenericException aex)
            {
                aex.CreateLog();
            }
            catch (Exception ex)
            {
                AffiliGenericException aex = new AffiliGenericException("Error in ImageDeleteManager.StartImageDeleteFromFs()", ex);
                aex.CreateLog();
            }
        }

        private void GetImagesFromCsv(string prodProgId, string imageTimeStamp)
        {
            CommonUtilities.CreateInfoLog("ImageDeleteManager.GetImagesFromCsv() Parameters: ProdProgId: " + prodProgId + " ImageTimeStamp: " + imageTimeStamp, 0);
            bool setImageDeleteStatusToProcessing = false;
            // Build path to csv files
            string sourcePath = Path.Combine(CommonUtilities.GetAppSettingValue(Constants.AppSettings.ImgCsvSaveLocation), prodProgId);
            string sourceFile = imageTimeStamp + "_" + prodProgId;
            int i = 1;
            string csvUrlWithPrefix = Path.Combine(sourcePath, sourceFile + "." + i + Constants.FileNaming.ImageDeleteCsvExtention);

            while (File.Exists(csvUrlWithPrefix)) //Import all sub files of a program
            {
                if (!setImageDeleteStatusToProcessing)
                {
                    _imgDao.UpdateImageProgramFilesCacheDeleteStatus(prodProgId, imageTimeStamp, ProdImageFilesCacheDeleteStatusEnum.processingfsdelete);
                    setImageDeleteStatusToProcessing = true;
                }

                // calculate the imageFilePath--> try to delete from FileSystem --> write two new csv files error/successful, Update ProductImage table set UnableToDelete Flag to true

                // try to delete images from filesystem, if withoutError write to new delete.csv
                List<ProductImageDeleted> listDelImages = GetImagesForDeletion(csvUrlWithPrefix, prodProgId);
                
                if (listDelImages.Count > 0)
                {
                    // delete all images from filesystem
                    DeleteImageFromFileSystem(listDelImages, prodProgId, imageTimeStamp);

                    // create the filePath for the new csv which store successful deleted imageInformations
                    string successfulDelImagesFileName = Path.Combine(sourcePath, sourceFile + "." + i + Constants.FileNaming.ImageDeleteWorkingCsvExtention);

                    WriteToImageDeleteStatusFile(listDelImages, successfulDelImagesFileName);
                }

                // increase counter and check for the next file in while loop
                i++;
                csvUrlWithPrefix = Path.Combine(sourcePath, sourceFile + "." + i + Constants.FileNaming.ImageDeleteCsvExtention);
            }

            // update [ImageDeleteStatus]
            if (setImageDeleteStatusToProcessing)
            {
                _imgDao.UpdateImageProgramFilesCacheDeleteStatus(prodProgId, imageTimeStamp, ProdImageFilesCacheDeleteStatusEnum.ready4dbdelete);
            }
            else
            {
                _imgDao.UpdateImageProgramFilesCacheDeleteStatus(prodProgId, imageTimeStamp, ProdImageFilesCacheDeleteStatusEnum.ready4delete);
            }

        }

        private void CheckIfAbortRequested(String prodProgId, string imageImportTimeStamp, ref int imageCounter)
        {
            int maxImagesToDelete = 100;
            if (Utilities.GetAppSettingValue(Constants.AppSettings.CheckAbortAfterImages) != null)
                maxImagesToDelete = int.Parse(Utilities.GetAppSettingValue(Constants.AppSettings.CheckAbortAfterImagesToDelete));
            if (imageCounter >= maxImagesToDelete)
            {
                imageCounter = 0;
                if (_imgDao.GetImageDeleteStatus(prodProgId, imageImportTimeStamp) == Constants.ProdImageFilesCacheDeleteStatus.abortRequestedByAdmin)
                    throw new AffiliAbortRequestedException("ImageDeleteManager.DeleteImageFromFileSystem job aborted by admin: Product Program Id: " + prodProgId + " ImageImportTimeStamp: " + imageImportTimeStamp);
                
            }
            
        }

        /// <summary>
        /// Loops through a list of images and try to delete each image from filesystem
        /// </summary>
        /// <param name="listDelImages">List of ProductImageDeleted</param>
        /// <param name="prodProdId">The ProdProgId</param>
        /// <param name="imageImportTimeStamp">The ImageImportTimeStamp</param>
        internal void DeleteImageFromFileSystem(List<ProductImageDeleted> listDelImages, string prodProdId, string imageImportTimeStamp)
        {
            int imgCounter = 0;
            foreach (var delImage in listDelImages)
            {
                imgCounter++;
                CheckIfAbortRequested(prodProdId, imageImportTimeStamp, ref imgCounter);
                try
                {
                    Utilities.DeleteFile(delImage.ImageFilePath);
                }
                catch (Exception ex)
                {
                    // set image to error and log the message
                    delImage.ErrorOnDeletion = true;
                    delImage.ErrorMessage = ex.Message;
                    delImage.StackTrace = ex.StackTrace;
                }
            }
        }

        public void DeleteFromDb()
        {
            try
            {
                CommonUtilities.CreateInfoLog("ImageDeleteManager.DeleteFromDb() Start DeleteFromDb()", 0);
                lock (HealthCheckLock)
                {
                    CommonUtilities.WriteHealthCheckTimeStamp(WinServiceHealthCheckFileTypesEnum.ImageDeleteService);    
                }

                // check the time window
                Utilities.ReloadConfigSection();
                DateTime delDbStartTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + Utilities.GetAppSettingValue(Constants.AppSettings.ImageDeleteFromDbStartTime));
                DateTime delDbEndTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + Utilities.GetAppSettingValue(Constants.AppSettings.ImageDeleteFromDbEndTime));

                if ((DateTime.Now.CompareTo(delDbStartTime) >= 0 && DateTime.Now.CompareTo(delDbEndTime) <= 0))
                {
                    StartDeleteFromDb();
                }
            }
            catch (Exception ex)
            {
                new AffiliGenericException("Error in ImageDeleteManager.DeleteFromDb()", ex).CreateLog();
            } 

        }

        private void StartDeleteFromDb()
        {
            CommonUtilities.CreateInfoLog("ImageDeleteManager.StartDeleteFromDb() ", 0);
            string prodProgId = string.Empty;
            string imageTimeStamp = string.Empty;
            
            // Get one program to delete the images from db
            DataTable dtNextProg = _imgDao.GetProdProgForImageDeleteFromDb();

            // check the return value from db
            if (dtNextProg != null && dtNextProg.Rows != null && dtNextProg.Rows.Count > 0)
            {
                if (dtNextProg.Rows[0][Constants.ProdImageFilesCache.ProdProgId] != DBNull.Value)
                    prodProgId = dtNextProg.Rows[0][Constants.ProdImageFilesCache.ProdProgId].ToString();

                if (dtNextProg.Rows[0][Constants.ProdImageFilesCache.ImageImportTimeStamp] != DBNull.Value)
                    imageTimeStamp =
                        dtNextProg.Rows[0][Constants.ProdImageFilesCache.ImageImportTimeStamp].ToString();
            }

            if (string.IsNullOrEmpty(prodProgId) || string.IsNullOrEmpty(imageTimeStamp))
                return;

            CallSsisPackage(prodProgId, imageTimeStamp);

        }

        private void CallSsisPackage(string prodProgId, string imageTimeStamp)
        {
            CommonUtilities.CreateInfoLog("ImageDeleteManager.CallSsisPackage() Parameters: ProdProgId: " + prodProgId + " ImageTimeStamp: " + imageTimeStamp, 0);
            SsisImportReportingInfo objReportLog = new SsisImportReportingInfo();
            objReportLog.SsisImageImportStartTime = DateTime.Now.ToString();

            try
            {
                bool ssisErrors = false;
                bool setImageDeleteStatusToProcessing = false;
                int i = 1;
                // Build path to csv files
                string sourcePath = Path.Combine(CommonUtilities.GetAppSettingValue(Constants.AppSettings.ImgCsvSaveLocation), prodProgId);
                string sourceFile = imageTimeStamp + "_" + prodProgId;
                string successfulDelImgFile = Path.Combine(sourcePath, sourceFile + "." + i + Constants.FileNaming.ImageDeleteWorkingCsvExtention);


                while (File.Exists(successfulDelImgFile)) //Import all sub files of a program
                {
                    if (!setImageDeleteStatusToProcessing)
                    {
                        _imgDao.UpdateImageProgramFilesCacheDeleteStatus(prodProgId, imageTimeStamp, ProdImageFilesCacheDeleteStatusEnum.processingssisdelete);
                        setImageDeleteStatusToProcessing = true;
                    }
                    // when the packages failed set error to true...
                    if (!ExecuteSsisPackage(prodProgId, successfulDelImgFile, objReportLog))
                    {
                        ssisErrors = true;
                        new AffiliGenericException(
                            "Error in ImageDeleteManager.CallSsisPackage() Executing SSIS ProdProgId:" + prodProgId +
                            ":Errors:" + objReportLog.SsisImageImportError + " :FileName:" + successfulDelImgFile , (int)ApplicationEventsEnum.SsisImportStart).CreateLog();
                    }

                    // go to the next file
                    i++;
                    successfulDelImgFile = Path.Combine(sourcePath, sourceFile + "." + i + Constants.FileNaming.ImageDeleteWorkingCsvExtention);
                }

                // Update status in DB
                if (!ssisErrors)
                {
                    _imgDao.UpdateImageProgramFilesCacheDeleteStatus(prodProgId, imageTimeStamp, ProdImageFilesCacheDeleteStatusEnum.ready4delete);
                }
                else
                {
                    _imgDao.UpdateImageProgramFilesCacheDeleteStatus(prodProgId, imageTimeStamp, ProdImageFilesCacheDeleteStatusEnum.ssiserror, objReportLog.SsisImageImportError);
                }

                objReportLog.SsisImageImportEndTime = DateTime.Now.ToString();
            }
                catch (AffiliGenericException aex)
            {
                aex.CreateLog();
            }
            catch (Exception ex)
            {
                AffiliGenericException aex = new AffiliGenericException(
                    "Error in ImageDeleteManager.CallSsisPackage() Launching SSIS ProdProgId:" + prodProgId + " imageTimeStamp:" + imageTimeStamp , ex,
                    (int)ApplicationEventsEnum.SsisImportStart);
                aex.CreateLog();
            }
            finally
            {
                //objReportLog.ImportStatus = "Failed:" + failure.ToString() + " :Succeeded:" + success.ToString();
                if ((!String.IsNullOrEmpty(prodProgId)) && objReportLog.SsisImageImportStatus != null)
                    CommonUtilities.CreateReportLog(objReportLog.GetReportLogs(prodProgId));
            }
        }

        /// <summary>
        /// Executes the ssis package.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="csvUrl">The CSV URL.</param>
        /// <param name="objReportLog">The obj report log.</param>
        /// <returns></returns>
        internal bool ExecuteSsisPackage(string prodProgId, string csvUrl, SsisImportReportingInfo objReportLog)
        {
            CommonUtilities.CreateInfoLog("ImageDeleteManager.ExecuteSsisPackage(). Parameters: prodProgId: " + prodProgId + " csvUrl: " + csvUrl , 0);

            string execResult = string.Empty;
            string pkgLocation = string.Empty;
            string strDtsErrors = string.Empty;
            Package package = null;
            DTSExecResult results;

            try
            {
                if (_app == null)
                    _app = new Application();

                if (_deletePackage == null)
                {
                    pkgLocation = Utilities.GetAppSettingValue(Constants.AppSettings.ImageImportSsisFolder);
                    pkgLocation = pkgLocation.Replace(Constants.FileNaming.DtsxFileExtension, Constants.FileNaming.ImageDeleteCsvExtention + Constants.FileNaming.DtsxFileExtension);
                    _deletePackage = _app.LoadPackage(pkgLocation, null);
                }

                package = _deletePackage;
                

                //Set SSIS Package's Global Variables
                if (package == null)
                    throw new NullReferenceException("ImageDeleteManager.ExecuteSsisPackage() package is null!");

                _vars = package.Variables;

                //Set file names and path
                _vars[Constants.SSISParameters.CsvFilePath].Value = csvUrl;

                //This value is picked up from ProductData connection string.;
                _vars[Constants.SSISParameters.DataSource].Value = Utilities.GetConnectionString(Data.Access.Constants.DBConnections.ProductSSIS);

                lock (ThisLock)
                {
                    results = package.Execute();
                }
                string path = package.GetPackagePath();
                //Start: Get Errors generated in SSIS package
                strDtsErrors = "-";
                foreach (DtsError dtsErr in package.Errors)
                {
                    strDtsErrors += dtsErr.Description + ":";
                }
                CommonUtilities.CreateInfoLog(
                    "SSIS ImageImport Data execute result PPID:" + prodProgId + " :" + results +
                    ":Errors:" + strDtsErrors + ":Package:" + package.Name + ":csv URL:" + csvUrl,
                    ApplicationEventsEnum.DocProcessFinish);
                objReportLog.SsisImageImportError = strDtsErrors;
                objReportLog.SsisImageImportStatus = results.ToString();
                //End: Get Errors generated in SSIS package
                execResult = results.ToString().ToUpper();
                return execResult == "SUCCESS";

            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("ImageDeleteManager.ExecuteSsisPackage execute result PPID:" + prodProgId + " :" + execResult +
                 ":Errors:" + strDtsErrors + ":Package:" + pkgLocation + ":csv URL:" + csvUrl, ex);
            }
        }

       
        

        

        /// <summary>
        /// Writes a successful/error csv file for the images which could be deleted/not deleted
        /// </summary>
        /// <param name="listDelImages">List of images with deleted images</param>
        /// <param name="successfulFileName">Path incl. file name of the csv file which will contain the successful deleted images</param>
        internal void WriteToImageDeleteStatusFile(List<ProductImageDeleted> listDelImages, string successfulFileName)
        {
            StreamWriter swSuccessfulDeletedImgCsv = null;

            try
            {
                swSuccessfulDeletedImgCsv = new StreamWriter(successfulFileName, false, Encoding.Unicode);
                swSuccessfulDeletedImgCsv.WriteLine(ProductImageDeleted.GetImageSuccessfulHeader());

                foreach (ProductImageDeleted delImage in listDelImages)
                {
                    if (delImage.ErrorOnDeletion)
                    {
                        // increase counter and save the last ExMessage for logging into ReportLog
                        _imagesWithDeleteError++;
                        _imagesWithDeleteErrorMes = delImage.ErrorMessage + " StackTrace: " + delImage.StackTrace;
                        //swErrorDeletedImgCsv.WriteLine(delImage.GetErrorImageLine);
                    }
                    else
                    {
                        swSuccessfulDeletedImgCsv.WriteLine(delImage.GetSuccessfulImageLine);
                    }
                }
                listDelImages.Clear();
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in ImageDeleteManager.WriteToImageDeleteStatusFile() successfulFileName:" + successfulFileName, ex, 1);
            }
            finally
            {
                if (swSuccessfulDeletedImgCsv != null)
                {
                    swSuccessfulDeletedImgCsv.Close();
                    swSuccessfulDeletedImgCsv.Dispose();
                }
            }
        }
      
        /// <summary>
        /// Read the ImagesIds and Url from delete.csv file into a list
        /// </summary>
        /// <param name="csvFile">Location of the csv file</param>
        /// <param name="prodProgId">ProductProgramId</param>
        /// <returns></returns>
        internal List<ProductImageDeleted> GetImagesForDeletion(string csvFile, string prodProgId)
        {
            List<ProductImageDeleted> list = new List<ProductImageDeleted>();
            try
            {
                using (StreamReader csvSource = new StreamReader(csvFile))
                {
                    String strLine;
                    // read the first line = HeaderLine ignore
                    csvSource.ReadLine();

                    // start reading the data
                    while ((strLine = csvSource.ReadLine()) != null)
                    {
                        if (!String.IsNullOrEmpty(strLine))
                        {
                            //split the line into array
                            String[] fields = strLine.Split(Constants.Generic.DestFieldSeperator[0]);
                            if (fields.Length > 1)
                            {
                                // remove fieldQualifier and add to list
                                string imageId = fields[0].Replace(Constants.Generic.DestFieldQualifier, "");
                                string imageUrl = fields[1].Replace(Constants.Generic.DestFieldQualifier, "");
                                list.Add(new ProductImageDeleted { ImageId = imageId, ImageUrl = imageUrl, ProdProgId = prodProgId });
                            }
                        }
                    } //end while
                }
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("ImageDeleteManager.GetImagesForDeletion() Error while working with source file:" + csvFile, ex);
            }

            return list;
        }

        /// <summary>
        /// Reset the DeleteStatus in [ProdImageFilesCache]
        /// </summary>
        /// <param name="newStatus"></param>
        /// <param name="oldStatus"></param>
        public void ResetImageFilesCacheImageDeleteStatus(ProdImageFilesCacheDeleteStatusEnum newStatus, ProdImageFilesCacheDeleteStatusEnum oldStatus)
        {
            try
            {
                _imgDao.ResetImageProgramFilesCacheDeleteStatus(oldStatus, newStatus);
            }
            catch (Exception ex)
            {
                new AffiliGenericException("Error in ImageDeleteManager.ResetImageFilesCacheStatus()", ex, (int)ApplicationEventsEnum.DocProcessInit).CreateLog();
            }
        }

        /// <summary>
        /// Start the broken image process
        /// </summary>
        internal void StartBrokenImageProcessing()
        {
            CommonUtilities.CreateInfoLog("ImageDeleteManager.StartBrokenImageProcessing() Start StartBrokenImageProcessing()", 0);

            SsisImportReportingInfo objReportLog = new SsisImportReportingInfo();
            
            try
            {
                int maxBrokenImageCounter = Int32.TryParse(CommonUtilities.GetAppSettingValue(Constants.AppSettings.MaxBrokenCounter), out maxBrokenImageCounter) ? maxBrokenImageCounter : 3;

                // get broken images from db
                DataTable dtBrokenImgages = _imgDao.GetBrokenImages(maxBrokenImageCounter);
                if (dtBrokenImgages == null || dtBrokenImgages.Rows.Count < 1)
                {
                    CommonUtilities.CreateInfoLog("ImageDeleteManager.StartBrokenImageProcessing() No data found in DB for BrokenImageProcessing.", 0);
                    return;
                }

                List<ProductImageDeleted> listDelImages = new List<ProductImageDeleted>();
                foreach (DataRow dr in dtBrokenImgages.Rows)
                {
                    listDelImages.Add(new ProductImageDeleted
                    {
                        ImageId = dr[Constants.ProductImage.DBID].ToString(),
                        ImageUrl = dr[Constants.ProductImage.ImageURL].ToString(),
                        ProdProgId = dr[Constants.ProductImage.ProductProgramID].ToString()
                    });
                }

                if (listDelImages.Count < 1)
                    return;


                // update ImageFilecache table and set -1 to processing; -1 is a not used programId; in this case we use it to store the StartBrokenImageProcessing status somewhere(dirty)
                _imgDao.UpdateImageProgramFilesCacheDeleteStatus("-1", "Broken image status", ProdImageFilesCacheDeleteStatusEnum.processingfsdelete);
                CommonUtilities.CreateInfoLog("ImageDeleteManager.StartBrokenImageProcessing(). Amount of BrokenImages(listDelImages.Count) to delete: " + listDelImages.Count, 0);

                // delete all images from filesystem and update the list
                DeleteImageFromFileSystem(listDelImages, "-1", "Broken image status");

                foreach (var productImageDeleted in listDelImages)
                {
                    if(!productImageDeleted.ErrorOnDeletion)
                    {
                        // Set BrokenCounter to 10 -> so that GetBrokenImages will not return the images again and again
                        _imgDao.UpdateProductImageBrokenCounter(productImageDeleted.ImageId, 10);
                    }
                }

                // write counter of not deleted Images to ReportLog and the last errormessage and ImageId
                if (_imagesWithDeleteError > 0)
                    objReportLog.ImageDeleteError = "Count of broken files with error on delete from filesystem " + _imagesWithDeleteError + "Last exception message: " + _imagesWithDeleteErrorMes;
                
                // set the status back in ProdImageFilesCache table when the processing is finished
                _imgDao.UpdateImageProgramFilesCacheDeleteStatus("-1", "Broken image status", ProdImageFilesCacheDeleteStatusEnum.new2delete);

            }
            catch(AffiliAbortRequestedException ex)
            {
                _imgDao.UpdateImageProgramFilesCacheDeleteStatus("-1", "Broken image status", ProdImageFilesCacheDeleteStatusEnum.error);
                throw new AffiliGenericException("Error in ImageDeleteManager.StartBrokenImageProcessing()", ex, 0);
            }
            catch (AffiliGenericException aex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in ImageDeleteManager.StartBrokenImageProcessing()", ex, 0);
            }
            finally
            {
                //objReportLog.ImportStatus = "Failed:" + failure.ToString() + " :Succeeded:" + success.ToString();
                if (objReportLog.SsisImageImportStatus != null)
                    CommonUtilities.CreateReportLog(objReportLog.GetReportLogs("0")); // set it to 0 because in DeleteBrokenImage we delete Images from more then one program
            }

        }
    }
}

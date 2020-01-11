using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Affili.ProductData.Common;
using Affilinet.Business.ImageImport.Entity;
using Affilinet.Exceptions;
using Microsoft.SqlServer.Dts.Runtime;
using Affilinet.Business.ImageImport.Common;
using CommonUtilities = Affili.ProductData.Common.Utilities;
using Affilinet.Business.ImageImport.DAO;
using Utilities = Affilinet.Business.ImageImport.Common.Utilities;

namespace Affilinet.Business.ImageImport
{
    public class SsisManager
    {
        #region Private Instances
        private Application App;
        private Package BrokenPackage;
        private Package UpdatePackage;
        private Package DeletePackage;
        private Package DeleteBrokenPackage;
        private Variables Vars;
        private readonly ImageDAO SsisDao;
        private int ImagesWithDeleteError;
        private string ImagesWithDeleteErrorMes = string.Empty;
        protected StreamReader SrCsvSource { get; set; }
        private int DailyCounter = 0;
        private int AutoReviewCounter = 0;
        private StringBuilder DailyProgIds=new StringBuilder();
        private StringBuilder AutoProgIds=new StringBuilder();
        private bool SendMail=false;
        
        /// <summary>
        /// This object instance is used to create thread synchronisation.
        /// </summary>
        protected Object ThisLock = new Object();

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SsisManager"/> class.
        /// </summary>
        public SsisManager()
        {  
            SsisDao = new ImageDAO();
        }

        /// <summary>
        /// Initiates the ImageImport.
        /// </summary>
        public void StartSsisImageImport()
        {
            try
            {
                CommonUtilities.CreateInfoLog("SsisManager.StartSsisImageImport() Start StartSsisImageImport()", 0);
                CommonUtilities.WriteHealthCheckTimeStamp(WinServiceHealthCheckFileTypesEnum.ImageImportSsisMgmt);

                // stores the actual processed prodProgId
                string processedProdProgId = String.Empty;
                ImagesWithDeleteError = 0;
                ImagesWithDeleteErrorMes = string.Empty;

                // 1. check for manual images to import; 1st priority; should run whole day
                ReviewImageImport(true, ref processedProdProgId);

                // if one program was processed leave ELSE go to the next step
                if (!string.IsNullOrEmpty(processedProdProgId)) 
                    return;
                
                Utilities.ReloadConfigSection();
                DateTime impStartTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " +
                                       Utilities.GetAppSettingValue(Constants.AppSettings.DailyImageImportStartTime));
                DateTime impEndTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " +
                                       Utilities.GetAppSettingValue(Constants.AppSettings.DailyImageImportEndTime));
                
                if ((DateTime.Now.CompareTo(impStartTime) >= 0 && DateTime.Now.CompareTo(impEndTime) <= 0))
                {
                    SendMail = true;
                    // 2.
                    DailyImageImport(ref processedProdProgId);

                    // if one program was processed leave ELSE go to the next step
                    if (!string.IsNullOrEmpty(processedProdProgId))
                        return;

                    // 3.
                    ReviewImageImport(false, ref processedProdProgId);

                    // if one program was processed leave ELSE go to the next step
                    if (!string.IsNullOrEmpty(processedProdProgId))
                        return;
                }
                else
                {
                    if(SendMail)
                    {
                        SendMail = false;
                        CommonUtilities.CreateEmailLog("Image SSIS status: Daily counter:" + DailyCounter +
                                                       " Auto Review counter:" + AutoReviewCounter + " Daily Ids:" +
                                                       DailyProgIds.ToString() + " Auto Review ids" +
                                                       AutoProgIds.ToString());
                        DailyCounter = 0;
                        AutoReviewCounter = 0;
                        DailyProgIds = new StringBuilder();
                        AutoProgIds = new StringBuilder();
                    }
                }
            }
            catch (Exception ex)
            {
                new AffiliGenericException("Error in SsisManager.StartSsisImageImport()", ex).CreateLog();
            }            
        }

        
        /// <summary>
        /// Get the next program for review imageImport and call the SSIS for the import
        /// </summary>
        internal void ReviewImageImport(bool manualReview, ref string processedProdProgId)
        {
            CommonUtilities.CreateInfoLog("SsisManager.ReviewImageImport() Start ReviewImageImport(). Parameters: manualReview: " + manualReview, 0);
            // Get first program for Image update from db and // set the out paramter, so that the calling method knows which program is processed
            processedProdProgId = SsisDao.GetProdProgIdForReviewSsisImport(manualReview);

            if (String.IsNullOrEmpty(processedProdProgId))
                return;

            AutoProgIds.Append("," + processedProdProgId);
            AutoReviewCounter++;
            // start the ssis import; 
            // set the ImageReview as value for timestamp because there is no real timestamp in the filename  e.g. ImageReview_620_15.image.broken.csv
            StartImageImportProcessing(processedProdProgId, Constants.Generic.ImageReviewCsvPrefix, false);
        }

        /// <summary>
        /// Get the next program for the daily image import and call the SSIS for the import
        /// </summary>
        internal void DailyImageImport(ref string processedProdProgId)
        {
            CommonUtilities.CreateInfoLog("SsisManager.DailyImageImport() Start DailyImageImport()." , 0);
            string imageTimeStamp = string.Empty;

            // Get first program for daily image import from db
            DataTable dtNextProg = SsisDao.GetProdProgIdForSsisImport();

            if (dtNextProg != null && dtNextProg.Rows != null && dtNextProg.Rows.Count > 0)
            {
                // set the out paramter, so that the calling method knows which program is processed
                if (dtNextProg.Rows[0][Constants.ProdImageFilesCache.ProdProgId] != DBNull.Value)
                    processedProdProgId = dtNextProg.Rows[0][Constants.ProdImageFilesCache.ProdProgId].ToString();

                if (dtNextProg.Rows[0][Constants.ProdImageFilesCache.ImageImportTimeStamp] != DBNull.Value)
                    imageTimeStamp = dtNextProg.Rows[0][Constants.ProdImageFilesCache.ImageImportTimeStamp].ToString();
            }

            if (string.IsNullOrEmpty(processedProdProgId) || string.IsNullOrEmpty(imageTimeStamp))
                return;

            DailyProgIds.Append("," + processedProdProgId);
            DailyCounter++;
            // start the ssis import
            StartImageImportProcessing(processedProdProgId, imageTimeStamp, true);
        }

        /// <summary>
        /// Prepare the settings for the SSIS call. Is called from manual, daily and review import.
        /// </summary>
        /// <param name="prodProgId">Id of the ProductProgram</param>
        /// <param name="imageTimeStamp">TimeStamp of the ProductProgram. The Timestamp is part of the filename</param>
        /// <param name="isDailyImport">It's a daily import?</param>
        internal void StartImageImportProcessing(string prodProgId, string imageTimeStamp, bool isDailyImport)
        {
            CommonUtilities.CreateInfoLog("SsisManager.StartImageImportProcessing() Parameters: ProdProgId: " + prodProgId + " ImageTimeStamp: " + imageTimeStamp + " IsDailyImport: " + isDailyImport, 0);
            SsisImportReportingInfo objReportLog = new SsisImportReportingInfo();
            objReportLog.SsisImageImportStartTime = DateTime.Now.ToString();

            try
            {
                // Build path to csv files
                string sourcePath = Path.Combine(CommonUtilities.GetAppSettingValue(Constants.AppSettings.ImgCsvSaveLocation),prodProgId);
                string sourceFile = imageTimeStamp + "_" + prodProgId;
                string[] fileExtensions = Utilities.GetFileExtensionsForSsisImageImport();
                bool ssisErrors = false;

                // update status in DB and set to ssisprocessing
                if (isDailyImport)
                    SsisDao.UpdateImageImportProgramFilesCache(prodProgId, imageTimeStamp, ProdImageFilesCacheStatusEnum.SsisImportProcessing);
                else
                    SsisDao.UpdateImageProgramStatus(ImageProgramStatusEnum.SsisImportProcessing, prodProgId);
                            
                
                foreach (string fileExtension in fileExtensions) // Import all images of a program
                {
                    int i = 1;
                    string csvUrlWithPrefix = Path.Combine(sourcePath, sourceFile + "." + i + fileExtension);
                                       
                   
                    while (File.Exists(csvUrlWithPrefix)) //Import all sub files of a program
                    {
                        // delete packages are handled by the new AffiliImageDeleteService
                        if (fileExtension == Constants.FileNaming.ImageDeleteCsvExtention)
                            break;

                        if (ExecuteSsisPackage(prodProgId, csvUrlWithPrefix, objReportLog, fileExtension))
                        {
                            // commented out by fan: delete csv files will be done by CleanUp job
                            // after SSIS runs fine delete csv file from filesystem
                            // CommonUtilities.DeleteFile(csvUrlWithPrefix);
                        }
                        else
                        {
                            ssisErrors = true;
                            new AffiliGenericException(
                                "Error in SsisManager.StartImageImportProcessing() Executing SSIS ProdProgId:" + prodProgId +
                                ":Errors:" + objReportLog.SsisImageImportError + " :FileName:" + csvUrlWithPrefix + " :IsDailyImport:" + isDailyImport,
                                (int) ApplicationEventsEnum.SsisImportStart).CreateLog();
                        }

                        i++;
                        csvUrlWithPrefix = Path.Combine(sourcePath, sourceFile + "." + i + fileExtension);
                    }
                                      
                }

                // Update status in DB
                if (!ssisErrors)
                {
                    if(isDailyImport)
                        SsisDao.UpdateImageImportProgramFilesCache(prodProgId, imageTimeStamp, ProdImageFilesCacheStatusEnum.Ready4delete, string.Empty, true);
                    else
                        SsisDao.UpdateImageProgramAfterSsisComplete(prodProgId, Constants.ImageProgramStatus.SsisImportComplete, String.Empty);
                }
                else
                {
                    if (isDailyImport)
                        SsisDao.UpdateImageImportProgramFilesCache(prodProgId, imageTimeStamp, ProdImageFilesCacheStatusEnum.SsisError, objReportLog.SsisImageImportError);
                    else
                    {
                        string error = string.IsNullOrEmpty(objReportLog.SsisImageImportError) ? "" : objReportLog.SsisImageImportError;
                        SsisDao.UpdateImageProgramAfterSsisComplete(prodProgId, Constants.ImageProgramStatus.SsisError, ("Errors:" + error + " IsDailyImport:" + isDailyImport));
                    }
                }
                objReportLog.SsisImageImportEndTime = DateTime.Now.ToString();

                // write counter of not deleted Images to ReportLog and the last errormessage and ImageId
                if (ImagesWithDeleteError > 0)
                {
                    objReportLog.ImageDeleteError = "Count of files with error on delete from filesystem " + ImagesWithDeleteError + ". Last exception message: " + ImagesWithDeleteErrorMes;
                    CommonUtilities.CreateWarningLog("SsisManager.StartImageImportProcessing() ImagesWithDeleteError: Count of files with error on delete from filesystem " + ImagesWithDeleteError + "Last exception message: " + ImagesWithDeleteErrorMes, 0);
                }
            }
            catch (AffiliGenericException aex)
            {
                aex.CreateLog();
            }
            catch (Exception ex)
            {
                AffiliGenericException aex = new AffiliGenericException(
                    "Error in SsisManager.StartImageImportProcessing() Launching SSIS ProdProgId:" + prodProgId + " imageTimeStamp:" + imageTimeStamp + " :IsDailyImport:" + isDailyImport, ex,
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
        /// <param name="fileExtension">The file extension.</param>
        /// <returns></returns>
        internal bool ExecuteSsisPackage(string prodProgId, string csvUrl, SsisImportReportingInfo objReportLog, string fileExtension)
        {
            CommonUtilities.CreateInfoLog("SsisManager.ExecuteSsisPackage(). Parameters: prodProgId: " + prodProgId + " csvUrl: " + csvUrl + " fileExtension: " + fileExtension, 0);

            string execResult = string.Empty;
            string pkgLocation = string.Empty;
            string strDtsErrors = string.Empty;
            Package package = null;
            DTSExecResult results;

            try
            {
                if (App == null)
                    App = new Application();

                if (DeletePackage == null)
                {
                    pkgLocation = Utilities.GetAppSettingValue(Constants.AppSettings.ImageImportSsisFolder);
                    pkgLocation = pkgLocation.Replace(Constants.FileNaming.DtsxFileExtension,
                                                      Constants.FileNaming.ImageDeleteCsvExtention + Constants.FileNaming.DtsxFileExtension);
                    DeletePackage = App.LoadPackage(pkgLocation, null);
                }
                if (UpdatePackage == null)
                {
                    pkgLocation = Utilities.GetAppSettingValue(Constants.AppSettings.ImageImportSsisFolder);
                    pkgLocation = pkgLocation.Replace(Constants.FileNaming.DtsxFileExtension,
                                                      Constants.FileNaming.ImageUpdateCsvExtention + Constants.FileNaming.DtsxFileExtension);
                    UpdatePackage = App.LoadPackage(pkgLocation, null);
                }
                if (BrokenPackage == null)
                {
                    pkgLocation = Utilities.GetAppSettingValue(Constants.AppSettings.ImageImportSsisFolder);
                    pkgLocation = pkgLocation.Replace(Constants.FileNaming.DtsxFileExtension,
                                                      Constants.FileNaming.ImageBrokenCsvExtention + Constants.FileNaming.DtsxFileExtension);
                    BrokenPackage = App.LoadPackage(pkgLocation, null);
                }
                //if (DeleteBrokenPackage == null)
                //{
                //    pkgLocation = Utilities.GetAppSettingValue(Constants.AppSettings.ImageImportSsisFolder);
                //    pkgLocation = pkgLocation.Replace(Constants.FileNaming.DtsxFileExtension,
                //                                      Constants.FileNaming.ImageDeleteBrokenOkCsvExtention + Constants.FileNaming.DtsxFileExtension);
                //    DeleteBrokenPackage = App.LoadPackage(pkgLocation, null);
                //}

                switch (fileExtension)
                {
                    case Constants.FileNaming.ImageDeleteCsvExtention:
                        package = DeletePackage;
                        break;
                    case Constants.FileNaming.ImageUpdateCsvExtention:
                        package = UpdatePackage;
                        break;
                    case Constants.FileNaming.ImageBrokenCsvExtention:
                        package = BrokenPackage;
                        break;
                    //case Constants.FileNaming.ImageDeleteBrokenOkCsvExtention:
                    //    package = DeleteBrokenPackage;
                    //    break;
                }

                //Set SSIS Package's Global Variables
                if (package == null)
                    throw new NullReferenceException("SsisManager.ExecuteSsisPackage() package is null!");
                
                Vars = package.Variables;

                //Set file names and path
                Vars[Constants.SSISParameters.CsvFilePath].Value = csvUrl;

                //This value is picked up from ProductData connection string.;
                Vars[Constants.SSISParameters.DataSource].Value = Utilities.GetConnectionString(Data.Access.Constants.DBConnections.ProductSSIS);

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
                throw new AffiliGenericException("SSIS ImageImport Data execute result PPID:" + prodProgId + " :" + execResult +
                 ":Errors:" + strDtsErrors + ":Package:" + pkgLocation + ":csv URL:" + csvUrl, ex);
            }
        }

       
        #region Helper methods

        /// <summary>
        /// Reset the ImageFilesCacheStatus
        /// </summary>
        /// <param name="newStatus"></param>
        /// <param name="oldStatus"></param>
        public void ResetImageFilesCacheStatus(ProdImageFilesCacheStatusEnum newStatus, ProdImageFilesCacheStatusEnum oldStatus)
        {
            try
            {
                SsisDao.ResetImageProgramFilesCacheStatus(oldStatus, newStatus);
            }
            catch (Exception ex)
            {
                new AffiliGenericException("Error in SSISManager.ResetImageFilesCacheStatus()", ex, (int)ApplicationEventsEnum.DocProcessInit).CreateLog();

            }
        }

        /// <summary>
        /// Reset the ImageProgramStatus
        /// </summary>
        /// <param name="newStatus">The new status.</param>
        /// <param name="oldStatus">The old status.</param>
        /// <param name="ManualImport">if set to <c>true</c> [manual import].</param>
        public void ResetImageProgramStatus(ImageProgramStatusEnum newStatus, ImageProgramStatusEnum oldStatus, bool ManualImport)
        {
            try
            {
                SsisDao.ResetImageProgramStatus(oldStatus, newStatus, ManualImport);
            }
            catch(Exception ex)
            {
                new AffiliGenericException("Error in SSISManager.ResetImageProgramStatus()", ex, (int)ApplicationEventsEnum.DocProcessInit).CreateLog();

            }
        }
        #endregion
        
    }
}
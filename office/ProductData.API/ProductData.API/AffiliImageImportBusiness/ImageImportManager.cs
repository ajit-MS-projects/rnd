using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Affili.ProductData.Common;
using Affilinet.Business.ImageImport.Common;
using Affilinet.Business.ImageImport.DAO;
using Affilinet.Business.ImageImport.Entity;
using Affilinet.Exceptions;
using Utilities = Affili.ProductData.Common.Utilities;

namespace Affilinet.Business.ImageImport
{
    
    public class ImageImportManager
    {
        protected List<String> SourceCsvHeader = null;
        protected StreamReader SrCsvSource { get; set; }
        protected int imgFileNumber { get; set; }
        protected ImageDAO objImageDAO { get; set; }
        DataTable dtdbImages { get; set; }
        private long imageId = 0;

        private int cntDBImagesRows = 0;
        
        public ImageImportManager()
        {
            Utilities.ReloadConfigSection();
        }
        #region Daily Image
        public void StartDailyImageProcessing()
        {
            Utilities.WriteHealthCheckTimeStamp(WinServiceHealthCheckFileTypesEnum.DailyImageService);
            if (!Utilities.IsServiceScheduled(Constants.AppSettings.DailyImageServiceStartTime, Constants.AppSettings.DailyImageServiceEndHours)) return;
            ImageAttributes objImageAttributes = null;
            imgFileNumber = 1;
            try
            {
                Utilities.ReloadConfigSection();
                objImageDAO = new ImageDAO();
                DataTable dtImgFiles = objImageDAO.ReadImageImportFileInfo(ImageStatusEnum.New);
                if (dtImgFiles.Rows.Count > 0)
                {
                    objImageAttributes = ReadImageAttributes(dtImgFiles);
                    Utilities.CreateInfoLog("Start: StartDailyImageProcessing, ProdProgId:" + objImageAttributes.ProdProgId, ApplicationEventsEnum.DocProcessInit);
                    objImageDAO.UpdateImageImportProgramFilesCache(objImageAttributes.ProdProgId, objImageAttributes.ExportTimeStamp, ProdImageFilesCacheStatusEnum.Processing);
                    while (ReadImageCsvToCollection(objImageAttributes))
                    {
                        ImageDownloader objImageDownloader = new ImageDownloader(true);
                        if (objImageDownloader.DownloadImage(objImageAttributes))
                        {
                            WriteToImageFile(objImageAttributes);
                        }
                    }
                    objImageDAO.UpdateImageImportProgramFilesCache(objImageAttributes.ProdProgId, objImageAttributes.ExportTimeStamp, ProdImageFilesCacheStatusEnum.Processed);
                    Utilities.CreateInfoLog("End: StartDailyImageProcessing, ProdProgId:" + objImageAttributes.ProdProgId, ApplicationEventsEnum.DocProcessInit);
                }
            }
            catch (AffiliBaseException aex)
            {
                aex.CreateLog();
                if(objImageAttributes != null)
                    objImageDAO.UpdateImageImportProgramFilesCache(objImageAttributes.ProdProgId, objImageAttributes.ExportTimeStamp, ProdImageFilesCacheStatusEnum.Error, aex.Message);
            }
            catch (Exception ex)
            {
                new AffiliGenericException("Error in ProductImportManager.StartDailyImageProcessing()", ex).CreateLog();
                if (objImageAttributes != null)
                    objImageDAO.UpdateImageImportProgramFilesCache(objImageAttributes.ProdProgId, objImageAttributes.ExportTimeStamp, ProdImageFilesCacheStatusEnum.Error, ex.Message);
            }
            finally
            {
                if (SrCsvSource != null)
                {
                    SrCsvSource.Close();
                    SrCsvSource.Dispose();
                    SrCsvSource = null;
                }
                if (SourceCsvHeader != null)
                {
                    SourceCsvHeader.Clear();
                    SourceCsvHeader = null;
                }
            }
        }
        private bool ReadImageCsvToCollection(ImageAttributes objImageAttributes)
        {
            try
            {
                int cntLines = 0;
                int linesPerImageCsv = int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ImgCsvMaxLines), out linesPerImageCsv) ? linesPerImageCsv : 10000;
                bool retVal = false;
                if (SrCsvSource == null)
                {
                    if (!File.Exists(objImageAttributes.SourceImageCsvFullPath)) return false;
                    SrCsvSource = OpenFileForRead(objImageAttributes);
                    CreateSourceCsvColumns(SrCsvSource.ReadLine());
                }
                String strLine = String.Empty;
                while ((strLine = SrCsvSource.ReadLine()) != null)
                {
                    retVal = true;
                    ProductImage img = null;
                    if (objImageAttributes.ProductImages.Count < linesPerImageCsv)
                        img = new ProductImage();
                    else
                    {
                        img = objImageAttributes.ProductImages[cntLines];
                        img.ReuseObject();
                    }
                    ReadLineToImageObject(img, strLine);
                    if (objImageAttributes.ProductImages.Count < linesPerImageCsv)
                        objImageAttributes.ProductImages.Add(img);
                    if (cntLines >= linesPerImageCsv - 1) return retVal;
                    cntLines++;
                }

                return retVal;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in ProductImportManager.ReadImageCsvToCollection", ex, 1);
            }
        }
        private void ReadLineToImageObject(ProductImage prodImage, String imageLine)
        {
            string fldSplitter = Constants.Generic.DestFieldQualifier +
                                  Constants.Generic.DestFieldSeperator +
                                  Constants.Generic.DestFieldQualifier;
            string[] splitChrs = new string[] { fldSplitter };

            String[] fields = imageLine.Split(splitChrs, StringSplitOptions.None);
            for (int i = 0; i < SourceCsvHeader.Count; i++)
            {
                switch (SourceCsvHeader[i])
                {
                    case Constants.ProductImage.ProgramID:
                        prodImage.ProgramID = fields[i].Replace(Constants.Generic.DestFieldQualifier, ""); ;
                        break;
                    case Constants.ProductImage.ProductProgramID:
                        prodImage.ProdProgId = fields[i].Replace(Constants.Generic.DestFieldQualifier, ""); ;
                        break;
                    case Constants.ProductImage.ImageID:
                        prodImage.ImageId = fields[i].Replace(Constants.Generic.DestFieldQualifier, ""); ;
                        break;
                    case Constants.ProductImage.ImageURL:
                        prodImage.ImageUrl = fields[i].Replace(Constants.Generic.DestFieldQualifier, ""); ;
                        break;
                    case Constants.ProductImage.ImageNb:
                        prodImage.ImageNumToUse = fields[i].Replace(Constants.Generic.DestFieldQualifier, ""); ;
                        break;
                }
            }
        }
        #endregion
        #region Auto & Manual image review
        public void StartImageReviewProcessing()
        {//Auto image review
            StartImageReviewProcessing(false);
        }

        public void StartImageReviewProcessing(bool isManual)
        {//Auto/Manual image review
            Utilities.CreateInfoLog("StartImageReviewProcessing(): start isManual?=" + isManual.ToString(), ApplicationEventsEnum.ServiceStart);
            if (isManual)
            {
                Utilities.WriteHealthCheckTimeStamp(WinServiceHealthCheckFileTypesEnum.ManualImageService);
                if (!Utilities.IsServiceScheduled(Constants.AppSettings.ManualImageServiceStartTime, Constants.AppSettings.ManualImageServiceEndHours)) return;
            }
            else
            {
                Utilities.WriteHealthCheckTimeStamp(WinServiceHealthCheckFileTypesEnum.ImageReviewService);
                if (!Utilities.IsServiceScheduled(Constants.AppSettings.ImageReviewServiceStartTime, Constants.AppSettings.ImageReviewServiceEndHours)) return;
            }

            Utilities.CreateInfoLog("StartImageReviewProcessing(): finished writing health check=" + isManual.ToString(), ApplicationEventsEnum.ServiceStart);

            ImageAttributes objImageAttributes = null;
            imgFileNumber = 1;
            try
            {
                Utilities.ReloadConfigSection();
                Utilities.CreateInfoLog("StartImageReviewProcessing(): ReloadConfigSection done", ApplicationEventsEnum.ServiceStart);
                objImageDAO = new ImageDAO();
                Utilities.CreateInfoLog("StartImageReviewProcessing(): imageDao object created", ApplicationEventsEnum.ServiceStart);
                objImageAttributes = new ImageAttributes();
                objImageAttributes.IsManualReview = isManual;
                if (objImageAttributes.IsManualReview)
                    dtdbImages = ReadDataSet(objImageAttributes, objImageDAO.GetImagesForManualReview());
                else
                    dtdbImages = ReadDataSet(objImageAttributes, objImageDAO.GetImagesToReview());
                Utilities.CreateInfoLog("StartImageReviewProcessing(): ReadDataSet() done rows image data tabel:" + dtdbImages.Rows.Count, ApplicationEventsEnum.ServiceStart);
                Utilities.CreateInfoLog("Start: " + (isManual ? "ManualImageReview " : "AutomaticImageReview ") + ", ProdProgId:" + objImageAttributes.ProdProgId, ApplicationEventsEnum.DocProcessInit);
                if (dtdbImages.Rows.Count == 0)
                {
                    objImageDAO.UpdateImageProgramStatus(ImageProgramStatusEnum.ReviewComplete, objImageAttributes.ProdProgId);
                    return; //if nothing to review
                }
                else
                {
                    //objImageAttributes.ProdProgId = dtdbImages.Rows[0][Constants.ProductImage.ProductProgramID].ToString();
                    objImageAttributes.ExportTimeStamp = Constants.Generic.ImageReviewCsvPrefix;
                    String sourcePath = Utilities.GetAppSettingValue(Constants.AppSettings.ImgCsvSaveLocation) + objImageAttributes.ProdProgId + @"\";
                    String sourceFile = Constants.Generic.ImageReviewCsvPrefix + "_" + objImageAttributes.ProdProgId + "*.csv";
                    if (!Directory.Exists(sourcePath))
                        Directory.CreateDirectory(sourcePath);
                    Utilities.DeleteFiles(sourceFile, sourcePath);//Delete old review files for this program
                    Utilities.CreateInfoLog("StartImageReviewProcessing(): DeleteFiles() done", ApplicationEventsEnum.ServiceStart);
                }
                do
                {
                    while (ReadDatabaseImagesToCollection(objImageAttributes))
                    {
                        ImageDownloader objImageDownloader = new ImageDownloader(false);
                        Utilities.CreateInfoLog("StartImageReviewProcessing(): create objImageDownloader object", ApplicationEventsEnum.ServiceStart);
                        objImageAttributes.CheckForImageChangesOnImageServer = !objImageAttributes.IsManualReview && Utilities.GetAppSettingValue(Constants.AppSettings.CheckForImageChangesOnImageServer) == "1";
                        Utilities.CreateInfoLog("StartImageReviewProcessing():  objImageAttributes.CheckForImageChangesOnImageServer" + objImageAttributes.CheckForImageChangesOnImageServer.ToString(), ApplicationEventsEnum.ServiceStart);
                        if (objImageDownloader.DownloadImage(objImageAttributes))
                        {
                            WriteToImageFile(objImageAttributes);
                        }
                    }
                    cntDBImagesRows = 0;
                    long.TryParse(dtdbImages.Rows[dtdbImages.Rows.Count - 1][Constants.ProductImage.DBID].ToString(), out imageId);
                    if (objImageAttributes.IsManualReview)
                        dtdbImages = objImageDAO.GetImagesForManualReview(imageId, objImageAttributes.ProdProgId).Tables[0];
                    else
                        dtdbImages = objImageDAO.GetImagesToReview(imageId, objImageAttributes.ProdProgId).Tables[0];
                } while (dtdbImages.Rows.Count > 0);
                objImageDAO.UpdateImageProgramStatus(ImageProgramStatusEnum.ReviewComplete, objImageAttributes.ProdProgId);
                Utilities.CreateInfoLog("End: " + (isManual ? "ManualImageReview " : "AutomaticImageReview ") + ", ProdProgId:" + objImageAttributes.ProdProgId, ApplicationEventsEnum.DocProcessInit);
            }
            catch (AffiliBaseException aex)
            {
                aex.CreateLog();
                objImageDAO.UpdateImageProgramStatus(ImageProgramStatusEnum.Error, objImageAttributes == null ? "0" : objImageAttributes.ProdProgId);
            }
            catch (Exception ex)
            {
                new AffiliGenericException("Error in ProductImportManager.StartImageReviewProcessing()", ex, 1).CreateLog();
                objImageDAO.UpdateImageProgramStatus(ImageProgramStatusEnum.Error, objImageAttributes == null ? "0" : objImageAttributes.ProdProgId);
            }
            finally
            {
                if (SrCsvSource != null)
                {
                    SrCsvSource.Close();
                    SrCsvSource.Dispose();
                    SrCsvSource = null;
                }
                if (SourceCsvHeader != null)
                {
                    SourceCsvHeader.Clear();
                    SourceCsvHeader = null;
                }
            }
        }
        private DataTable  ReadDataSet(ImageAttributes objImageAttributes, DataSet objDataSet)
        {
            if (objDataSet.Tables[1].Rows.Count>0)
                objImageAttributes.ProdProgId = objDataSet.Tables[1].Rows[0][Constants.ProductImage.ProductProgramID].ToString();
            return objDataSet.Tables[0];
        }

        private bool ReadDatabaseImagesToCollection(ImageAttributes objImageAttributes)
        {
            int cntLines = 0;
            int linesPerImageCsv = int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ImgCsvMaxLines), out linesPerImageCsv) ? linesPerImageCsv : 10000; 
            bool retVal = false;
            

            for (; cntDBImagesRows < dtdbImages.Rows.Count; cntDBImagesRows++)
            {
                retVal = true;
                ProductImage img = null;
                if (objImageAttributes.ProductImages.Count < linesPerImageCsv)
                    img = new ProductImage();
                else
                {
                    img = objImageAttributes.ProductImages[cntLines];
                    img.ReuseObject();
                }
                ReadDataRowToImageObject(img, dtdbImages.Rows[cntDBImagesRows]);
                if (objImageAttributes.ProductImages.Count < linesPerImageCsv)
                    objImageAttributes.ProductImages.Add(img);
                if (cntLines >= linesPerImageCsv - 1) { cntDBImagesRows++; return retVal; }
                cntLines++;
            }
           
            return retVal;
        }
        private void ReadDataRowToImageObject(ProductImage prodImage,DataRow dr)
        {
            foreach(DataColumn dc in dr.Table.Columns)
            {
                switch (dc.ColumnName)
                {
                    case Constants.ProductImage.ProgramID:
                        prodImage.ProgramID = CheckForNull(dr[dc.ColumnName],"").ToString();
                        break;
                    case Constants.ProductImage.ProductProgramID:
                        prodImage.ProdProgId = CheckForNull(dr[dc.ColumnName], "").ToString();
                        break;
                    case Constants.ProductImage.DBID:
                        prodImage.ImageId = CheckForNull(dr[dc.ColumnName], "").ToString();
                        break;
                    case Constants.ProductImage.ImageURL:
                        prodImage.ImageUrl = CheckForNull(dr[dc.ColumnName], "").ToString();
                        break;
                    case Constants.ProductImage.ImageNb:
                        prodImage.ImageNumToUse = CheckForNull(dr[dc.ColumnName], "").ToString();
                        break;
                    case Constants.ProductImage.AdvUpdatedDate:
                        prodImage.AdvUpdatedDate = (DateTime)CheckForNull(dr[dc.ColumnName], DateTime.MinValue);
                        break;
                }
            }
        }
        #endregion
        #region Download single file
        public bool DownloadSingleImage(String prodProgId, String ImageURL, out String exceptionMess)
        {
            bool retVal = false;
            try
            {
                exceptionMess = String.Empty;
                objImageDAO = new ImageDAO();
                ImageAttributes objImageAttributes = new ImageAttributes();
                objImageAttributes.ProdProgId = prodProgId;
                ProductImage img = new ProductImage();
                img.ImageUrl = ImageURL;
                img.ProdProgId = prodProgId;
                objImageAttributes.ProductImages.Add(img);
                ImageDownloader objImageDownloader = new ImageDownloader(null);
                objImageDownloader.DownloadImage(objImageAttributes);
                if (!img.IsValidImage)
                {
                    objImageDAO.IncrSingleImageBrokenCounter(prodProgId, ImageURL);
                    exceptionMess = "Broken Image...! " + objImageDownloader.ExceptionMessage;
                }
                else
                {
                    objImageDAO.UpdateSingleImageInDB(img);
                    retVal = true;
                }
            }
            catch (AffiliBaseException aex)
            {
                exceptionMess = aex.Message + aex.InnerException.Message;
                aex.CreateLog();
                objImageDAO.IncrSingleImageBrokenCounter(prodProgId, ImageURL);
            }
            catch (Exception ex)
            {
                exceptionMess = "Error in ProductImportManager.DownloadSingleImage() " + ex.Message;
                new AffiliGenericException("Error in ProductImportManager.DownloadSingleImage()", ex).CreateLog();
                objImageDAO.IncrSingleImageBrokenCounter(prodProgId, ImageURL);
            }
            return retVal;
        }

        #endregion
        private void WriteToImageFile(ImageAttributes objImageAttributes)
        {
            StreamWriter swImgCsv = null;
            StreamWriter swBrockenImgCsv = null;
            StreamWriter swNotChangedImgCsv = null;
            try
            {
                string fileName = String.Format(objImageAttributes.TargetImageCsvFullPath, objImageAttributes.ProdProgId + "." + imgFileNumber.ToString());
                string brockenFileName = String.Format(objImageAttributes.TargetBrockenImageCsvFullPath, objImageAttributes.ProdProgId + "." + imgFileNumber.ToString());
                string notChangedFileName = String.Format(objImageAttributes.TargetNotChangedImageCsvFullPath, objImageAttributes.ProdProgId + "." + imgFileNumber.ToString());
                imgFileNumber++;
                bool appendFile = false; 
                swImgCsv = new StreamWriter(fileName, appendFile, Encoding.Unicode);
                swBrockenImgCsv = new StreamWriter(brockenFileName, appendFile, Encoding.Unicode);
                if (objImageAttributes.CheckForImageChangesOnImageServer)
                {// not changed is created only if a comparision to adv server is done or allowed
                    swNotChangedImgCsv = new StreamWriter(notChangedFileName, appendFile, Encoding.Unicode);
                    swNotChangedImgCsv.WriteLine(ProductImage.GetNotChangedImageHeader());
                }
                swImgCsv.WriteLine(ProductImage.GetImageHeader());
                swBrockenImgCsv.WriteLine(ProductImage.GetBrokenImageHeader());
                foreach (ProductImage prodImage in objImageAttributes.ProductImages)
                {
                    if (prodImage.IsValidImage)
                    {
                        if (!prodImage.IsImageChanged)
                        {
                            if (objImageAttributes.CheckForImageChangesOnImageServer && swNotChangedImgCsv != null)
                                swNotChangedImgCsv.WriteLine(prodImage.GetNotChangedImageLine);
                        }
                        else
                            swImgCsv.WriteLine(prodImage.GetLine);
                    }
                    else if (!prodImage.IsNewImage)
                        swBrockenImgCsv.WriteLine(prodImage.GetBrokenImageLine);
                    prodImage.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in ProductImportManager.WriteToImageFile", ex, 1);
            }
            finally
            {
                if (swImgCsv != null)
                {
                    swImgCsv.Close();
                    swImgCsv.Dispose();
                    swImgCsv = null;
                }
                if (swBrockenImgCsv != null)
                {
                    swBrockenImgCsv.Close();
                    swBrockenImgCsv.Dispose();
                    swBrockenImgCsv = null;
                }
                if (swNotChangedImgCsv != null)
                {
                    swNotChangedImgCsv.Close();
                    swNotChangedImgCsv.Dispose();
                    swNotChangedImgCsv = null;
                }
            }
        }
        protected void CreateSourceCsvColumns(String strLine)
        {
            SourceCsvHeader = new List<string>();
            String[] fields = strLine.Split(Constants.Generic.DestFieldSeperator[0]);
            foreach (String fld in fields)
            {
                SourceCsvHeader.Add(fld);
            }
        }
        private ImageAttributes ReadImageAttributes(DataTable dtImgFiles)
        {
            ImageAttributes objImageAttributes = new ImageAttributes
            {
                ProdProgId = dtImgFiles.Rows[0][Constants.ProdImageFilesCache.ProdProgId].ToString(),
                ExportTimeStamp = dtImgFiles.Rows[0][Constants.ProdImageFilesCache.ImageImportTimeStamp].ToString(),
            };
            return objImageAttributes;
        }
        private object CheckForNull(object value, object defValue)
        {
            if (value == DBNull.Value || value == null)
                return defValue;
            else
                return value;
        }
        /// <summary>
        /// Opens the file for read.
        /// </summary>
        /// <param name="objImageAttributes">The obj image attributes.</param>
        /// <returns></returns>
        protected StreamReader OpenFileForRead(ImageAttributes objImageAttributes)
        {
            try
            {
                StreamReader SrCsvSource = new StreamReader(objImageAttributes.SourceImageCsvFullPath, Encoding.UTF8);
                return SrCsvSource;
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.OpenFileForRead()", ex);
            }
        }
        public void ResetImageFilesCacheStatus(ProdImageFilesCacheStatusEnum newStatus, ProdImageFilesCacheStatusEnum oldStatus)
        {
            objImageDAO = new ImageDAO();
            objImageDAO.ResetImageProgramFilesCacheStatus(newStatus, oldStatus);            
        }
        public void ResetImageProgramStatus(ImageProgramStatusEnum newStatus, ImageProgramStatusEnum oldStatus, bool manualImport)
        {
            objImageDAO = new ImageDAO();
            objImageDAO.ResetImageProgramStatus(newStatus, oldStatus, manualImport);
        }
   
    }
}

using System;
using System.Data;
using System.IO;
using Affili.ProductData.Common;
using Affilinet.Exceptions;
using Affilinet.Business.ImageImport.Common;
using Affilinet.Business.ImageImport.DAO;
using Utilities=Affilinet.Business.ImageImport.Common.Utilities;
using CommonUtilities = Affili.ProductData.Common.Utilities;

namespace Affilinet.Business.ImageImport
{
    /// <summary>
    /// Deletes only the obsolete CSV files from filessystem (it will not delete any images!)
    /// </summary>
    public class ImageFilesManager
    {
        /// <summary>
        /// Clean up all old image program files.
        /// </summary>
        public void StartCleanUp()
        {
            CommonUtilities.WriteHealthCheckTimeStamp(WinServiceHealthCheckFileTypesEnum.ImageCsvFileCleanUp);
            StartDailyCleanUp();
            StartReviewCleanUp();
        }
         private void StartReviewCleanUp()
         {
             try
             {
                 ImageDAO objImageDAO = new ImageDAO();
                 DataTable dtImgFileForDelete = objImageDAO.GetImageReviewFilesToDelete();

                 if (dtImgFileForDelete != null && dtImgFileForDelete.Rows.Count > 0)
                 {
                     foreach (DataRow fileForDeleteRow in dtImgFileForDelete.Rows)
                     {
                         try
                         {
                             String sourcePath = Utilities.GetAppSettingValue(Constants.AppSettings.ImgCsvSaveLocation) + fileForDeleteRow[Constants.ProductImage.ProductProgramID] + @"\";
                             String sourceFile = Constants.Generic.ImageReviewCsvPrefix + "_" + fileForDeleteRow[Constants.ProductImage.ProductProgramID] + "*.csv";
                             if (!Directory.Exists(sourcePath))
                                 Directory.CreateDirectory(sourcePath);
                             Utilities.DeleteFiles(sourceFile, sourcePath);//Delete old review files for this program
                         }
                         catch (AffiliGenericException ex)
                         {
                             ex.CreateLog();
                         }
                         catch (Exception ex)
                         {
                             new AffiliGenericException("ImageFilesManager.StartReviewCleanUp(). Error on delete image files. Error details:", ex).CreateLog();
                         }
                     }
                 }
             }
             catch (Exception ex)
             {
                 new AffiliGenericException("ImageFilesManager.StartCleanUp(). Error details:", ex).CreateLog();
             }
         }
        private void StartDailyCleanUp()
         {
            try
            {
                ImageDAO objImageDAO = new ImageDAO();
                DataTable dtImgFileForDelete = objImageDAO.ReadImageImportFileInfo(ImageStatusEnum.Ready4delete);
                string imageTimeStamp = "";
                string prodProgId = "";
                string sourceFile = "";
                string sourcePath = "";

                if (dtImgFileForDelete != null && dtImgFileForDelete.Rows.Count > 0)
                {
                    foreach (DataRow fileForDeleteRow in dtImgFileForDelete.Rows)
                    {
                        try
                        {
                            // get the imageTimeStamp
                            if (fileForDeleteRow[Constants.ProdImageFilesCache.ImageImportTimeStamp] != DBNull.Value)
                                imageTimeStamp = fileForDeleteRow[Constants.ProdImageFilesCache.ImageImportTimeStamp].ToString();

                            // get the ProdProgId
                            if (fileForDeleteRow[Constants.ProdImageFilesCache.ProdProgId] != DBNull.Value)
                                prodProgId = fileForDeleteRow[Constants.ProdImageFilesCache.ProdProgId].ToString();

                            sourcePath = Utilities.GetAppSettingValue(Constants.AppSettings.ImgCsvSaveLocation) + prodProgId + @"\";
                            sourceFile = imageTimeStamp + "_" + prodProgId + "*.csv";
                            //sourcePath = Path.Combine(dir, prodProgId);
                            if (Directory.Exists(sourcePath))
                            {
                                Utilities.DeleteFiles(sourceFile, sourcePath);
                            }
                            else
                            {
                                new AffiliGenericException(
                                    "ImageFilesManager.StartDailyCleanUp() Could not reach path: " + sourcePath);
                            }
                            // delete from DB
                            objImageDAO.DeleteImgFileFromDB(prodProgId, imageTimeStamp);
                        }
                        catch (AffiliGenericException ex)
                        {
                            ex.CreateLog();
                        }
                        catch (Exception ex)
                        {
                            new AffiliGenericException("ImageFilesManager.StartDailyCleanUp(). Error on delete image files. Error details:", ex).CreateLog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new AffiliGenericException("ImageFilesManager.StartCleanUp(). Error details:", ex).CreateLog();
            }
        }
    }
}

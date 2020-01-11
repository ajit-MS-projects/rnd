using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using Affili.ProductData.Common;
using Affilinet.Business.ImageImport.DAO;
using Affilinet.Business.ImageImport.Entity;
using Affilinet.Business.ImageImport.Common;
using Affilinet.Exceptions;
using ImageAttributes=Affilinet.Business.ImageImport.Entity.ImageAttributes;
using CommonUtilities = Affili.ProductData.Common.Utilities;
using Utilities = Affilinet.Business.ImageImport.Common.Utilities;

namespace Affilinet.Business.ImageImport
{
    public class ImageDownloader
    {
        protected bool DebugMode {get;set;}
        protected String DebugImageUrl { get; set; }
        protected int ImageCounter { get; set; }
        protected bool? IsDailyJob { get; set; }
        public String ExceptionMessage { get; set; }
        public ImageDownloader(bool? isDailyJob)
        {
            this.IsDailyJob = isDailyJob;
        }
        public bool DownloadImage(ImageAttributes objImageAttributes)
        {
            try
            {
                DebugMode = CommonUtilities.GetAppSettingValue(Constants.AppSettings.DownloadImagesInDebugMode) == "1";
                DebugImageUrl = CommonUtilities.GetAppSettingValue(Constants.AppSettings.DownloadImagesDebugModeImageUrl);
                ImageCounter = 0;
                foreach(ProductImage img in  objImageAttributes.ProductImages)
                {
                    ImageCounter++;
                    try
                    {
                        CheckIfAbortRequested(objImageAttributes.ProdProgId);
                        if (!String.IsNullOrEmpty(img.ImageUrl))
                        {
                            if (objImageAttributes.CheckForImageChangesOnImageServer)
                            {
                                if (IsImageChanged(img, objImageAttributes))
                                    DownloadThumbnail(objImageAttributes, img);
                                img.IsValidImage = true;
                            }
                            else
                                DownloadThumbnail(objImageAttributes, img);
                        }
                    }
                    catch(AffiliAbortRequestedException)
                    {
                        throw;
                    }
                    catch(AffiliBaseException ex)
                    {
                        objImageAttributes.ErrorImages++;
                        img.IsValidImage = false;
                        if (img.ErrorCode <= 0) img.ErrorCode = 1500;
                        ExceptionMessage = ex.ToString();
                        ex.CreateLog();
                    }
                    catch (Exception ex)
                    {
                        objImageAttributes.ErrorImages++;
                        img.IsValidImage = false;
                        if (img.ErrorCode <= 0) img.ErrorCode = 2000;
                        ExceptionMessage = ex.ToString();
                        new AffiliGenericException("ImageDownloader.DownloadImage() Error while downloading image.ProdProgId:" + objImageAttributes.ProdProgId, ex).CreateLog();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("ImageDownloader.DownloadImage() Error while downloading image. ProdProgId:" + objImageAttributes.ProdProgId, ex);
            }
        }
        private void CheckIfAbortRequested(String prodProgId)
        {
            if (IsDailyJob != null)
            {
                ImageDAO objImageDAO = new ImageDAO();
                int maxImages = 100;
                if(Utilities.GetAppSettingValue(Constants.AppSettings.CheckAbortAfterImages)!=null)
                    maxImages = int.Parse(Utilities.GetAppSettingValue(Constants.AppSettings.CheckAbortAfterImages));
                if (ImageCounter >= maxImages)
                {
                    ImageCounter = 0;
                    if (IsDailyJob==true)
                    {
                        if (objImageDAO.GetDailyImageStatus(prodProgId) == Constants.ProdImageFilesCacheStatus.AbortRequestedByAdmin)
                            throw new AffiliAbortRequestedException("Daily job aborted by admin: Product Program Id:" + prodProgId);
                    }
                    else
                    {
                        if (objImageDAO.GetImageProgramStatus(prodProgId) == Constants.ImageProgramStatus.AbortRequestedByAdmin)
                            throw new AffiliAbortRequestedException("Image Review job aborted by admin: Product Program Id:" + prodProgId);
                    }
                }
            }
        }
        private void DownloadThumbnail(ImageAttributes objImageAttributes, ProductImage img)
        {
            Thumbnail thumb = null;
            try
            {
                if (DebugMode)
                    thumb = new Thumbnail(DebugImageUrl);
                else
                    thumb = new Thumbnail(img.ImageUrl);
                String md5Hash = Utilities.GetMd5HashBytes(img.ImageUrl);
                String destDir = Utilities.GetAppSettingValue(Constants.AppSettings.ImagesSaveLocation);
                destDir += CommonUtilities.PadProdProgId(objImageAttributes.ProdProgId) + @"\" + md5Hash[md5Hash.Length - 1] + @"\";
                if (!Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);

                #region "Hotfix"

                // SJI: Hotfix for checking if image is already on our side which is identical
                String tempImageDir = Utilities.GetAppSettingValue(Constants.AppSettings.ImagesTempSaveLocation);
                if (!Directory.Exists(tempImageDir))
                    Directory.CreateDirectory(tempImageDir);

                String tempFilename = tempImageDir + @"\" + md5Hash + ".jpg";
                String imageFilename = destDir + @"\" + md5Hash + ".jpg";

                thumb.Image.Save(tempFilename, ImageFormat.Jpeg);

                if (!File.Exists(imageFilename))
                {
                    thumb.Image.Save(imageFilename, ImageFormat.Jpeg);
                    CommonUtilities.CreateDebugLog(imageFilename, ApplicationEventsEnum.ServiceStart);
                }
                else
                {
                    if (!CommonUtilities.FileCompare(tempFilename, imageFilename))
                    {
                        thumb.Image.Save(imageFilename, ImageFormat.Jpeg);
                        CommonUtilities.CreateDebugLog(imageFilename, ApplicationEventsEnum.LogImageNames);
                    }
                }

                File.Delete(tempFilename);

                #endregion


                img.AdvUpdatedDate = thumb.AdvUpdatedDate;

                double wFactor = thumb.Image.Width > thumb.Image.Height ? 1.0 : thumb.Image.Width * 1.0 / thumb.Image.Height * 1.0;
                double hFactor = thumb.Image.Width > thumb.Image.Height ? thumb.Image.Height * 1.0 / thumb.Image.Width * 1.0 : 1.0;
                img.ImageWidth = thumb.Image.Width;
                img.ImageHeight = thumb.Image.Height;
                img.Img30Width = (int)(030*wFactor);
                img.Img30Height = (int)(030 * hFactor);
                img.Img60Width = (int)(060 * wFactor);
                img.Img60Height = (int)(060 * hFactor);
                img.Img90Width = (int)(090 * wFactor);
                img.Img90Height = (int)(090 * hFactor);
                img.Img120Width = (int)(120 * wFactor);
                img.Img120Height = (int)(120 * hFactor);
                img.Img180Width = (int)(180 * wFactor);
                img.Img180Height = (int)(180 * hFactor);
                img.ErrorCode = 0;
                img.IsValidImage = true;
                img.IsImageChanged = true;
            }
            catch(WebException ex)
            {
                ExceptionMessage = ex.ToString();
                if (ex.Response != null)
                {
                    HttpWebResponse a = (HttpWebResponse) ex.Response;
                    img.ErrorCode = (int) a.StatusCode;
                }
                else
                    img.ErrorCode = (int)ex.Status;
                //throw new AffiliGenericException("ImageDownloader.DownloadThumbnail() Error while downloading image. ProdProgId:" + objImageAttributes.ProdProgId+" " + img.ImageUrl , ex);
            }
            catch (Exception ex)
            {
                ExceptionMessage = ex.ToString();
                img.ErrorCode = 1000;
                //throw new AffiliGenericException("ImageDownloader.DownloadThumbnail() Error while downloading image. ProdProgId:" + objImageAttributes.ProdProgId, ex);
            }
            finally
            {
              if (thumb != null) thumb.Dispose();
            }
        }
        private bool IsImageChanged(ProductImage img, ImageAttributes objImageAttributes)
        {
            bool retVal = false;
            try
            {
                if (!IsImageExists(img, objImageAttributes)) return true;//If image does not exists download anyway 
                if (img.AdvUpdatedDate == DateTime.MinValue) return true;//If adv image update date is null/min value in db download image
                WebClient objWebClient = new WebClient();
                MemoryStream objMemoryStream = null;
                if (DebugMode)
                    objMemoryStream = new MemoryStream(objWebClient.DownloadData(DebugImageUrl), 0, 1);
                else
                     objMemoryStream = new MemoryStream(objWebClient.DownloadData(img.ImageUrl), 0, 1);
                if (objWebClient.ResponseHeaders.GetValues("Last-Modified") == null || objWebClient.ResponseHeaders.GetValues("Last-Modified").Length<=0) return true;
                String lastUpdDate = objWebClient.ResponseHeaders.GetValues("Last-Modified")[0].ToString();
                DateTime dtLastUpdDate; // = DateTime.MinValue;
                if (DateTime.TryParse(lastUpdDate, out dtLastUpdDate))
                {
                    if (dtLastUpdDate.CompareTo(img.AdvUpdatedDate) > 0)
                    {
                        retVal = true;
                        img.AdvUpdatedDate = dtLastUpdDate;
                    }
                }
                else
                {
                    retVal = true;
                    CommonUtilities.CreateInfoLog("ImageDownloader.IsImageChanged(): Can not parse date for ProdProgId:" + objImageAttributes.ProdProgId + " image:" + img.ImageUrl, Affili.ProductData.Common.ApplicationEventsEnum.DocProcessing);
                }
            }catch(Exception ex)
            {
                throw new AffiliGenericException("ImageDownloader.IsImageChanged(): Error while comparing httpheader date, ProdProgId:" + objImageAttributes.ProdProgId, ex);
            }
            return retVal;
        }
        private bool IsImageExists(ProductImage img, ImageAttributes objImageAttributes)
        {
            String imgUrl = DebugMode ? DebugImageUrl : img.ImageUrl;
            String md5Hash = Utilities.GetMd5HashBytes(img.ImageUrl);
            String destDir = Utilities.GetAppSettingValue(Constants.AppSettings.ImagesSaveLocation);
            destDir += CommonUtilities.PadProdProgId(objImageAttributes.ProdProgId) + @"\" + md5Hash[md5Hash.Length - 1] + @"\";
            
            return File.Exists(destDir + "\\" + md5Hash + ".jpg");
        }
    }
}

using System;
using System.IO;
using System.Text;
using Affilinet.Business.ImageImport.Common;
using Affilinet.Exceptions;
using Affili.ProductData.Common;
using Utilities = Affili.ProductData.Common.Utilities;



namespace Affilinet.Business.ImageImport.Entity
{
    class ProductImageDeleted
    {
        public string ImageId { get; set; }
        public string ProdProgId { get; set; }
        public string ImageUrl { get; set; }
        public bool ErrorOnDeletion { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string Md5Hash
        {
            get
            {
                return Utilities.GetMd5HashBytes(ImageUrl);
            }
        }
        public string ImagePath
        {
            get
            {
                String destDir = Utilities.GetAppSettingValue(Constants.AppSettings.ImagesSaveLocation);
                String imgLocation = Path.Combine(destDir, ProdProgId.PadProdProgId());
                return Path.Combine(imgLocation, Md5Hash[Md5Hash.Length - 1].ToString());
                
            }
        }
        public string ImageFilePath
        {
            get { return Path.Combine(ImagePath, Md5Hash + ".jpg"); }

        }
        public string GetErrorImageLine
        {
            get
            {
                StringBuilder line = new StringBuilder();
                line.Append(Constants.Generic.DestFieldQualifier + ImageId + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + ImageUrl + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + ImagePath + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + ErrorMessage + Constants.Generic.DestFieldQualifier);
                return line.ToString();
            }
        }
        public string GetSuccessfulImageLine
        {
            get
            {
                StringBuilder line = new StringBuilder();
                line.Append(Constants.Generic.DestFieldQualifier + ImageId + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + ImageUrl + Constants.Generic.DestFieldQualifier );
                return line.ToString();
            }
        }

        /// <summary>
        /// Create a delimiter seperated header line for successful deleted image CSV files.
        /// </summary>
        public static String GetImageSuccessfulHeader()
        {
            try
            {
                string imageColHeaders = Constants.ProductImage.ImageID + Constants.Generic.DestFieldSeperator;
                imageColHeaders += Constants.ProductImage.ImageURL;
                return imageColHeaders;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in SsisManager.WriteToImageDeleteStatusFile() on GetImageSuccessfulHeader", ex);
            }
        }

        /// <summary>
        /// Create a delimiter seperated header line for error during delete image file.
        /// </summary>
        public static String GetImageErrorHeader()
        {
            try
            {
                string imageColHeaders = Constants.ProductImage.ImageID + Constants.Generic.DestFieldSeperator;
                imageColHeaders += Constants.ProductImage.ImageURL + Constants.Generic.DestFieldSeperator;
                imageColHeaders += Constants.ProductImage.ErrorMessage + Constants.Generic.DestFieldSeperator;
                imageColHeaders += Constants.ProductImage.ImageFilePath;
                return imageColHeaders;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in SsisManager.WriteToImageDeleteStatusFile() on GetImageErrorHeader", ex);
            }
        }
    }
}

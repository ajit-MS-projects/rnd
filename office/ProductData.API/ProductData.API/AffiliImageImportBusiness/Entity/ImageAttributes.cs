using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Affilinet.Business.ImageImport.Common;

namespace Affilinet.Business.ImageImport.Entity
{
    public class ImageAttributes
    {
        public String ProdProgId { get; set; }
        public String ExportTimeStamp { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public int ErrorImages { get; set; }
        public bool CheckForImageChangesOnImageServer { get; set; }
        public bool IsManualReview { get; set; }
        public String SourceImageCsvFullPath
        {
            get
            {
                String dir = Utilities.GetAppSettingValue(Constants.AppSettings.ImgCsvSaveLocation) + ProdProgId + @"\";
                String fileName = ExportTimeStamp + "_" + ProdProgId + Constants.FileNaming.ImageUpdateCsvExtention;
                return dir + fileName;
            }
        }
        public String TargetImageCsvFullPath
        {
            get
            {
                String dir = Utilities.GetAppSettingValue(Constants.AppSettings.ImgCsvSaveLocation) + ProdProgId + @"\";
                String fileName = ExportTimeStamp + "_{0}" + Constants.FileNaming.ImageUpdateCsvExtention;
                return dir + fileName;
            }
        }
        public String TargetBrockenImageCsvFullPath
        {
            get
            {
                String dir = Utilities.GetAppSettingValue(Constants.AppSettings.ImgCsvSaveLocation) + ProdProgId + @"\";
                String fileName = ExportTimeStamp + "_{0}" + Constants.FileNaming.ImageBrokenCsvExtention;
                return dir + fileName;
            }
        }
        public String TargetNotChangedImageCsvFullPath
        {
            get
            {
                String dir = Utilities.GetAppSettingValue(Constants.AppSettings.ImgCsvSaveLocation) + ProdProgId + @"\";
                String fileName = ExportTimeStamp + "_{0}" + Constants.FileNaming.ImageNotChangedCsvExtention;
                return dir + fileName;
            }
        }
        public ImageAttributes()
        {
            ProductImages = new List<ProductImage>();
        }
    }
}

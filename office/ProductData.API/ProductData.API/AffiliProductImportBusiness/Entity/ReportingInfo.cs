using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Affilinet.Utility.Logging;
using Affilinet.Business.ProductImport.Common;
using Constants=Affilinet.Business.ProductImport.Common.Constants;

namespace Affilinet.Business.ProductImport.Entity
{
    /// <summary>
    /// Entity for maintaining report log parameters
    /// </summary>
    public class ReportingInfo
    {
        private string _Attention=String.Empty;

        public int NewImages { get; set; }

        public int NewProducts { get; set; }

        public int UpdatedProducts { get; set; }

        public int TotalProductsProcessed { get; set; }

        public int ErrorProducts { get; set; }

        public int Warnings { get; set; }

        public int DeletedProducts { get; set; }

        public int DeletedImages { get; set; }

        public int NewProductCategories { get; set; }

        public int ProductsInAffiliCategories { get; set; }

        public string AutoOrManualImport { get; set; }

        public string ImageError { get; set; }
        //public int TotalProductsInDB { get; set; }

        //public int ImportStatus { get; set; }

        public int NotChangedProducts { get; set; }

        public int CountOfProductsInCategories { get; set; }

        public string SanitizationDuration { get; set; }

        public int TotalShopCategories { get; set; }

        public int ProductsWithInvalidDeeplinkUrl { get; set; }

        public int ProductsWithoutCategoryText { get; set; }
       
        public string Attention
        {
            get
            {
                return _Attention;
            } 
            set
            {
                _Attention += " : " + value;
            }
        }



        /// <summary>
        /// Gets the report logs. Loops through properties using reflection.
        /// </summary>
        /// <param name="productProgramId">The product program id.</param>
        /// <returns></returns>
        public List<ReportLog> GetReportLogs(string productProgramId)
        {
            List<ReportLog> logs = new List<ReportLog>();
            PropertyInfo[] arrPi = this.GetType().GetProperties();
            foreach(PropertyInfo pi in arrPi)
            {
                ReportLog log = new ReportLog();
                log.Id = productProgramId;
                log.SystemListId = Constants.SystemListIds.ProductImportReportParams;
                log.Value = pi.GetValue(this, null) == null ? "" : pi.GetValue(this, null).ToString();
                log.ListEnum = GetListEnum(pi.Name);
                logs.Add(log);
            }
            return logs;
        }
        /// <summary>
        /// Gets the list enum.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public int GetListEnum(string propertyName)
        {
            int retVal = 0;
            switch (propertyName)
            {
                case "NewImages":
                    retVal = (int)ProductImportReportParamsEnum.NewImages;
                    break;
                case "NewProducts":
                    retVal = (int)ProductImportReportParamsEnum.NewProducts;
                    break;
                case "UpdatedProducts":
                    retVal = (int)ProductImportReportParamsEnum.UpdatedProducts;
                    break;
                case "TotalProductsProcessed":
                    retVal = (int)ProductImportReportParamsEnum.TotalProductsProcessed;
                    break;
                case "ErrorProducts":
                    retVal = (int)ProductImportReportParamsEnum.ErrorProducts;
                    break;
                case "Warnings":
                    retVal = (int)ProductImportReportParamsEnum.Warnings;
                    break;
                case "DeletedProducts":
                    retVal = (int)ProductImportReportParamsEnum.DeletedProducts;
                    break;
                case "NewProductCategories":
                    retVal = (int)ProductImportReportParamsEnum.NewProductCategories;
                    break;
                case "ProductsInAffiliCategories":
                    retVal = (int)ProductImportReportParamsEnum.ProductsInAffiliCategories;
                    break;
                case "AutoOrManualImport":
                    retVal = (int)ProductImportReportParamsEnum.AutoOrManualImport;
                    break;
                case "TotalProductsInDB":
                    retVal = (int)ProductImportReportParamsEnum.TotalProductsInDB;
                    break;
                case "FileDownloadStatus":
                    retVal = (int)ProductImportReportParamsEnum.FileDownloadStatus;
                    break;
                case "CountOfProductsInCategories":
                    retVal = (int)ProductImportReportParamsEnum.CountOfProductsInCategories;
                    break;
                case "NotChangedProducts":
                    retVal = (int)ProductImportReportParamsEnum.NotChangedProducts;
                    break;
                case "Attention":
                    retVal = (int)ProductImportReportParamsEnum.Attention;
                    break;
                case "ImageError":
                    retVal = (int)ProductImportReportParamsEnum.ImageError;
                    break;
                case "DeletedImages":
                    retVal = (int)ProductImportReportParamsEnum.DeletedImages;
                    break;
                case "SanitizationDuration":
                    retVal = (int)ProductImportReportParamsEnum.SanitizationDuration;
                    break;
                case "TotalShopCategories":
                    retVal = (int)ProductImportReportParamsEnum.TotalShopCategories;
                    break;
                case "ProductsWithoutCategoryText":
                    retVal = (int)ProductImportReportParamsEnum.ProductsWithoutCategoryText;
                    break;
                case "ProductsWithInvalidDeeplinkUrl":
                    retVal = (int)ProductImportReportParamsEnum.ProductsWithInvalidDeeplinkUrl;
                    break;
                
            }
            return retVal;
        }

    }
}

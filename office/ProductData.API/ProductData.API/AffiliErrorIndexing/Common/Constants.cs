namespace AffiliErrorIndexing.Common
{
    /// <summary>
    /// Wrapper class to wrap all constant structures
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Stored procedures which only return a dataSet(s)/value but do not change data.
        /// </summary>
        public struct ReadOnlyStoredProcs
        {
            public const string GetValuesForErrorIndexIndicator = "spGetValuesForErrorIndexIndicator";
            public const string GetValuesForErrorIndexCalculation = "spGetValuesForErrorIndexCalculation";
            public const string GetErrorIndicatorWeight = "spSelectErrorIndicatorWeight";

        }

        /// <summary>
        /// Stored procedures which change data and may return dataSet(s)
        /// </summary>
        public struct DmlStoredProcs
        {
            public const string UpdateErrorIndex = "spUpdateErrorIndex";
            public const string UpdatePa_PPErrorIndex = "spUpdatePa_PPErrorIndex";
            public const string UpdateProductProgramPriority = "spUpdateProductProgramPriority";
        }

        /// <summary>
        /// Application configuration settings
        /// </summary>
        public struct AppSettings
        {
            //public const string ProdProgProcessOrder = "ProdProgProcessOrder";
        }

        /// <summary>
        /// Provides a list of system list ids for multilingual support.
        /// </summary>
        public struct MasterListNames
        {
            public const string TotalProductsProcessed = "TotalProductsProcessed";
            public const string TotalImagesProcessed = "TotalImagesProcessed";
            public const string NewProductCategories = "NewProductCategories";
            public const string TotalShopCategories = "TotalShopCategories";
            public const string ProductsInAffiliCategories = "ProductsInAffiliCategories";
            public const string ProductsWithoutCategoryText = "ProductsWithoutCategoryText";
            public const string ProductsWithInvalidDeeplinkUrl = "ProductsWithInvalidDeeplinkUrl";
            public const string ProductsWithInvalidImageUrl = "ProductsWithInvalidImageUrl";
            public const string SanitizationDuration = "SanitizationDuration";
            public const string DownloadDuration = "DownloadDuration";
            public const string UpdatedProducts = "UpdatedProducts";
        }

        /// <summary>
        /// Provides a list of system list ids for multilingual support.
        /// </summary>
        public struct MasterSystemListNames
        {
            public const string ProductQualityAlerting = "ProductQualityAlerting";
            public const string ProductImportReportParams = "ProductImportReportParams";
            public const string ProductImportStatus = "ProductImportStatus";
            public const string ObsoleteProductDeleteReport = "ObsoleteProductDeleteReport";
            public const string ProductImportReportSsisParams = "ProductImportReportSSISParams";
            public const string ImageImportReportSsisParams = "ImageImportReportSSISParams";
        }

        public struct ErrorIndexMasterListNames
        {
            public const string HeaderError = "HeaderError";
            public const string DownloadError = "DownloadError";
            public const string DownloadTimeError = "DownloadTimeError";
            public const string OtherSanitisationError = "OtherSanitisationError";
            public const string SanitationTimeError = "SanitationTimeError";
            public const string TotalProducts = "TotalProducts";
            public const string TotalImages = "TotalImages";
            public const string TotalShopCategories = "TotalShopCategories";
            public const string NewShopCategories = "NewShopCategories";
            public const string MappedProducts = "MappedProducts";
            public const string ProductsWithoutCategoryText = "ProductsWithoutCategoryText";
            public const string ProductsWithInvalidDeeplinkUrl = "ProductsWithInvalidDeeplinkUrl";
            public const string ProductsWithInvalidImageUrl = "ProductsWithInvalidImageUrl";
            public const string UpdatedProducts = "UpdatedProducts";
        }

         /// <summary>
        /// Pa_WeightageConfiguration table's columns
        /// </summary>
        public struct WeightageConfiguration
        {
            public const string WeightConfigurationId = "WeightConfigurationId";
            public const string Weight = "Weight";
            public const string DeviationFactor = "DeviationFactor";
        }

        /// <summary>
        /// Return table for spGetValuesForErrorIndexCalculation
        /// </summary>
        public struct ErrorIndexCalculation
        {
            public const string LastMonthPublisherCommission = "LastMonthPublisherCommission";
            public const string QIFactor_ForErrorIndex = "QIFactor_ForErrorIndex";
            public const string RevenueFactor_ForErrorIndex = "RevenueFactor_ForErrorIndex";
        }
        
        /// <summary>
        /// Pa_ProductProgramErrorIndex table's columns
        /// </summary>
        public struct Pa_ProductProgramErrorIndex
        {
            public const string Activated = "Activated";
        }
    }
}

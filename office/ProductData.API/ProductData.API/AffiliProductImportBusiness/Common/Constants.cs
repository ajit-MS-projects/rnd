namespace Affilinet.Business.ProductImport.Common
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
            public const string GetScheduledPrograms = "GetScheduledPrograms";
            public const string GetProgramSettings = "GetProgramSettings";
            public const string GetProductColumns = "GetProductColumns";
            public const string GetProductCatForLC = "GetProductCatForLC";
            public const string GetProductHashForLC = "GetProductHashForLC";
            public const string GetProductImageForLC = "GetProductImageForLC";
            public const string GetScheduledProgramsForSsisImport = "GetScheduledProgramsForSsisImport";
            public const string GetAffilicatForLC = "GetAffilicatForLC";
            public const string GetProgramImportStatus = "spSelectProgramImportStatus";
            public const string GetProductProgramStatus = "GetProductProgramStatus";
            public const string GetProductProgramFileChecksum = "GetProductProgramFileChecksum";
        }
        /// <summary>
        /// Stored procedures which change data and may return dataSet(s)
        /// </summary>
        public struct DmlStoredProcs
        {
            public const string Sp_getCategoryID = "sp_getCategoryID";
            public const string UpdateProductProgramStatus = "UpdateProductProgramStatus";
            public const string AddNewImageToProductDB = "AddNewImageToProductDB";
            public const string UpdateProductImportProtocol = "UpdateProductImportProtocol";
            public const string UpdateProductProgram = "UpdateProductProgram";
            public const string ResetProductProgram = "ResetProductProgram";  
            public const string UpdateProductCount = "UpdateProductCount";
            public const string UpdateProductProgramColumn = "UpdateProductProgramColumn";
            public const string SpGetNextID = "spGetNextID";
            public const string SpInsertProdExportProgramFilesCache = "spInsertProdExportProgramFilesCache";
            public const string spInsertImageImportProgramFilesCache = "spInsertImageImportProgramFilesCache";
            public const string UpdateProductProgramAdditionalImportStatus = "UpdateProductProgramAdditionalImportStatus";
            public const string UpdateProductProgramFileChecksum = "UpdateProductProgramFileChecksum";
            public const string UpdateLastExceptionAndGetHeaderCheck = "spUpdateLastExceptionAndGetHeaderCheck";
            public const string UpdateErrorIndex = "spUpdateErrorIndex";
        }
        /// <summary>
        /// Stored procedures which exits only in DB to be used as local cahe
        /// There are executed only if cache is set to SQL in configuration
        /// </summary>
        public struct LocalCacheStoredProcs
        {
            public const string InitLocalCache = "InitLocalCache";
            public const string GetCategoryId = "GetCategoryId";
            public const string AddNewCategoryToLC = "AddNewCategoryToLC";
            public const string GetProductHashStatus = "GetProductHashStatus";
            public const string GetProductsToDelete = "GetProductsToDelete";
            public const string GetImageId = "GetImageId";
            public const string AddNewImageToLC = "AddNewImageToLC";
            public const string UnloadLocalCache = "UnloadLocalCache";
        }
        /// <summary>
        /// Application configuration settings
        /// </summary>
        public struct AppSettings   
        {
            public const string ProdProgProcessOrder = "ProdProgProcessOrder";
            public const string ProgsToProcessPerCycle = "ProgsToProcessPerCycle";
            public const string CSVSaveLocation = "CSVSaveLocation";
            public const string DefDateFormat = "DefDateFormat";
            public const string LoadLocalCacheSSIS = "LoadLocalCacheSSIS";
            public const string MaxCsvLineLength = "MaxCsvLineLength";
            public const string Cache = "Cache";
            public const string AffiliDecimalSep = "AffiliDecimalSep";
            public const string DefaultDecimalCulture = "DefaultDecimalCulture";
            public const string ProductSetCount = "ProductSetCount";
            public const string GenerateImageHash = "GenerateImageHash";
            public const string SkipDownloadingCsvFiles = "SkipDownloadingCsvFiles";
            public const string ImportStartTime = "ImportStartTime";
            public const string ImportEndTime = "ImportEndTime";
            public const string ProductImportSSIS = "ProductImportSSIS";
            public const string ImportedFilesFolder = "ImportedFilesFolder";
            public const string ArchiveFolder = "ArchiveFolder";
            public const string DateTimeFormat = "DateTimeFormat";
            public const string PropertyXmlTag = "PropertyXmlTag";
            public const string PropertyTitleAttrib = "PropertyTitleAttrib";
            public const string PropertyNumberAttrib = "PropertyNumberAttrib";
            public const string PropertyTextAttrib = "PropertyTextAttrib";
            public const string ServiceTimeInervalInMilliSecs = "ServiceTimeInervalInMilliSecs";
            public const string LoadImagesSSIS = "LoadImagesSSIS";
            public const string DownloadCsvOnlyIfNotPresent = "DownloadCsvOnlyIfNotPresent";
            public const string MaxLinesInCSV = "MaxLinesInCSV";
            public const string NotImportedFilesFolder = "NotImportedFilesFolder";
            public const string MaxContinuousErrors = "MaxContinuousErrors";
            public const string ProgramsWithMultiProductLines = "ProgramsWithMultiProductLines";
            public const string PriceWholeDigits = "PriceWholeDigits";
            public const string PricePrecisionDigits = "PricePrecisionDigits";
            public const string ProductImageIdsUpdateSSIS = "ProductImageIdsUpdateSSIS";
            public const string MaxAllowedLinesInSourceCsv = "MaxAllowedLinesInSourceCsv";
            public const string PermissibleFileSizeInMB = "PermissibleFileSizeInMB";
            public const string AppSleepProductCount = "AppSleepProductCount";
            public const string MaxDateYear = "MaxDateYear";
            public const string ExportFilesLocation = "ExportFilesLocation";
            public const string FTPUrlWithoutEncoding = "FTPUrlWithoutEncoding";
            public const string ImgCsvSaveLocation = "ImgCsvSaveLocation";
            public const string ProductsToCheckIfAbortRequested = "ProductsToCheckIfAbortRequested";
            public const string WinServiceHealthCheckFilePath = "WinServiceHeathCheckFilePath";
            public const string HourlyProductImportMaxThreads = "HourlyProductImportMaxThreads";
            public const string MultiThreadingEnabled = "HourlyMultiThreadingEnabled";
            public const string ApproachingMaxProductsPerList = "ApproachingMaxProductsPerList";
        }
        /// <summary>
        /// ProductProgram table's columns
        /// </summary>
        public struct ProductProgram
        {
            public const string SourceURL = "SourceURL";
            public const string PordProgId = "ProductProgramID";
            public const string DestinationURL = "URL";// e.g. in DB files/3778_digitalo.csv
            public const string Encoding = "EncodingString";
            public const string LoginName = "LoginName";
            public const string Password = "Password";
            public const string ProgramName = "Name";
            public const string FileType = "FileType";
            public const string ImportStatus = "ImportStatus";
            public const string ManualImport = "ManualImport";
            public const string AutoUpdateNext = "AutoUpdateNext";
            public const string AutoUpdateIntervalType = "AutoUpdateIntervallType";
            public const string AutoUpdateInterval = "AutoUpdateIntervall";
            public const string LinesChecked = "LinesChecked";
            public const string FileCheckSum = "FileCheckSum";
            public const string AdditionalImportStatus = "AdditionalImportStatus";
            public const string AccountManager = "AccountManager";
            public const string AccountManagerEmail = "AccountManagerEmail";
        }
        /// <summary>
        /// Product table's columns
        /// </summary>
        public struct Product
        {
            public const string ProdCatID = "ProductCategoryID";
            public const string ArtikelNumber = "ArtikelNumber";
            public const string ProductProgramID = "ProductProgramID";
            public const string affiliProductCategoryID = "affiliProductCategoryID";
            public const string ID = "ID";
            public const string ImportDate = "ImportDate";
            public const string Title = "Title";
            public const string Description_short = "Description_short";
            public const string Description = "Description";
            public const string ProductCategoryID = "ProductCategoryID";
            public const string ProductCategoryText = "ProductCategoryText";
            public const string affiliProductCategoryText = "affiliProductCategoryText";
            public const string Price = "Price";
            public const string Price_old = "Price_old";
            public const string Currency_Symbol = "Currency_Symbol";
            public const string ImageURL1 = "ImageURL1";
            public const string ImageURL2 = "ImageURL2";
            public const string ImageURL3 = "ImageURL3";
            public const string DeepLink1 = "DeepLink1";
            public const string DeepLink2 = "DeepLink2";
            public const string Keywords = "Keywords";
            public const string Manufacturer = "Manufacturer";
            public const string Brand = "Brand";
            public const string Distributor = "Distributor";
            public const string ImgWidth1 = "ImgWidth1";
            public const string ImgHeight1 = "ImgHeight1";
            public const string ImgWidth2 = "ImgWidth2";
            public const string ImgHeight2 = "ImgHeight2";
            public const string ImgWidth3 = "ImgWidth3";
            public const string ImgHeight3 = "ImgHeight3";
            public const string update_date = "update_date";
            public const string valid_from = "valid_from";
            public const string valid_to = "valid_to";
            public const string EAN = "EAN";
            public const string ImageID = "ImageID";
            public const string HashCode = "HashCode";
            public const string PricePrefix = "PricePrefix";
            public const string PriceSuffix = "PriceSuffix";
            public const string Shipping = "Shipping";
            public const string ISBN = "ISBN";
            public const string ShippingPrefix = "ShippingPrefix";
            public const string ShippingSuffix = "ShippingSuffix";
            public const string SearchText = "SearchText";
            public const string Properties = "Properties";
            public const string PropertyHash = "PropertyHash";

            //Start: Extra columns for export only
            public const string ImageOK = "ImageOK";  //- Part of DB.Product table
            public const string ImgWidth = "ImgWidth";//-Note: Not part of DB.Product table
            public const string ImgHeight = "ImgHeight";//-Note: Not part of DB.Product table
            public const string Img30Width = "Img30Width";//-Note: Not part of DB.Product table
            public const string Img30Height = "Img30Height";//-Note: Not part of DB.Product table
            public const string Img60Width = "Img60Width";//-Note: Not part of DB.Product table
            public const string Img60Height = "Img60Height";//-Note: Not part of DB.Product table
            public const string Img90Width = "Img90Width";//-Note: Not part of DB.Product table
            public const string Img90Height = "Img90Height";//-Note: Not part of DB.Product table
            public const string Img120Width = "Img120Width";//-Note: Not part of DB.Product table
            public const string Img120Height = "Img120Height";//-Note: Not part of DB.Product table
            public const string Img180Width = "Img180Width";//-Note: Not part of DB.Product table
            public const string Img180Height = "Img180Height";//-Note: Not part of DB.Product table
            //End: Extra columns for export only

            public const string ImageName = "ImageName"; 

            // BasePrice
            public const string BasePrice = "BasePrice";
            public const string BasePricePrefix = "BasePricePrefix";
            public const string BasePriceSuffix = "BasePriceSuffix";
        }
        /// <summary>
        /// ProductImage table's columns
        /// </summary>
        public struct ProductImage
        {
            public const string ImageID = "ImageID";
            public const string ProductProgramID = "ProductProgramID";
            public const string ImageURL = "ImageURL";
            public const string HashCode = "HashCode";
            public const string ImageNb = "ImageNb";
            public const string ImgWidth = "ImgWidth";
            public const string ImgHeight = "ImgHeight";
            public const string ProgramID = "ProgramID";
            //Start: Columns for export only
            public const string Img30Width = "Img30Width";
            public const string Img30Height = "Img30Height";
            public const string Img60Width = "Img60Width";
            public const string Img60Height = "Img60Height";
            public const string Img90Width = "Img90Width";
            public const string Img90Height = "Img90Height";
            public const string Img120Width = "Img120Width";
            public const string Img120Height = "Img120Height";
            public const string Img180Width = "Img180Width";
            public const string Img180Height = "Img180Height";
            //End: Columns for export only
            public const string ImageName = "ImageName"; 
        }
        /// <summary>
        /// ProductProgram table's & fn_drpLists's columns which represent all settings of program
        /// </summary>
        public struct ProgramSettings
        {
            public const string ImportSchemaID = "ID";
            public const string ProgramID = "ProgramID";
            public const string Title = "Title";
            public const string Description = "Description";
            public const string ImportType = "ImportType";
            public const string FieldSeperator = "FldSep";//"FieldSeperator";
            public const string LineSeperator = "LineSeperator";
            public const string Headlines = "Headlines";
            public const string CSVHeadline = "CSVHeadline";
            public const string TextQualifier = "TextQualifier";
            public const string DateFormat = "DateFormat";
            public const string DecimalSymbol = "DecimalSymbol";
            public const string DigitGroupingSymbol = "DigitGroupingSymbol";
            public const string CategorySeperator = "CategorySeperator";
            public const string Active = "Active";
            public const string DefaultAffiliCat = "DefaultAffiliCat";
            public const string ImageNb = "ImageNb";
            public const string Encoding = "EncodingString";
            public const string XmlProductTag = "XmlProductTag";
            public const string PlatformID = "PlatformID";
            public const string EncodeDeeplink = "EncodeDeeplink";
        }
        /// <summary>
        /// ImportField table's columns which represent settings and types of each CSV field
        /// </summary>
        public struct Fields
        {
            public const string ImportSchemaID = "ImportSchemaID";
            public const string ColumnOrder = "ColumnOrder";
            public const string Title = "Title";
            public const string Description = "Description";
            public const string MaxLength = "Length";
            public const string DataType = "DataType";
            public const string DestField = "DestFld";
            public const string RevOrder = "RevOrder";
            public const string IsProperty = "IsProperty";
            public const string IsNumeric = "IsNumeric";
            public const string IsElement = "IsElement";
            public const string IsAttribute = "IsAttribute";
            public const string IsProductTag = "IsProductTag";
            public const string ParentTitle = "ParentTitle";
        }
        /// <summary>
        /// Parameters of various SSIS packages executed
        /// </summary>
        public struct SSISParameters
        {
            public const string ProdProgId = "ProdProgID";
            public const string DataSource = "ConnectionString";
            public const string LocalConnectionString = "LocalConnectionString";//Local cache
            public const string CsvInsertURL = "insertFilePath";
            public const string CsvUpdateURL = "updateFilePath";
            public const string CsvDeleteURL = "deleteFilePath";
            public const string CsvImageURL = "CsvImageURL";
            public const string ProductDbCon = "ProductDbCon";
            public const string ImagesOnly = "ImagesOnly";
            public const string CsvFilePath = "CsvFilePath";
        }
        /// <summary>
        /// All regular expressions used in project
        /// </summary>
        public struct RegExp
        {
            public const string  LineFieldQualifiedSeperator = @"{0}\s*\{1}\s*{0}";
            public const string LineFieldSeperator = @"\s*\{0}\s*";
            public const string UrlValidator = @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
        }
        /// <summary>
        /// Processed CSV file extensions
        /// </summary>
        public struct FileNaming
        {
            //public const string PropertyCsvExtention = ".property.csv";
            public const string ProductInsertCsvExtention = ".product.insert.csv";
            public const string ProductUpdateCsvExtention = ".product.update.csv";
            public const string ProductDeleteCsvExtention = ".product.delete.csv";
            public const string ImageDeleteCsvExtention = ".image.delete.csv";
            public const string ProductImageFileExtention = ".image.update.csv";
            public const string ProductNotchangedCsvExtention = ".product.notChanged.csv";
            public const string DtsxFileExtension = ".dtsx";
        }
        /// <summary>
        /// Common constants
        /// </summary>
        public struct Generic
        {
            public const string DestFieldSeperator = ";";
            public const string DestFieldQualifier = "~\"";//"~";
            public const string ProductHashActionField = "action";
            public const string InsUpdDel = "InsUpdDel";
            public const string DestFieldQualifierAlternate = "-";
            public const string DuplicateImageUrlError = "Duplicate images exists, processing of this program is slow";
            public const string Tab = "\t";
            public const string TabReplacementFieldSeperator1 = "~";
            public const string TabReplacementFieldSeperator2 = "^";
            public const string TabReplacementFieldSeperator3 = "`";
            public const int CategoryTextFldLength = 250;
            public const string DoubleQuoteReplaceChars = "´´";
            public const string SingleQuoteReplaceChars = "´";
            public const string DefaultCurrencySymbol = "EUR";
            public const string HtmlNewLine = "<BR>";
            public const string XmlCategorySeperator = ">";
            public const string GetNextIdProduct = "Product";
            public const string GetNextIdImage = "Image";
            public const string DeleteAction = "D";
        }

        public struct PlateformDefaultCurrencies
        {
            public const string UK_DefaultCurrency = "GBP";
            public const string DE_DefaultCurrency = "EUR";
            public const string NL_DefaultCurrency = "EUR";
            public const string FR_DefaultCurrency = "EUR";
            public const string ES_DefaultCurrency = "EUR";
        }
        /// <summary>
        /// Various processing status
        /// </summary>
        public struct ProcessingStatus
        {
            public const string Success = "Success";
            public const string Error = "Error";
            public const string Disabled = "Disabled";
        }

        /// <summary>
        /// ProductCategory table's columns
        /// </summary>
        public struct ProductCategory
        {
            public const string ProductProgramID = "ProductProgramID";
            public const string CatName = "CatName";
            public const string ID = "ID";
            public const string AffilinetCatID = "affilinetCatID";
            public const string AffiliCatPathText = "AffiliCatPathText";
            public const string CatPathText = "CatPathText";
        }

        /// <summary>
        /// Provides a list of system list ids for multilingual support.
        /// </summary>
        public struct SystemListIds
        {
            public const string ProductImportReportParams = "A91E266F-8A52-42AC-8EF0-BC4035658DB8";
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
        }

    }
}
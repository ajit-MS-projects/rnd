namespace Affilinet.Business.ProductImport.Common
{
    /// <summary>
    /// Designated various documents supported for product import
    /// </summary>
    public enum DocumentTypesEnum
    {
        /// <summary>
        /// CSV source document
        /// </summary>
        CSV,
        /// <summary>
        /// XML source document
        /// </summary>
        XML
    }

    /// <summary>
    /// Various extensions of files to download
    /// </summary>
    public enum FileExtensionsEnum
    {
        /// <summary>
        /// Represents .csv files
        /// </summary>
        CSV,
        /// <summary>
        /// Represents .txt files
        /// </summary>
        TXT,
        /// <summary>
        /// Represents .zip (compressed) files
        /// </summary>
        ZIP,
        /// <summary>
        /// Represents .gz (compressed) files
        /// </summary>
        GZ
    }

    /// <summary>
    /// Various protocols supported for file download
    /// </summary>
    public enum SourceURLProtocolEnum
    {
        /// <summary>
        /// HTTP source urls        
        /// </summary>
        HTTP,
        /// <summary>
        /// FTP source urls
        /// </summary>
        FTP
    }

    /// <summary>
    /// Specific Application events, useful in logging
    /// </summary>
    public enum ApplicationEventsEnum
    {
        /// <summary>
        /// Identifies the Product import win service starting event
        /// </summary>
        ServiceStart = 1,

        /// <summary>
        /// Identifies the Product import win service stopping event
        /// </summary>
        ServiceStop = 2,

        /// <summary>
        /// Identifies the file download process begun event
        /// </summary>
        DownloadStart = 3,

        /// <summary>
        /// Identifies the file download process is in progress event
        /// </summary>
        DownloadInProgress = 4,

        /// <summary>
        /// Identifies the file download process end event
        /// </summary>
        DownloadEnd = 5,
       
        /// <summary>
        /// Identifies the initiation of sanitization and processing of data in csv
        /// </summary>
        DocProcessInit = 6,

        /// <summary>
        /// Identifies 1 of the steps of sanitization and processing of data in csv
        /// </summary>
        DocProcessing = 7,

        /// <summary>
        /// Identifies the end of sanitization and processing of data in csv
        /// </summary>
        DocProcessFinish = 8,

        /// <summary>
        /// Identifies the end of sanitization and processing of data in csv
        /// </summary>
        SsisImportStart = 9,

        /// <summary>
        /// Identifies the end of sanitization and processing of data in csv
        /// </summary>
        SsisImportFinsh= 10,

        /// <summary>
        /// Identifies the CopyFileToExportServers process
        /// </summary>
        CopyExportFiles = 11
    }

    /// <summary>
    /// Various product states, determining SQL operation for the product
    /// </summary>
    public enum ProductActionsEnum
    {
        INSERT,
        UPDATE,
        NOT_CHANGED_ACTION,
        IGNORE_ACTION
    }

    /// <summary>
    /// Represents variuos status of product import life cycle.
    /// </summary>
    public enum ProgramImportStatusEnum
    {
        MANUAL_IMPORT=1,
        MANUAL_CANCLED = 15,
        DOWLOADING=10,
        DOWLOADING_COMPLETE = 11,
        PROCESSING_CSV=12,
        CSV_PROCESSING_COMPLETE=13,
        IMPORTING_CSV=14,
        CSV_IMPORT_COMPLETE = 15,
        DOWNLOAD_ERROR=16,
        CSV_PROCESSING_ERROR=18,
        CSV_IMPORT_ERROR=19,
        ABORT_REQUESTED=20
    }

    /// <summary>
    /// User in admin tool may schedule a manual import
    /// He has an option to specify download CSV and initiate import
    /// or to only process existing CSV and import to DB
    /// (in this case downloading of CSV from advertiser is done manually by support team)
    /// </summary>
    public enum ManualImportEnum
    {
        NONE=0,
        PROCESS_AND_IMPORT_ONLY = 2,
        DOWNLOAD_AND_IMPORT = 1
    }

    /// <summary>
    /// Represents current status of application
    /// </summary>
    public enum ApplicationStatusEnum
    {
        /// <summary>
        /// 
        /// </summary>
        Idle,
        /// <summary>
        /// 
        /// </summary>
        DownloadingCSV,
        /// <summary>
        /// 
        /// </summary>
        ImportingCSV,
    }

    /// <summary>
    /// Import process Report parameters to log
    /// </summary>
    public enum ProductImportReportParamsEnum
    {
        /// <summary>
        /// 
        /// </summary>
        NewImages = 1,
        /// <summary>
        /// 
        /// </summary>
        NewProducts = 2,
        /// <summary>
        /// 
        /// </summary>
        UpdatedProducts = 3,
        /// <summary>
        /// 
        /// </summary>
        TotalProductsProcessed = 4,
        /// <summary>
        /// 
        /// </summary>
        ErrorProducts = 5,
        /// <summary>
        /// 
        /// </summary>
        Warnings = 6,
        /// <summary>
        /// 
        /// </summary>
        DeletedProducts = 7,
        /// <summary>
        /// 
        /// </summary>
        NewProductCategories = 8,
        /// <summary>
        /// 
        /// </summary>
        ProductsInAffiliCategories = 9,
        /// <summary>
        /// 
        /// </summary>
        AutoOrManualImport = 10,
        /// <summary>
        /// 
        /// </summary>
        TotalProductsInDB = 11,
        /// <summary>
        /// 
        /// </summary>
        ImportStatus = 12,
        /// <summary>
        /// 
        /// </summary>
        FileDownloadStatus = 13,
        /// <summary>
        /// 
        /// </summary>
        CountOfProductsInCategories = 14,
        /// <summary>
        /// 
        /// </summary>
        NotChangedProducts = 15,
        /// <summary>
        /// 
        /// </summary>
        DownloadStart=16,
        /// <summary>
        /// 
        /// </summary>
        DownloadEnd=17,
        /// <summary>
        /// 
        /// </summary>
        DownloadError = 18,
        /// <summary>
        /// 
        /// </summary>
        SsisError=19,
        /// <summary>
        /// 
        /// </summary>
        SsisImportStartTime=20,
        /// <summary>
        /// 
        /// </summary>
        SsisImportEndTime=21,
        Attention=22,
        ImageError=23,
        DeletedImages=24,
        SanitizationDuration=25,
        DownloadDuration = 34,
        TotalShopCategories = 28,
        ProductsWithoutCategoryText=31,
        ProductsWithInvalidDeeplinkUrl=32


    }
    /// <summary>
    /// Designates Location where processed files will be moved
    /// </summary>
    public enum FileLocationsEnum
    {
        /// <summary>
        /// 
        /// </summary>
        Archive,
        /// <summary>
        /// 
        /// </summary>
        Imported,
        /// <summary>
        /// 
        /// </summary>
        NotImported,

        ImageCsvs
    }

    /// <summary>
    /// Designates various status of product field while spliting product line using scan each character method
    /// </summary>
    public enum FieldStatusEnum
    {
        /// <summary>
        /// 
        /// </summary>
        ResetState = -1,
        /// <summary>
        /// 
        /// </summary>
        NotQualified = 0,
        /// <summary>
        /// 
        /// </summary>
        StartQualifier = 1,
        /// <summary>
        /// 
        /// </summary>
        EndQualifier = 2
    }
    /// <summary>
    /// Designated next id type product or image
    /// </summary>
    public enum GetNextIdEnum
    {
        Product,
        Image
    }
    /// <summary>
    /// All plateforms supported bby affilinet and there DB ids
    /// </summary>
    public enum AffilinetPlatforms
    {
        None=0,
        DE = 1,
        EN = 2,
        FR = 3,
        NL = 4,
        ES = 5
    }
    public enum WinServiceHealthCheckFileTypesEnum
    {
        ProductImport,
        ProductImportSsis,
        CsvDownload,
        ExportFileMgmtCopy,
        ExportFileMgmtDelete,
        HourlyProductImport,
    }
}

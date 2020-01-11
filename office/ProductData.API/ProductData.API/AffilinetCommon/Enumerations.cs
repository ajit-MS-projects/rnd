
namespace Affili.ProductData.Common
{
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
        SsisImportFinsh = 10,

        /// <summary>
        /// Identifies the CopyFileToExportServers process
        /// </summary>
        CopyExportFiles = 11,

        /// <summary>
        /// Indentifies the images that have been saved to the local system
        /// </summary>
        LogImageNames = 12
    }

    public enum WinServiceHealthCheckFileTypesEnum
    {
        ProductImport,
        ProductImportSsis,
        CsvDownload,
        ExportFileMgmtCopy,
        ExportFileMgmtDelete,
        ImageImportSsisMgmt,
        DailyImageService,
        ManualImageService,
        ImageReviewService,
        ImageCsvFileCleanUp,
        ImageDeleteService
    }

}

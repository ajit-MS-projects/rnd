using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Affilinet.Business.ProductExport.Common
{
    public enum ExportfileTypesEnum
    {
        CSV,
        XML,
        CSVZip,
        XMLZip
    }

    /// <summary>
    /// Specific Application events, useful in logging
    /// </summary>
    public enum ApplicationEventsEnum
    {
        /// <summary>
        /// Identifies the CopyFileToExportServers process
        /// </summary>
        DeleteOldExportFiles = 1,
        CopyNewExportFiles = 2,
        StartUserDownload = 3,
        PrepareImageData = 4,
    }
    public enum PropertiesAttributesEnum
    {
        Title=0,
        TextOrNumber=1
    }
    public enum WinServiceHealthCheckFileTypesEnum
    {
        ProductImport,
        CsvDownload,
        ExportFileMgmtCopy,
        ExportFileMgmtDelete,
    }
}

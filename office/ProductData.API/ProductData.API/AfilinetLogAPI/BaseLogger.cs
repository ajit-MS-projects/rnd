using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Affilinet.Utility.Logging
{
    /// <summary>
    /// Abstract Base class for all logging implementations.
    /// Change History: Version;Date;Changed by;Change description
    /// 1.0;15-Feb-2009;Ajit Chahal;-
    /// </summary>
    public abstract class BaseLogger
    {
        /// <summary>
        /// Methods uses MS Enterprise libray's logger object to generate Log.
        /// </summary>
        /// <param name="objLoggingInfo">Object of Logging info to store various log parameters.</param>
        /// <param name="loggingCategory">One of the log categories defined in configuration file.</param>
        public abstract void CreateLog(LoggingInfo objLoggingInfo, LoggingCategoriesEnum loggingCategory);

        /// <summary>
        /// Creates the report log.
        /// </summary>
        /// <param name="ReportLogs">The report logs.</param>
        public abstract void CreateReportLog(List<ReportLog> ReportLogs);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solr.Utility.Logging.Common;
using Solr.Utility.Logging.Entity;

namespace Solr.Utility.Logging
{
    /// <summary>
    /// Abstract Base class for all logging implementations.
    /// Change History: Version;Date;Changed by;Change description
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
        public abstract void CreateDatabaseLog(List<DatabaseLog> ReportLogs);
    }
}

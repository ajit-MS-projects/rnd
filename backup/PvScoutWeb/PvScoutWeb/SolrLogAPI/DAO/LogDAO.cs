using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solr.Data.Access;
using System.Data;
using Solr.Utility.Logging.Common;

namespace Solr.Utility.Logging.DAO
{
    /// <summary>
    /// Represents data access object for Product abstraction.
    /// </summary>
    public class LogDAO : IDisposable
    {
        /// <summary>
        /// Refers to Live product DB
        /// </summary>
        private ISolrDatabase _loggingDBdatabase;

        private ISolrDatabase loggingDBdatabase
        {
            get
            {
                if (_loggingDBdatabase == null)
                    _loggingDBdatabase = new SolrGenericDataBase(DatabaseConnectionsEnum.Logging);
                return _loggingDBdatabase;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LogDAO"/> class.
        /// </summary>
        public LogDAO()
        {
            
        }

        /// <summary>
        /// Create a log entry for reporting.
        /// </summary>
        /// <param name="commit">if set to <c>true</c> [commit].</param>
        /// <param name="logText">Text to log</param>
        public void CreateDatabaseLog(bool commit, string logText, int LogCategory)
        {//Keep on adding commands and execute only once

            loggingDBdatabase.SetupMultipleCommands(Solr.Utility.Logging.Common.Constants.DmlStoredProcs.WriteDBLogEntry);
            loggingDBdatabase.AddInParameter("LogText", DbType.String, logText);
            loggingDBdatabase.AddInParameter("LogCategory", DbType.Int32, LogCategory);
            if(commit)
                loggingDBdatabase.ExecuteMultipleNonQuery(true);
        }
        #region Dispose

        private bool disposed;
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        public void Dispose(bool disposing)
        {
            if (!(this.disposed))
            {
                if (disposing)
                {
                    _loggingDBdatabase = null;
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// It also suppresses finalization
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);//This call is to makes sure that disposed object does not get put on the GC's finalize queue.
        }
        /// <summary>
        /// Finalizes this instance, if dispose is not called explicitly
        /// </summary>
        protected void Finalize()
        {
            Dispose(false);
        }
        #endregion

    }
}
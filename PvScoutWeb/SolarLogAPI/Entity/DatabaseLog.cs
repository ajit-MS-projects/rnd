using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solar.Utility.Logging.Entity
{
    /// <summary>
    /// Entiry class used to create create reporting logs, stored in DB
    /// </summary>
    public class DatabaseLog
    {
        /// <summary>
        /// Gets or sets the text to log
        /// </summary>
        /// <value>The id.</value>
        public string LogText { get; set; }
        /// <summary>
        /// Gets or sets the log category.
        /// </summary>
        /// <value>The application.</value>
        public int LogCategory { get; set; }

        public void Dispose()
        {
            LogText = null;
        }
    }
}

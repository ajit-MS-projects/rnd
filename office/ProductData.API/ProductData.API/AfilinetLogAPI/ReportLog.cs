using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Affilinet.Utility.Logging
{
    /// <summary>
    /// Entiry class used to create create reporting logs, stored in DB
    /// </summary>
    public class ReportLog
    {
        /// <summary>
        /// Gets or sets the unique id such as product program id.
        /// </summary>
        /// <value>The id.</value>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the System list id for multilingual support.
        /// </summary>
        /// <value>The list id.</value>
        public string SystemListId { get; set; }
        /// <summary>
        /// Gets or sets the value of property to log.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the enum value.
        /// </summary>
        /// <value>The application.</value>
        public int ListEnum { get; set; }

        public void Dispose()
        {
            Id = null;
            SystemListId = null;
            Value = null;
        }
    }
}

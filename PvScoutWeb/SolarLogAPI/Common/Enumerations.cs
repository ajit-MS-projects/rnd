using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solar.Utility.Logging.Common
{
    /// <summary>
    /// Enum designating log types.
    /// </summary>
    public enum LoggingCategoriesEnum
    {
        /// <summary>
        /// Debug log category
        /// </summary>
        Debug=1,
        /// <summary>
        /// Represents a custom exception raised in code.
        /// </summary>
        CustomException=2,
        /// <summary>
        /// Represents a system exception.
        /// </summary>
        SystemException=3,
        /// <summary>
        /// Information Log.
        /// </summary>
        Information=4,
        /// <summary>
        /// Warning Log.
        /// </summary>
        Warning=5,
         /// <summary>
        /// Log entries used to generate reports.
        /// </summary>
        Report=6,
        /// <summary>
        /// Log entries to be send as an email
        /// </summary>
        Email=7
    }
}

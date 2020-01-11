using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solar.Utility.Logging.Common
{
    /// <summary>
    ///  Class to contain all constants, categorised in inner structures.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Provides a list of log types (must be defined in configuration).
        /// </summary>
        public struct LogCategories
        {
            public const string Debug = "Debug";
            public const string CustomException = "CustomException";
            public const string SystemException = "SystemException";
            public const string Information = "Information";
            public const string Warning = "Warning";
            public const string Database = "Database";
            public const string Email = "Email";
        }
        public struct DmlStoredProcs
        {
            public const string WriteDBLogEntry = "WriteDBLogEntry"; 
        }
    }
}

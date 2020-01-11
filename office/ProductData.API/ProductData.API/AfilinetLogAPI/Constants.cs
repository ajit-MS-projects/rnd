using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Affilinet.Utility.Logging
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
            public const string Report = "Report";
            public const string Email = "Email";
        }
    }
}

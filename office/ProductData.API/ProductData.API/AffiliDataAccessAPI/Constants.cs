namespace Affilinet.Data.Access
{
    /// <summary>
    /// Class to contain all constants, categorised in inner structures.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Provides a list of connections in config file.
        /// </summary>
        public struct DBConnections
        {
            public const string ProductData = "ProductData";
            public const string LocalCache = "LocalCache";
            public const string LocalMachine = "LocalMachine";
            public const string Logging = "Logging";
            public const string ProductSSIS = "ProductSSIS";
            public const string LocalCacheSSIS = "LocalCacheSSIS";
            public const string ProductDataLiveReadOnly = "ProductDataLiveReadOnly";
            public const string Affilinet = "Affilinet";
            public const string AdminUser = "AdminUser";
        }
        /// <summary>
        /// Provides a list of stored procedures used to write log entries.
        /// </summary>
        public struct DmlStoredProcs
        {
            public const string WriteReportLogEntry = "WriteReportLogEntry";
        }
        /// <summary>
        /// Provides a list of sqls.
        /// </summary>
        public struct Sqls
        {
            public const string GetServerName = "SELECT @@SERVERNAME AS [Server]";
        }

        /// <summary>
        /// Provides a list of application configuration setting.
        /// </summary>
        public struct AppSettings
        {
            public const string IsDbServerNameDependencyEnabled = "IsDbServerNameDependencyEnabled";
            public const string DbServerName = "DbServerName";
        }

    }
}

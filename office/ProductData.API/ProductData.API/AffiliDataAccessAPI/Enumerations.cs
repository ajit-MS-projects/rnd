namespace Affilinet.Data.Access
{
    /// <summary>
    /// Enum that represents Affilinet DB connections.
    /// </summary>
    public enum DatabaseConnectionsEnum
    {
        /// <summary>
        /// Product data DB connection
        /// </summary>
        ProductData,
        /// <summary>
        /// A connection to Local DB for performance
        /// </summary>
        LocalCache,
        /// <summary>
        /// Default connection to local machine.
        /// </summary>
        LocalMachine,
        /// <summary>
        /// Connection to database where log entries will be stored.
        /// </summary>
        Logging,
        /// <summary>
        /// Refers to a read only connection to live DB, used to read program fields and settings
        /// </summary>
        ProductDataLiveReadOnly,
        /// <summary>
        /// Affilinet DB connection
        /// </summary>
        Affilinet,
        /// <summary>
        /// AdminUser DB used for affilinet admin and productAdmin UI authentication and authorisation
        /// </summary>
        AdminUser
    }
}

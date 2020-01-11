using System;

namespace Affilinet.ProductExport.Authentication.Common
{
    /// <summary>
    /// All constants for the export authentication
    /// </summary>
    public class Constants
    {

        /// <summary>
        /// Stored procedures that reads data from DB but do not make any DML changes
        /// </summary>
        public struct ReadOnlyStoredProcs
        {
            public const String fn_IsAuthorizedForShop = "SELECT dbo.fn_IsAuthorizedForShop(@ShopID,@PartnerID)";
            public const String GetListPermission = "dnGetListPermission";
            public const String VerifyPartnerPassword = "dnVerifyPartnerPassword";            
        }

        /// <summary>
        /// Common constants
        /// </summary>
        public struct Generic
        {
            public const String PASSWORD_PARAMETER = "auth";
            public const String MANUAL_DOWNLOAD_REFERRER = "productSearch.aspx";            
        }
    }
}

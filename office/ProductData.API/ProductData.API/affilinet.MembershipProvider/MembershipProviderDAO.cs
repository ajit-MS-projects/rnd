using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Affilinet.Data.Access;

namespace affilinet.MembershipProvider
{
    public class MembershipProviderDAO
    {
        private IAffiliDatabase productDBdatabase;
        private IAffiliDatabase adminUserDatabase;
        public MembershipProviderDAO()
        {
            productDBdatabase = new AffiliGenericDataBase(DatabaseConnectionsEnum.ProductData);
            adminUserDatabase = new AffiliGenericDataBase(DatabaseConnectionsEnum.AdminUser);
        }

        public bool ValidateUser(String userName, String password)
        {
            adminUserDatabase.SetupCommand(Constants.ReadOnlyStoredProcs.ValidateUser);
            adminUserDatabase.AddInParameter("UserName", DbType.String, userName);
            adminUserDatabase.AddInParameter("Password", DbType.String, password);
            object retVal = adminUserDatabase.ExecuteScalar();
            return retVal != null && retVal != DBNull.Value;
        }
        public DataTable GetRoles(String userName)
        {
            adminUserDatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetRoles);
            adminUserDatabase.AddInParameter("UserName", DbType.String, userName);

            return adminUserDatabase.ExecuteReaderProcessed();
        }
    }
}

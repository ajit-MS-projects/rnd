using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace affilinet.MembershipProvider
{
    public class Constants
    {
        public struct AppSettings
        {
            //public const string WindowsServices = "WindowsServices";
        }
        public struct ReadOnlyStoredProcs
        {
            public const string ValidateUser = "spValidateUser";
            public const string GetRoles = "spGetProductAdminRoles";
        }
        public struct DmlStoredProcs
        {
        }
    }
}

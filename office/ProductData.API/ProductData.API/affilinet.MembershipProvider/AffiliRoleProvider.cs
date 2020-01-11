using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace affilinet.MembershipProvider
{
    public class AffiliRoleProvider : System.Web.Security.RoleProvider
    {
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            //Initialize the abstract base class.
            base.Initialize(name, config);

            ApplicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        }
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
            {
                return defaultValue;
            }

            return configValue;
        }
        
        //select * FROM AdminUsers au
        //INNER JOIN AdminUsersGroups aug ON au.auid = aug.AUGadminUSERID AND au.auid=234
        //INNER JOIN AdminGroups ag ON ag.agid=aug.AUGAdminGroupID
        //[PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
        public override string[] GetRolesForUser(string username)
        {
            List<string> roles = new List<string>();
            MembershipProviderDAO  objMembershipProviderDAO = new MembershipProviderDAO();
            DataTable dtRoles = objMembershipProviderDAO.GetRoles(username);
            foreach (DataRow dr in dtRoles.Rows)
                roles.Add(dr["RightName"].ToString());
            return roles.ToArray();
        }
        public override string ApplicationName
        {
            get;
            set;
        }

        #region Not Implemented
        public override bool IsUserInRole(string username, string roleName)
        {
            throw new System.NotImplementedException();
        }
        public override void CreateRole(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new System.NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new System.NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new System.NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new System.NotImplementedException();
        }

       
        #endregion
    }
}

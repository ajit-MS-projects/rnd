using System;
using Affilinet.Utility.Logging;
using System.Configuration;
using Affilinet.Utility.Logging;
using System.Security.Cryptography;
using System.Text;

namespace PIMaintenanceTasks
{
    /// <summary>
    /// Contains common utility methods
    /// </summary>
    public class MUtilities
    {
        private static string dataSource = string.Empty;
        
        #region Configuration
        /// <summary>
        /// Gets the app setting value.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns></returns>
        public static string GetAppSettingValue(string settingName)
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get(settingName);
        }

        /// <summary>
        /// Gets the connection string from configuration file.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public static string GetConnectionString(string connectionName)
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        /// <summary>
        /// Gets the data source as Sql server where localCache db is installed
        /// To avoid multiple localtion of similar info, this value is picked up from LocalCahe connection string.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public static string GetDataSource(string connectionName)
        {
            if (string.IsNullOrEmpty(dataSource))
            {
                string conStr = GetConnectionString(connectionName);
                if (!string.IsNullOrEmpty(conStr))
                {
                    string[] strArr = conStr.Split(';');
                    foreach (string strTmp in strArr)
                    {
                        if (strTmp.ToUpper().IndexOf("SERVER") > -1)
                            dataSource = strTmp.Split('=')[1];
                    }
                }
            }
            return dataSource;
        }

        /// <summary>
        /// Gets the MD5 hash.
        /// </summary>
        /// <param name="strInput">The String to be used as hash input.</param>
        /// <returns></returns>
        public static string GetMd5Hash(string strInput)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();

            Byte[] _secretBytes = encoder.GetBytes(strInput);
            MD5 md5 = MD5.Create();
            Byte[] computedHash = md5.ComputeHash(_secretBytes);
            string mystring = encoder.GetString(computedHash).Replace('\'', '´');//Replace sigle quotes
            mystring = mystring.Replace((char)0, '+');//Replace null
            mystring = mystring.Replace("~", "ú");
            return mystring;


        }


        #endregion
    }
}

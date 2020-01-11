using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Affilinet.Business.ImageImport.Common
{
    public class Utilities
    {
        /// <summary>
        /// Delete one ore more files in one folder.
        /// </summary>
        /// <param name="fileName">Name of the file. Also possible to give an * expression inside the fileName</param>        
        /// <param name="filePath">Path of the file.</param>        
        internal static void DeleteFiles(string fileName, string filePath)
        {
            DirectoryInfo sourceDir = new DirectoryInfo(filePath);
            if (!sourceDir.Exists) return;
            foreach (FileInfo file in sourceDir.GetFiles(fileName))
            {
                file.Delete();
            }

        }

        /// <summary>
        /// Delete a file in one folder
        /// </summary>
        /// <param name="fileName">Name of the source file</param>
        /// <param name="filePath">The path of the file</param>
        internal static void DeleteFile(string fileName, string filePath)
        {
            string sourceFileUri = Path.Combine(filePath, fileName);
            if (File.Exists(sourceFileUri))
            {
                File.Delete(sourceFileUri);
            }
        }

        /// <summary>
        /// Delete a file in one folder
        /// </summary>
        /// <param name="fileUri">The complete path inclusive the filename</param>
        internal static void DeleteFile(string fileUri)
        {
            if (File.Exists(fileUri))
            {
                File.Delete(fileUri);
            }
        }

        /// <summary>
        /// Gets the MD5 hash.
        /// </summary>
        /// <param name="strInput">The String to be used as hash input.</param>
        /// <returns></returns>
        public static string GetMd5HashBytes(string strInput)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();

            Byte[] _secretBytes = encoder.GetBytes(strInput);
            MD5 md5 = MD5.Create();
            Byte[] computedHash = md5.ComputeHash(_secretBytes);
            string mystring = "";
            foreach (byte b in computedHash)
            {
                String tmp = b.ToString("x");
                if (tmp.Length == 1) tmp = "0" + tmp;
                mystring += tmp;
            }
            return mystring;
        }
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
        /// Reloads the appSettings config section from application configuration file
        /// </summary>
        public static void ReloadConfigSection()
        {
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
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
        /// Gets the file extensions.
        /// </summary>
        /// <returns></returns>
        public static string[] GetFileExtensionsForSsisImageImport()
        {
            return new string[]{
                                      Constants.FileNaming.ImageBrokenCsvExtention,
                                      Constants.FileNaming.ImageUpdateCsvExtention,
                                      Constants.FileNaming.ImageDeleteCsvExtention
                                  };
        }

        public static bool IsServiceScheduled(String startTimeAppSetting, String endTimeAppSetting)
        {
            ReloadConfigSection();
            DateTime impStartTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + GetAppSettingValue(startTimeAppSetting));
            //DateTime impEndHour = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + GetAppSettingValue(endTimeAppSetting));
            double hours = double.Parse(GetAppSettingValue(endTimeAppSetting));
            DateTime impEndHour = impStartTime.AddMinutes(hours * 60d);

            return (DateTime.Now.CompareTo(impStartTime) >= 0 && DateTime.Now.CompareTo(impEndHour) <= 0);
        }
    }
}

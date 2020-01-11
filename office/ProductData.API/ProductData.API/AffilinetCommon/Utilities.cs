using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using Affilinet.Exceptions;
using Affilinet.Utility.Logging;

namespace Affili.ProductData.Common
{
    public static class Utilities
    {
        private static string _dataSource = string.Empty;
        private static string _dataSourceDbName = string.Empty;
        private const string StartString = "Start: ";
        private const string EndString = "End: ";

        /// <summary>
        /// Method logs a debug log designating start of an operation
        /// </summary>
        /// <param name="message"></param>
        /// <param name="eventId"></param>
        public static void CreateStartLog(String message, ApplicationEventsEnum eventId)
        {
            CreateLog(StartString + message, eventId, LoggingCategoriesEnum.Debug);
        }
        /// <summary>
        /// Method logs a debug log designating end of an operation
        /// </summary>
        /// <param name="message"></param>
        /// <param name="eventId"></param>
        public static void CreateEndLog(String message, ApplicationEventsEnum eventId)
        {
            CreateLog(EndString + message, eventId, LoggingCategoriesEnum.Debug);
        }
        /// <summary>
        /// Method logs an information log 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="eventId"></param>
        public static void CreateInfoLog(String message, ApplicationEventsEnum eventId)
        {
            CreateLog(message, eventId, LoggingCategoriesEnum.Information);
        }
        /// <summary>
        /// Method sends an email of information log 
        /// </summary>
        /// <param name="message"></param>
        public static void CreateEmailLog(String message)
        {
            CreateLog(message, ApplicationEventsEnum.DocProcessing, LoggingCategoriesEnum.Email);
        }
        /// <summary>
        /// Method logs an debug log 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="eventId"></param>
        public static void CreateDebugLog(String message, ApplicationEventsEnum eventId)
        {
            CreateLog(message, eventId, LoggingCategoriesEnum.Debug);
        }
        /// <summary>
        /// Method logs an warning log 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="eventId"></param>
        public static void CreateWarningLog(String message, ApplicationEventsEnum eventId)
        {
            CreateLog(message, eventId, LoggingCategoriesEnum.Warning);
        }
        /// <summary>
        /// Method instantiate affili log objects and sends log requests
        /// </summary>
        /// <param name="message"></param>
        /// <param name="eventId"></param>
        /// <param name="logCat"></param>
        private static void CreateLog(String message, ApplicationEventsEnum eventId, LoggingCategoriesEnum logCat)
        {
            BaseLogger objLogger = new GenericLogger();
            LoggingInfo objLogInfo = new LoggingInfo();
            objLogInfo.Message = message;
            //objLogInfo.ExceptionObject = objExcep;
            objLogInfo.EventId = (int)eventId;
            objLogger.CreateLog(objLogInfo, logCat);
        }

        /// <summary>
        /// Creates the report log.
        /// </summary>
        /// <param name="log">The log.</param>
        public static void CreateReportLog(List<ReportLog> log)
        {
            BaseLogger objLogger = new GenericLogger();
            objLogger.CreateReportLog(log);
        }

        /// <summary>
        /// Gets the app setting value.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns></returns>
        public static string GetAppSettingValue(string settingName)
        {
            return ConfigurationManager.AppSettings.Get(settingName);
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
            if (string.IsNullOrEmpty(_dataSource))
            {
                string conStr = GetConnectionString(connectionName);
                if (!string.IsNullOrEmpty(conStr))
                {
                    string[] strArr = conStr.Split(';');
                    foreach (string strTmp in strArr)
                    {
                        if (strTmp.ToUpper().IndexOf("SERVER") > -1 || strTmp.ToUpper().IndexOf("DATA SOURCE") > -1)
                            _dataSource = strTmp.Split('=')[1];
                    }
                }
            }
            return _dataSource;
        } 
        /// <summary>
        /// Gets the data source as Sql server where localCache db is installed
        /// To avoid multiple localtion of similar info, this value is picked up from LocalCahe connection string.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public static string GetDataSourceDbName(string connectionName)
        {
            if (string.IsNullOrEmpty(_dataSourceDbName))
            {
                string conStr = GetConnectionString(connectionName);
                if (!string.IsNullOrEmpty(conStr))
                {
                    string[] strArr = conStr.Split(';');
                    foreach (string strTmp in strArr)
                    {
                        if (strTmp.ToUpper().IndexOf("DATABASE") > -1)
                            _dataSourceDbName = strTmp.Split('=')[1];
                    }
                }
            }
            return _dataSourceDbName;
        }
        /// <summary>
        /// Reloads the appSettings config section from application configuration file
        /// </summary>
        public static void ReloadConfigSection()
        {
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// Delete a file
        /// </summary>
        /// <param name="filePath">Path with file.</param>
        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// Delete one ore more files in one folder.
        /// </summary>
        /// <param name="fileName">Name of the file. Also possible to give an * expression inside the fileName</param>        
        /// <param name="filePath">Path of the file.</param>        
        public static void DeleteFiles(string fileName, string filePath)
        {
            DirectoryInfo sourceDir = new DirectoryInfo(filePath);
            if (!sourceDir.Exists) return;
            foreach (FileInfo file in sourceDir.GetFiles(fileName))
            {
                file.Delete();
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
        /// Add zero in front of ProdProgId so the length of ProdProgId become 6. E.g.: 000123
        /// </summary>
        /// <param name="prodProgId"></param>
        /// <returns></returns>
        public static string PadProdProgId(this string prodProgId)
        {
            return (prodProgId.Length < 6) ? prodProgId.PadLeft(6, '0') : prodProgId;
        }

        /// <summary>
        /// Opens the file for write and writes datetime stamp.
        /// </summary>
        public static void WriteHealthCheckTimeStamp(WinServiceHealthCheckFileTypesEnum winSerHealthCheckFileType)
        {
            StreamWriter swFile = null;
            try
            {
                String fileFullPath = GetAppSettingValue("WinServiceHeathCheckFilePath");
                String fileName = "";
                switch (winSerHealthCheckFileType)
                {
                    case WinServiceHealthCheckFileTypesEnum.ProductImport:
                        fileName = "ProductImport.WinServiceHeathCheck.txt";
                        break;
                    case WinServiceHealthCheckFileTypesEnum.ProductImportSsis:
                        fileName = "ProductImportSsis.WinServiceHeathCheck.txt";
                        break;
                    case WinServiceHealthCheckFileTypesEnum.CsvDownload:
                        fileName = "CsvDownload.WinServiceHeathCheck.txt";
                        break;
                    case WinServiceHealthCheckFileTypesEnum.ExportFileMgmtCopy:
                        fileName = "ExportFileMgmt.Copy.WinServiceHeathCheck.txt";
                        break;
                    case WinServiceHealthCheckFileTypesEnum.ExportFileMgmtDelete:
                        fileName = "ExportFileMgmt.Delete.WinServiceHeathCheck.txt";
                        break;
                    case WinServiceHealthCheckFileTypesEnum.ImageImportSsisMgmt:
                        fileName = "ImageImportSsisMgmt.WinServiceHealthCheck.txt";
                        break;
                    case WinServiceHealthCheckFileTypesEnum.DailyImageService:
                        fileName = "DailyImageService.WinServiceHeathCheck.txt";
                        break;
                    case WinServiceHealthCheckFileTypesEnum.ManualImageService:
                        fileName = "ManualImageService.WinServiceHeathCheck.txt";
                        break;
                    case WinServiceHealthCheckFileTypesEnum.ImageReviewService:
                        fileName = "ImageReviewService.WinServiceHealthCheck.txt";
                        break;
                    case WinServiceHealthCheckFileTypesEnum.ImageCsvFileCleanUp:
                        fileName = "ImageCsvFileCleanUp.WinServiceHealthCheck.txt";
                        break;
                    case WinServiceHealthCheckFileTypesEnum.ImageDeleteService:
                        fileName = "ImageDelete.WinServiceHealthCheck.txt";
                        break;
                }
                swFile = new StreamWriter(fileFullPath + fileName, false, Encoding.Unicode);
                swFile.Write(DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                new AffiliGenericException("Error in Utilities.WriteHealthCheckTimeStamp()", ex).CreateLog();
            }
            finally
            {
                if (swFile != null)
                {
                    swFile.Close();
                    swFile.Dispose();
                }
            }
        }

        private static String ImpStartTime ="";
        public static bool IsServiceScheduled(String startTimeAppSetting, String endTimeAppSetting)
        {
            ReloadConfigSection();
            DateTime impStartTime;
            double hours = 0;
            if (ImpStartTime == "")
            {
                impStartTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + GetAppSettingValue(startTimeAppSetting));
                //DateTime impEndHour = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + GetAppSettingValue(endTimeAppSetting));
            }
            else
            {
                impStartTime = DateTime.Parse(ImpStartTime);
            }
            hours = double.Parse(GetAppSettingValue(endTimeAppSetting));
            DateTime impEndHour = impStartTime.AddMinutes(hours*60d);
            if (DateTime.Now.CompareTo(impStartTime) >= 0 && DateTime.Now.CompareTo(impEndHour) <= 0)
            {
                ImpStartTime = impStartTime.ToString();
                return true;
            }
            else
            {
                ImpStartTime = "";
                return false;
            }
        }
        /// <summary>
        /// Checks for null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defValue">The def value.</param>
        /// <returns></returns>
        public static object CheckForNull(object value, object defValue)
        {
            if (value == DBNull.Value || value == null)
                return defValue;
            else
                return value;
        }
        /// <summary>
        /// Gets the M d5 hash from file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }


        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="content">The content.</param>
        public static void SendMail(String from, String to, String subject, String content)
        {
            SendMail(from, to, null, subject, content);
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="content">The content.</param>
        public static void SendMail(String from, String to, String cc, String subject, String content)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(from);

            message.To.Add(new MailAddress(to));

            if(!String.IsNullOrEmpty(cc))
                message.CC.Add(new MailAddress(cc));
            message.Subject = subject;
            message.Body = content;

            SmtpClient client = new SmtpClient();
            client.Send(message);

        }
        /// <summary>
        /// Moves all files to backup folder.
        /// </summary>
        /// <param name="path">The path.</param>
        public static void MoveAllFilesToBackupFolder(string sourcePath)
        {
            if (Directory.Exists(sourcePath))
            {
                String backipDir = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + "_Backup";
                Directory.CreateDirectory(sourcePath + @"\" + backipDir);
                String[] files = Directory.GetFiles(sourcePath);
                foreach (String file in files)
                {
                    File.Move(file, sourcePath + @"\" + backipDir + @"\" + Path.GetFileName(file));
                }
            }
        }
        /// <summary>
        /// Moves all file to backup folder.
        /// </summary>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="backupExtension">The backup extension.</param>
        public static void BackupFile(String sourceFilePath,String backupExtension)
        {
            if (File.Exists(sourceFilePath))
            {
                File.Copy(sourceFilePath, sourceFilePath + backupExtension,true);
            }
        }
        /// <summary>
        /// Moves all file to backup folder.
        /// </summary>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="backupExtension">The backup extension.</param>
        public static void RestoreFile(String sourceFilePath, String backupExtension, String restoreExtension)
        {
            if (File.Exists(sourceFilePath))
            {
                File.Copy(sourceFilePath, sourceFilePath.Replace(restoreExtension,"").Replace(backupExtension, restoreExtension),true);
                File.Delete(sourceFilePath);
            }
        }
        #region file Compare
        // This method accepts two strings the represent two files to 
        // compare. A return value of 0 indicates that the contents of the files
        // are the same. A return value of any other value indicates that the 
        // files are not the same.
        public static bool FileCompare(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            // Open the two files.
            fs1 = new FileStream(file1, FileMode.Open);
            fs2 = new FileStream(file2, FileMode.Open);

            // Check the file sizes. If they are not the same, the files 
            // are not the same.
            if (fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Read and compare a byte from each file until either a
            // non-matching set of bytes is found or until the end of
            // file1 is reached.
            do
            {
                // Read one byte from each file.
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is 
            // equal to "file2byte" at this point only if the files are 
            // the same.
            return ((file1byte - file2byte) == 0);
        }
        #endregion
    }
}

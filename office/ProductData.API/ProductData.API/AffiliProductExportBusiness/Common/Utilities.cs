﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using Affilinet.Exceptions;
using Affilinet.Utility.Logging;
using System.Configuration;
using Affilinet.Utility.Logging;
using System.Security.Cryptography;
using System.Text;
using Constants = Affilinet.Business.ProductExport.Common.Constants;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Net;

namespace Affilinet.Business.ProductExport.Common
{
    /// <summary>
    /// Contains common utility methods
    /// </summary>
    public class Utilities
    {
        #region PInvoke
        // [DllImportAttribute("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet =
        // CharSet.Ansi, SetLastError = true)]
        // private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int
        // maximumWorkingSetSize);
        // [DllImportAttribute("kernel32.dll", EntryPoint = "SetProcessWorkingSetSizeEx", ExactSpelling = true, CharSet =
        // CharSet.Ansi, SetLastError = true)]
        // private static extern int SetProcessWorkingSetSizeEx(IntPtr process, int minimumWorkingSetSize, int
        //maximumWorkingSetSize, int Flags);
        #endregion

        private static string dataSource = string.Empty;
        #region Logging
        private const string START_STRING = "Start: ";
        private const string END_STRING = "End: ";
        /// <summary>
        /// Method logs a debug log designating start of an operation
        /// </summary>
        /// <param name="message"></param>
        /// <param name="eventId"></param>
        public static void CreateStartLog(String message, ApplicationEventsEnum eventId)
        {
            CreateLog(START_STRING + message, eventId, LoggingCategoriesEnum.Debug);
        }
        /// <summary>
        /// Method logs a debug log designating end of an operation
        /// </summary>
        /// <param name="message"></param>
        /// <param name="eventId"></param>
        public static void CreateEndLog(String message, ApplicationEventsEnum eventId)
        {
            CreateLog(END_STRING + message, eventId, LoggingCategoriesEnum.Debug);
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

        #endregion

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
                        if (strTmp.ToUpper().IndexOf("SERVER") > -1 || strTmp.ToUpper().IndexOf("DATA SOURCE") > -1)
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
        //public static string GetMd5Hash(string strInput)
        //{
        //    ASCIIEncoding encoder = new ASCIIEncoding();

        //    Byte[] _secretBytes = encoder.GetBytes(strInput);
        //    MD5 md5 = MD5.Create();
        //    Byte[] computedHash = md5.ComputeHash(_secretBytes);
        //    string mystring = encoder.GetString(computedHash).Replace('\'', '´');//Replace single quotes
        //    mystring = mystring.Replace((char)0, '+');//Replace null
        //    mystring = mystring.Replace(Constants.Generic.DestFieldQualifier, "ú");
        //    return mystring;
        //}

        #endregion

        //public static string IncrementDate(DateTime objDateToIncr, string intervalType, int incrValue)
        //{
        //    //if ((objDateToIncr.Hour < 1) || (objDateToIncr.Hour > 9))
        //    //{
        //    //    //Keeping next update time in Product DB update window
        //    //    objDateToIncr = objDateToIncr.AddHours(1 - objDateToIncr.Hour);
        //    //}
        //    while (objDateToIncr < DateTime.Now.AddHours(12))
        //    {
        //        switch (intervalType)
        //        {
        //            case "H":
        //                objDateToIncr = objDateToIncr.AddHours(incrValue);
        //                break;
        //            case "W":
        //                objDateToIncr = objDateToIncr.AddDays(incrValue * 7);
        //                break;
        //            case "M":
        //                objDateToIncr = objDateToIncr.AddMonths(incrValue);
        //                break;
        //            default:
        //                objDateToIncr = objDateToIncr.AddDays(incrValue);
        //                break;
        //        }
        //    }

        //    return objDateToIncr.ToString(Utilities.GetAppSettingValue(Constants.AppSettings.DateTimeFormat));
        //}

        /// <summary>
        /// Moves the file.
        /// </summary>
        /// <param name="fileDestination">The file destination.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="moveTo">The move to.</param>
        //public static void MoveFile(string fileDestination, string fileName, FileLocationsEnum moveTo)
        //{
        //    string destination = GetDestinationFolder(moveTo);
        //    destination = fileDestination + destination + @"\";
        //    if (!Directory.Exists(destination))
        //        Directory.CreateDirectory(destination);
        //    if (File.Exists(fileDestination + fileName))
        //        File.Move(fileDestination + fileName, destination + DateTime.Now.ToString("yyyyMMdd_HHmmss_") + fileName);
        //}

        /// <summary>
        /// Gets the destination folder.
        /// </summary>
        /// <param name="moveTo">The move to.</param>
        /// <returns></returns>
        //private static string GetDestinationFolder(FileLocationsEnum moveTo)
        //{
        //    string destination = string.Empty;
        //    switch (moveTo)
        //    {
        //        case FileLocationsEnum.Archive:
        //            destination = Utilities.GetAppSettingValue(Constants.AppSettings.ArchiveFolder);
        //            break;
        //        case FileLocationsEnum.Imported:
        //            destination = Utilities.GetAppSettingValue(Constants.AppSettings.ImportedFilesFolder);
        //            break;
        //        case FileLocationsEnum.NotImported:
        //            destination = Utilities.GetAppSettingValue(Constants.AppSettings.NotImportedFilesFolder);
        //            break;
        //    }
        //    return destination;
        //}

        /// <summary>
        /// Moves the file.
        /// </summary>
        /// <param name="csvURL">The CSV URL.</param>
        /// <param name="fileDestination">The file destination.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="moveTo">The move to.</param>
        /// <param name="fileNum">The file num.</param>
        /// <param name="fileExtension">The file extension.</param>
        //public static void MoveFile(string csvURL, string fileDestination, string fileName, FileLocationsEnum moveTo, int fileNum, string fileExtension)
        //{
        //    //move processed CSVs
        //    string destination = GetDestinationFolder(moveTo);
        //    destination = (fileDestination + destination + @"\").Replace("/", @"\");
        //    if (!Directory.Exists(destination))
        //        Directory.CreateDirectory(destination);
        //    if (File.Exists(csvURL))
        //        File.Move(csvURL,
        //                  destination + DateTime.Now.ToString("yyyyMMdd_HHmmss_") + fileName + "." + fileNum.ToString() +
        //                  fileExtension);
        //}
        /// <summary>
        /// Gets the file extensions.
        /// </summary>
        /// <returns></returns>
        public static string[] GetFileExtensions()
        {
            return new string[]
                                                      {
                                                          Constants.FileNaming.ProductNotchangedCsvExtention,
                                                          Constants.FileNaming.ProductUpdateCsvExtention,
                                                          Constants.FileNaming.ProductInsertCsvExtention
                                                      };
        }

        /// <summary>
        /// Gets the file extensions.
        /// </summary>
        /// <returns></returns>
        //public static string[] GetFileExtensionsForExport()
        //{
        //    return new string[]
        //                                              {
        //                                                  Constants.FileNaming.ProductNotchangedCsvExtention,
        //                                                  Constants.FileNaming.ProductUpdateCsvExtention,
        //                                                  Constants.FileNaming.ProductInsertCsvExtention
        //                                              };
        //}

        /// <summary>
        /// Copy the file to 1 or more folders
        /// </summary>
        /// <param name="sourceFileURI">The source file URI.</param>
        /// <param name="fileDestination">The file destination.</param>
        /// <param name="destFolder">The dest folder.</param>
        /// <param name="fileName">Name of the file.</param>
        public static void CopyFileToFolders(string sourceFileURI, string[] fileDestination, string destFolder, string fileName)
        {
            string destinationFileURI = "";
            foreach (string fdest in fileDestination)
            {
                destinationFileURI = Path.Combine(fdest, destFolder);
                if (!Directory.Exists(destinationFileURI))
                    Directory.CreateDirectory(destinationFileURI);
                destinationFileURI = Path.Combine(destinationFileURI, fileName);
                if (File.Exists(sourceFileURI))
                    File.Copy(sourceFileURI, destinationFileURI);
            }
        }

        /// <summary>
        /// Copy one file to one folder.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destPath">The destination path.</param>        
        /// <param name="fileName">Name of the file. Also possible to give an * expression inside the fileName</param>
        /// <param name="overrideExistingFile">Override if the file already exists?</param>
        public static void CopyFileToFolder(string sourcePath, string destPath, string fileName, bool overrideExistingFile)
        {
            if (!Directory.Exists(destPath))
                Directory.CreateDirectory(destPath);

            DirectoryInfo sourceDir = new DirectoryInfo(sourcePath);
            FileInfo[] filesToCopy = sourceDir.GetFiles(fileName);
            // check if minimum one file exists
            if (filesToCopy.GetLength(0) == 0)
                throw new FileNotFoundException();

            foreach (FileInfo file in filesToCopy)
            {
                file.CopyTo(Path.Combine(destPath, file.Name), overrideExistingFile);
            } 
        }

        /// <summary>
        /// Delete a file in one or more folders
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="filePath">Array of source paths.</param>
        /// <returns>True, if the file was delete successfully or not exists. Only return false if an error occure during deletion.</returns>
        internal static bool DeleteFile(string sourceFile, string[] filePath)
        {
            bool deleteSuccessfully = false;
        
            string sourceFileURI = "";
            foreach (string fdest in filePath)
            {
                sourceFileURI = Path.Combine(fdest, sourceFile);
                if (File.Exists(sourceFileURI))
                {
                    File.Delete(sourceFileURI);
                    deleteSuccessfully = true;
                }
                else
                {
                    deleteSuccessfully = true;
                }
            }
            
            

            return deleteSuccessfully;            
        }

        /// <summary>
        /// Delete one ore more files in one folder.
        /// </summary>
        /// <param name="fileName">Name of the file. Also possible to give an * expression inside the fileName</param>        
        /// <param name="filePath">Path of the file.</param>        
        internal static void DeleteFiles(string fileName, string filePath)
        {            
            DirectoryInfo sourceDir = new DirectoryInfo(filePath);
            foreach (FileInfo file in sourceDir.GetFiles(fileName))
            {
                file.Delete();
            } 
         
        }          
        

        /// <summary>
        /// Frees the memory.
        /// </summary>
        //public static void FreeMemory()
        //{
        //    try
        //    {
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        //        {
        //            //SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
        //            //SetProcessWorkingSetSizeEx(System.Diagnostics.Process.GetCurrentProcess().Handle, 20, 500,8);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CreateWarningLog("Error in Utilities.FreeMemory():" + ex.Message, ApplicationEventsEnum.DocProcessing);
        //    }

        //}
        /// <summary>
        /// method for validating a url with regular expressions
        /// </summary>
        /// <param name="url">url we're validating</param>
        /// <returns>true if valid, otherwise false</returns>
        public static bool IsValidUrl(string url)
        {
            string pattern = Constants.RegExp.UrlValidator;
            Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(url);
        }
        public static String GetOutputFilePath(HttpContext context,String fileExtension)
        {
            String retVal = context.Request.FilePath.Replace(context.Request.ApplicationPath, "");
            if(!String.IsNullOrEmpty(fileExtension))
                retVal = retVal.ToLower().Replace(fileExtension.ToLower(), "");
            return retVal;
        }
        public static String RemoveZipExtensions(String outputFile)
        {
            Regex regex = new Regex("(" + Constants.ExportFileExtensions.Zip + " | " + Constants.ExportFileExtensions.GZip + ")", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            outputFile = regex.Replace(outputFile, "");
            regex = new Regex("(" + Constants.ExportFileExtensions.Zip.ToUpper() + " | " + Constants.ExportFileExtensions.GZip.ToUpper() + ")", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            outputFile = regex.Replace(outputFile, "");
            return outputFile;
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
                    case WinServiceHealthCheckFileTypesEnum.CsvDownload:
                        fileName = "CsvDownload.WinServiceHeathCheck.txt";
                        break;
                    case WinServiceHealthCheckFileTypesEnum.ExportFileMgmtCopy:
                        fileName = "ExportFileMgmt.Copy.WinServiceHeathCheck.txt";
                        break;
                    case WinServiceHealthCheckFileTypesEnum.ExportFileMgmtDelete:
                        fileName = "ExportFileMgmt.Delete.WinServiceHeathCheck.txt";
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
    }
}
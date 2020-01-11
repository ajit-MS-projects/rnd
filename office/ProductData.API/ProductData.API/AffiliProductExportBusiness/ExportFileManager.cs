using System;
using System.Data;
using Affilinet.Business.ProductExport.DAO;
using Affilinet.Exceptions;
using Affilinet.Business.ProductExport.Common;
using System.IO;

namespace Affilinet.Business.ProductExport
{
    public class ExportFileManager
    {
        private readonly ExportCleanUpDAO ExportCleanUpDao;
        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportFileManager"/> class.
        /// </summary>
        public ExportFileManager()
        {
            ExportCleanUpDao = new ExportCleanUpDAO();
        }
        #endregion

        
        #region methods
        
        /// <summary>
        /// Initiates the CleanUp job which delete old ExportFiles from filesystem
        /// </summary>
        public void CleanUpProdExportProgramFiles()
        {
            try
            {
                Utilities.WriteHealthCheckTimeStamp(WinServiceHealthCheckFileTypesEnum.ExportFileMgmtDelete);
                // delete only between 1:00AM and 3:00AM(or check config for the actual timeframe) in this time the sanitisation is stopped for SSIS Import
                Utilities.ReloadConfigSection();
                DateTime delStartTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " +
                                       Utilities.GetAppSettingValue(Constants.AppSettings.ExportCleanUpStartTime));
                DateTime delEndTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " +
                                       Utilities.GetAppSettingValue(Constants.AppSettings.ExportCleanUpEndTime));
                
                if ((DateTime.Now.CompareTo(delStartTime) >= 0 && DateTime.Now.CompareTo(delEndTime) <= 0))
                    StartCleanUp();

            }
            catch (Exception ex)
            {
                new AffiliGenericException("Error in ExportFileManager.CleanUpProdExportProgramFiles()", ex).CreateLog();
            }            
        }

        /// <summary>
        /// Clean up all old export program files.
        /// </summary>
        private void StartCleanUp()
        {
            Utilities.CreateInfoLog("ExportFileManager.StartCleanUp() Start StartCleanUp()", ApplicationEventsEnum.DeleteOldExportFiles);
            try
            {
                string[] delServers = GetServerForDeletion();
                if (delServers.Length == 0)
                {
                    Utilities.CreateWarningLog("ExportFileManager.StartCleanUp() GetExportServers: No Path found in config.", ApplicationEventsEnum.DeleteOldExportFiles);
                    return;
                }
                
                DataTable dtExpFileForDelete = ExportCleanUpDao.GetExportFilesForDelete();
                string exportTimeStamp = "";
                string prodProgId = "";

                if (dtExpFileForDelete != null && dtExpFileForDelete.Rows.Count > 0)
                {
                    Utilities.CreateInfoLog("ExportFileManager.StartCleanUp() exportCleanUpDAO.GetExportFilesForDelete() return " + dtExpFileForDelete.Rows.Count + " rows.", ApplicationEventsEnum.DeleteOldExportFiles);

                    foreach (DataRow fileForDeleteRow in dtExpFileForDelete.Rows)
                    {
                        try
                        {
                            // get the ExportTimeStamp
                            if (fileForDeleteRow[Constants.ProdExportProgramFilesCache.ExportTimeStamp] != DBNull.Value)
                                exportTimeStamp = fileForDeleteRow[Constants.ProdExportProgramFilesCache.ExportTimeStamp].ToString();

                            // get the ProdProgId
                            if (fileForDeleteRow[Constants.ProdExportProgramFilesCache.ProdProgId] != DBNull.Value)
                                prodProgId = fileForDeleteRow[Constants.ProdExportProgramFilesCache.ProdProgId].ToString();

                            string sourceFile = exportTimeStamp + "_" + prodProgId + "*.csv";
                            foreach (string delServer in delServers)
                            {
                                string sourcePath = Path.Combine(delServer, prodProgId);
                                if (Directory.Exists(sourcePath))
                                {
                                    Utilities.DeleteFiles(sourceFile, sourcePath);
                                }
                                else
                                {
                                    new AffiliGenericException("ExportFileManager.StartCleanUp() Could not reach path: " + sourcePath).CreateLog();
                                }
                            }
                            // delete from DB
                            ExportCleanUpDao.DeleteExpFileFromDB(prodProgId, exportTimeStamp);
                        }                            
                        catch (AffiliGenericException ex)
                        {
                            ex.CreateLog();
                        }
                        catch (Exception ex)
                        {
                            new AffiliGenericException("ExportFileManager.StartCleanUp(). Error on delete export files. Error details:", ex).CreateLog();
                        }
                    }                    
                }
                else
                {
                    Utilities.CreateInfoLog("ExportFileManager.StartCleanUp() exportCleanUpDAO.GetExportFilesForDelete() Datatable dtExpFileForDelete is NULL or contain 0 rows.", ApplicationEventsEnum.DeleteOldExportFiles);
                }
            }
            catch (Exception ex)
            {
                new AffiliGenericException("ExportFileManager.StartCleanUp(). Error details:", ex).CreateLog();
            }
        }



        /// <summary>
        /// Copy all files for the product export process from local folder to the Live server(s)
        /// </summary>
        public void CopyProdExportProgramFiles()
        {
            Utilities.CreateInfoLog("ExportFileManager.CopyProdExportProgramFiles() Start CopyProdExportProgramFiles()", ApplicationEventsEnum.CopyNewExportFiles);
            try
            {
                Utilities.WriteHealthCheckTimeStamp(WinServiceHealthCheckFileTypesEnum.ExportFileMgmtCopy);
                Utilities.ReloadConfigSection();

                // Get Settings from config                
                string[] exportServers;
                string sourceServer;
                int maxAttempts = 10;
                int waitForCopyExpFiles = 60;

                GetConfigSettings(ref maxAttempts, ref waitForCopyExpFiles, out exportServers, out sourceServer);
                
                // get values from DB                
                string exportTimeStamp = "";
                string prodProgId = "";
                int counterAttempts = 0;
                DateTime lastModified=DateTime.MinValue;

                if (!GetExportFileForCopy(ExportCleanUpDao, ref exportTimeStamp, ref prodProgId, ref counterAttempts, ref lastModified, maxAttempts, waitForCopyExpFiles))
                {
                    Utilities.CreateInfoLog("ExportFileManager.CopyProdExportProgramFiles() GetExportFileForCopy is false. No files found to copy. EXIT. \n Paramters: \n exportTimeStamp: " + exportTimeStamp + " \n prodProgId: " + prodProgId + " \n counterAttempts: " + counterAttempts + " \n lastModified: " + lastModified + " \n maxAttempts: " + maxAttempts + " \n waitForCopyExpFiles: " + waitForCopyExpFiles, ApplicationEventsEnum.CopyNewExportFiles);
                    return;
                }

                Utilities.CreateInfoLog("ExportFileManager.CopyProdExportProgramFiles() GetExportFileForCopy is TRUE. \n Paramters: \n exportTimeStamp: " + exportTimeStamp + " \n prodProgId: " + prodProgId + " \n counterAttempts: " + counterAttempts + " \n lastModified: " + lastModified + " \n maxAttempts: " + maxAttempts + " \n waitForCopyExpFiles: " + waitForCopyExpFiles, ApplicationEventsEnum.CopyNewExportFiles);

                #region check maxAttempts
                //// if maxAttempts is reached wait for a spefific duration
                //if (counterAttempts >= maxAttempts)
                //{
                //    // check duration; if waitForCopyExpFiles is reached-> reset attempts and try again to copy
                //    if (lastModified.CompareTo(DateTime.Now.AddMinutes(-waitForCopyExpFiles)) <= 0)
                //    {
                //        exportCleanUpDAO.ResetCounterAttepts(prodProgId, exportTimeStamp);
                //    }
                //    else
                //    {
                //        // waitForCopyExpFiles is not reached
                //        return;
                //    }
                //}
                #endregion

                string fileName = exportTimeStamp + "_" + prodProgId + "*.csv";
                string sourcePath = Path.Combine(sourceServer, prodProgId);
                string destPath = "";

                try
                {
                    Utilities.CreateInfoLog("ExportFileManager.CopyProdExportProgramFiles() Utilities.CopyFileToFolder(). \n Parameters: \n sourcePath: " + sourcePath + " \n fileName: " + fileName + " \n exportServers[0]: " + exportServers[0] , ApplicationEventsEnum.CopyNewExportFiles);
                    // copy to each server inside this loop
                    foreach (string expServer in exportServers)
                    {
                        destPath = Path.Combine(expServer, prodProgId);
                        Utilities.CopyFileToFolder(sourcePath, destPath, fileName, true);
                    }
                }
                catch (FileNotFoundException ex)
                {                    
                    // Set FileStatus to error that it will not try again this program
                    string exMessage = "ExportFileManager: Error on call CopyFileToFolder. Parameters: destPath: " + destPath + " sourcePath: " + sourcePath + " fileName: " + fileName + " ExceptionMessage: " + ex.Message;
                    // Increase counterAttempts in DB
                    ExportCleanUpDao.InsertProdExportProgramFilesCacheException(prodProgId, exportTimeStamp, exMessage, maxAttempts, Constants.ProdExportProgramFilesCacheFileStatus.Error);
                    throw new AffiliGenericException("ExportFileManager.CopyProdExportProgramFiles(): Error on call CopyFileToFolder. \n Paramters: \n destPath: " + destPath + "\n sourcePath: " + sourcePath + "\n fileName: " + fileName, ex);
                }
                catch (Exception ex)
                {
                    string exMessage = "ExportFileManager.CopyProdExportProgramFiles(): Error on call CopyFileToFolder. Parameters destPath: " + destPath + " sourcePath: " + sourcePath + " fileName: " + fileName + " ExceptionMessage: " + ex.Message;
                    // Increase counterAttempts in DB
                    ExportCleanUpDao.InsertProdExportProgramFilesCacheException(prodProgId, exportTimeStamp, exMessage, maxAttempts);
                    throw new AffiliGenericException("ExportFileManager.CopyProdExportProgramFiles(): Error on call CopyFileToFolder. \n Paramters: \n destPath: " + destPath + "\n sourcePath: " + sourcePath + "\n fileName: " + fileName, ex);
                }
                // Update DB, set status to copied
                ExportCleanUpDao.UpdateProdExportProgramFilesCache(prodProgId, exportTimeStamp);

                // delete files from local cache folder
                Utilities.DeleteFiles(fileName, sourcePath);
            }
            catch (AffiliGenericException ex)
            {
                ex.CreateLog();
            }
            catch (Exception ex)
            {
                new AffiliGenericException("ExportFileManager.CopyProdExportProgramFiles(): An error occured.", ex).CreateLog();
            } 
        }

        /// <summary>
        /// Return the required settings from the config
        /// </summary>
        /// <param name="maxAttempts"></param>
        /// <param name="waitForCopyExpFiles"></param>
        /// <param name="exportServers"></param>
        /// <param name="sourceServer"></param>
        private void GetConfigSettings(ref int maxAttempts, ref int waitForCopyExpFiles, out string[] exportServers, out string sourceServer)
        {
            exportServers = GetExportServers();
            if (exportServers == null || exportServers.Length == 0)
            {
                throw new AffiliGenericException("ExportFileManager.GetConfigSettings(). GetExportServers: No Path found in config. Check config file!");
            }

            sourceServer = Utilities.GetAppSettingValue(Constants.AppSettings.ExportFilesLocation);
            if (string.IsNullOrEmpty(sourceServer))
            {
                throw new AffiliGenericException("ExportFileManager.GetConfigSettings(). Error on get the ExportFilesLocation path from config. Check config file!");
            }
            
            if (!int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.MaxCopyAttempts), out maxAttempts))
            {
                Utilities.CreateWarningLog("Start ExportFileManager.GetConfigSettings(). Could not find MaxCopyAttempts value in config file.", ApplicationEventsEnum.CopyNewExportFiles);
            }
            
            if (!int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.WaitForCopyExportFiles), out waitForCopyExpFiles))
            {
                Utilities.CreateWarningLog("Start ExportFileManager.GetConfigSettings(). Could not find WaitForCopyExportFiles value in config file.", ApplicationEventsEnum.CopyNewExportFiles);
            }
        }

        /// <summary>
        /// Return the values from db
        /// </summary>
        /// <param name="exportCleanUpDao"></param>
        /// <param name="exportTimeStamp"></param>
        /// <param name="prodProgId"></param>
        /// <param name="counterAttempts"></param>
        /// <param name="lastModified"></param>
        /// <param name="maxAttempts"></param>
        /// <param name="waitForCopyExpFiles"></param>
        /// <returns>TRUE if one row was find in db else FALSE</returns>
        private static bool GetExportFileForCopy(ExportCleanUpDAO exportCleanUpDao, ref string exportTimeStamp, ref string prodProgId, ref int counterAttempts, ref DateTime lastModified, int maxAttempts, int waitForCopyExpFiles)
        {
            bool getExpFileForCopyFromDb = false;
                        
            DataTable dtExpFileForCopy = exportCleanUpDao.GetExportFilesForCopy(maxAttempts, waitForCopyExpFiles);

            if (dtExpFileForCopy != null && dtExpFileForCopy.Rows.Count > 0)
            {
                if (dtExpFileForCopy.Rows[0][Constants.ProdExportProgramFilesCache.ExportTimeStamp] != DBNull.Value)
                    exportTimeStamp = dtExpFileForCopy.Rows[0][Constants.ProdExportProgramFilesCache.ExportTimeStamp].ToString();

                if (dtExpFileForCopy.Rows[0][Constants.ProdExportProgramFilesCache.ProdProgId] != DBNull.Value)
                    prodProgId = dtExpFileForCopy.Rows[0][Constants.ProdExportProgramFilesCache.ProdProgId].ToString();

                if (dtExpFileForCopy.Rows[0][Constants.ProdExportProgramFilesCache.CounterAttempts] != DBNull.Value)
                    counterAttempts = Convert.ToInt16(dtExpFileForCopy.Rows[0][Constants.ProdExportProgramFilesCache.CounterAttempts]);

                lastModified = Convert.ToDateTime(dtExpFileForCopy.Rows[0][Constants.ProdExportProgramFilesCache.LastModified]);
                
                // if all variables are fine set to true
                getExpFileForCopyFromDb = true;
            }
            
            return getExpFileForCopyFromDb;
        }

        #region helper

        /// <summary>
        /// Return all servers where export files have to be deleted
        /// </summary>
        /// <returns></returns>
        private string[] GetServerForDeletion()
        {
            // get the export servers
            string[] delServers = GetExportServers();
            // get the local file cache
            string localFileCache = Utilities.GetAppSettingValue(Constants.AppSettings.ExportFilesLocation);
            // add the local file cache to the server array
            Array.Resize(ref delServers, delServers.Length + 1);
            Array.Copy(new object[] { localFileCache }, 0, delServers, delServers.Length - 1, 1);            
            
            return delServers;
        }

        /// <summary>
        /// Reads the Export server into an array.
        /// </summary>
        /// <returns></returns>
        protected string[] GetExportServers()
        {
            string[] arrExportServer = Utilities.GetAppSettingValue(Constants.AppSettings.ExportServers).Split(new[] { ';' });
            return arrExportServer;
        }

        #endregion
        #endregion


    }


}

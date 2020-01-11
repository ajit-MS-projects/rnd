using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Timers;
using Affilinet.Business.ProductImport.Common;
using Affilinet.Business.ProductImport.DAO;
using Affilinet.Business.ProductImport.Download;
using Affilinet.Business.ProductImport.Entity;
using Affilinet.Business.ProductImport.Processor;
using Affilinet.Exceptions;
using Affilinet.Utility.Logging;
using Microsoft.SqlServer.Dts.Runtime;
using System.IO;
using Constants=Affilinet.Business.ProductImport.Common.Constants;

namespace Affilinet.Business.ProductImport
{
    /// <summary>
    /// Main class for managing file downloads and their data sanitization/procssing which results in procssed docs ready to be imported.  
    /// </summary>
    public class ProductImportManager : IDisposable
    {
        #region Private Instances
        private BaseDownloadManager objDownloadManager;
        //private DataTable dtScheduledPrograms;
        private ProductDAO objProductDao;
        //private FileProcessor FileProcessor;
        private ApplicationStatusEnum ApplicationSanitizeStatus;
        private ApplicationStatusEnum ApplicationDownloadStatus;
        private bool isManualImport  = false;

        private Application app = null;
        private Package insertPackage = null;
        private Package updatePackage = null;
        private Package deletePackage = null;
        private Package imagepackage = null;
        private DTSExecResult results;
        private Variables vars;
        /// <summary>
        /// This object instance is used to create thread synchronisation.
        /// </summary>
        protected Object thisLock = new Object();
    #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductImportManager"/> class.
        /// </summary>
        public ProductImportManager()
        {
            //objDownloadManager = new DownloadManager();
            objProductDao = new ProductDAO();
        }
           /// <summary>
        /// Initiates the Auto download, processing & import of product CSVs.
        /// </summary>
        public void AutoImport()
           {
               AutoImport(false);
           }

        /// <summary>
        /// Initiates the Auto download, processing & import of product CSVs.
        /// </summary>
        public void AutoImport(bool isHourly)
        {
            try
            {
                Utilities.WriteHealthCheckTimeStamp(isHourly ? WinServiceHealthCheckFileTypesEnum.HourlyProductImport : WinServiceHealthCheckFileTypesEnum.ProductImport);
                if (ApplicationSanitizeStatus == ApplicationStatusEnum.Idle)
                {
                    Utilities.ReloadConfigSection();
                    DateTime impStartTime =
                        Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " +
                                           Utilities.GetAppSettingValue(Constants.AppSettings.ImportStartTime));
                    DateTime impEndTime =
                        Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " +
                                           Utilities.GetAppSettingValue(Constants.AppSettings.ImportEndTime));
                    DataTable dtScheduledPrograms = null;
                    //if (IsManualImportScheduled(out dtScheduledPrograms) || (DateTime.Now.CompareTo(impStartTime) >= 0
                    //                                && DateTime.Now.CompareTo(impEndTime) <= 0))
                    //StartImport(dtScheduledPrograms);
                    //else
                    if (!(DateTime.Now.CompareTo(impStartTime) >= 0 && DateTime.Now.CompareTo(impEndTime) <= 0))
                        StartFileProcessing(isHourly);
                }
                //Utilities.CreateInfoLog("Garbage collection start: " + GC.GetTotalMemory(true).ToString(), ApplicationEventsEnum.SsisImportFinsh);
                GC.Collect();
                GC.WaitForFullGCComplete();
                //Utilities.CreateInfoLog("Garbage collection end: " + GC.GetTotalMemory(true).ToString(), ApplicationEventsEnum.SsisImportFinsh);
            }
            catch (Exception ex)
            {
                ApplicationSanitizeStatus = ApplicationStatusEnum.Idle;
                new AffiliGenericException("Error in ProductImportManager.AutoImport()", ex).CreateLog();
            }
        }

        /// <summary>
        /// Initiates the Auto download, processing & import of product CSVs.
        /// </summary>
        public void AutoImportSsis()
        {
            try
            {
                Utilities.WriteHealthCheckTimeStamp(WinServiceHealthCheckFileTypesEnum.ProductImportSsis);
                if (ApplicationSanitizeStatus == ApplicationStatusEnum.Idle)
                {
                    Utilities.ReloadConfigSection();
                    DateTime impStartTime =
                        Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " +
                                           Utilities.GetAppSettingValue(Constants.AppSettings.ImportStartTime));
                    DateTime impEndTime =
                        Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " +
                                           Utilities.GetAppSettingValue(Constants.AppSettings.ImportEndTime));
                    DataTable dtScheduledPrograms = null;
                    if (IsManualImportScheduled(out dtScheduledPrograms) || (DateTime.Now.CompareTo(impStartTime) >= 0
                                                      && DateTime.Now.CompareTo(impEndTime) <= 0))
                        StartImport(dtScheduledPrograms);
                    //else
                    //    StartFileProcessing();
                }
                //Utilities.CreateInfoLog("Garbage collection start: " + GC.GetTotalMemory(true).ToString(), ApplicationEventsEnum.SsisImportFinsh);
                GC.Collect();
                GC.WaitForFullGCComplete();
                //Utilities.CreateInfoLog("Garbage collection end: " + GC.GetTotalMemory(true).ToString(), ApplicationEventsEnum.SsisImportFinsh);
            }
            catch (Exception ex)
            {
                ApplicationSanitizeStatus = ApplicationStatusEnum.Idle;
                new AffiliGenericException("Error in ProductImportManager.AutoImportSsis()", ex).CreateLog();
            }
        }
        #region Download file
        /// <summary>
        /// Starts the CSV processing.
        /// </summary>
        public void StartCsvDownloading()
        {
            try
            {
                Utilities.WriteHealthCheckTimeStamp(WinServiceHealthCheckFileTypesEnum.CsvDownload);
                if (ApplicationDownloadStatus == ApplicationStatusEnum.Idle)
                {
                    ApplicationDownloadStatus = ApplicationStatusEnum.DownloadingCSV;
                    DataTable dtScheduledPrograms = objProductDao.GetScheduledPrograms(true);
                    if (dtScheduledPrograms.Rows.Count > 0)
                    {
                        PrioritiseManualSchedules(dtScheduledPrograms);
                        //objDownloadManager.DownloadCompleteEvent +=
                        //    new BaseDownloadManager.DownloadComplete(DownloadCompleteEventHandler);
                        objDownloadManager = new DownloadManager();
                        objDownloadManager.ObjProductImportManager = this;
                        objDownloadManager.ObjProductDao = objProductDao;
                        objDownloadManager.DocAttribList = SetupDocumentAttributes(dtScheduledPrograms);
                        dtScheduledPrograms.Clear();
                        dtScheduledPrograms.Dispose();
                        dtScheduledPrograms = null;
                        GC.Collect();
                        objDownloadManager.DownloadDocuments();
                        objDownloadManager.DocAttribList.Clear();
                        objDownloadManager.DocAttribList = null;
                        //DownloadCompleteEventHandler(objDownloadManager);
                        //objDownloadManager.DownloadDocumentsAsynch();
                    }
                }
            }
            catch (AffiliBaseException aex)
            {
                aex.CreateLog();
            }
            catch (Exception ex)
            {
                new AffiliGenericException("Error in ProductImportManager.StartCsvDownloading", ex, (int)ApplicationEventsEnum.DownloadStart).CreateLog();
            }
            finally
            {
                ApplicationDownloadStatus = ApplicationStatusEnum.Idle;
            }
        }
        private void PrioritiseManualSchedules(DataTable dtScheduledPrograms)
        {
            DataRow[] mRows = dtScheduledPrograms.Select("ManualImport=1");
            if (mRows != null && mRows.Length > 0)
            {
                isManualImport = true;
                dtScheduledPrograms.DefaultView.RowFilter = "ManualImport=1";
            }
            else
                isManualImport = false;
        }
        /// <summary>
        /// Setups the list of scheduled documents for download manager.
        /// </summary>
        private List<DocumentAttributes> SetupDocumentAttributes(DataTable dtScheduledPrograms)
        {
            
            List<DocumentAttributes> docAttribList = new List<DocumentAttributes>();
            for (int i = 0; i < dtScheduledPrograms.DefaultView.Count; i++)
            {
                DataRowView dr = null;
                try
                {
                    dr = dtScheduledPrograms.DefaultView[i];//.Rows[i];
                    object FileSourceURI = dr[Constants.ProductProgram.SourceURL];
                    if (FileSourceURI == DBNull.Value || FileSourceURI == null || FileSourceURI.ToString() == string.Empty)
                        throw new Exception("Source URL is empty in ProductProgram table.");
                    DocumentAttributes objDocAtt = new DocumentAttributes
                                                       {
                                                           ProdProgId = int.Parse(dr[Constants.ProductProgram.PordProgId].ToString()),
                                                           FileSourceURI = FileSourceURI.ToString(),
                                                           FileDestination = Utilities.GetAppSettingValue(Constants.AppSettings.CSVSaveLocation) + int.Parse(dr[Constants.ProductProgram.PordProgId].ToString()) + @"\",
                                                           FileName = dr[Constants.ProductProgram.DestinationURL].ToString().Replace("files/", ""),
                                                           DocumentEncoding = dr[Constants.ProductProgram.Encoding] == DBNull.Value || dr[Constants.ProductProgram.Encoding].ToString().Equals("") ? Encoding.Default : Encoding.GetEncoding(dr[Constants.ProductProgram.Encoding].ToString()),
                                                           UserId = dr[Constants.ProductProgram.LoginName] == DBNull.Value ? "" : dr[Constants.ProductProgram.LoginName].ToString(),
                                                           Password = dr[Constants.ProductProgram.Password] == DBNull.Value ? "" : dr[Constants.ProductProgram.Password].ToString(),
                                                           ProgName = dr[Constants.ProductProgram.ProgramName].ToString(),
                                                           DocumentType = DocumentAttributes.GetDocumentType(dr[Constants.ProductProgram.FileType]),
                                                           ManualImport = (ManualImportEnum)(dr[Constants.ProductProgram.ManualImport] == DBNull.Value ? 0 : int.Parse(dr[Constants.ProductProgram.ManualImport].ToString())),
                                                           AccountManager = dr[Constants.ProductProgram.AccountManager] == DBNull.Value ? "" : dr[Constants.ProductProgram.AccountManager].ToString(),
                                                           AccountManagerEmail = dr[Constants.ProductProgram.AccountManagerEmail] == DBNull.Value ? "cschwarze@affili.net;ukotonski@affili.net" : dr[Constants.ProductProgram.AccountManagerEmail].ToString()
                                                       };
                    docAttribList.Add(objDocAtt);
                    objDocAtt.Report.AutoOrManualImport = isManualImport ? "Manual" : "Auto";
                }
                catch (Exception ex)
                {//No throw statement as issue in 1 program must not affect others
                    objProductDao.UpdateProductProgramStatus(int.Parse(dr[Constants.ProductProgram.PordProgId].ToString()),
                                                               (int)ProgramImportStatusEnum.DOWNLOAD_ERROR);
                    new AffiliGenericException("PIM.SetupDocumentAttributes() Error while looping though product program" + dr[Constants.ProductProgram.PordProgId].ToString(), ex);
                }
            }
            return docAttribList;
        } 
        #endregion
        #region "Import"
        /// <summary>
        /// Determines whether [is manual import scheduled].
        /// Resultset contains only programs for which csv processing is complete
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is manual import scheduled]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsManualImportScheduled(out DataTable dtScheduledPrograms)
        {
            bool retVal = false;
            dtScheduledPrograms = objProductDao.GetScheduledProgramsForSsisImport(true);
            retVal = (dtScheduledPrograms != null && dtScheduledPrograms.Rows.Count > 0);
            return retVal;
        }
        /// <summary>
        /// Starts the ssis import of processed csvs.
        /// </summary>
        /// 
        public void StartImport(DataTable dtScheduledPrograms)
        {
            ApplicationSanitizeStatus = ApplicationStatusEnum.ImportingCSV;
          
            string prodProgId = string.Empty;
            try
            {
                if (dtScheduledPrograms == null || dtScheduledPrograms.Rows.Count == 0) //Manually scheduled?
                    dtScheduledPrograms = objProductDao.GetScheduledProgramsForSsisImport(false);
                foreach (DataRow dr in dtScheduledPrograms.Rows) //Launch import SSIS for each program
                {
                    SsisImportReportingInfo objReportLog = new SsisImportReportingInfo();
                    objReportLog.SsisImportStartTime = DateTime.Now.ToString();
                    int success = 0;
                    int failure = 0;
                    try
                    {
                        prodProgId = dr[Constants.ProductProgram.PordProgId].ToString();
                        objProductDao.UpdateProductProgramStatus(int.Parse(prodProgId),
                                                                 (int) ProgramImportStatusEnum.IMPORTING_CSV);
                        objReportLog.ImportStatus = Constants.ProcessingStatus.Success;

                        string FileDestination = Utilities.GetAppSettingValue(Constants.AppSettings.CSVSaveLocation) + @"\" + prodProgId + @"\";
                        string FileName = dr[Constants.ProductProgram.DestinationURL].ToString().Replace("files/", "");

                        string csvURLPrefix = (FileDestination + FileName).Replace("/", @"\");
                        int j = 1;
                        String notChangedFile = csvURLPrefix + "." + j.ToString() + Constants.FileNaming.ProductNotchangedCsvExtention;
                        while (File.Exists(notChangedFile))
                        {
                            Utilities.MoveFile(notChangedFile , FileDestination, FileName, FileLocationsEnum.Imported, j, Constants.FileNaming.ProductNotchangedCsvExtention, true);
                            j++;
                            notChangedFile = csvURLPrefix + "." + j.ToString() + Constants.FileNaming.ProductNotchangedCsvExtention;
                        }

                        string[] fileExtensions = Utilities.GetFileExtensions();
                        bool ssisErrors = false;
                        bool productsChanged = false;
                        foreach (string fileExtension in fileExtensions) //Import all files of a program
                        {
                            int i = 1;
                            string csvURL = csvURLPrefix + "." + i.ToString() + fileExtension;
                            while (File.Exists(csvURL)) //Import all sub files of a program
                            {
                                if (ExecuteSsisPackage(prodProgId, csvURL, objReportLog, fileExtension))
                                {
                                    productsChanged = true;
                                    Utilities.MoveFile(csvURL, FileDestination, FileName, FileLocationsEnum.Imported, i, fileExtension, true);
                                }
                                else
                                {
                                    ssisErrors = true;
                                    new AffiliGenericException(
                                        "Error in ProductImportManager.StartImport() Executing SSIS ProdProgId:" +
                                        prodProgId +
                                        ":Errors:" + objReportLog.SsisError + " :FileName:" + csvURL,
                                        (int) ApplicationEventsEnum.SsisImportStart).CreateLog();
                                    Utilities.MoveFile(csvURL, FileDestination, FileName, FileLocationsEnum.NotImported, i, fileExtension, true);
                                }
                                i++;
                                csvURL = csvURLPrefix + "." + i.ToString() + fileExtension;
                            }
                        }

                        //Move source csv to archive
                        if (!ssisErrors)
                        {
                            success++;
                            UpdateProductProgramAfterSsisImport(dr, int.Parse(prodProgId), productsChanged);
                            //Utilities.MoveFile(FileDestination, FileName, FileLocationsEnum.Archive);
                        }
                        else
                            failure++;

                        objProductDao.UpdateProductProgramStatus(int.Parse(prodProgId), ssisErrors
                                                                     ? (int) ProgramImportStatusEnum.CSV_IMPORT_ERROR
                                                                     : (int) ProgramImportStatusEnum.CSV_IMPORT_COMPLETE);
                        objReportLog.SsisImportEndTime = DateTime.Now.ToString();

                        //updating image ids
                        if (productsChanged)
                            ExecuteUpdateImageSSIS(prodProgId, objReportLog);

                    }
                    catch (AffiliBaseException aex)
                    {//No throw statement in for loop as exception in 1 program must not affect other programs
                        aex.CreateLog();
                        failure++;
                        objReportLog.SsisError = aex.Message;
                    }
                    catch (Exception ex)
                    {
                        objProductDao.UpdateProductProgramStatus(int.Parse(prodProgId), (int)ProgramImportStatusEnum.CSV_IMPORT_ERROR);
                        new AffiliGenericException(
                            "Error in ProductImportManager.StartImport() Launcing SSIS ProdProgId:" + prodProgId, ex,
                            (int)ApplicationEventsEnum.SsisImportStart).CreateLog();
                        failure++;
                        objReportLog.SsisError = ex.Message;
                    }
                    finally
                    {
                        objReportLog.ImportStatus = "Failed:" + failure.ToString() + " :Succeeded:" + success.ToString();
                        Utilities.CreateReportLog(objReportLog.GetReportLogs(prodProgId));
                    }
                    objProductDao.UpdateProductCount(prodProgId);
                }//End of foreach (DataRow dr in dtScheduledPrograms.Rows)
            }
            catch(AffiliGenericException aex)
            {
                aex.CreateLog();
            }
            catch (Exception ex)
            {
                AffiliGenericException aex = new AffiliGenericException(
                    "Error in ProductImportManager.StartImport() Launcing SSIS ProdProgId:" + prodProgId, ex,
                    (int) ApplicationEventsEnum.SsisImportStart);
                aex.CreateLog();
            }
            finally
            {
                ApplicationSanitizeStatus = ApplicationStatusEnum.Idle;
                if (dtScheduledPrograms != null) dtScheduledPrograms.Clear();
                dtScheduledPrograms = null;
            }
        }

        /// <summary>
        /// Executes the update image SSIS.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        private void ExecuteUpdateImageSSIS(string prodProgId, SsisImportReportingInfo objReportLog)
         {
             string execResult = string.Empty;
             string pkgLocation = string.Empty;
             string strDtsErrors = string.Empty;
             Package package = null;
             try
             {
                 if (app == null)
                    app = new Application();

                 if (imagepackage == null)
                 {
                     pkgLocation = Utilities.GetAppSettingValue(Constants.AppSettings.ProductImageIdsUpdateSSIS);
                     imagepackage = app.LoadPackage(pkgLocation, null);
                 }
                 package = imagepackage;

                 //Set SSIS Package's Global Variables
                 vars = package.Variables;

                 //Set Program id
                 vars[Constants.SSISParameters.ProdProgId].Value = prodProgId;

                 //This value is picked up from ProductData connection string.;
                 vars[Constants.SSISParameters.DataSource].Value =
                     Utilities.GetConnectionString(Affilinet.Data.Access.Constants.DBConnections.ProductSSIS);

                 lock (thisLock)
                 {
                     results = package.Execute();
                 }

                 //Start: Get Errors generated in SSIS package
                 strDtsErrors = "-";
                 foreach (DtsError dtsErr in package.Errors)
                 {
                     strDtsErrors += dtsErr.Description + ":";
                 }
                 Utilities.CreateInfoLog(
                     "SSIS update image ids execute result PPID:" + prodProgId + " :" + results.ToString() +
                     ":Errors:" + strDtsErrors + ":Package:" + pkgLocation ,
                     ApplicationEventsEnum.DocProcessFinish);
                 objReportLog.SsisError += strDtsErrors;
                 objReportLog.ImportStatus = results.ToString();
                 //End: Get Errors generated in SSIS package
             }
             catch(Exception ex)
             {
                 throw new AffiliGenericException("SSIS update image ids  execute result PPID:" + prodProgId + " :" + execResult +
                  ":Errors:" + strDtsErrors + ":Package:" + pkgLocation,ex);
             }
             finally
             {
                 //if (package != null) package.Dispose();
                 //app = null;
             }
         }

        /// <summary>
        /// Executes the ssis package.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="csvURL">The CSV URL.</param>
        /// <param name="objReportLog">The obj report log.</param>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns></returns>
        private bool ExecuteSsisPackage(string prodProgId, string csvURL, SsisImportReportingInfo objReportLog, string fileExtension)
        {
           
            string execResult = string.Empty;
            string pkgLocation = string.Empty;
            string strDtsErrors = string.Empty;
            Package package = null;
            try
            {
                if (app == null)
                    app = new Application();

                if (deletePackage == null)
                {
                    pkgLocation = Utilities.GetAppSettingValue(Constants.AppSettings.ProductImportSSIS);
                    pkgLocation = pkgLocation.Replace(Constants.FileNaming.DtsxFileExtension,
                                                      Constants.FileNaming.ProductDeleteCsvExtention + Constants.FileNaming.DtsxFileExtension);
                    deletePackage = app.LoadPackage(pkgLocation, null);
                }
                if (updatePackage == null)
                {
                    pkgLocation = Utilities.GetAppSettingValue(Constants.AppSettings.ProductImportSSIS);
                    pkgLocation = pkgLocation.Replace(Constants.FileNaming.DtsxFileExtension,
                                                      Constants.FileNaming.ProductUpdateCsvExtention + Constants.FileNaming.DtsxFileExtension);
                    updatePackage = app.LoadPackage(pkgLocation, null);
                }
                if (insertPackage == null)
                {
                    pkgLocation = Utilities.GetAppSettingValue(Constants.AppSettings.ProductImportSSIS);
                    pkgLocation = pkgLocation.Replace(Constants.FileNaming.DtsxFileExtension,
                                                      Constants.FileNaming.ProductInsertCsvExtention + Constants.FileNaming.DtsxFileExtension);
                    insertPackage = app.LoadPackage(pkgLocation, null);
                }
                switch (fileExtension)
                {
                    case Constants.FileNaming.ProductDeleteCsvExtention:
                        package = deletePackage;
                        break;
                    case Constants.FileNaming.ProductUpdateCsvExtention:
                        package = updatePackage;
                        break;
                    case Constants.FileNaming.ProductInsertCsvExtention:
                        package = insertPackage;
                        break;
                }

                //pkgLocation = Utilities.GetAppSettingValue(Constants.AppSettings.ProductImportSSIS);
                //pkgLocation = pkgLocation.Replace(Constants.FileNaming.DtsxFileExtension, fileExtension + Constants.FileNaming.DtsxFileExtension);
                //package = app.LoadPackage(pkgLocation, null);

                //Set SSIS Package's Global Variables
                vars = package.Variables;

                //Set Program id
                vars[Constants.SSISParameters.ProdProgId].Value = prodProgId;

                //Set file names and path
                vars[Constants.SSISParameters.CsvFilePath].Value = csvURL;
                //vars[Constants.SSISParameters.CsvUpdateURL].Value = csvUpdateURL;
                //vars[Constants.SSISParameters.CsvDeleteURL].Value = csvDeleteURL;

                //This value is picked up from ProductData connection string.;
                vars[Constants.SSISParameters.DataSource].Value =
                    Utilities.GetConnectionString(Affilinet.Data.Access.Constants.DBConnections.ProductSSIS);
                    //Utilities.GetDataSource(Affilinet.Data.Access.Constants.DBConnections.ProductData);

                lock (thisLock)
                {
                    results = package.Execute();
                }

                //Start: Get Errors generated in SSIS package
                strDtsErrors = "-";
                foreach (DtsError dtsErr in package.Errors)
                {
                    strDtsErrors += dtsErr.Description + ":";
                }
                Utilities.CreateInfoLog(
                    "SSIS Import Data execute result PPID:" + prodProgId + " :" + results.ToString() +
                    ":Errors:" + strDtsErrors + ":Package:" + pkgLocation + ":csv URL:" + csvURL,
                    ApplicationEventsEnum.DocProcessFinish);
                objReportLog.SsisError = strDtsErrors;
                objReportLog.ImportStatus = results.ToString();
                //End: Get Errors generated in SSIS package
                execResult = results.ToString().ToUpper();
                return execResult == "SUCCESS";

            }
            catch(Exception ex)
            {
                throw new AffiliGenericException("SSIS Import Data execute result PPID:" + prodProgId + " :" + execResult +
                 ":Errors:" + strDtsErrors + ":Package:" + pkgLocation + ":csv URL:" + csvURL,ex);
            }
            finally
            {
                //if (package != null) package.Dispose();
                //app = null;
            }
        }

        /// <summary>
        /// Updates the product program after ssis import.
        /// Updates AutoUpdateNext date time
        /// </summary>
        /// <param name="dr">The dr.</param>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="productsChanged">if set to <c>true</c> products are deemed changed and ProductProgram.ProductChangeDate field is updated to current date.</param>
        public void UpdateProductProgramAfterSsisImport(DataRow dr, int prodProgId, bool productsChanged)
        {
            try
            {
                //Get AutoUpdateNext from db
                DateTime AutoUpdateNext = DateTime.Now;
                if (dr[Constants.ProductProgram.AutoUpdateNext] != DBNull.Value)
                    DateTime.TryParse(dr[Constants.ProductProgram.AutoUpdateNext].ToString(), out AutoUpdateNext);


                //Get Increment value in hours, days or months
                int incrValue = 1;
                if (dr[Constants.ProductProgram.AutoUpdateNext] != DBNull.Value)
                    int.TryParse(dr[Constants.ProductProgram.AutoUpdateInterval].ToString(), out incrValue);

                //Get interval type from db H for hour, W for week, M for month
                string strIntervalType = dr[Constants.ProductProgram.AutoUpdateIntervalType] == DBNull.Value
                                             ? "D"
                                             : dr[Constants.ProductProgram.AutoUpdateIntervalType].ToString().
                                                   ToUpper();
                string nextUpdateDateTime = String.Empty;
                if (DateTime.Now.CompareTo(AutoUpdateNext) >= 0)
                {//Update autoUpdateNext only if it is less than scheduled
                    nextUpdateDateTime = Utilities.IncrementDate(AutoUpdateNext, strIntervalType, incrValue);//Add increment to date
                }
                objProductDao.UpdateProductProgram(prodProgId, nextUpdateDateTime, 0, productsChanged);

            }
            catch (Exception ex)
            {
                throw new AffiliGenericException(
                    "Error in ProductImportManager.UpdateProductProgramAfterSsisImport()  ProdProgId:" + prodProgId, ex,
                    (int)ApplicationEventsEnum.SsisImportFinsh);
            }
        }

      #endregion
        #region Start CSV sanitization
        /// <summary>
        /// Handles the Download complete event. It initiats the processing of downloaded documents.
        /// </summary>
        /// <param name="objDwndMgr">The obj DWND MGR.</param>
        private void StartFileProcessing(bool isHourly)//BaseDownloadManager objDwndMgr)
        {
            FileProcessor fileProcessor = null;
            try
            {
                DataTable dtScheduledPrograms = objProductDao.GetScheduledPrograms(false, isHourly);
                if (dtScheduledPrograms.Rows.Count <= 0) return;
                List<DocumentAttributes> docAttribList = SetupDocumentAttributes(dtScheduledPrograms);
                PrioritiseManualSchedules(dtScheduledPrograms);
                fileProcessor = new FileProcessor(docAttribList, objProductDao);
                fileProcessor.StartProcessing();
                docAttribList.Clear();
                docAttribList = null;
                dtScheduledPrograms.Clear();
                dtScheduledPrograms = null;
            }
            catch (AffiliBaseException aex)
            {
                aex.CreateLog();
            }
            catch (Exception ex)
            {
                new AffiliGenericException("Error in ProductImportManager.DownloadCompleteEventHandler", ex, (int)ApplicationEventsEnum.DocProcessInit).CreateLog();
            }
            finally
            {
                ApplicationSanitizeStatus = ApplicationStatusEnum.Idle;
                if (fileProcessor != null)
                {
                    fileProcessor.Dispose();
                    fileProcessor = null;
                }
            }
        }
        #endregion
          /// <summary>
        /// Resets the product program(s) which were aborted due to errors or any reason.
        /// </summary>
        /// 
        /// 
        public void ResetProductProgram(ProgramImportStatusEnum resetFromStatus)
        {
          ResetProductProgram(resetFromStatus, false);
        }

        /// <summary>
        /// Resets the product program(s) which were aborted due to errors or any reason.
        /// </summary>
        /// 
        /// 
        public void ResetProductProgram(ProgramImportStatusEnum  resetFromStatus, bool isHourly)
        {
            try
            {
                objProductDao.ResetProductProgram((int) resetFromStatus, isHourly);
            }
            catch (Exception ex)
            {
                new AffiliGenericException("Error in ProductImportManager.ResetProductProgram()", ex,
                                           (int) ApplicationEventsEnum.DocProcessInit).CreateLog();
            }
        }

        #region Dispose

        private bool disposed;
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        public void Dispose(bool disposing)
        {
            if (!(this.disposed))
            {
                if (disposing)
                {
                    objDownloadManager = null;
                    if (objProductDao!=null)
                        objProductDao.Dispose();
                    objProductDao = null;
                    thisLock = null;
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// It also suppresses finalization
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);//This call is to makes sure that disposed object does not get put on the GC's finalize queue.
        }
        /// <summary>
        /// Finalizes this instance, if dispose is not called explicitly
        /// </summary>
        protected void Finalize()
        {
            Dispose(false);
        }
        #endregion
    }
}

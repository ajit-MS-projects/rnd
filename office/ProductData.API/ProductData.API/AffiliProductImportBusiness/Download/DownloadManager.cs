﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using AffiliErrorIndexing;
using Affilinet.Business.ProductImport.Common;
using Affilinet.Business.ProductImport.Download;
using Affilinet.Business.ProductImport.Entity;
using Affilinet.Business.ProductImport.Processor;
using Affilinet.Exceptions;
using System.Data;
using Affilinet.Exceptions;
using Affilinet.Utility.Logging;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;
using Constants=Affilinet.Business.ProductImport.Common.Constants;
using CommonUtilities = Affili.ProductData.Common.Utilities;

namespace Affilinet.Business.ProductImport.Download
{
    /// <summary>
    /// Its a generic class to authenticate, download & Unzip files from http/ftp URLs
    /// Change History: Version;Date;Changed by;Change description
    /// 1.0;15-Feb-2009;Ajit Chahal;-
    /// </summary>
    public class DownloadManager : BaseDownloadManager
    {
        #region Private varibales
        /// <summary>
        /// Buffer for reading input streams
        /// </summary>
        private const int BufferSize = 1024;
        protected readonly ErrorIndexingManager ErrorIndexingManager = null; 
        #endregion
        
        #region Public Properties
        /// <summary>
        /// Gets or sets the document attribute list objects, this list use to itrate though all files and download them.
        /// </summary>
        /// <value>The doc attrib list.</value>
        public override List<DocumentAttributes> DocAttribList { get; set; }
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadManager"/> class.
        /// </summary>
        public DownloadManager()
        {
            DocAttribList = new List<DocumentAttributes>();
            ErrorIndexingManager = new ErrorIndexingManager();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadManager"/> class.
        /// </summary>
        /// <param name="objDocAtts">List of document attribute objects.</param>
        public DownloadManager(List<DocumentAttributes> objDocAtts)
            : this()
        {
            this.DocAttribList = objDocAtts;
        }
        #endregion
      
        #region Public Methods
        /// <summary>
        /// Public Methods internally reads a file from a URI and writes to the disk.
        /// </summary>
        /// <returns></returns>
        public override void DownloadDocuments()
        {

            try
            {
                if (DocAttribList.Count == 0)
                    throw new Exception("DocumentAttributes object must be set to call DownloadDocument ");
                foreach (DocumentAttributes docAttribs in DocAttribList)
                {
                    FileProcessor fileProcessor = new FileProcessor(DocAttribList, ObjProductDao);
                    fileProcessor.ReadProgramSettings(docAttribs);
                    try
                    {
                        if (IsDownloadEnabled(docAttribs))
                        {
                            if (!Directory.Exists(docAttribs.FileDestination))
                                Directory.CreateDirectory(docAttribs.FileDestination);

                            // get the download time
                            Stopwatch watch = new Stopwatch();
                            watch.Start();

                            docAttribs.DownloadReport.DownloadStart = DateTime.Now.ToString();
                            ObjProductDao.UpdateProductProgramStatus(docAttribs.ProdProgId, (int) ProgramImportStatusEnum.DOWLOADING);
                            switch (docAttribs.SourceURLProtocol)
                            {
                                case SourceURLProtocolEnum.FTP:
                                    DownloadFtp(docAttribs);
                                    break;
                                case SourceURLProtocolEnum.HTTP:
                                    if (docAttribs.Extension == FileExtensionsEnum.ZIP ||
                                        docAttribs.Extension == FileExtensionsEnum.GZ)
                                        DownloadHTTPZip(docAttribs);
                                    else
                                        DownloadHTTP(docAttribs);
                                    break;
                            }
                            // get the download time end
                            watch.Stop();
                            docAttribs.DownloadReport.DownloadDuration = Math.Round(watch.Elapsed.TotalSeconds, 0).ToString();
                            
                            UnzipFile(docAttribs);
                        }

                        docAttribs.DownloadReport.DownloadEnd = DateTime.Now.ToString();

                        if (IsFileHeaderRowSame (docAttribs) && CompareFileChecksum(docAttribs) && CheckIfCountOfProductsInAllowedRange(docAttribs))
                        {
                            ObjProductDao.UpdateProductProgramStatus(docAttribs.ProdProgId,
                                                                     (int) ProgramImportStatusEnum.DOWLOADING_COMPLETE);
                        }
                        ObjProductDao.UpdateErrorIndex(docAttribs.ProdProgId, Constants.ErrorIndexMasterListNames.DownloadError, 0);
                        docAttribs.DownloadReport.FileDownloadStatus = Constants.ProcessingStatus.Success;
                        
                        docAttribs.FileDownloadSuccess = true;
                    }
                    catch (AffiliBaseException aex)
                    {//No throw statement, issue in 1 program must not affect others
                        ObjProductDao.UpdateProductProgramStatus(docAttribs.ProdProgId,
                                                                 (int) ProgramImportStatusEnum.DOWNLOAD_ERROR);
                        docAttribs.DownloadReport.FileDownloadStatus = Constants.ProcessingStatus.Error;
                        docAttribs.DownloadReport.DownloadError = aex.InnerException.Message;
                        ObjProductDao.UpdateErrorIndex(docAttribs.ProdProgId, Constants.ErrorIndexMasterListNames.DownloadError, 1);

                        // start the error index calculation
                        StartErrorIndexCalc(docAttribs.ProdProgId.ToString());

                        aex.CreateLog();
                        ObjProductDao.UpdateLastExceptionAndGetHeaderCheckEnabledFlag(docAttribs.ProdProgId, aex.ToString());
                    }
                    catch (Exception ex)
                    {
                        new AffiliGenericException("Error in DownloadManager.DownloadDocument-foreach", ex,
                                                   (int) ApplicationEventsEnum.DownloadStart).CreateLog();
                        ObjProductDao.UpdateProductProgramStatus(docAttribs.ProdProgId,
                                                                 (int) ProgramImportStatusEnum.DOWNLOAD_ERROR);
                        docAttribs.DownloadReport.FileDownloadStatus = Constants.ProcessingStatus.Error;
                        ObjProductDao.UpdateErrorIndex(docAttribs.ProdProgId, Constants.ErrorIndexMasterListNames.DownloadError, 1);
                        
                        // start the error index calculation
                        StartErrorIndexCalc(docAttribs.ProdProgId.ToString());
                        
                        docAttribs.DownloadReport.DownloadError = ex.Message;
                        ObjProductDao.UpdateLastExceptionAndGetHeaderCheckEnabledFlag(docAttribs.ProdProgId, ex.ToString());
                    }
                    finally
                    {
                        Utilities.CreateReportLog(docAttribs.DownloadReport.GetReportLogs(docAttribs.ProdProgId.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                new AffiliGenericException("Error in DownloadManager.DownloadDocument", ex,
                                           (int) ApplicationEventsEnum.DownloadStart);
            }
        }

        private void StartErrorIndexCalc(string prodProgId)
        {
            ErrorIndexingManager.StartErrorIndexCalculation(prodProgId);
        }

        /// <summary>
        /// Compares the file header row from newly downloaded file to saved one.
        /// </summary>
        /// <returns></returns>
        private bool IsFileHeaderRowSame(DocumentAttributes docAttribs)
        {
            bool retVal = true;
            String headerLine = String.Empty;
            String errorMsg="Header line in Advertiser CSV is different than in DB";
            StreamReader srCsvSource = null;
            try
            {
                srCsvSource = new StreamReader(docAttribs.FileFullPath);
                headerLine = srCsvSource.ReadLine();
                if (docAttribs.ProgSettings.ImportType!="XML"  && //do not check for xml
                    (String.IsNullOrEmpty(headerLine) || 
                      (docAttribs.ProgSettings.CSVHeadline != headerLine.Trim() && 
                        docAttribs.ProgSettings.CSVHeadline + docAttribs.ProgSettings.FieldSeperator != headerLine.Trim())))
                {
                    if (ObjProductDao.UpdateLastExceptionAndGetHeaderCheckEnabledFlag(docAttribs.ProdProgId, errorMsg))
                    {
                        ObjProductDao.UpdateProductProgramStatus(docAttribs.ProdProgId, (int)ProgramImportStatusEnum.CSV_PROCESSING_ERROR);
                        ObjProductDao.UpdateErrorIndex(docAttribs.ProdProgId, Constants.ErrorIndexMasterListNames.HeaderError, 1);
                        // start the error index calculation
                        StartErrorIndexCalc(docAttribs.ProdProgId.ToString());
                        retVal = false;
                        docAttribs.DownloadReport.DownloadError = errorMsg;
                    }
                }
                else
                {
                    ObjProductDao.UpdateErrorIndex(docAttribs.ProdProgId, Constants.ErrorIndexMasterListNames.HeaderError, 0);
                }
            }catch(Exception ex)
            {
                throw new AffiliGenericException("DownloadManager.IsFileHeaderRowSame()",ex); 
            }
            finally
            {
                if(srCsvSource!=null)
                {
                    srCsvSource.Close();
                    srCsvSource.Dispose();
                    srCsvSource = null;
                }
            }
            return retVal;
        }

        private bool CompareFileChecksum(DocumentAttributes docAttribs)
        {
            bool retVal = true;

            try
            {
            String newCheckSum = CommonUtilities.GetMD5HashFromFile(docAttribs.FileFullPath);
            DataTable dtProdProg = ObjProductDao.GetProductProgramFileChecksum(docAttribs.ProdProgId);
            String oldCheckSum="";
            if (dtProdProg.Rows.Count > 0 && dtProdProg.Rows[0][Constants.ProductProgram.FileCheckSum]!= DBNull.Value)
                oldCheckSum = dtProdProg.Rows[0][Constants.ProductProgram.FileCheckSum].ToString();
            if (oldCheckSum != "" && newCheckSum == oldCheckSum)
            {
                retVal = false;
                ObjProductDao.UpdateProductProgramStatus(docAttribs.ProdProgId, (int)ProgramImportStatusEnum.CSV_IMPORT_COMPLETE);
                ObjProductDao.UpdateProductProgramAdditionalImportStatus(docAttribs.ProdProgId,
                                                                         DateTime.Now + ": File CheckSum is equal, sanitization skipped.");
                //increament date
                ObjProductImportManager.UpdateProductProgramAfterSsisImport(dtProdProg.Rows[0], docAttribs.ProdProgId, false);
            }
            else
            {
                ObjProductDao.UpdateProductProgramFileChecksum(docAttribs.ProdProgId, newCheckSum);
                ObjProductDao.UpdateProductProgramAdditionalImportStatus(docAttribs.ProdProgId, DateTime.Now + ": File CheckSum is different");
            }
            }
            catch (Exception ex)
            {
                new AffiliGenericException(
                    "Error in DownloadManager.CompareFileChecksum()  ProdProgId:" +docAttribs.ProdProgId, ex,
                    (int)ApplicationEventsEnum.DownloadEnd).CreateLog();
            }
            return retVal;
        }

        /// <summary>
        /// Checks if count of products in allowed range.
        /// </summary>
        /// <param name="docAttribs">The doc attribs.</param>
        private bool CheckIfCountOfProductsInAllowedRange(DocumentAttributes docAttribs)
        {
            bool retVal = true;
            try
            {
                string maxAllowedLinesInSourceCsv =
                    Utilities.GetAppSettingValue(Constants.AppSettings.MaxAllowedLinesInSourceCsv);
                string permissibleFileSizeInMB =
                    Utilities.GetAppSettingValue(Constants.AppSettings.PermissibleFileSizeInMB);
                int maxLines = 0;
                long maxFileSize = 0;
                long.TryParse(permissibleFileSizeInMB, out maxFileSize);
                int.TryParse(maxAllowedLinesInSourceCsv, out maxLines);
                if (maxFileSize > 0)
                {
                    FileInfo objFile = new FileInfo(docAttribs.FileFullPath);
                    if (objFile.Length > (maxFileSize*1024*1024)) //Check if file size if > then allowed
                    {
                        if (maxLines > 0)
                        {
                            int linesInCsv = GetCountOfLinesInCSV(docAttribs);
                            {
                                int approachingMaxProductsPerList = 0;
                                if (!int.TryParse(CommonUtilities.GetAppSettingValue(Constants.AppSettings.ApproachingMaxProductsPerList), out approachingMaxProductsPerList))
                                    approachingMaxProductsPerList = 950000;
                                if (linesInCsv > approachingMaxProductsPerList)
                                    SendMailToKam(docAttribs, linesInCsv);
                            }
                            if (linesInCsv > maxLines) //Check if no. of product lines are more then allowed
                            {
                                ObjProductDao.UpdateProductProgramStatus(docAttribs.ProdProgId,
                                                                         (int)
                                                                         ProgramImportStatusEnum.CSV_PROCESSING_ERROR);
                                docAttribs.Report.Attention = "Source File is too large:" + linesInCsv +
                                                              " product lines in CSV, max allowed is " + maxLines +
                                                              " product lines. ";
                                retVal = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException(
                    "Error in CheckIfCountOfProductsInAllowedRange PPID:" + docAttribs.ProdProgId, ex);
            }
            return retVal;
        }
        private void SendMailToKam(DocumentAttributes docAttribs, int linesInCsv)
        {
            CommonUtilities.SendMail("ProductDownload@affili.net",  docAttribs.AccountManagerEmail,
                                     "Products approaching 1 million",
                                     "Hi " + docAttribs.AccountManager + ", Products for following list is approaching 1 million, you may like to get it split to smaller lists. ProductProgramId: " +
                                     docAttribs.ProdProgId + " No of products: " + linesInCsv);
        }

        /// <summary>
        /// Gets the count of lines in CSV.
        /// </summary>
        /// <param name="docAttribs">The doc attribs.</param>
        /// <returns></returns>
        private int GetCountOfLinesInCSV(DocumentAttributes docAttribs)
        {
            int cnt = 0;
            StreamReader srCsvSource = new StreamReader(docAttribs.FileFullPath);
            while (srCsvSource.ReadLine() != null)
            {
                cnt++;
            }
            return cnt;
        }

        /// <summary>
        /// Determines whether [is download enabled] [the specified doc attribs].
        /// </summary>
        /// <param name="docAttribs">The doc attribs.</param>
        /// <returns>
        /// 	<c>true</c> if [is download enabled] [the specified doc attribs]; otherwise, <c>false</c>.
        /// </returns>
        private bool  IsDownloadEnabled(DocumentAttributes docAttribs)
        {
            bool retVal = true;
            if (!(docAttribs.ManualImport == ManualImportEnum.PROCESS_AND_IMPORT_ONLY))
            {//This step is ensure file is not downloaded if support has specifically set it not to download
                if (Utilities.GetAppSettingValue(Constants.AppSettings.SkipDownloadingCsvFiles) == "0")
                {
                    if (Utilities.GetAppSettingValue(Constants.AppSettings.DownloadCsvOnlyIfNotPresent) == "1")
                    {
                        retVal = !File.Exists(docAttribs.FileFullPath);
                    }
                }
                else
                {//Create log downloading disabled
                    retVal = false;
                    Utilities.CreateWarningLog("CSV downloading is disabled in web.config",
                                               ApplicationEventsEnum.DownloadEnd);
                    docAttribs.DownloadReport.FileDownloadStatus = Constants.ProcessingStatus.Disabled;
                }

                //Move source file to archive if already present
                if (retVal && File.Exists(docAttribs.FileFullPath))
                    Utilities.MoveFile(docAttribs.FileDestination, docAttribs.FileName, FileLocationsEnum.Archive, true);
            }
            else
                retVal = false;
            return retVal;
        }

        /// <summary>
        /// Downloads the documents asynchronously.
        /// </summary>
        public override void DownloadDocumentsAsynch()
        {
            BeginDownloadDocuments objBeginDwnDocs = new BeginDownloadDocuments(DownloadDocuments);
            objBeginDwnDocs.BeginInvoke(DownloadCompleteResults,new object());
        }
       
        #endregion  

        #region Private Methods
        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <param name="docAttribs">The doc attribs.</param>
        /// <returns></returns>
        private string GetFilePath(DocumentAttributes docAttribs)
        {
            string filePath = docAttribs.FileFullPath;
            if (docAttribs.Extension == FileExtensionsEnum.GZ || docAttribs.Extension == FileExtensionsEnum.ZIP)
                filePath += "." + docAttribs.Extension;
            return filePath;
        }
        /// <summary>
        /// Downloads file from an FTP connection.
        /// </summary>
        /// <param name="docAttribs">The doc attribs.</param>
        private void DownloadFtp(DocumentAttributes docAttribs)
        {
            FtpWebRequest objFtpWebRequest = null;
            FtpWebResponse objFtpWebResponse = null;
            FileStream objFileStreamTarget = null;
            Stream objSourceStream = null;
            try
            {
                SanitizeFtpUrI(docAttribs);

                objFtpWebRequest = GetFtpWebRequest(docAttribs);
                objFtpWebResponse = (FtpWebResponse)objFtpWebRequest.GetResponse();

                objFileStreamTarget = new FileStream(GetFilePath(docAttribs), FileMode.Create, FileAccess.Write, FileShare.Read, BufferSize);

                //Write the file 
                objSourceStream = objFtpWebResponse.GetResponseStream();
                byte[] buffer = new byte[BufferSize];
                int BytesRead = 0;
                do
                {
                    BytesRead = objSourceStream.Read(buffer, 0, buffer.Length);
                    objFileStreamTarget.Write(buffer, 0, BytesRead);
                } while (BytesRead > 0);

                docAttribs.FileDownloadSuccess = true;
                objFileStreamTarget.Flush();
            }
            catch (AffiliBaseException) { throw; }
            catch (Exception ex)
            {
                throw new AffiliGenericException("DownloadManager.DownloadFtp() Error in DownloadManager.DownloadFtp PPID:" + docAttribs.ProdProgId.ToString(), ex, (int)ApplicationEventsEnum.DownloadStart);
            }
            finally
            {
                if (objFileStreamTarget != null) objFileStreamTarget.Close();
                if (objSourceStream != null) objSourceStream.Close();
                if ((objFtpWebResponse != null) && (objFtpWebResponse.StatusCode != FtpStatusCode.ConnectionClosed))
                    objFtpWebResponse.Close();
            }
        }
        /// <summary>
        /// Downloads the file from HTTP connection.
        /// </summary>
        /// <param name="docAttribs">The doc attribs.</param>
        private void DownloadHTTP(DocumentAttributes docAttribs)
        {
            StreamReader objStreamReader = null;
            try
            {
                objStreamReader = ReadDocument(docAttribs);
                docAttribs.FileDownloadSuccess = WriteToFile(objStreamReader, docAttribs);
            }
            catch (AffiliBaseException) { throw; }
            catch (Exception ex)
            {
                throw new AffiliGenericException("DownloadManager.DownloadHTTP() Error in DownloadManager.DownloadHttP PPID:" + docAttribs.ProdProgId.ToString(), ex, (int)ApplicationEventsEnum.DownloadStart);
            }
            finally
            {
                if (objStreamReader != null) { objStreamReader.Close(); objStreamReader.Dispose(); objStreamReader = null; }
            }
        }
        /// <summary>
        /// Downloads the file zip file from HTTP conenction.
        /// </summary>
        /// <param name="docAttribs">The doc attribs.</param>
        private void DownloadHTTPZip(DocumentAttributes docAttribs)
        {
            WebClient objWebClient = null;
            try
            {
                objWebClient = GetWebClient(docAttribs);
                objWebClient.DownloadFile(docAttribs.FileSourceURI, GetFilePath(docAttribs));
                docAttribs.FileDownloadSuccess = true;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("DownloadManager.DownloadHTTPZip() Error in DownloadManager.DownloadHTTPZip", ex, (int)ApplicationEventsEnum.DownloadStart);
            }
            finally
            {
                if (objWebClient != null) objWebClient.Dispose();
            }
        }
        /// <summary>
        /// Mtehods opens a http web connection to a file URL and reads its content to a streamReader object.
        /// </summary>
        /// <param name="docAttribs">DocumentAttributes object containing file uri attributes</param>
        /// <returns>StreamReader object.</returns>
        private StreamReader ReadDocument(DocumentAttributes docAttribs)
        {
            StreamReader objStreamReader = null;
            WebRequest objWebRequest = null;
            WebResponse myResponse = null;
            try
            {
                objWebRequest = GetWebRequest(docAttribs);
                myResponse = objWebRequest.GetResponse();
                Stream objReadStream = myResponse.GetResponseStream();
                objStreamReader = new StreamReader(objReadStream, docAttribs.DocumentEncoding);
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("DownloadManager.ReadDocument() Error at Reading document process PPID:" + docAttribs.ProdProgId.ToString(), ex, (int)ApplicationEventsEnum.DownloadStart);
            }
            finally
            {
                if (objWebRequest != null) objWebRequest = null;
            }
            return objStreamReader;
        }
        /// <summary>
        /// Gets the FTP web request.
        /// </summary>
        /// <param name="docAttribs">The doc attribs.</param>
        /// <returns></returns>
        private FtpWebRequest GetFtpWebRequest(DocumentAttributes docAttribs)
        {
            FtpWebRequest objFtpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(docAttribs.FileSourceURI, true));
            objFtpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            objFtpWebRequest.KeepAlive = false;
            objFtpWebRequest.Proxy = null;
            objFtpWebRequest.UseBinary = true;
            if (docAttribs.IsAuthenticationRequired)
            {
                NetworkCredential objNetworkCredential = new NetworkCredential(docAttribs.UserId, docAttribs.Password);
                objFtpWebRequest.Credentials = objNetworkCredential;
            }

            return objFtpWebRequest;
        }
        /// <summary>
        /// Gets the web client for zip files download.
        /// </summary>
        /// <param name="docAttribs">The doc attribs.</param>
        /// <returns></returns>
        private WebClient GetWebClient(DocumentAttributes docAttribs)
        {
            WebClient objWebClient = new WebClient();
            if (docAttribs.IsAuthenticationRequired)
            {
                NetworkCredential objNetworkCredential = new NetworkCredential(docAttribs.UserId, docAttribs.Password);
                objWebClient.Credentials = objNetworkCredential;
            }
            return objWebClient;
        }

        /// <summary>
        /// Gets the web request for reading file streams over Http.
        /// </summary>
        /// <param name="docAttribs">The doc attribs.</param>
        /// <returns></returns>
        private WebRequest GetWebRequest(DocumentAttributes docAttribs)
        {
            WebRequest objWebRequest = WebRequest.Create(docAttribs.FileSourceURI);
            objWebRequest.Timeout = 500000;
            if (docAttribs.IsAuthenticationRequired)
            {
                objWebRequest.PreAuthenticate = true;
                NetworkCredential objNetworkCredential = new NetworkCredential(docAttribs.UserId, docAttribs.Password);
                objWebRequest.Credentials = objNetworkCredential;
            }

            return objWebRequest;
        }

        /// <summary>        
        /// If between file and server other directories are available every path must %2F previously written
        /// </summary>
        /// <param name="docAttribs"></param>
        private void SanitizeFtpUrI(DocumentAttributes docAttribs)
        {
            try
            {
                // BugFix: for autoscout and some programs it's not required to encode the ftp path, these progIds are stored in config
                if (!String.IsNullOrEmpty(Utilities.GetAppSettingValue(Constants.AppSettings.FTPUrlWithoutEncoding)))
                {                                        
                    string[] arrProgramsWithoutEncoding = Utilities.GetAppSettingValue(Constants.AppSettings.FTPUrlWithoutEncoding).Split(new char[] { ',' });
                    foreach (string prog in arrProgramsWithoutEncoding)
                    {
                        if (prog == docAttribs.ProdProgId.ToString())
                        {
                            return; // without encode the ftp path
                        }
                    }
                }

                string[] arrFilePath = docAttribs.FileSourceURI.Split((char)'/');

                if (arrFilePath.Length > 4)
                {

                    string strNewFilePath = "ftp://" + arrFilePath[2] + "/";

                    for (int i = 3; i < arrFilePath.Length; i++)
                    {
                        if (i == arrFilePath.Length - 1)
                            strNewFilePath += arrFilePath[i];
                        else
                            strNewFilePath += "%2F" + arrFilePath[i] + "/";
                    }

                    docAttribs.FileSourceURI = strNewFilePath;
                }
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in DownloadManager.SanitizeFtpUrI()", ex, (int)ApplicationEventsEnum.DownloadStart);
            }
        }

        /// <summary>
        /// Selects type of compression based on file extension & Unzips the file.
        /// </summary>
        /// <param name="docAttribs">The doc attribs.</param>
        private void UnzipFile(DocumentAttributes docAttribs)
        {
            if (docAttribs.FileDownloadSuccess)
                switch (docAttribs.Extension)
                {
                    case FileExtensionsEnum.ZIP:
                        UnzipZipFile(docAttribs);
                        break;
                    case FileExtensionsEnum.GZ:
                        UnzipGZipFile(docAttribs);
                        break;
                }
        }
        
        /// <summary>
        /// Unzips the zip file.
        /// </summary>
        /// <param name="docAttribs">The doc attribs.</param>
        private void UnzipZipFile(DocumentAttributes docAttribs)
        {
            byte[] bData;
            ZipEntry objZipEntry;
            ZipInputStream objZipInputStream = null;
            string destfileName = docAttribs.FileDestination + docAttribs.FileName;
            string uzFileName = docAttribs.FileDestination + docAttribs.FileName + docAttribs.ExtensionString;
            try
            {
                if (File.Exists(uzFileName))
                {
                    objZipInputStream = new ZipInputStream(File.OpenRead(uzFileName));

                    while ((objZipEntry = objZipInputStream.GetNextEntry()) != null)
                    {
                        string fileName = Path.GetFileName(objZipEntry.Name);
                        if (fileName != String.Empty)
                        {
                            //fileName = docAttribs.FileDestination + fileName;
                            FileStream streamWriter = File.Create(destfileName);
                            int size = 4096;
                            bData = new byte[size];
                            while (true)
                            {
                                size = objZipInputStream.Read(bData, 0, bData.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(bData, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            streamWriter.Close();
                        }
                    }
                }
                else
                {
                    throw new AffiliGenericException("DownloadManager.UnzipZipFile() File not found for unzip: " + uzFileName);
                }
            }
            catch (Exception ex)
            {
                new AffiliGenericException("Error in DownloadManager.UnzipZipFile()", ex, (int)ApplicationEventsEnum.DownloadEnd).CreateLog();
            }
            finally
            {
                if (objZipInputStream != null)
                    objZipInputStream.Close();
            }
        }

        /// <summary>
        /// Unzips the G zip file.
        /// </summary>
        /// <param name="docAttribs">The doc attribs.</param>
        private void UnzipGZipFile(DocumentAttributes docAttribs)
        {
            GZipInputStream objGZipInputStream = null;
            byte[] bData;
            string destfileName = docAttribs.FileDestination + docAttribs.FileName;
            string gzFileName = docAttribs.FileDestination + docAttribs.FileName + docAttribs.ExtensionString;
            try
            {
                if (File.Exists(gzFileName))
                {
                    objGZipInputStream = new GZipInputStream(File.OpenRead(gzFileName));
                    if (destfileName != String.Empty)
                    {
                        FileStream streamWriter = File.Create(destfileName);
                        int size = 4096;
                        bData = new byte[size];
                        while (true)
                        {
                            size = objGZipInputStream.Read(bData, 0, bData.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(bData, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                        streamWriter.Close();
                    }
                }
                else
                {
                    throw new AffiliGenericException("File not found for unzip: " + destfileName);
                }
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in DownloadManager.UnzipGZipFile()", ex, (int)ApplicationEventsEnum.DownloadEnd);
            }
            finally
            {
                if (objGZipInputStream != null) objGZipInputStream.Close();
            }
        }

        /// <summary>
        /// This method itratively reads a stream and writes it's contents to a file on disk
        /// </summary>
        /// <param name="objStreamReader"></param> Object of Stream reader to be written as a .csv file on disk
        /// <param name="docAttribs">DocumentAttributes object containing file uri attributes</param>
        /// <returns>Boolean true - write process success, False - write process failure</returns>
        private bool WriteToFile(StreamReader objStreamReader, DocumentAttributes docAttribs)
        {
            bool fileWriteSuccess = false;
            StreamWriter objStreamWriter = null;
            try
            {
                if (objStreamReader != null)
                {
                    objStreamWriter = new StreamWriter(GetFilePath(docAttribs));
                    string line = objStreamReader.ReadLine();
                    while (line != null)
                    {
                        objStreamWriter.WriteLine(line);
                        line = objStreamReader.ReadLine();
                    }
                    fileWriteSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error at write document process PPID:" + docAttribs.ProdProgId.ToString(), ex, (int)ApplicationEventsEnum.DownloadStart);
            }
            finally
            {
                if (objStreamWriter != null) { objStreamWriter.Close(); objStreamWriter.Dispose(); objStreamWriter = null; }
                if (objStreamReader != null) { objStreamReader.Close(); objStreamReader.Dispose(); objStreamReader = null; }
            }
            return fileWriteSuccess;
        }
        #endregion
    }
}
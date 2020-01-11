using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data;
using System.Text;
using Affilinet.Business.ProductImport.Cache;
using Affilinet.Business.ProductImport.Common;
using Affilinet.Business.ProductImport.DAO;
using Affilinet.Business.ProductImport.Entity;
using Affilinet.Business.ProductImport.Processor;
using Affilinet.Exceptions;
using System.IO;

namespace Affilinet.Business.ProductImport.Processor
{
    /// <summary>
    /// Main class provides functinalities for processing of CSV/XML/... filesread
    /// 
    /// </summary>
    public class FileProcessor : IDisposable
    {

        /// <summary>
        /// Gets or sets the doc attrib list, this is main object for all product, program and field settings.
        /// One must refer to this object for any settings or information
        /// </summary>
        /// <value>The doc attrib list.</value>
        public List<DocumentAttributes> DocAttribList { get; set; }
        /// <summary>
        /// Gets or sets the obj CSV processor object instance.
        /// </summary>
        /// <value>The obj CSV processor.</value>
        public IProcessor objCSVProcessor { get; set; }
        /// <summary>
        /// Gets or sets the obj product Data Access Object.
        /// </summary>
        /// <value>The obj product DAO.</value>
        protected ProductDAO ObjProductDao { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileProcessor"/> class.
        /// </summary>
        /// <param name="objDocAttribList">The obj doc attrib list.</param>
        /// <param name="objProductDao">The obj product DAO.</param>
        public FileProcessor(List<DocumentAttributes> objDocAttribList, ProductDAO objProductDao)
        {
            this.ObjProductDao = objProductDao;
            this.DocAttribList = objDocAttribList;
        }

        /// <summary>
        /// Starts the processing of file asynch .
        /// </summary>
        public void StartProcessingAsynch()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Determines type of source document and Starts its processing.
        /// </summary>
        public void StartProcessing()
        {
            try
            {
                // counter for qualityAlerting - get the sanitization time
                Stopwatch watch = new Stopwatch();
                watch.Start();

                if (DocAttribList != null && DocAttribList.Count>0)
                {
                
                CacheManager objCacheManager = CacheManager.GetCacheManager();
                 
                objCacheManager.InitLocalCache();

                foreach (DocumentAttributes docAttribs in DocAttribList)
                {
                    //Loop through each program and process respective CSVs aa
                    try
                    {
                        //if (docAttribs.FileDownloadSuccess)
                        {
                            ObjProductDao.UpdateProductProgramStatus(docAttribs.ProdProgId,
                                                                     (int) ProgramImportStatusEnum.PROCESSING_CSV);
                            objCacheManager.DocAttribs = docAttribs;
                            objCacheManager.LoadLocalCache(docAttribs.ProdProgId);
                            switch (docAttribs.DocumentType)
                            {
                                case DocumentTypesEnum.CSV:
                                    objCSVProcessor = new CSVProcessor(docAttribs, objCacheManager);
                                    break;
                                case DocumentTypesEnum.XML:
                                    objCSVProcessor = new XMLProcessor(docAttribs, objCacheManager);
                                    break;
                            }
                            ReadProgramSettings(docAttribs);
                            if (objCSVProcessor != null) objCSVProcessor.ProcessDocumemt();
                            //Free memory
                            Utilities.FreeMemory();
                            ObjProductDao.UpdateProductProgramColumn(docAttribs.ProdProgId,Constants.ProductProgram.LinesChecked,"0");//0 hints that copying is started
                            // get the sanitization time
                            watch.Stop();
                            docAttribs.Report.SanitizationDuration = Math.Round(watch.Elapsed.TotalSeconds, 0).ToString();
                            CopyMoveSanitizedFiles(docAttribs);//copy file to exportCache folder and image csv files to imageFiles folder
                            ObjProductDao.UpdateProductProgramStatus(docAttribs.ProdProgId,
                                                                     (int)
                                                                     ProgramImportStatusEnum.CSV_PROCESSING_COMPLETE, docAttribs.Report.ErrorProducts);
                        }
                    }
                    catch(OutOfMemoryException ex)
                    {
                        if (docAttribs!=null)
                        {
                            docAttribs.Report.Attention = "Out of memory while loading cache";
                            ObjProductDao.UpdateProductProgramStatus(docAttribs.ProdProgId, (int)ProgramImportStatusEnum.CSV_PROCESSING_ERROR);
                        }
                        new AffiliGenericException("Out of memory while loading cache",ex).CreateLog();
                        Utilities.FreeMemory();
                    }
                    catch (AffiliBaseException aex)
                    {
                        aex.CreateLog();
                        Utilities.FreeMemory();
                        ObjProductDao.UpdateProductProgramStatus(docAttribs.ProdProgId,
                                                                 (int) ProgramImportStatusEnum.CSV_PROCESSING_ERROR);
                    }
                    catch (Exception ex)
                    {
                        Utilities.FreeMemory();
                        //No throw statement as error in 1 program must not affect others
                        ObjProductDao.UpdateProductProgramStatus(docAttribs.ProdProgId,
                                                                 (int) ProgramImportStatusEnum.CSV_PROCESSING_ERROR);
                        new AffiliGenericException("FileProcessor.StartProcessing() Error deatils:", ex,
                                                   (int) ApplicationEventsEnum.DocProcessing).CreateLog();
                    }
                    finally
                    {
                        if (objCacheManager != null)
                        { 
                            objCacheManager.UnloadLocalCache();//Truncate cache data
                            objCacheManager.Dispose();
                            objCacheManager.UnloadLocalCache();
                        }
                        if (objCSVProcessor != null) objCSVProcessor.Dispose();
                        objCacheManager = null;
                        objCSVProcessor = null;
                    }
                    Utilities.CreateReportLog(docAttribs.Report.GetReportLogs(docAttribs.ProdProgId.ToString()));
                }//for loop
            }
        }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in ProductImportManager.StartProcessing", ex, (int)ApplicationEventsEnum.DocProcessing);
            }
        }

        private void CopyMoveSanitizedFiles(DocumentAttributes docAttribs)
        {
            // return the actual ProgramStatus. If status equal CSV_PROCESSING_COMPLETE --> copy files for the export job
            //int programImportStatus = ObjProductDao.GetProgramImportStatus(docAttribs.ProdProgId);
            //if (programImportStatus == (int)ProgramImportStatusEnum.CSV_PROCESSING_COMPLETE)
            //{
                CopySanitaisedFiles(docAttribs);
                MoveImageFiles(docAttribs);
            //}
        }
        /// <summary>
        /// Reads the program settings.
        /// </summary>
        public void ReadProgramSettings(DocumentAttributes docAttribs)
        {
            try
            {
                DataSet dsProgramSettings = null;
                //StringBuilder strProgIds = new StringBuilder();
                DataTable dtProgramFields = null;
                DataTable dtProgramSettings = null;
                //foreach (DocumentAttributes docAttribs in DocAttribList)
                //{
                //    if (strProgIds.Length > 0)
                //        strProgIds.Append("," + docAttribs.ProdProgId);
                //    else
                //        strProgIds.Append(docAttribs.ProdProgId);
                //}
                //dsProgramSettings = ObjProductDao.GetProgramSettings(strProgIds.ToString());
                dsProgramSettings = ObjProductDao.GetProgramSettings(docAttribs.ProdProgId.ToString(), docAttribs.DocumentType == DocumentTypesEnum.XML);
                dtProgramSettings = dsProgramSettings.Tables[0];
                dtProgramFields = dsProgramSettings.Tables[1];
                ReadSettingsToCollection(dtProgramSettings);
                ReadFieldsToCollection(dtProgramFields);
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("FileProcessor.ReadProgramSettings() Error deatils:", ex, (int) ApplicationEventsEnum.DocProcessing);
            }
        }
        /// <summary>
        /// Reads the fields to collection.
        /// </summary>
        /// <param name="dtProgramFields">The dt program fields.</param>
        protected void ReadFieldsToCollection(DataTable dtProgramFields)
        {
            try
            {
                dtProgramFields.DefaultView.Sort = Constants.ProductProgram.PordProgId;
                ProgramFields objProgramFields = null;
                foreach (DocumentAttributes docAttribs in DocAttribList)
                {
                    try
                    {
                        dtProgramFields.DefaultView.RowFilter = Constants.ProductProgram.PordProgId + "=" +
                                                                docAttribs.ProdProgId.ToString();
                        foreach (DataRowView oRow in dtProgramFields.DefaultView)
                        {
                            objProgramFields = new ProgramFields
                                                   {
                                                       ImportSchemaID = oRow[Constants.Fields.ImportSchemaID] == DBNull.Value ? 0 : int.Parse(oRow[Constants.Fields.ImportSchemaID].ToString()),
                                                       ColumnOrder = oRow[Constants.Fields.ColumnOrder] == DBNull.Value ? 0 : int.Parse(oRow[Constants.Fields.ColumnOrder].ToString()),
                                                       Title = oRow[Constants.Fields.Title] == DBNull.Value ? "" : oRow[Constants.Fields.Title].ToString(),
                                                       Description = oRow[Constants.Fields.Description] == DBNull.Value ? "" : oRow[Constants.Fields.Description].ToString(),
                                                       MaxLength = oRow[Constants.Fields.MaxLength] == DBNull.Value ? 0 : int.Parse(oRow[Constants.Fields.MaxLength].ToString()),
                                                       DataType = oRow[Constants.Fields.DataType] == DBNull.Value ? "" : oRow[Constants.Fields.DataType].ToString(),
                                                       DestField = oRow[Constants.Fields.DestField] == DBNull.Value ? "" : oRow[Constants.Fields.DestField].ToString(),
                                                       ReverseOrder = oRow[Constants.Fields.RevOrder] == DBNull.Value ? false : bool.Parse(oRow[Constants.Fields.RevOrder].ToString()),
                                                       IsProperty = oRow[Constants.Fields.IsProperty] == DBNull.Value ? false : bool.Parse(oRow[Constants.Fields.IsProperty].ToString()),
                                                       IsNumeric = oRow[Constants.Fields.IsNumeric] == DBNull.Value ? false : bool.Parse(oRow[Constants.Fields.IsNumeric].ToString()),
                                                       IsElement = oRow[Constants.Fields.IsElement] == DBNull.Value ? false : bool.Parse(oRow[Constants.Fields.IsElement].ToString()),
                                                       IsAttribute = oRow[Constants.Fields.IsAttribute] == DBNull.Value ? false : bool.Parse(oRow[Constants.Fields.IsAttribute].ToString()),
                                                       IsProductTag = oRow[Constants.Fields.IsProductTag] == DBNull.Value ? false : bool.Parse(oRow[Constants.Fields.IsProductTag].ToString()),
                                                       ParentTag = CheckForNull(oRow[Constants.Fields.ParentTitle], "").ToString() 
                                                   };
                            docAttribs.ProgFields.Add(objProgramFields);
                        }
                    }
                    catch (Exception ex)
                    {//Must not break for loop
                        new AffiliGenericException("FileProcessor.ReadFieldsToCollection()-foreach, Error details: ", ex, (int)ApplicationEventsEnum.DocProcessing).CreateLog();
                    }
                }//foreach end
                dtProgramFields.DefaultView.RowFilter = string.Empty;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("FileProcessor.ReadFieldsToCollection() Error deatils:", ex, (int)ApplicationEventsEnum.DocProcessing);
            }
        }
        /// <summary>
        /// Checks for null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defValue">The def value.</param>
        /// <returns></returns>
        private object CheckForNull(object value, object defValue)
        {
            if (value == DBNull.Value || value == null)
                return defValue;
            else
                return value;
        }
        /// <summary>
        /// Reads the program settings to collection.
        /// </summary>
        /// <param name="dtProgramSettings">The dt program settings.</param>
        protected void ReadSettingsToCollection(DataTable dtProgramSettings)
        {
            try
            {
                ProgramSettings objProgSett = null;
                IEnumerable<DocumentAttributes> docAttribs = null;
                DocumentAttributes tmp = null;
                foreach (DataRow oRow in dtProgramSettings.Rows)
                {
                    try
                    {
                        objProgSett = new ProgramSettings
                                          {
                                              Title = CheckForNull(oRow[Constants.ProgramSettings.Title], string.Empty).ToString(),
                                              ImportSchemaID = CheckForNull(oRow[Constants.ProgramSettings.ImportSchemaID], string.Empty).ToString(),
                                              ProgramID = CheckForNull(oRow[Constants.ProgramSettings.ProgramID], string.Empty).ToString(),
                                              Description = CheckForNull(oRow[Constants.ProgramSettings.Description], string.Empty).ToString(),
                                              FldSep = (char)Convert.ToSByte(CheckForNull(oRow[Constants.ProgramSettings.FieldSeperator], "").ToString()),
                                              FieldSeperator = ((char)int.Parse(CheckForNull(oRow[Constants.ProgramSettings.FieldSeperator], "").ToString())).ToString(),
                                              LineSeperator = CheckForNull(oRow[Constants.ProgramSettings.LineSeperator], "").ToString(),
                                              TextQualifier = CheckForNull(oRow[Constants.ProgramSettings.TextQualifier], "").ToString(),
                                              //TextQual = CheckForNull(oRow[Constants.ProgramSettings.TextQualifier], "").ToString().Length>0?CheckForNull(oRow[Constants.ProgramSettings.TextQualifier], "").ToString()[0]:null,
                                              DateFormat = CheckForNull(oRow[Constants.ProgramSettings.DateFormat], Utilities.GetAppSettingValue(Constants.AppSettings.DefDateFormat)).ToString(),
                                              DecimalSymbol = CheckForNull(oRow[Constants.ProgramSettings.DecimalSymbol], ".").ToString(),
                                              CategorySeperator = CheckForNull(oRow[Constants.ProgramSettings.CategorySeperator], "").ToString(),
                                              CSVHeadline = CheckForNull(oRow[Constants.ProgramSettings.CSVHeadline], "").ToString(),
                                              Active = bool.Parse(CheckForNull(oRow[Constants.ProgramSettings.Active], 0).ToString()),
                                              Headlines = int.Parse(CheckForNull(oRow[Constants.ProgramSettings.Headlines], 0).ToString()),
                                              ImportType = CheckForNull(oRow[Constants.ProgramSettings.ImportType], "").ToString(),
                                              ProcessedFilePath = Utilities.GetAppSettingValue(Constants.AppSettings.CSVSaveLocation) +
                                                                  CheckForNull(oRow[Constants.ProgramSettings.ProgramID], "").ToString() + "/",
                                              DigitGroupSymbol = CheckForNull(oRow[Constants.ProgramSettings.DigitGroupingSymbol], "").ToString(),
                                              DefaultAffiliCat = CheckForNull(oRow[Constants.ProgramSettings.DefaultAffiliCat], "").ToString(),
                                              ImageNumToUse = int.Parse(CheckForNull(oRow[Constants.ProgramSettings.ImageNb], 0).ToString()),
                                              Encoding = oRow[Constants.ProgramSettings.Encoding] == DBNull.Value || oRow[Constants.ProgramSettings.Encoding].ToString().Equals("") ? Encoding.Default : Encoding.GetEncoding(oRow[Constants.ProgramSettings.Encoding].ToString()),
                                              XmlProductTag = CheckForNull(oRow[Constants.ProgramSettings.XmlProductTag], "").ToString(),
                                              PlatformID = (int)CheckForNull(oRow[Constants.ProgramSettings.PlatformID],AffilinetPlatforms.None),
                                              EncodeDeeplink = (int)CheckForNull(oRow[Constants.ProgramSettings.EncodeDeeplink], 0),
                                          };
                        int tmpProgId = int.Parse(oRow[Constants.ProductProgram.PordProgId].ToString());
                        docAttribs = from item in DocAttribList
                                     where item.ProdProgId == tmpProgId
                                     select item;
                        docAttribs.ToList()[0].ProgSettings = objProgSett;
                    }
                    catch (Exception ex)
                    {//Must not break for loop
                        new AffiliGenericException("FileProcessor.ReadSettingsToCollection()-foreach, Error details: ", ex, (int)ApplicationEventsEnum.DocProcessing).CreateLog();
                    }
                }//foreach end
            }catch(FormatException ex)
            {
                throw new AffiliGenericException("FileProcessor.ReadSettingsToCollection() format FieldSeperator in program setting is incoreect", ex);
            }
        }

        private void MoveImageFiles(DocumentAttributes docAttribs)
        {
            try
            {
                string exportTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string destinationFileName = exportTimeStamp + "_"+ docAttribs.ProdProgId + Constants.FileNaming.ProductImageFileExtention;
                
                string imageFileName = docAttribs.FileFullPath + Constants.FileNaming.ProductImageFileExtention;
                if (File.Exists(imageFileName))
                    Utilities.MoveImageFile(imageFileName,destinationFileName,docAttribs.ProdProgId.ToString(), FileLocationsEnum.ImageCsvs);

                int j = 1;
                imageFileName = docAttribs.FileFullPath + "." + j.ToString() + Constants.FileNaming.ImageDeleteCsvExtention;
                while (File.Exists(imageFileName))
                {
                    destinationFileName = exportTimeStamp + "_" + docAttribs.ProdProgId + "." + j.ToString()+ Constants.FileNaming.ImageDeleteCsvExtention;
                    Utilities.MoveImageFile(imageFileName, destinationFileName,docAttribs.ProdProgId.ToString(), FileLocationsEnum.ImageCsvs);
                    j++;
                    imageFileName = docAttribs.FileFullPath + "." + j.ToString() + Constants.FileNaming.ImageDeleteCsvExtention;
                }
                ObjProductDao.AddToImageImportProgramFilesCache(docAttribs.ProdProgId, exportTimeStamp);
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("FileProcessor.MoveImageFiles() Error deatils:", ex);
            }
        }

        /// <summary>
        /// Copy the sanitaised files to an Folder for the export process
        /// </summary>
        /// <param name="docAttribs"></param>
        protected void CopySanitaisedFiles(DocumentAttributes docAttribs)
        {
            try
            {
                Utilities.CreateInfoLog("Start CopyFileToLocalFolder process in FileProcessor.StartProcessing(). ProdProgId: " + docAttribs.ProdProgId.ToString(), ApplicationEventsEnum.CopyExportFiles);
                                
                string expFolder = Utilities.GetAppSettingValue(Constants.AppSettings.ExportFilesLocation);
                if (expFolder == null || expFolder.Length == 0)
                {
                    new AffiliGenericException("FileProcessor.CopySanitaisedFiles(). Error on get the ExportFilesLocation path from config. Check config file!").CreateLog();                    
                    return;
                }

                string exportTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string destinationFilenName = exportTimeStamp + "_" + docAttribs.ProdProgId.ToString() + "_{0}{1}";

                string[] fileExtensions = Utilities.GetFileExtensionsForExport();
                foreach (string expFileExt in fileExtensions)
                {
                    int i = 1;
                    string fileName = docAttribs.FileFullPath + "." + i.ToString() + expFileExt;
                    while (File.Exists(fileName))
                    {
                        // move the all files from type notChanged.csv else copy the files (because of memory issues on live servers)
                        if (expFileExt == Constants.FileNaming.ProductNotchangedCsvExtention)
                        {
                            Utilities.MoveFileToFolder(fileName, expFolder, docAttribs.ProdProgId.ToString(), String.Format(destinationFilenName, i.ToString(), expFileExt));          
                        }
                        else
                        {
                            Utilities.CopyFileToFolder(fileName, expFolder, docAttribs.ProdProgId.ToString(), String.Format(destinationFilenName, i.ToString(), expFileExt));    
                        }

                        i++;
                        fileName = docAttribs.FileFullPath + "." + i.ToString() + expFileExt;
                    }
                }

                // Insert into ProdExportProgramFilesCache table with status new 
                ObjProductDao.AddToProdExportProgramFilesCache(docAttribs.ProdProgId, exportTimeStamp);

                Utilities.CreateInfoLog("End CopyFileToLocalFolder process succesfull in FileProcessor.StartProcessing(). ProdProgId: " + docAttribs.ProdProgId.ToString(), ApplicationEventsEnum.CopyExportFiles);
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("FileProcessor.CopySanitaisedFiles() Error deatils:", ex);
            }
        }

        /// <summary>
        /// Reads the Export folder(s) into an array.
        /// </summary>
        /// <returns></returns>
        

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
                    if (DocAttribList != null)
                    {
                        DocAttribList.Clear();
                        DocAttribList = null;
                    }
                    if (objCSVProcessor != null)
                    {
                        objCSVProcessor.Dispose();

                    }
                    ObjProductDao = null;
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using Affilinet.Business.ProductExport.Common;
using Affilinet.Business.ProductExport.DAO;
using Affilinet.Business.ProductExport.Entity;
using Affilinet.Business.ProductExport.Converters;
using Affilinet.Exceptions;
using Affilinet.Utility.Logging;
using Constants = Affilinet.Business.ProductExport.Common.Constants;

namespace Affilinet.Business.ProductExport
{
    public class ExportRequestManager
    {
        private ExportDao objExportDao = new ExportDao();
        private ExportfileTypesEnum exportFileType;
        /// <summary>
        /// Starts the export.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="publisherId">The publisher id.</param>
        /// <param name="ExportfileType">Type of the exportfile.</param>
        /// <param name="isZipped">if set to <c>true</c> [is zipped].</param>
        /// <param name="context">The context.</param>
        public void StartExport(String prodProgId, String publisherId, ExportfileTypesEnum ExportfileType, bool isZipped, HttpContext context, String outputFile)
        {
            if (prodProgId != null)
            {
                // Bug 13929: store the ExportFileType and only sort the ExportColumns for csv exports
                exportFileType = ExportfileType; 

                ExportAttributes objExportAttributes = new ExportAttributes(publisherId);
                objExportAttributes.HttpContext = context;
                if (ReadPublisherAndFileLocationSettings(objExportAttributes, prodProgId, publisherId))
                {
                    BaseConverter objConverter = null;
                    switch (ExportfileType)
                    {
                        case ExportfileTypesEnum.CSV:
                            objConverter = new CsvToCsvConverter(objExportAttributes, false, outputFile);
                            break;
                        case ExportfileTypesEnum.XML:
                            objConverter = new CsvToXmlConverter(objExportAttributes, false, outputFile);
                            break;
                        case ExportfileTypesEnum.CSVZip:
                            objConverter = new CsvToCsvConverter(objExportAttributes, true, outputFile);
                            objConverter.IsZipped = true;
                            break;
                        case ExportfileTypesEnum.XMLZip:
                            objConverter = new CsvToXmlConverter(objExportAttributes, true, outputFile);
                            objConverter.IsZipped = true;
                            break;
                    }
                    if (objConverter != null) objConverter.ProcessDocumemt();
                }
            }
        }
        private bool ReadPublisherAndFileLocationSettings(ExportAttributes objExportAttributes, String prodProgId, String publisherId)
        {
            DataSet dsSettings =  objExportDao.ReadPublisherAndFileLocationSettings(prodProgId, publisherId);
            DataTable dtPubSet = dsSettings.Tables[0];
            DataTable dtFileList = dsSettings.Tables[1];
            DataTable dtProgSettings = dsSettings.Tables[2];

            ReadPublisherSetting(dtPubSet, objExportAttributes);
            ReadProgramSettings(dtProgSettings, objExportAttributes);
            ReadExportConfiguration(objExportAttributes);

            // commented out by FAN:
            // Always call ReadFileNames also if we have no TimeStamp returned from db; because it's possible that we get old data from Standby DB
            // now we check the filesystem for existing csv files for the export process
            //if (dtFileList.Rows.Count > 0)
            //{
            ReadFileNames(dtFileList, objExportAttributes, prodProgId);
            //}
            return true;
        }

        private void ReadExportConfiguration(ExportAttributes objExportAttributes)
        {
            try
            {
                if (objExportAttributes.HttpContext.Cache[Constants.Generic.CacheKeyColumnConfig] == null)
                {
                    DataSet dsConfigs = objExportDao.ReadExportconfiguration();
                    DataTable dtClickUrls = dsConfigs.Tables[0];
                    DataTable dtProductColumns = dsConfigs.Tables[1];
                    DataTable dtXmlTagGroups = dsConfigs.Tables[2];

                    //Read click urls
                    foreach (DataRow dr in dtClickUrls.Rows)
                    {
                        objExportAttributes.PlateFormClickUrls.Add((int)dr[Constants.ProductClickURL.PlatformID], (string)dr[Constants.ProductClickURL.ClickURL]);
                    }

                    //read column configuration
                    foreach (DataRow dr in dtProductColumns.Rows)
                    {
                        ExportColumn objExpcol = new ExportColumn()
                        {
                            ProductTableColumn = CheckForNull(dr[Constants.ProdExpConfig.ProductTableColumn], "").ToString(),
                            ExportCsvColumn = CheckForNull(dr[Constants.ProdExpConfig.ExportCsvColumn], "").ToString(),
                            ExportXmlColumn = CheckForNull(dr[Constants.ProdExpConfig.ExportXmlColumn], "").ToString(),
                            XmlGroup = CheckForNull(dr[Constants.ProdExpConfig.XmlGroup], "").ToString(),
                            ColumnValue = CheckForNull(dr[Constants.ProdExpConfig.ColumnValue], "").ToString(),
                            IsCalculated = (bool)CheckForNull(dr[Constants.ProdExpConfig.IsCalculated], ""),
                            IsDefault = (bool)CheckForNull(dr[Constants.ProdExpConfig.IsDefault], false),
                            IsActive = (bool)CheckForNull(dr[Constants.ProdExpConfig.IsActive], false),
                            FieldLength = int.Parse(CheckForNull(dr[Constants.ProdExpConfig.FieldLength], 0).ToString()),
                            DataType = CheckForNull(dr[Constants.ProdExpConfig.DataType], "").ToString()
                        };
                        String tmpcols = CheckForNull(dr[Constants.ProdExpConfig.ComputedColumns], "").ToString();
                        if (tmpcols != "")
                        {
                            String[] ComputedCols = tmpcols.Split(',');
                            foreach (String col in ComputedCols)
                                objExpcol.ComputedColumns.Add(col);
                        }
                        tmpcols = CheckForNull(dr[Constants.ProdExpConfig.ColumnsDefault], "").ToString();
                        if (tmpcols != "")
                        {
                            String[] DefaultCols = tmpcols.Split(',');
                            foreach (String col in DefaultCols)
                                objExpcol.ColumnsDefault.Add(col);
                        }
                        objExportAttributes.ExportColumns.Add(objExpcol);
                    }

                   
                    //Read xml tag groups
                    foreach (DataRow dr in dtXmlTagGroups.Rows)
                    {
                        if (dr[Constants.ExportXmlTagGroupsTable.XmlTag] != DBNull.Value)
                            objExportAttributes.XmlTagGroups.Add(dr[Constants.ExportXmlTagGroupsTable.XmlTag].ToString());
                    }

                    Double absExpirationMinutes = 60d;
                    double.TryParse(Utilities.GetAppSettingValue(
                                    Constants.AppSettings.AbsoluteExpirationMinutesForExportConfiguration), out absExpirationMinutes);
                    objExportAttributes.HttpContext.Cache.Insert(Constants.Generic.CacheKeyColumnConfig, objExportAttributes.ExportColumns, null, DateTime.Now.AddMinutes(absExpirationMinutes), Cache.NoSlidingExpiration);
                    objExportAttributes.HttpContext.Cache.Insert(Constants.Generic.CacheKeyClickUrls, objExportAttributes.PlateFormClickUrls, null, DateTime.Now.AddMinutes(absExpirationMinutes), Cache.NoSlidingExpiration);
                    objExportAttributes.HttpContext.Cache.Insert(Constants.Generic.CacheKeyXmlTagGroups, objExportAttributes.XmlTagGroups, null, DateTime.Now.AddMinutes(absExpirationMinutes), Cache.NoSlidingExpiration);
                }
                else
                {
                    objExportAttributes.ExportColumns = (List<ExportColumn>)objExportAttributes.HttpContext.Cache[Constants.Generic.CacheKeyColumnConfig];
                    objExportAttributes.PlateFormClickUrls = (Dictionary<int, String>)objExportAttributes.HttpContext.Cache[Constants.Generic.CacheKeyClickUrls];
                    objExportAttributes.XmlTagGroups = (List<String>)objExportAttributes.HttpContext.Cache[Constants.Generic.CacheKeyXmlTagGroups];
                }

                // Bug 13929: only sort the ExportColumns for csv exports
                if (exportFileType == ExportfileTypesEnum.CSV || exportFileType == ExportfileTypesEnum.CSVZip)
                    SortExportColumns(objExportAttributes);

            }catch(Exception ex)
            {
                throw new AffiliGenericException("ExportRequestManager.ReadExportConfiguration()",ex);
            }
        }


        /// <summary>
        /// Sort the columns in the same order like the publisher wants to have (table [PartnerCSVFormat]) and add the not selected columns at the end; Properties must always be at the End
        /// </summary>
        private void SortExportColumns(ExportAttributes ObjExportAttributes)
        {
            try
            {
                // temporary list to store the sorted columns
                List<ExportColumn> sortedExportColumns = new List<ExportColumn>();

                if (ObjExportAttributes.PubSettings.Columns.Count == 0)
                    return; // nothing to do if no publisher fields set; return default ExportColumns

                foreach (var pubField in ObjExportAttributes.PubSettings.Columns)
                {
                    foreach (ExportColumn exportColumn in ObjExportAttributes.ExportColumns)
                    {

                        if (sortedExportColumns.Contains(exportColumn) ||
                            exportColumn.ExportCsvColumn.ToUpper() == Constants.ExportProduct.Properties.ToUpper())
                            continue;


                        if (pubField.ToUpper() == exportColumn.ExportCsvColumn.ToUpper())
                        {
                            sortedExportColumns.Add(exportColumn);
                            break;
                        }
                    }
                }

                // at the end loop trough all fields again and add the fields which are not already in sortedExportColumns; put it at the end that we loose no fields; in this loop properties field should also be added
                foreach (ExportColumn exportColumn in ObjExportAttributes.ExportColumns)
                {
                    if (!sortedExportColumns.Contains(exportColumn))
                    {
                        sortedExportColumns.Add(exportColumn);
                    }
                }


                ObjExportAttributes.ExportColumns = sortedExportColumns;
            }
            catch (Exception ex)
            {
                new AffiliGenericException("ExportRequestManager.SortExportColumns()", ex).CreateLog();
            }
        }

        private void ReadProgramSettings(DataTable dtProgSettings, ExportAttributes objExportAttributes)
        {
            try
            {
                objExportAttributes.ProgSettings = new ProgramSettings()
                {
                    ImageNumToUse = int.Parse(CheckForNull(dtProgSettings.Rows[0][Constants.ProgramSettings.ImageNb], "1").ToString()),
                    PlatformID = int.Parse(CheckForNull(dtProgSettings.Rows[0][Constants.ProgramSettings.PlatformID], "").ToString()),
                    DiURL = (bool)CheckForNull(dtProgSettings.Rows[0][Constants.ProgramSettings.DiURL], false),
                    TextLinkNb = int.Parse(CheckForNull(dtProgSettings.Rows[0][Constants.ProgramSettings.TextLinkNb], "").ToString()),
                    ProgramID = int.Parse(CheckForNull(dtProgSettings.Rows[0][Constants.ProgramSettings.ProgramID], "").ToString())
                };
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("ExportRequestManager.ReadPublisherAndFileLocationSettings() Invalid program settings:", ex);
            }
        }
        private void ReadFileNames(DataTable dtFileList, ExportAttributes objExportAttributes, String prodProgId)
        {
            if (!dtFileList.Columns.Contains(Constants.ProdExportProgramFilesCache.LastModified))
                dtFileList.Columns.Add(Constants.ProdExportProgramFilesCache.LastModified);
            //Read file names
            if (dtFileList.Rows.Count>0)
                objExportAttributes.ProgramFiles = new ProgramFilesList()
                                                   {
                                                       ModifiedDateTime = CheckForNull(dtFileList.Rows[0][Constants.ProdExportProgramFilesCache.LastModified], "").ToString(),
                                                       ExportTimeStamp = dtFileList.Rows.Count > 0 ? CheckForNull(dtFileList.Rows[0][Constants.ProdExportProgramFilesCache.ExportTimeStamp], ".").ToString() : ".",
                                                       ProdProgId = prodProgId
                                                   };
            else
                objExportAttributes.ProgramFiles = new ProgramFilesList()
                                                  {
                                                           ModifiedDateTime = "",
                                                           ExportTimeStamp="",
                                                           ProdProgId = prodProgId
                                                  };
        }

        private void ReadPublisherSetting(DataTable dtPubSet, ExportAttributes objExportAttributes)
        {
            if (dtPubSet == null || dtPubSet.Rows.Count == 0)
            {
                new AffiliGenericException("ReadPublisherSetting(): Publisher settings not found in PartnerCsvFormat table, request aborted. Publisher:" + objExportAttributes.PublisherId).CreateLog();
                objExportAttributes.PubSettings = new PublisherSettings()
                {
                    DeciSeperator = ".",
                    FieldSeperator =  ";",
                    FieldQualifier =  "\"",
                    ReplaceEmptyWithNull =  false,
                    ImageWithSizeOnly =  false,
                    DateFormat = "104",
                };
            }
            else
            {
                //Read Publisher settings
                objExportAttributes.PubSettings = new PublisherSettings()
                {
                    DeciSeperator = CheckForNull(dtPubSet.Rows[0][Constants.PartnerCSVFormat.DecimalSymbol], ".").ToString(),
                    FieldSeperator = CheckForNull(dtPubSet.Rows[0][Constants.PartnerCSVFormat.FieldSeparator], "|").ToString(),
                    FieldQualifier = CheckForNull(dtPubSet.Rows[0][Constants.PartnerCSVFormat.TextQualifier], "").ToString(),
                    ReplaceEmptyWithNull = (bool)CheckForNull(dtPubSet.Rows[0][Constants.PartnerCSVFormat.Nulls], false),
                    ImageWithSizeOnly = (bool)CheckForNull(dtPubSet.Rows[0][Constants.PartnerCSVFormat.ImagesWithSize], false),
                    DateFormat = CheckForNull(dtPubSet.Rows[0][Constants.PartnerCSVFormat.DateFormat], "104").ToString(),
                };

                String[] columns = CheckForNull(dtPubSet.Rows[0][Constants.PartnerCSVFormat.CSVFields], "").ToString().Split(',');
                foreach (String col in columns)
                {
                    switch (col)
                    {
                        case Constants.PublisherSettingsColumns.ProductProgram_ID:
                            objExportAttributes.PubSettings.Columns.Add(Constants.ExportProduct.ProductProgramID.ToUpper());
                            break;
                        case Constants.PublisherSettingsColumns.shippingCosts:
                            objExportAttributes.PubSettings.Columns.Add(Constants.ExportProduct.Shipping.ToUpper());
                            break;
                        default:
                            objExportAttributes.PubSettings.Columns.Add(col.ToUpper());
                            break;
                    }
                }
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
    }
}

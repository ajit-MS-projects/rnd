using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AffiliErrorIndexing;
using Affilinet.Business.ProductImport.Cache;
using Affilinet.Business.ProductImport.Common;
using Affilinet.Business.ProductImport.Entity;
using Affilinet.Business.ProductImport.Processor;
using Affilinet.Exceptions;
using System.Web;
using CommonUtilities = Affili.ProductData.Common.Utilities;



namespace Affilinet.Business.ProductImport.Processor
{
    /// <summary>
    /// Class provides interface for processing an input csv file and generate output documents.
    /// </summary>
    /// </summary>
    public class CSVProcessor : BaseProcessor
    {
        

        /// <summary>
        /// Initializes a new instance of the <see cref="CSVProcessor"/> class.
        /// </summary>
        /// <param name="docAttribs">The doc attribs.</param>
        /// <param name="oCacheManager">The o cache manager.</param>
        public CSVProcessor(DocumentAttributes docAttribs, CacheManager oCacheManager) : base(docAttribs, oCacheManager) { }
        private bool IsProductInMultiLine { get; set; }
        protected long NoOfLinesWithExtraColumns { get; set; }
        private long NoOfLinesWithInvalidUrls { get; set; }
        private long NoOfLinesWithoutCatText{ get; set; }
        private long NoOfLinesWithScanLines { get; set; }
        private bool IsScanLineCharactersBeingUsed { get; set; }
        private int BasePricesWithoutSuffix { get; set; }
        /// <summary>
        /// Main method to initiate processing of source CSV documemt.
        /// </summary>
        public override void ProcessDocumemt()
        {
            try
            {
               if (File.Exists(DocAttribs.FileFullPath))
                {
                    NoOfLinesWithExtraColumns = 0;
                    NoOfLinesWithInvalidUrls = 0;
                    NoOfLinesWithoutCatText = 0;
                    BasePricesWithoutSuffix = 0;
                    OpenFileForRead();
                    base.PrepareHeader();
                    SkipHeaderRows(); //Skipping header rows in csv
                    ReadDataLines();
                    base.FinishWritingCSVFiles(); //Write to csv files
                    if (NoOfLinesWithExtraColumns > 0) DocAttribs.Report.Attention = "Number of columns in CSV are more than expected:" + NoOfLinesWithExtraColumns;
                    if (NoOfLinesWithInvalidUrls > 0) DocAttribs.Report.Attention = "There were Products with Invalid URL:" + NoOfLinesWithInvalidUrls;
                    if (NoOfLinesWithoutCatText > 0) DocAttribs.Report.Attention = "There were Products Without Category Text:" + NoOfLinesWithoutCatText;
                }
                else
                {
                    throw new AffiliGenericException("Source CSV File not found PPID:" + DocAttribs.ProdProgId + ":" + DocAttribs.FileFullPath,(int)ApplicationEventsEnum.DocProcessing);
                }
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.ProcessDocumemt() PPID:" + DocAttribs.ProdProgId, ex, (int)ApplicationEventsEnum.DocProcessing);
            }
            finally
            {
                if (SrCsvSource != null)
                {
                    SrCsvSource.Close();
                    SrCsvSource.Dispose();
                    SrCsvSource = null;
                }
            }
        }
        /// <summary>
        /// Virtual method Reads the line from file.
        /// </summary>
        /// <param name="strLine">The STR line.</param>
        /// <returns></returns>
        protected virtual bool ReadLineFromFile(out string strLine)
        {
            return ((strLine = SrCsvSource.ReadLine()) != null);
        }
        /// <summary>
        /// Reads the data lines from source CSV and sanitizes them to product objects.
        /// </summary>
        protected void ReadDataLines()
        {
            string strLine = String.Empty;
            string[] fields = null;
            AffiliGenericException lineException = null;
            int continuousLineErrors = 0;
            int maxContErrors = 1000;
            int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.MaxContinuousErrors), out maxContErrors);
            try
            {
                CheckiFProductsAreInMultiLine();
                Product objProduct = null;
                IsScanLineCharactersBeingUsed = false;
                while (ReadLineFromFile(out strLine))
                {
                    DocAttribs.Report.TotalProductsProcessed++;
                    base.CheckForThreadSleep();
                    
                    try
                    {
                        //strLine = HttpUtility.HtmlDecode(strLine);
                        //HTML Decode Line if ((txtTextQualifier.Text.Length == 0) && (cFldSep == ';'))
                        fields = ReadLineAndFixColCount(strLine);
                        int cnt = 0;
                        if (objProduct != null)
                        {
                            objProduct.Initialize(DtProduct, DocAttribs.ProdProgId);
                        }
                        else
                            objProduct = new Product(DtProduct, DocAttribs.ProdProgId);

                        foreach (ProgramFields prgFld in DocAttribs.ProgFields) //Loop Through Cols Of Line
                        {
                            if (prgFld.IsProperty)
                                PrepareProperty(objProduct, cnt, prgFld, fields);
                            if (!String.IsNullOrEmpty(prgFld.DestField))
                                PrepareProduct(objProduct, prgFld, fields[cnt]);
                            cnt++;
                        }
                        SetMiscProductFields(objProduct);
                        if (IsValidProduct(objProduct))
                        {
                            ObjCSVDataCollections.Products.Add(objProduct);
                            objProduct = null;
                        }

                        if (ObjCSVDataCollections.Products.Count >= base.ProductsToCheckIfAbortRequested)
                        {//Check if abort requested by admin
                            if (ObjProductDao.GetProductProgramStatus(DocAttribs.ProdProgId) == (int)ProgramImportStatusEnum.ABORT_REQUESTED)
                                throw new AffiliAbortRequestedException("Abort requested by admin");

                        }
                        if (ObjCSVDataCollections.Products.Count >= base.ProductSetCount)
                        {
                            Utilities.CreateDebugLog(
                               "Errors in lines:" + DocAttribs.Report.ErrorProducts + " , Total lines processed:" +
                               DocAttribs.Report.TotalProductsProcessed + ":Program:" + DocAttribs.ProdProgId.ToString(),
                               ApplicationEventsEnum.DocProcessing);
                            base.WriteToCSVFiles(); //Write to csv files
                            if (ObjCSVDataCollections != null) ObjCSVDataCollections.Dispose();
                            ObjCSVDataCollections = new CSVDataCollections();
                            base.PrepareHeader();
                            Utilities.FreeMemory();
                        }
                        for (int i = 0; i < fields.Length; i++) {
                            fields[i] = null; }
                        continuousLineErrors = 0;//Success with product line reset cont error count
                    }
                    catch(OutOfMemoryException ex)
                    {
                        AffiliGenericException objEx = new AffiliGenericException("Out of memory after " + DocAttribs.Report.TotalProductsProcessed + " products ", ex);
                        objEx.CreateLog();
                        SaveErrorIndex(DocAttribs.ProdProgId, objEx);
                        DocAttribs.Report.Attention = " ::Out of memory after processing " + DocAttribs.Report.TotalProductsProcessed + " products::";
                        ObjCacheManager.Dispose();
                        Utilities.FreeMemory();
                        break;
                    }
                    catch(AffiliAbortRequestedException ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {//Error in 1 line must not break processing of rest of lines
                        continuousLineErrors++;
                        if (continuousLineErrors >= maxContErrors)//Abondon prrogram in max continuous errors are greater than configured
                        {
                            DocAttribs.Report.Attention = "There were continuous " + continuousLineErrors + "errors; program sanitization stopped." + ex.Message;
                            throw new AffiliGenericException("There were continuous " + continuousLineErrors + "errors; program sanitization stopped.",ex);
                        }
                        Utilities.FreeMemory();
                        DocAttribs.Report.ErrorProducts++;
                        lineException = new AffiliGenericException(
                            "Error in CSVProcessor.ReadDataLines().loop in Line:" + DocAttribs.Report.TotalProductsProcessed.ToString() + " Article:" +
                            (objProduct != null ? objProduct.ArtNumber : "") + 
                                                                        ":Program:" + DocAttribs.ProdProgId, ex,
                            (int) ApplicationEventsEnum.DocProcessing);
                    }
                    strLine = null;
                }//End while (ReadLineFromFile(out strLine))
                if (IsScanLineCharactersBeingUsed)
                    CommonUtilities.CreateEmailLog("ScanLineCharacters() is being used for splitting Product Program Id:" + DocAttribs.ProdProgId + " Total such line found:" + NoOfLinesWithScanLines);
                if (lineException != null) lineException.CreateLog();
                if(continuousLineErrors>=DocAttribs.Report.TotalProductsProcessed)
                {
                    DocAttribs.Report.Attention = "All Products " + DocAttribs.Report.TotalProductsProcessed + " with " + continuousLineErrors + " errors. " + (lineException == null ? "" : lineException.Message);
                    throw new AffiliGenericException(" All Products " + DocAttribs.Report.TotalProductsProcessed + " with " + continuousLineErrors + " errors; program sanitization stopped.", lineException);
                }
                if(BasePricesWithoutSuffix > 0)
                    DocAttribs.Report.Attention += "BasePrices WithoutSuffix, total such products=" + BasePricesWithoutSuffix;
                ObjProductDao.UpdateErrorIndex(DocAttribs.ProdProgId, Constants.ErrorIndexMasterListNames.OtherSanitisationError, 0);
                DocAttribs.Report.TotalShopCategories = ObjCacheManager.ShopCategoryCount;
                DocAttribs.Report.ProductsWithInvalidDeeplinkUrl = (int)NoOfLinesWithInvalidUrls;

                // start the error index calculation
                ErrorIndexingManager.StartErrorIndexCalculation(DocAttribs.ProdProgId.ToString());

            }
            catch (AffiliBaseException aex)
            {//Add program id to message
                SaveErrorIndex(DocAttribs.ProdProgId, aex);
                throw new AffiliGenericException(aex.Message + ":Program:" + DocAttribs.ProdProgId, aex.InnerException);
            }
            catch(Exception ex)
            {
                SaveErrorIndex(DocAttribs.ProdProgId, ex);
                throw new AffiliGenericException(
                            "Error in CSVProcessor.ReadDataLines() in Line:" + DocAttribs.Report.TotalProductsProcessed.ToString() + " Article:" +
                                                                        ":Program:" + DocAttribs.ProdProgId, ex,
                            (int)ApplicationEventsEnum.DocProcessing);
            }
        }
        private void SaveErrorIndex(int prodProgId, Exception ex)
        {
            ObjProductDao.UpdateErrorIndex(prodProgId, Constants.ErrorIndexMasterListNames.OtherSanitisationError, 1);
            ObjProductDao.UpdateLastExceptionAndGetHeaderCheckEnabledFlag(prodProgId, ex.ToString());
        }
        #region remove illegal characters from csv-line
        private String RemoveIllegalCharaters(String line)
        {
            try
            {
                if (!string.IsNullOrEmpty(line))
                {
                    for (char c = (char) 0; c < (char) 32; c++)
                    {
                        if (c != (char) 9)
                        {
                            line = line.Replace(c.ToString(), "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException(
                            "Error in CSVProcessor.RemoveIllegalCharaters() in Line:" + DocAttribs.Report.TotalProductsProcessed.ToString() + " Article:" +
                                                                        ":Program:" + DocAttribs.ProdProgId, ex,
                            (int)ApplicationEventsEnum.DocProcessing);
            }
            return line;
        }

        #endregion

        #region Setup Product fields
        /// <summary>
        /// Prepares the property XML node and adds to properties field of Product.
        /// </summary>
        /// <param name="objProduct">The obj product.</param>
        /// <param name="order">The order.</param>
        /// <param name="prgFld">The PRG FLD.</param>
        /// <param name="fields">The fields.</param>
        private void PrepareProperty(Product objProduct, int order, ProgramFields prgFld, string[] fields)
        {
            try
            {
                if (order < fields.Length)
                {
                    fields[order] = HttpUtility.HtmlDecode(fields[order]);
                    fields[order] = SanitizePropertyField(prgFld, fields[order]);
                    if (string.IsNullOrEmpty(fields[order])) fields[order] = string.Empty;//Create empty nodes, so that csv/XML export works
                    string xmlNode = base.CreatePropertyNode(prgFld.Title, fields[order], prgFld.IsNumeric, objProduct);
                    objProduct.SetField(Constants.Product.Properties, xmlNode, true);
                }
            }
            catch (AffiliBaseException aex)
            {//Article no. to message
                throw new AffiliGenericException(aex.Message + ":Article:" + objProduct.ArtNumber, aex.InnerException);
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.PrepareProperty() Article:" + objProduct.ArtNumber, ex);
            }
        }
        /// <summary>
        /// Gets the category id.
        /// //Returns category id (insert new category in product DB)
        /// </summary>
        /// <param name="objProduct">The obj product.</param>
        /// <param name="objProdCat">The obj prod cat.</param>
        /// <returns></returns>
        private string GetCategoryId(Product objProduct, out ProductCategory objProdCat)
        {
            try
            {
                objProdCat = null;
                //String catId = string.Empty;
                if (!string.IsNullOrEmpty(objProduct.CategoryText))
                {

                    if (!ObjCacheManager.IsCategoryInCache(objProduct, out objProdCat)) //Get id from LocalCache
                    {//Insert new category in Product DB and LocalCache
                        DocAttribs.Report.NewProductCategories++;
                        DataTable dtCat = ObjProductDao.InsertNewCategory(DocAttribs.ProdProgId,
                                                                DocAttribs.ProgSettings.CategorySeperator,
                                                                objProduct.CategoryText, objProduct.MerchantCat);
                        objProdCat = new ProductCategory();
                        if (dtCat.Rows.Count > 0)
                        {
                            objProdCat.ShopCategoryId = dtCat.Rows[0][Constants.ProductCategory.ID].ToString();
                            objProdCat.CatPathText = dtCat.Rows[0][Constants.ProductCategory.CatPathText].ToString();
                        }
                        ObjCacheManager.AddNewCategoryToLC(DocAttribs.ProdProgId, objProduct.CategoryText, objProdCat);
                    }
                    if (objProdCat != null && !string.IsNullOrEmpty(objProdCat.ShopCategoryId)) DocAttribs.Report.CountOfProductsInCategories++;
                }
                else
                {
                    objProdCat = new ProductCategory();
                    NoOfLinesWithoutCatText++;
                }
                return objProdCat != null ? objProdCat.ShopCategoryId : string.Empty;
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.GetCategoryId() Article:" + objProduct.ArtNumber, ex);
            }
        }
        /// <summary>
        /// Prepares the category.
        /// Extracts merchant category
        /// Concatenates various categories of product seperated by category delimiter 
        /// </summary>
        /// <param name="objProduct">The obj product.</param>
        /// <param name="prgFld">The PRG FLD.</param>
        /// <param name="fieldValue">The field value.</param>
        private void PrepareCategory(Product objProduct, ProgramFields prgFld, string fieldValue)
        {
            try
            {
                if(prgFld.IsNumeric)
                {
                    objProduct.MerchantCat = fieldValue;
                }
                else
                {
                    if (string.IsNullOrEmpty(objProduct.CategoryText))
                        objProduct.CategoryText = fieldValue;
                    else if (prgFld.ReverseOrder)
                        objProduct.CategoryText = fieldValue + DocAttribs.ProgSettings.CategorySeperator + objProduct.CategoryText;
                    else
                        objProduct.CategoryText = objProduct.CategoryText + DocAttribs.ProgSettings.CategorySeperator + fieldValue;
                }
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.PrepareCategory() Article:" + objProduct.ArtNumber, ex);
            }
        }
        /// <summary>
        /// Prepares the product field.
        /// Classifies field and accordingly sanitizes a field of product object
        /// </summary>
        /// <param name="objProduct">The obj product.</param>
        /// <param name="prgFld">The Program Field.</param>
        /// <param name="fieldValue">The field value.</param>
        private void PrepareProduct(Product objProduct,ProgramFields prgFld, string fieldValue)
        {
            try
            {
                if ((!prgFld.IsNumeric) || prgFld.DataType.ToLower() != "decimal")
                {
                    if (!String.IsNullOrEmpty(fieldValue))
                    {
                            if (prgFld.ReverseOrder)
                                fieldValue = fieldValue + "" + prgFld.Description.Trim();
                            else
                                fieldValue = prgFld.Description.Trim() + "" + fieldValue;
                    }
                }

                switch (prgFld.DestField)
                {
                    case Constants.Product.ProdCatID:
                        fieldValue = HttpUtility.HtmlDecode(fieldValue);
                        fieldValue = SanitizeField(prgFld, fieldValue, Constants.Generic.CategoryTextFldLength);
                        PrepareCategory(objProduct, prgFld, fieldValue);
                        break;
                    case Constants.Product.affiliProductCategoryID:
                        string tmpCat = PrepareAffiliCategory(objProduct, fieldValue);
                        if (!string.IsNullOrEmpty(tmpCat)) DocAttribs.Report.ProductsInAffiliCategories++;
                        objProduct.SetField(prgFld.DestField, tmpCat);
                        break;
                    case Constants.Product.Price:
                    case Constants.Product.Price_old:
                    case Constants.Product.Shipping:
                    case Constants.Product.BasePrice:
                        fieldValue = SanitizeNumbers(fieldValue);
                        CheckForPriceDigitLength(fieldValue, objProduct, prgFld.DestField);
                        objProduct.SetField(prgFld.DestField, fieldValue);
                        break;
                    case Constants.Product.DeepLink1:                        
                    case Constants.Product.DeepLink2:
                        fieldValue = SanitizeUrlField(prgFld, fieldValue, prgFld.MaxLength);
                        if (!String.IsNullOrEmpty(fieldValue) && DocAttribs.ProgSettings.EncodeDeeplink > 0)
                        {
                            for (int i = 0; i < DocAttribs.ProgSettings.EncodeDeeplink; i++)
                            {
                                fieldValue = HttpUtility.UrlEncode(fieldValue);
                            }                            
                        }
                        objProduct.SetField(prgFld.DestField, fieldValue);
                        break;
                    case Constants.Product.ImageURL1:
                    case Constants.Product.ImageURL2:
                    case Constants.Product.ImageURL3:
                        fieldValue = SanitizeUrlField(prgFld, fieldValue, prgFld.MaxLength);
                        objProduct.SetField(prgFld.DestField, fieldValue);
                        break;
                    case Constants.Product.Description:
                        fieldValue = HttpUtility.HtmlDecode(fieldValue);
                        fieldValue = SanitizeField(prgFld, fieldValue, prgFld.MaxLength);
                        string oldDesc = objProduct.GetField(Constants.Product.Description, "").ToString();
                        if (!String.IsNullOrEmpty(fieldValue))
                        {
                            fieldValue = oldDesc + (oldDesc==""?"":Constants.Generic.HtmlNewLine) + fieldValue;
                            objProduct.SetField(prgFld.DestField, fieldValue);
                        }
                        break;
                    case Constants.Product.Currency_Symbol:
                        fieldValue = SanitizeField(prgFld, fieldValue, prgFld.MaxLength);
                        if (String.IsNullOrEmpty(fieldValue) || fieldValue.Length<=1)
                        {
                         switch(DocAttribs.ProgSettings.PlatformID)
                         {
                             case (int)AffilinetPlatforms.EN:
                                 fieldValue = Constants.PlateformDefaultCurrencies.UK_DefaultCurrency;
                                 break;
                             case (int)AffilinetPlatforms.DE:
                                 fieldValue = Constants.PlateformDefaultCurrencies.DE_DefaultCurrency;
                                 break;
                             case (int)AffilinetPlatforms.FR:
                                 fieldValue = Constants.PlateformDefaultCurrencies.FR_DefaultCurrency;
                                 break;
                             case (int)AffilinetPlatforms.NL:
                                 fieldValue = Constants.PlateformDefaultCurrencies.NL_DefaultCurrency;
                                 break;
                             case (int)AffilinetPlatforms.ES:
                                 fieldValue = Constants.PlateformDefaultCurrencies.ES_DefaultCurrency;
                                 break;
                         }
                        }
                        objProduct.SetField(prgFld.DestField, fieldValue);
                        break;
                    default:
                        if (prgFld.IsNumeric || prgFld.DataType.ToLower() == "decimal")
                            fieldValue = SanitizeNumbers(fieldValue);
                        else
                        {
                            fieldValue = HttpUtility.HtmlDecode(fieldValue);
                            fieldValue = SanitizeField(prgFld, fieldValue, prgFld.MaxLength);
                        }
                        objProduct.SetField(prgFld.DestField, fieldValue);
                        break;
                }
            }
            catch (AffiliBaseException aex)
            {//Article no. to message
                throw new AffiliGenericException(aex.Message + ":Article:" + objProduct.ArtNumber + ":Program:" + DocAttribs.ProdProgId, aex.InnerException);
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.PrepareProduct() Article:" + objProduct.ArtNumber + ":Program:" + DocAttribs.ProdProgId, ex);
            }
        }

        /// <summary>
        /// Checks the length of for price digits.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="objProduct">The obj product.</param>
        /// <param name="fieldName">Name of the field.</param>
        private void CheckForPriceDigitLength(string fieldValue,Product objProduct,string fieldName)
        {
            if(!string.IsNullOrEmpty(fieldValue))
            {
                fieldValue = fieldValue.Trim();
                fieldValue = fieldValue.TrimStart("0"[0]);
                string decimalSep = Utilities.GetAppSettingValue(Constants.AppSettings.AffiliDecimalSep);
                if (string.IsNullOrEmpty(decimalSep)) decimalSep = string.Empty;
                string[] splitter = new string[] { decimalSep };
                string[] price = fieldValue.Split(splitter,StringSplitOptions.None);
                if (price.Length>0 && price[0].Length > PriceWholeDigits) 
                    throw new AffiliGenericException(fieldName + " value too large:" + fieldValue + " Article:" + objProduct.ArtNumber);
                if (price.Length > 1 && price[1].Length > PricePrecisionDigits)
                    throw new AffiliGenericException(fieldName + " value too large:" + fieldValue + " Article:" + objProduct.ArtNumber);
            }
        }

        /// <summary>
        /// Sets the misc product fields.
        /// This method is called after all the fields of product line  are processed
        /// It sets product fields not in CSV feed 
        /// and the ones calculated after complete line is processed
        /// </summary>
        /// <param name="objProduct">The obj product.</param>
        private void SetMiscProductFields(Product objProduct)
        {
            try
            {
                SetupDescShort(objProduct);
                SetupTitle(objProduct);
                SetupImages(objProduct);
                SetupProductAndAffiliCategory(objProduct);

                //SET DEFAULT CURRENCY 
                if (objProduct.GetField(Constants.Product.Currency_Symbol) == null || objProduct.GetField(Constants.Product.Currency_Symbol).ToString() == string.Empty)
                    objProduct.SetField(Constants.Product.Currency_Symbol, GetDefaultCurrency());

                SetupBasePrice(objProduct);

                //Set # always at the end of line processing and even for null props
                objProduct.SetField(Constants.Product.PropertyHash, objProduct.HashCodeProperty);

                objProduct.SetField(Constants.Product.HashCode, objProduct.HashCode);
                
                //Insert, update or noAction status
                objProduct.DbAction = ObjCacheManager.GetProductHashStatus(objProduct);
                if(objProduct.DbAction==ProductActionsEnum.INSERT)
                {
                    NewProductsPer10KCycle++;
                }
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.SetMiscProductFields()Article:" + objProduct.ArtNumber, ex);
            }
        }

        /// <summary>
        /// Validate and prepare the BasePrice field
        /// </summary>
        /// <param name="objProduct">The product</param>
        private void SetupBasePrice(Product objProduct)
        {
            decimal basePrice = 0;

            if (objProduct.GetField(Constants.Product.BasePrice) != null && !string.IsNullOrEmpty(objProduct.GetField(Constants.Product.BasePrice).ToString()))
            {
                if(decimal.TryParse(objProduct.GetField(Constants.Product.BasePrice).ToString(), out basePrice))
                {
                    // BasePrice must be greater than zero
                    if (basePrice < 1)
                    {
                        SetBasePriceFieldsToNull(objProduct);
                    }   
                    else if (objProduct.GetField(Constants.Product.BasePriceSuffix) == null || string.IsNullOrEmpty(objProduct.GetField(Constants.Product.BasePriceSuffix).ToString()))
                    {
                        // if no BasePriceSuffix is there set in to null
                        SetBasePriceFieldsToNull(objProduct);
                        // increase error counter to write it into Logfile at the end
                        BasePricesWithoutSuffix++;
                    }
                }
                else
                {
                    //if base price not numeric set it to null
                    SetBasePriceFieldsToNull(objProduct);
                }
            }
            else
            {   // set these fields to null when basePrice is null
                SetBasePriceFieldsToNull(objProduct);
            }

            //Commented as sanitize no.s method already trims off extra characters
            //// set the basePrice and round it
            //if (objProduct.GetField(Constants.Product.BasePrice) != null && !string.IsNullOrEmpty(objProduct.GetField(Constants.Product.BasePrice).ToString()))
            //{
            //    objProduct.SetField(Constants.Product.BasePrice, decimal.Round(basePrice, 2).ToString());
            //}
        }

        /// <summary>
        /// Set the BasePrice, BasePricePrefix and BasePriceSuffix field to null
        /// </summary>
        /// <param name="objProduct">The product</param>
        private void SetBasePriceFieldsToNull(Product objProduct)
        {
            objProduct.SetFieldToNull(Constants.Product.BasePrice);
            objProduct.SetFieldToNull(Constants.Product.BasePricePrefix);
            objProduct.SetFieldToNull(Constants.Product.BasePriceSuffix);
        }

        private String GetDefaultCurrency()
        {
            String fieldValue = Constants.PlateformDefaultCurrencies.DE_DefaultCurrency;
            switch (DocAttribs.ProgSettings.PlatformID)
            {
                case (int)AffilinetPlatforms.EN:
                    fieldValue = Constants.PlateformDefaultCurrencies.UK_DefaultCurrency;
                    break;
                case (int)AffilinetPlatforms.DE:
                    fieldValue = Constants.PlateformDefaultCurrencies.DE_DefaultCurrency;
                    break;
                case (int)AffilinetPlatforms.FR:
                    fieldValue = Constants.PlateformDefaultCurrencies.FR_DefaultCurrency;
                    break;
                case (int)AffilinetPlatforms.NL:
                    fieldValue = Constants.PlateformDefaultCurrencies.NL_DefaultCurrency;
                    break;
                case (int)AffilinetPlatforms.ES:
                    fieldValue = Constants.PlateformDefaultCurrencies.ES_DefaultCurrency;
                    break;
            }
            return fieldValue;
        }
        /// <summary>
        /// Setups the title.
        /// </summary>
        /// <param name="objProduct">The obj product.</param>
        private void SetupTitle(Product objProduct)
        {
            if (objProduct.GetField(Constants.Product.Title) == null || objProduct.GetField(Constants.Product.Title).ToString() == string.Empty)
            {//Creating title out of short description of category text, if not provided in CSV

                if (objProduct.GetField(Constants.Product.Description_short) != null && objProduct.GetField(Constants.Product.Description_short).ToString() != string.Empty)
                {
                    String descShort = objProduct.GetField(Constants.Product.Description_short).ToString();
                    objProduct.SetField(Constants.Product.Title, descShort.Length <= 40 ? descShort : descShort.Substring(0, 40));
                }
                else if (objProduct.GetField(Constants.Product.ProductCategoryText) != null && objProduct.GetField(Constants.Product.ProductCategoryText).ToString() != string.Empty)
                {
                    String catText = objProduct.GetField(Constants.Product.ProductCategoryText).ToString();
                    objProduct.SetField(Constants.Product.Title, catText.Length <= 40 ? catText : catText.Substring(0, 40));
                }
            }
        }
        /// <summary>
        /// Setups the desc short.
        /// </summary>
        /// <param name="objProduct">The obj product.</param>
        private void SetupDescShort(Product objProduct)
        {
            if (objProduct.GetField(Constants.Product.Description_short) == null || objProduct.GetField(Constants.Product.Description_short).ToString() == string.Empty)
            {//Creating short description out of description if not provided in CSV.
                string descShort = string.Empty;
                string desc = objProduct.GetField(Constants.Product.Description) == null
                                  ? "" : objProduct.GetField(Constants.Product.Description).ToString();
              
                string[] tags = { "<p>", "<P>","</p>", "</P>", "<BR>","<br>", "<BR/>", "<br/>", "<DIV>", "</div>","<BR />", "<br />", "<BR /", "<BR", "<DIV", "<br /", "<br", "<div" };

                foreach (string str in tags)
                    desc = desc.Replace(str, " ");

                if (desc.Length > 500)
                    descShort = desc.Substring(0, 500);
                else
                    descShort = desc;

                objProduct.SetField(Constants.Product.Description_short, descShort.Trim());
            }
        }
        /// <summary>
        /// Setups the affili category.
        /// </summary>
        /// <param name="objProduct">The obj product.</param>
        private void SetupProductAndAffiliCategory(Product objProduct)
        {
            if(String.IsNullOrEmpty(objProduct.CategoryText))
            {
                // counter for qualityAlerting - ProductsWithoutCategoryText;
                DocAttribs.Report.ProductsWithoutCategoryText++;;
            }

            ProductCategory objProdCat = null;
            //Get category id using category text(extracted and concatenated from CSV product line)
            objProduct.SetField(Constants.Product.ProdCatID, GetCategoryId(objProduct, out objProdCat));

            //Set shop category text 
            objProduct.SetField(Constants.Product.ProductCategoryText, objProdCat.CatPathText);

            //Set Default category if csv feed does not have a Affili category field
            Object affiliCat = objProduct.GetField(Constants.Product.affiliProductCategoryID);
            String affiliCatText = String.Empty;
            if (affiliCat != null && ObjCacheManager.IsAffiliCategoryInCache(affiliCat.ToString(), out affiliCatText))
            {
                objProduct.SetField(Constants.Product.affiliProductCategoryID, affiliCat.ToString());
                objProduct.SetField(Constants.Product.affiliProductCategoryText, affiliCatText);
            }
            else
            {//If Affili cat not in CSV get mapped affili id to shop id
                if (objProdCat != null && !string.IsNullOrEmpty(objProdCat.AffiliCategoryId) &&
                    objProdCat.AffiliCategoryId != "0")
                {
                    affiliCatText = String.Empty;
                    if (ObjCacheManager.IsAffiliCategoryInCache(objProdCat.AffiliCategoryId, out affiliCatText))
                    {
                        objProduct.SetField(Constants.Product.affiliProductCategoryID, objProdCat.AffiliCategoryId);
                        objProduct.SetField(Constants.Product.affiliProductCategoryText, affiliCatText);
                    }
                }
                else
                {
                    //If no affili Cat Mapped get default for shop from settings
                    if (!String.IsNullOrEmpty(DocAttribs.ProgSettings.DefaultAffiliCat))
                    {
                        affiliCatText = String.Empty;
                        if (ObjCacheManager.IsAffiliCategoryInCache(DocAttribs.ProgSettings.DefaultAffiliCat, out affiliCatText))
                        {
                            objProduct.SetField(Constants.Product.affiliProductCategoryID, DocAttribs.ProgSettings.DefaultAffiliCat);
                            objProduct.SetField(Constants.Product.affiliProductCategoryText, affiliCatText);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Prepares the affili category.
        /// </summary>
        /// <param name="objProduct">The obj product.</param>
        /// <param name="fieldValue">The field value.</param>
        /// <returns></returns>
        private string PrepareAffiliCategory(Product objProduct, string fieldValue)
        {
            string affiliCat = fieldValue;
            try
            {
                if (string.IsNullOrEmpty(fieldValue))
                {
                    affiliCat = DocAttribs.ProgSettings.DefaultAffiliCat; //Set default category from prog settings
                }
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.PrepareAffiliCategory()Article:" + objProduct.ArtNumber, ex);
            }
            if (!string.IsNullOrEmpty(affiliCat)) DocAttribs.Report.ProductsInAffiliCategories++;
            return affiliCat;
        }
        /// <summary>
        /// Setups the images.
        /// There can be three images associated with 1 product (ImageUrl1, ImageUrl2, ImageUrl3)
        /// We keep them all in Product table
        /// ProductProgram.ImageNB column stores which image is to be used and also determines which image to be stored in ProductImage table 
        /// If ProductProgram.ImageNB = 1 insert ImageURL1 to ProductImage table
        /// If ProductProgram.ImageNB = 2 insert ImageURL2 to ProductImage table
        /// If ProductProgram.ImageNB = 3 insert ImageURL3 to ProductImage table
        /// PRODUCT.ImageId column will store imageId for 1 of the above images only
        /// </summary>
        /// <param name="objProduct">The product object.</param>
        private void SetupImages(Product objProduct)
        {
            ProductImage objProductImageFromCache = null;
            ProductImage objProductImage = null;
            try
            {
                //string imgId = string.Empty;
                objProductImage = new ProductImage();
                string ImageField= string.Empty;
                string ImageWidthField = string.Empty;
                string ImageHeightField = string.Empty;
                //Determine which image to use
                switch (DocAttribs.ProgSettings.ImageNumToUse)
                {
                    case 1:
                        ImageField = Constants.Product.ImageURL1;
                        ImageWidthField = Constants.Product.ImgWidth1;
                        ImageHeightField = Constants.Product.ImgHeight1;
                        break;
                    case 2:
                        ImageField = Constants.Product.ImageURL2;
                        ImageWidthField = Constants.Product.ImgWidth2;
                        ImageHeightField = Constants.Product.ImgHeight2;
                        break;
                    case 3:
                        ImageField = Constants.Product.ImageURL3;
                        ImageWidthField = Constants.Product.ImgWidth3;
                        ImageHeightField = Constants.Product.ImgHeight3;
                        break;
                }

                if (!string.IsNullOrEmpty(ImageField) && objProduct.GetField(ImageField) != null)
                {
                    objProductImage.ImageUrl = objProduct.GetField(ImageField,"").ToString();
                    objProduct.SetField(Constants.Product.ImageName, Utilities.GetMd5HashBytes(objProductImage.ImageUrl));

                    //If image is not in local cache insert image in product db and get image id
                    if (objProductImage.ImageUrl.Length >=4 && objProductImage.ImageUrl.Substring(0, 4).ToUpper() == "HTTP") //Check if valid image url?
                    {
                        objProduct.SetField(Constants.Product.ImageID, "-1");
                        if (ObjCacheManager.IsImageInCache(objProduct, objProductImage.ImageUrlHash, objProductImage.ImageUrl, out objProductImageFromCache))//Check in cache
                        {
                            if (objProductImageFromCache != null && !string.IsNullOrEmpty(objProductImageFromCache.ImageId) && objProductImageFromCache.ImageId.Trim() != "0")
                            {
                                if (!String.IsNullOrEmpty(objProductImageFromCache.ImageWidth) && objProductImageFromCache.ImageWidth.Trim() != "0")
                                {//if found in cahce image width & height from image table is preffred over adv's 
                                    objProduct.SetField(ImageWidthField, objProductImageFromCache.ImageWidth);
                                    objProduct.SetField(ImageHeightField, objProductImageFromCache.ImageHeight);
                                    SetImageFieldsForExport(objProduct,objProductImageFromCache);
                                }
                                objProduct.SetField(Constants.Product.ImageID, objProductImageFromCache.ImageId);
                            }
                        }
                        else
                        {//Not in cache so add to product images collection later to be inserted to ProductDB.ProductImages
                            DocAttribs.Report.NewImages++;
                            NewImagesPer10KCycle++;
                        }
                    }
                }
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.SetupImages() Article:" + objProduct.ArtNumber, ex);
            }
            finally
            {
                if (objProductImage != null) objProductImage.Dispose();
                objProductImageFromCache = null;
                objProductImage = null;
            }
        }

        /// <summary>
        /// Sets the image fields for export.
        /// </summary>
        /// <param name="objProduct">The obj product.</param>
        /// <param name="objProductImageFromCache">The obj product image from cache.</param>
        private void SetImageFieldsForExport(Product objProduct, ProductImage objProductImageFromCache)
        {
            objProduct.SetField(Constants.Product.ImgWidth, objProductImageFromCache.ImageWidth);
            objProduct.SetField(Constants.Product.ImgHeight, objProductImageFromCache.ImageHeight);
            objProduct.SetField(Constants.Product.Img30Width, objProductImageFromCache.Img30Width);
            objProduct.SetField(Constants.Product.Img30Height, objProductImageFromCache.Img30Height);
            objProduct.SetField(Constants.Product.Img60Width, objProductImageFromCache.Img60Width);
            objProduct.SetField(Constants.Product.Img60Height, objProductImageFromCache.Img60Height);
            objProduct.SetField(Constants.Product.Img90Width, objProductImageFromCache.Img90Width);
            objProduct.SetField(Constants.Product.Img90Height, objProductImageFromCache.Img90Height);
            objProduct.SetField(Constants.Product.Img120Width, objProductImageFromCache.Img120Width);
            objProduct.SetField(Constants.Product.Img120Height, objProductImageFromCache.Img120Height);
            objProduct.SetField(Constants.Product.Img180Width, objProductImageFromCache.Img180Width);
            objProduct.SetField(Constants.Product.Img180Height, objProductImageFromCache.Img180Height);
        }
        #endregion
        #region Sanitize
        /// <summary>
        /// Sanitizes the Property fields.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="fieldValue">The field value.</param>
        /// <returns></returns>
        private string SanitizePropertyField(ProgramFields field, string fieldValue)
        {
            try
            {
                if (fieldValue != null)
                {
                    fieldValue = fieldValue.Trim();
                    if (fieldValue != "")
                    {
                        if (!String.IsNullOrEmpty(DocAttribs.ProgSettings.TextQualifier))
                        {
                            int QualLen = DocAttribs.ProgSettings.TextQualifier.Length;
                            //Since 1st char is " of line and so does the field qualifier 1st value will always have extra "
                            if (fieldValue.Substring(0, QualLen) == DocAttribs.ProgSettings.TextQualifier)
                                fieldValue = fieldValue.Substring(QualLen, fieldValue.Length - QualLen);

                            //Since last char is " of line and so does the field qualifier 1st value will always have extra "
                            if (fieldValue.Length > 0 &&
                                fieldValue.Substring(fieldValue.Length - QualLen, QualLen) == DocAttribs.ProgSettings.TextQualifier)
                                fieldValue = fieldValue.Substring(0, fieldValue.Length - QualLen);
                        }
                        //since exported csvs must not have these characters
                        fieldValue = fieldValue.Replace("'", Constants.Generic.SingleQuoteReplaceChars);
                        fieldValue = fieldValue.Replace("\"", Constants.Generic.DoubleQuoteReplaceChars);
                    }
                }
                return fieldValue;
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.SanitizePropertyField() field:" + field.DestField + ":Value:" + fieldValue, ex);
            }

        }
        /// <summary>
        /// Sanitizes the URL fields and truncates extra charaters in source fields.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="fieldMaxLength">Length of the field max.</param>
        /// <returns></returns>
        private string SanitizeUrlField(ProgramFields field, string fieldValue, int fieldMaxLength)
        {
            try
            {
                if (DtProduct != null && DtProduct.Columns.Contains(field.DestField) && fieldValue != null)
                {
                    fieldValue = fieldValue.Trim();
                    if (fieldValue != "")
                    {
                        int QualLen = DocAttribs.ProgSettings.TextQualifier.Length;
                        //Since 1st char is " of line and so does the field qualifier 1st value will always have extra "
                        if (fieldValue.Substring(0, QualLen) == DocAttribs.ProgSettings.TextQualifier)
                            fieldValue = fieldValue.Substring(QualLen, fieldValue.Length - QualLen);

                        //Since last char is " of line and so does the field qualifier 1st value will always have extra "
                        if (fieldValue.Length > 0 &&
                            fieldValue.Substring(fieldValue.Length - QualLen, QualLen) == DocAttribs.ProgSettings.TextQualifier)
                            fieldValue = fieldValue.Substring(0, fieldValue.Length - QualLen);

                        if (fieldValue != null && fieldValue.Length > fieldMaxLength)
                        {
                            throw new AffiliGenericException("Error in CSVProcessor.SanitizeUrlField(), field legth greater then DB field:" + field.DestField + ":Value:" + fieldValue);
                        }

                        //Check if valid url?
                        if (fieldValue.Length <=4 || fieldValue.Substring(0, 4).ToUpper() != "HTTP") 
                        //if(!Utilities.IsValidUrl(fieldValue))
                        {
                            NoOfLinesWithInvalidUrls++;
                            fieldValue = string.Empty;
                            //throw new AffiliGenericException("Invalid URL for field:" + field.DestField + " Value:" + fieldValue);
                        }
                    }
                }
                return fieldValue;
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.SanitizeUrlField() field:" + field.DestField + ":Value:" + fieldValue, ex);
            }
           
        }


        /// <summary>
        /// Sanitizes the string fields and truncates extra charaters in source fields.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="fieldMaxLength">Length of the field max.</param>
        /// <returns></returns>
        private string SanitizeField(ProgramFields field, string fieldValue,int fieldMaxLength)
        {
            try
            {
                if (DtProduct != null && DtProduct.Columns.Contains(field.DestField) &&
                    fieldValue != null)
                {
                    fieldValue = fieldValue.Trim();
                    if (fieldValue != "")
                    {
                        if (!String.IsNullOrEmpty(DocAttribs.ProgSettings.TextQualifier))
                        {
                            int QualLen = DocAttribs.ProgSettings.TextQualifier.Length;
                            //Since 1st char is " of line and so does the field qualifier 1st value will always have extra "
                            if (fieldValue.Substring(0, QualLen) == DocAttribs.ProgSettings.TextQualifier)
                                fieldValue = fieldValue.Substring(QualLen, fieldValue.Length - QualLen);

                            //Since last char is " of line and so does the field qualifier 1st value will always have extra "
                            if (fieldValue.Length > 0 &&
                                fieldValue.Substring(fieldValue.Length - QualLen, QualLen) == DocAttribs.ProgSettings.TextQualifier)
                                fieldValue = fieldValue.Substring(0, fieldValue.Length - QualLen);
                        }

                        //since SQL text parser does not support text qualifier in qualified field
                        fieldValue = fieldValue.Replace(Constants.Generic.DestFieldQualifier,
                                                        Constants.Generic.DestFieldQualifierAlternate);

                        //since exported csvs must not have these characters
                        fieldValue = fieldValue.Replace("'", Constants.Generic.SingleQuoteReplaceChars);
                        fieldValue = fieldValue.Replace("\"", Constants.Generic.DoubleQuoteReplaceChars);

                        //Remove Invalid datetime values
                        DateTime dtTmp;
                        if (DtProduct.Columns[field.DestField].DataType == Type.GetType("System.DateTime"))
                        {
                            fieldValue = SanitizeDate(fieldValue);
                        }
                        else
                        {//Rtrim field value if greater than expected/allowed destination field.  
                            if (fieldValue != null && fieldValue.Length > fieldMaxLength)
                            {
                                //Utilities.CreateWarningLog("Truncated field:" + field.DestField + ":Allowed length:" +
                                //                           field.MaxLength.ToString() + ":length in CSV:" +
                                //                           fieldValue.Length.ToString(), ApplicationEventsEnum.DocProcessing);
                                fieldValue = fieldValue.Substring(0, field.MaxLength);
                            }
                        }

                    }
                }
                return fieldValue;
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.SanitizeField() field:" + field.DestField + ":Value:" + fieldValue, ex);
            }
        }
        /// <summary>
        /// Sanitizes the DateTime fields.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <returns></returns>
        private string SanitizeDate(string fieldValue)
        {
            String retVal = string.Empty;
            try
            {
                if (fieldValue == null || fieldValue.Trim() == String.Empty) return String.Empty;
                if (!string.IsNullOrEmpty(DocAttribs.ProgSettings.TextQualifier))
                    fieldValue = fieldValue.Replace(DocAttribs.ProgSettings.TextQualifier, "");

                int maxDateYear = 2079;
                int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.MaxDateYear), out maxDateYear);
                DateTime maxDate = new DateTime(maxDateYear, 1, 1);

                DateTime dtTmp = DateTime.MinValue;
                if (DocAttribs.ProgSettings.DateFormat != string.Empty)
                {
                    dtTmp = DateTime.ParseExact(fieldValue, DocAttribs.ProgSettings.DateFormat,
                                                CultureInfo.CurrentCulture);
                    if (dtTmp.CompareTo(maxDate) > 0)
                        retVal = maxDate.ToString("f");
                    else
                        retVal = dtTmp.ToString("f");
                }
                else 
                if (DateTime.TryParse(fieldValue, out dtTmp))
                {
                    if (dtTmp.CompareTo(maxDate) > 0)
                        retVal = maxDate.ToString("f");
                    else
                        retVal = dtTmp.ToString("f");
                }
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.SanitizeDate() Can not convert date field to specified format:" + DocAttribs.ProgSettings.DateFormat + ":Value:" + fieldValue, ex);
            }
            return retVal;
        }
        /// <summary>
        /// Sanitizes the numbers.
        /// Removes all non numeric characters from numeric values
        /// Replaces all decimal seperators with dot(Configurable)
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <returns></returns>
        private string SanitizeNumbers(string fieldValue)
        {
            try
            {
                fieldValue = fieldValue.Trim();
                if (string.IsNullOrEmpty(fieldValue)) return fieldValue;
                string progDeci = DocAttribs.ProgSettings.DecimalSymbol;
                string progGroupSymbol = DocAttribs.ProgSettings.DigitGroupSymbol;
                string affiliDeci = Utilities.GetAppSettingValue(Constants.AppSettings.AffiliDecimalSep);

                // Remove program's digit grouping symbol
                if (progGroupSymbol.Length > 0) fieldValue = fieldValue.Replace(progGroupSymbol, "");

                fieldValue = fieldValue.Replace(progDeci, affiliDeci);// replace shops decimal seperator with affilinet's decimal seperator

                //Replace shops various symbols to represent zeros
                fieldValue = fieldValue.Replace(",--", ",00");
                fieldValue = fieldValue.Replace(",-", ",0");
                fieldValue = fieldValue.Replace(".--", ".00");
                fieldValue = fieldValue.Replace(".-", ".0");

                String strTmp = string.Empty;
                foreach (char c in fieldValue) //filter chars except .  and 0-9 
                {
                    if ((c >= 48 && c <= 57) || c == affiliDeci[0])
                        strTmp += c;
                }
                if (string.IsNullOrEmpty(strTmp)) return "";
                fieldValue = strTmp.Trim();

                //Remove field seperator from number
                fieldValue = fieldValue.Replace(DocAttribs.ProgSettings.FieldSeperator, "");

                if (fieldValue.IndexOf(affiliDeci) < 0) //If decimal sep is missing add .00 as precision
                    fieldValue = fieldValue + affiliDeci + "00";

                if (fieldValue.IndexOf(affiliDeci) == 0)//If number contains only precision values prefix a zero before decimal
                    fieldValue = "0" + fieldValue;

                if (fieldValue.IndexOf(affiliDeci) == fieldValue.Length - 1)//If decimal is last cahr in number add zeros as precision
                    fieldValue = fieldValue + "00";

                if (fieldValue.IndexOf(".") > fieldValue.Length - 3)//Right pad zeros if length of preciosion is less than 2
                    fieldValue = fieldValue.PadRight(fieldValue.Length + fieldValue.Length - 1 - 
                                                     fieldValue.IndexOf("."),'0');

                fieldValue = fieldValue.Substring(0, fieldValue.IndexOf(".") + 3);//Precision length must be 2, trim off extra precision numbers

                return fieldValue;
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.SanitizeNumbers() :Value:" + fieldValue, ex);
            }
        }
        #endregion
        #region Misc.

        /// <summary>
        /// Reads the line and fix col count.
        /// </summary>
        /// <param name="strLine">The STR line.</param>
        /// <returns></returns>
        private string[] ReadLineAndFixColCount( string strLine)
        {
            try
            {
                if (!IsProductInMultiLine)
                {
                    return FixColumnCountForProductInSingleLine(strLine);
                }
                else
                {
                    return FixColumnCountForProductInMultiLine(strLine);
                }
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.ReadLineAndFixColCount()", ex);
            }
        }

         private string[] FixColumnCountForProductInSingleLine( string strLine)
         {
             string[] tmpFields = null;
             strLine = RemoveIllegalCharaters(strLine);
             tmpFields = SplitLine(strLine); //split line
             if (tmpFields.Length < DocAttribs.ProgFields.Count)
             {
                 if (!string.IsNullOrEmpty(DocAttribs.ProgSettings.TextQualifier))
                 {
                     tmpFields = SplitLine(strLine, false); //split line only on field seperator
                     if (tmpFields.Length < DocAttribs.ProgFields.Count)
                     {
                         throw new AffiliGenericException("No. of columns are less after split line only on field seperator, product line aborted, cols in line: " + tmpFields.Length + " :Required: " + DocAttribs.ProgFields.Count);
                     }
                     else if (tmpFields.Length > DocAttribs.ProgFields.Count)
                     {
                         NoOfLinesWithScanLines++;
                         if (!IsScanLineCharactersBeingUsed)
                         {
                             CommonUtilities.CreateEmailLog("ScanLineCharacters() is being used for splitting Product Program Id: " + DocAttribs.ProdProgId);
                             IsScanLineCharactersBeingUsed = true;
                         }
                         tmpFields = ScanLineCharacters(strLine);
                         if (tmpFields.Length < DocAttribs.ProgFields.Count)
                             throw new AffiliGenericException("No. of columns are less after ScanLineCharacters, product line aborted, cols in line: " + tmpFields.Length + " :Required: " + DocAttribs.ProgFields.Count);
                         else if (tmpFields.Length > DocAttribs.ProgFields.Count)
                         {
                             NoOfLinesWithExtraColumns++;
                             //new AffiliGenericException("No. of columns are more after ScanLineCharacters, cols in line: " + tmpFields.Length + " :Required: " + DocAttribs.ProgFields.Count).CreateLog();
                         }
                     }
                 }
                 else//If no qualifier abort product line if cols are less or more
                     throw new AffiliGenericException("No. of columns are less, product line aborted, cols in line: " + tmpFields.Length + " :Required: " + DocAttribs.ProgFields.Count);
             }
             return tmpFields;
         }

        /// <summary>
        /// Fixes the column count for product in multi line.
        /// </summary>
        /// <param name="strLine">The STR line.</param>
        /// <returns></returns>
       private string[] FixColumnCountForProductInMultiLine( string strLine)
       {
           List<string> fields = null;
           string[] tmpFields = null;
           int maxLineLen = 8000;
           string[] retVal = new string[1];
           int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.MaxCsvLineLength), out maxLineLen);
           do //Fix col count
           {
               strLine = RemoveIllegalCharaters(strLine);
               if (strLine.Trim().Length > 0)
               {
                   tmpFields = SplitLine(strLine); //split line
                   if (tmpFields.Length < DocAttribs.ProgFields.Count)
                   {
                       if (!string.IsNullOrEmpty(DocAttribs.ProgSettings.TextQualifier))
                       {
                           tmpFields = SplitLine(strLine, false); //split line only on field seperator
                       }
                   }
                   if (fields == null)
                       fields = tmpFields.ToList();
                   else
                   {
                       if (tmpFields.Length >= 1)
                       {
                           fields[fields.Count() - 1] += tmpFields[0];
                           for (int i = 1; i < tmpFields.Length; i++)
                               fields.Add(tmpFields[i]);
                       }
                   }
               }
           } while (strLine.Length < maxLineLen && //line length must be less than specified in config
                    fields.Count < DocAttribs.ProgFields.Count &&
                    (strLine = SrCsvSource.ReadLine()) != null);
           if (fields != null) retVal = fields.ToArray();
           return retVal;
       }
        /// <summary>
        /// Scans the line characters in product lilne and splits line.
        /// </summary>
        /// <param name="strLine">The STR line.</param>
        /// <returns></returns>
        private string[] ScanLineCharacters(string strLine)
        {
            string FieldSeperator = DocAttribs.ProgSettings.FieldSeperator;
            char FldSep = DocAttribs.ProgSettings.FldSep;
           
            if(DocAttribs.ProgSettings.FieldSeperator == Constants.Generic.Tab)
            { //Special processing for tab white space as field seperator character
                FieldSeperator = Constants.Generic.TabReplacementFieldSeperator1;
                if (strLine.IndexOf(FieldSeperator) > -1)
                {
                    FieldSeperator = Constants.Generic.TabReplacementFieldSeperator2;
                    if (strLine.IndexOf(FieldSeperator) > -1)
                    {
                        FieldSeperator = Constants.Generic.TabReplacementFieldSeperator3;
                        if (strLine.IndexOf(FieldSeperator) > -1)
                        {
                            throw new AffiliGenericException("Error in CSVProcessor.ScanLineCharacters(): Seperator is tab; can not find suitable replacement for tab");
                        }
                    }
                }
                strLine = strLine.Replace(Constants.Generic.Tab, FieldSeperator);
                FldSep = FieldSeperator[0];
            }

            //remove white space from around the seperator-Start
            string expr = string.Format(Constants.RegExp.LineFieldSeperator, FieldSeperator);
            strLine = Regex.Replace(strLine, expr, FieldSeperator);
            //remove white space from around the seperator-End

            List<String> fields = new List<string>();
            //string[] retVal = new string[DocAttribs.ProgFields.Count];
            string tmpfield = string.Empty;
            //int cnt = 0;
            FieldStatusEnum fieldStatus = FieldStatusEnum.ResetState;
            
            foreach(char c in strLine)
            {
                if (fieldStatus == FieldStatusEnum.ResetState)//Start of a field
                {
                    if (c == DocAttribs.ProgSettings.TextQual)
                    { fieldStatus = FieldStatusEnum.StartQualifier; continue; }//This field is qualified or first qualifier of field found
                    else
                    {
                        fieldStatus = FieldStatusEnum.NotQualified;//This field is not qualified or this field has 0 qualifiers
                        if (c == FldSep)//Field seperator found(Multiple field seperators 1 after another)
                        {
                            fieldStatus = FieldStatusEnum.ResetState; //cnt++;
                            fields.Add(tmpfield);
                            tmpfield = string.Empty;
                            continue;//field seperator found start scan for next field
                        }
                    }
                }
                else if (fieldStatus == FieldStatusEnum.StartQualifier && c == DocAttribs.ProgSettings.TextQual)//Any character in qualified field
                    {
                        fieldStatus = FieldStatusEnum.EndQualifier;//Second qualifier of field found
                        continue;
                    }
                else if (c == FldSep)//Field seperator found
                {
                    if (fieldStatus == FieldStatusEnum.NotQualified || fieldStatus == FieldStatusEnum.EndQualifier)//Non qualified field or Field seperator must follow qualifier for a qualified field
                    {
                        fieldStatus = FieldStatusEnum.ResetState; //cnt++;
                        fields.Add(tmpfield);
                        tmpfield = string.Empty;
                        continue;//field seperator found start scan for next field
                    }
                }

                if (fieldStatus == FieldStatusEnum.EndQualifier)
                {
                    if (c != DocAttribs.ProgSettings.TextQual)
                        fieldStatus = FieldStatusEnum.StartQualifier;//If a qualifier is in between field or next character to qualifier must be seperator}
                }
                
                tmpfield += c.ToString();//Add character to field
            }
            
            //if (tmpfield != string.Empty) fields.Add(tmpfield);// commented for jpc 414
            fields.Add(tmpfield);

            return fields.ToArray();
            //return retVal;
        }
        /// <summary>
        /// Checks the F products are in multi line.
        /// </summary>
        private void CheckiFProductsAreInMultiLine()
        {
            IsProductInMultiLine = false;
            try
            {
                string progs = Utilities.GetAppSettingValue(Constants.AppSettings.ProgramsWithMultiProductLines);
                if (!string.IsNullOrEmpty(progs))
                {
                    string[] prgArray = progs.Split(',');
                    foreach (string prg in prgArray)
                    {
                        if (DocAttribs.ProdProgId.ToString() == prg.Trim())
                        {
                            IsProductInMultiLine = true;
                        }
                    }
                }
            }catch(Exception ex)
            {
                new AffiliGenericException("Error in CsvProcessor.CheckiFProductsAreInMultiLine()", ex).CreateLog();
            }
        }

        /// <summary>
        /// Splits the line.
        /// </summary>
        /// <param name="strLine">The STR line.</param>
        /// <returns></returns>
        private string[] SplitLine(string strLine)
        {
            return SplitLine(strLine, true);
        }

        /// <summary>
        /// Splits the line.
        /// </summary>
        /// <param name="strLine">The STR line.</param>
        /// <param name="useQualifiers">if set to <c>true</c> [use qualifiers].</param>
        /// <returns></returns>
        private string[] SplitLine(string strLine,bool useQualifiers)
        {
            try
            {
                string[] fields = null;

                string fldSplitter = DocAttribs.ProgSettings.TextQualifier +
                                  DocAttribs.ProgSettings.FieldSeperator +
                                  DocAttribs.ProgSettings.TextQualifier;
                if (useQualifiers && !string.IsNullOrEmpty(DocAttribs.ProgSettings.TextQualifier))
                {
                    
                    string expr = string.Format(Constants.RegExp.LineFieldQualifiedSeperator,
                                                DocAttribs.ProgSettings.TextQualifier,
                                                DocAttribs.ProgSettings.FieldSeperator);
                    strLine = Regex.Replace(strLine, expr, fldSplitter);
                }
                else
                {
                    strLine = strLine.Replace(fldSplitter, DocAttribs.ProgSettings.FieldSeperator);
                    strLine = strLine.Replace(DocAttribs.ProgSettings.TextQualifier + DocAttribs.ProgSettings.FieldSeperator, DocAttribs.ProgSettings.FieldSeperator);
                    strLine = strLine.Replace(DocAttribs.ProgSettings.FieldSeperator + DocAttribs.ProgSettings.TextQualifier, DocAttribs.ProgSettings.FieldSeperator);
                    fldSplitter = DocAttribs.ProgSettings.FieldSeperator;
                }
                string[] splitChrs = new string[] { fldSplitter };

                fields = strLine.Split(splitChrs,StringSplitOptions.None);
                return fields;
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.SplitLine()", ex);
            }
        }


        /// <summary>
        /// Skips the header rows.
        /// </summary>
        private void SkipHeaderRows()
        {
            try
            {
                int i = 0;
                while (i < DocAttribs.ProgSettings.Headlines && !string.IsNullOrEmpty(SrCsvSource.ReadLine())) i++;
                if(i==0)
                    Utilities.CreateWarningLog("CSV file has no data: PPID" + DocAttribs.ProdProgId,ApplicationEventsEnum.DocProcessing);
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.SplitLine()", ex);
            }
        }
        /// <summary>
        /// Opens the file for read.
        /// </summary>
        protected virtual void OpenFileForRead()
        {
            try
            {
                SrCsvSource = new StreamReader(DocAttribs.FileFullPath, DocAttribs.ProgSettings.Encoding);
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.OpenFileForRead()", ex);
            }
        }
        /// <summary>
        /// Checks for redundant products and determines whether objProduct is valid product (with all mendatory fields).
        /// </summary>
        /// <param name="objProduct">The product object.</param>
        private bool IsValidProduct(Product objProduct)
        {
            try
            {
                bool retval = false;
                if (string.IsNullOrEmpty(objProduct.ArtNumber))
                {
                    DocAttribs.Report.ErrorProducts++;
                    throw new AffiliGenericException("Invalid product article no. is missing");
                }
                if (objProduct.DbAction == ProductActionsEnum.NOT_CHANGED_ACTION)
                    DocAttribs.Report.NotChangedProducts++;
                return !string.IsNullOrEmpty(objProduct.ArtNumber);// && objProduct.DbAction != ProductActionsEnum.NO_Action; commented to include not changed products in not changed csv
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in CSVProcessor.IsValidProduct()", ex);
            }
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using AffiliErrorIndexing;
using Affilinet.Business.ProductImport.Cache;
using Affilinet.Business.ProductImport.Common;
using Affilinet.Business.ProductImport.DAO;
using Affilinet.Business.ProductImport.Entity;
using Affilinet.Exceptions;
using Microsoft.SqlServer.Dts.Runtime;

namespace Affilinet.Business.ProductImport.Processor
{
    /// <summary>
    /// Implements all common methods needed to process a product data source.
    /// </summary>
    public abstract class BaseProcessor : IProcessor
    {
        #region Properties

        /// <summary>
        /// Gets or sets the doc attrib list, this is main object for all product, program and field settings.
        /// One must refer to this object for any settings or information
        /// </summary>
        /// <value>The doc attrib list.</value>
        public DocumentAttributes DocAttribs { get; set; }
        /// <summary>
        /// Gets or sets the obj product Data Access Object.
        /// </summary>
        /// <value>The obj product DAO.</value>
        public ProductDAO ObjProductDao { get; set; }
        /// <summary>
        /// Gets or sets the stream reader for downloaded source CSV file.
        /// </summary>
        /// <value>The sr CSV source.</value>
        public StreamReader SrCsvSource { get; set; }
        /// <summary>
        /// Gets or sets the XML text reader source stream.
        /// </summary>
        /// <value>The xr XML source.</value>
        public XmlTextReader XrXmlSource { get; set; }
        /// <summary>
        /// Gets or sets the obj CSV data. 
        /// Contains collection of product objects.
        /// </summary>
        /// <value>The obj CSV data.</value>
        public CSVDataCollections ObjCSVDataCollections { get; set; }
        /// <summary>
        /// Gets or sets the dt product, represents structure of product.
        /// </summary>
        /// <value>The dt product.</value>
        public DataTable DtProduct { get; set; }
        /// <summary>
        /// Gets or sets the obj cache manager.
        /// </summary>
        /// <value>The obj cache manager.</value>
        public CacheManager ObjCacheManager { get; set; }
        /// <summary>
        /// Gets the no. of products to be products to be processed in a set or group.
        /// </summary>
        /// <value>The product set count.</value>
        public int ProductSetCount
        {
            get
            {
                int cnt = 1000;
                string strCnt = Utilities.GetAppSettingValue(Constants.AppSettings.ProductSetCount);
                int.TryParse(strCnt, out cnt);
                return cnt;
            }
        }
        /// <summary>
        /// No of products to process, after which check if admin has requested abort
        /// </summary>
        public int ProductsToCheckIfAbortRequested
        {
            get
            {
                int cnt = 1000;
                string strCnt = Utilities.GetAppSettingValue(Constants.AppSettings.ProductsToCheckIfAbortRequested);
                int.TryParse(strCnt, out cnt);
                return cnt;
            }
        }
        /// <summary>
        /// Gets the max lines in CSV.
        /// </summary>
        /// <value>The max lines in CSV.</value>
        public int MaxLinesInCSV
        {
            get
            {
                int cnt = 50000;
                string strCnt = Utilities.GetAppSettingValue(Constants.AppSettings.MaxLinesInCSV);
                int.TryParse(strCnt, out cnt);
                return cnt;
            }
        }
        #endregion
        /// <summary>
        /// This object instance is used to create thread synchronisation.
        /// </summary>
        protected Object thisLock = new Object();
        private int insertFileNumber = 1;
        private int updateFileNumber = 1;
        private int NotChangedFileNumber = 1;
        private int insertedProductsInFile = 0;
        private int updatedProductsInFile = 0;
        private int notChangedProductsInFile = 0;
        protected int PriceWholeDigits = 6;
        protected int PricePrecisionDigits = 2;
        protected long AppSleepProductCount = 100000;
        private long NoOfProductsProcessed=0;
        protected int NewProductsPer10KCycle = 0;
        protected int NewImagesPer10KCycle = 0;
        protected readonly ErrorIndexingManager ErrorIndexingManager = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="BaseProcessor"/> class.
        /// </summary>
        /// <param name="docAttribs">The doc attribs.</param>
        /// <param name="oCacheManager">The o cache manager.</param>
        protected BaseProcessor(DocumentAttributes docAttribs, CacheManager oCacheManager)
        {
            try
            {
                this.DocAttribs = docAttribs;
                this.ObjProductDao = new ProductDAO();
                ObjCacheManager = oCacheManager;
                ObjCSVDataCollections = new CSVDataCollections();
                MoveExistingFiles();
                DtProduct = ObjProductDao.GetProductColumns();
                int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.PriceWholeDigits), out PriceWholeDigits);
                int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.PricePrecisionDigits), out PricePrecisionDigits);
                long.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.AppSleepProductCount), out AppSleepProductCount);
                Regex.CacheSize = 0;
                ErrorIndexingManager = new ErrorIndexingManager();
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in BaseProcessor.BaseProcessor()", ex);
            }
        }
        /// <summary>
        /// Moves the existing csv files, if any.
        /// </summary>
        private void MoveExistingFiles()
        {
            string[] fileExtensions = Utilities.GetFileExtensions();
            foreach (string fileExtension in fileExtensions)
            {
                int i = 1;
                string fileName = DocAttribs.FileFullPath + "." + i.ToString() + fileExtension;
                while (File.Exists(fileName))
                {
                    Utilities.MoveFile(fileName, DocAttribs.FileDestination, DocAttribs.FileName,
                                       FileLocationsEnum.NotImported, i, fileExtension, true);
                    i++;
                    fileName = DocAttribs.FileFullPath + "." + i.ToString() + fileExtension;
                }
            }
            int j = 1;
            String notChangedFile = DocAttribs.FileFullPath + "." + j.ToString() + Constants.FileNaming.ProductNotchangedCsvExtention;
            while (File.Exists(notChangedFile))
            {
                Utilities.MoveFile(notChangedFile, DocAttribs.FileDestination, DocAttribs.FileName, FileLocationsEnum.NotImported, j, Constants.FileNaming.ProductNotchangedCsvExtention, true);
                j++;
                notChangedFile = DocAttribs.FileFullPath + "." + j.ToString() + Constants.FileNaming.ProductNotchangedCsvExtention;
            }

            string imageFileName = DocAttribs.FileFullPath + Constants.FileNaming.ProductImageFileExtention;
            if (File.Exists(imageFileName))
                Utilities.MoveFile(DocAttribs.FileDestination, DocAttribs.FileName + Constants.FileNaming.ProductImageFileExtention, FileLocationsEnum.NotImported, true);

            j = 1;
            imageFileName = DocAttribs.FileFullPath + "." + j.ToString() + Constants.FileNaming.ImageDeleteCsvExtention;
            while (File.Exists(imageFileName))
            {
                Utilities.MoveFile(imageFileName, DocAttribs.FileDestination, DocAttribs.FileName, FileLocationsEnum.NotImported, j, Constants.FileNaming.ImageDeleteCsvExtention, true);
                j++;
                imageFileName = DocAttribs.FileFullPath + "." + j.ToString() + Constants.FileNaming.ImageDeleteCsvExtention;
            }
        }
        /// <summary>
        /// Checks for thread sleep.
        /// </summary>
        /// <param name="noOfProductsProcessed">The no of products processed.</param>
        protected void CheckForThreadSleep()
        {
            try
            {
                NoOfProductsProcessed++;
                if (NoOfProductsProcessed >= AppSleepProductCount)
                {
                    ObjProductDao.UpdateProductProgramColumn(DocAttribs.ProdProgId,
                                                             Constants.ProductProgram.LinesChecked,
                                                             DocAttribs.Report.TotalProductsProcessed.ToString());
                    System.Threading.Thread.Sleep(30*1000);
                    NoOfProductsProcessed = 0;
                }
            }catch (Exception ex)
            {
                new AffiliGenericException("Error in BaseProcessor.CheckForThreadSleep", ex).CreateLog();
            }
        }
        #region IProcessor Members
        /// <summary>
        /// Create a delimiter seperated header line for processed CSV files.
        /// </summary>
        public void PrepareHeader()
        {
            try
            {
                foreach (DataColumn col in DtProduct.Columns)
                {
                    if (string.IsNullOrEmpty(ObjCSVDataCollections.ProductColHeaders))
                        ObjCSVDataCollections.ProductColHeaders = col.ColumnName;
                    else
                        ObjCSVDataCollections.ProductColHeaders += Constants.Generic.DestFieldSeperator + col.ColumnName;
                }
                ObjCSVDataCollections.ProductImageColHeaders = Constants.ProductImage.ImageID + Constants.Generic.DestFieldSeperator; ;
                ObjCSVDataCollections.ProductImageColHeaders += Constants.ProductImage.ProductProgramID + Constants.Generic.DestFieldSeperator;
                ObjCSVDataCollections.ProductImageColHeaders += Constants.ProductImage.ProgramID + Constants.Generic.DestFieldSeperator;
                ObjCSVDataCollections.ProductImageColHeaders += Constants.ProductImage.ImageURL + Constants.Generic.DestFieldSeperator;
                //ObjCSVDataCollections.ProductImageColHeaders += Constants.ProductImage.HashCode + Constants.Generic.DestFieldSeperator;
                //ObjCSVDataCollections.ProductImageColHeaders += Constants.ProductImage.ImgHeight + Constants.Generic.DestFieldSeperator;
                //ObjCSVDataCollections.ProductImageColHeaders += Constants.ProductImage.ImgWidth + Constants.Generic.DestFieldSeperator;
                ObjCSVDataCollections.ProductImageColHeaders += Constants.ProductImage.ImageNb;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in BaseProcessor.PrepareHeader()", ex);
            }
        }
        /// <summary>
        /// Writes to CSV files in chunks.
        /// </summary>
        public void WriteToCSVFiles()
        {
            if (ObjCSVDataCollections.Products != null)
            {
                CheckToIncrementCsvFileNumber();
                WriteToProductFiles();
            }
            if (ObjCSVDataCollections.ProductImages != null && ObjCSVDataCollections.ProductImages.Count > 0)
            {
                WriteToProductImagesFiles();
                //ImportImagesToProductDB();
                //ObjCacheManager.LoadImageLocalCache(DocAttribs.ProdProgId);
                //SetImageIds();
            }
        }

        /// <summary>
        /// Processed CSV files contains only a subset of (configurable)products
        /// This method determines and increments CSV file number.
        /// A new file is created foe each file number incremented.
        /// </summary>
        private void CheckToIncrementCsvFileNumber()
        {
            //insertedProductsInFile += DocAttribs.Report.NewProducts;
            //updatedProductsInFile += DocAttribs.Report.UpdatedProducts;

            if (insertedProductsInFile >= MaxLinesInCSV)
            {
                insertFileNumber++;
                insertedProductsInFile = 0;
            }
            if (updatedProductsInFile >= MaxLinesInCSV)
            {
                updateFileNumber++;
                updatedProductsInFile = 0;
            }
            if (notChangedProductsInFile >= MaxLinesInCSV)
            {
                NotChangedFileNumber++;
                notChangedProductsInFile = 0;
            }
        }
        /// <summary>
        /// Finishes the writing CSV files.
        /// Writes CSV for deleted products
        /// </summary>
        public void FinishWritingCSVFiles()
        {
            CheckToIncrementCsvFileNumber();
            WriteToCSVFiles();

            ObjCacheManager.GetProductsToDelete(DocAttribs.ProdProgId, ObjCSVDataCollections, DtProduct);
            if (ObjCSVDataCollections.DeletedProducts != null)
            {
                DocAttribs.Report.DeletedProducts = ObjCSVDataCollections.DeletedProducts.Count;
                WriteToDeletedProductFile();
            }

            ObjCacheManager.GetImagessToDelete(ObjCSVDataCollections);
            if (ObjCSVDataCollections.DeletedImages != null)
            {
                DocAttribs.Report.DeletedImages = ObjCSVDataCollections.DeletedImages.Count;
                WriteToDeletedImageFile();
            }

            ObjCSVDataCollections.Dispose();
            ObjCSVDataCollections = null;
            ImagesForDailyReview.Clear();
            ImagesForDailyReview = null;
        }
        Dictionary<String, String> ImagesForDailyReview = new Dictionary<string, string>();
        /// <summary>
        /// Writes to product files.
        /// </summary>
        public void WriteToProductFiles()
        {
            StreamWriter swInsProdCsv = null;
            StreamWriter swUpdProdCsv = null;
            StreamWriter swNotChdProdCsv = null;
            try
            {
                if (ObjCSVDataCollections.Products != null && ObjCSVDataCollections.Products.Count > 0)
                {
                    //Insert file
                    string fileName = DocAttribs.FileFullPath + "." + insertFileNumber.ToString() + Constants.FileNaming.ProductInsertCsvExtention;
                    bool appendFile = File.Exists(fileName);
                    swInsProdCsv = new StreamWriter(fileName, appendFile, Encoding.Unicode);
                    if (!appendFile)
                        swInsProdCsv.WriteLine(ObjCSVDataCollections.ProductColHeaders);

                    //Update File
                    fileName = DocAttribs.FileFullPath + "." + updateFileNumber.ToString() + Constants.FileNaming.ProductUpdateCsvExtention;
                    appendFile = File.Exists(fileName);
                    swUpdProdCsv = new StreamWriter(fileName, appendFile, Encoding.Unicode);
                    if (!appendFile)
                        swUpdProdCsv.WriteLine(ObjCSVDataCollections.ProductColHeaders);

                    //Not Changed File
                    fileName = DocAttribs.FileFullPath + "." + NotChangedFileNumber.ToString() + Constants.FileNaming.ProductNotchangedCsvExtention;
                    appendFile = File.Exists(fileName);
                    swNotChdProdCsv = new StreamWriter(fileName, appendFile, Encoding.Unicode);
                    if (!appendFile)
                        swNotChdProdCsv.WriteLine(ObjCSVDataCollections.ProductColHeaders);
                    
                    long minProductId = GetNextProductId(GetNextIdEnum.Product , NewProductsPer10KCycle);
                    long minImageId = GetNextProductId(GetNextIdEnum.Image, NewImagesPer10KCycle);

                    foreach (Product prod in ObjCSVDataCollections.Products)
                    {
                        if (prod.DbAction == ProductActionsEnum.INSERT)
                        {
                            if (prod.GetField(Constants.Product.ImageID, "").ToString() == "-1")
                            {//new image
                                String duplicateImgId = "";
                                if (IsDuplicateImage(prod, out duplicateImgId))
                                    prod.SetField(Constants.Product.ImageID, duplicateImgId);
                                else
                                {
                                    ImagesForDailyReview.Add(prod.GetImageUrl(DocAttribs), minImageId.ToString());
                                    prod.SetField(Constants.Product.ImageID, minImageId.ToString());
                                    minImageId++;
                                    AddImageObject(prod);
                                }
                            }
                            else
                            {//existing image
                                String duplicateImgId = "";
                                if (!IsDuplicateImage(prod, out duplicateImgId))
                                {
                                    ImagesForDailyReview.Add(prod.GetImageUrl(DocAttribs), prod.GetField(Constants.Product.ImageID, "").ToString());
                                    AddImageObject(prod);
                                }
                            }
                            prod.SetField(Constants.Product.ID, minProductId.ToString());
                            minProductId++;
                            swInsProdCsv.WriteLine(prod.GetLine); //Write Product row to insert product file
                            prod.Dispose();
                            DocAttribs.Report.NewProducts++;
                            insertedProductsInFile++;
                        }
                        else if (prod.DbAction == ProductActionsEnum.UPDATE)
                        {
                            if (prod.GetField(Constants.Product.ImageID, "").ToString() == "-1")
                            {//new image
                                String duplicateImgId = "";
                                if (IsDuplicateImage(prod, out duplicateImgId))
                                    prod.SetField(Constants.Product.ImageID, duplicateImgId);
                                else
                                {
                                    ImagesForDailyReview.Add(prod.GetImageUrl(DocAttribs), minImageId.ToString());
                                    prod.SetField(Constants.Product.ImageID, minImageId.ToString());
                                    minImageId++;
                                    AddImageObject(prod);
                                }
                            }
                            else
                            {//existing image
                                String duplicateImgId = "";
                                if (!IsDuplicateImage(prod, out duplicateImgId))
                                {
                                    ImagesForDailyReview.Add(prod.GetImageUrl(DocAttribs), prod.GetField(Constants.Product.ImageID, "").ToString());
                                    AddImageObject(prod);
                                }
                            }
                            swUpdProdCsv.WriteLine(prod.GetLine); //Write Product row to update product file
                            prod.Dispose();
                            DocAttribs.Report.UpdatedProducts++;
                            updatedProductsInFile++;
                        }
                        else if (prod.DbAction == ProductActionsEnum.NOT_CHANGED_ACTION)
                        {
                            swNotChdProdCsv.WriteLine(prod.GetLine); //Write Product row to not changed product file
                            prod.Dispose();
                            notChangedProductsInFile++;
                        }
                    }
                    ObjCSVDataCollections.Products.Clear(); //Remove object as already written to file
                    NewProductsPer10KCycle = 0;
                    NewImagesPer10KCycle = 0;
                }
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in BaseProcessor.WriteToProductFiles()", ex);
            }
            finally
            {
                CloseFile(swInsProdCsv);
                CloseFile(swUpdProdCsv);
                CloseFile(swNotChdProdCsv);
            }
        }

        /// <summary>
        /// Adds the image object to product images collection later to be written to new images csv.
        /// </summary>
        /// <param name="prod">The prod.</param>
        /// <param name="imageId">The image id.</param>
        private void AddImageObject(Product prod)//, long imageId)
        {
            try
            {
                string ImageField = string.Empty;
                //Determine which image to use
                switch (DocAttribs.ProgSettings.ImageNumToUse)
                {
                    case 1:
                        ImageField = Constants.Product.ImageURL1;
                        break;
                    case 2:
                        ImageField = Constants.Product.ImageURL2;
                        break;
                    case 3:
                        ImageField = Constants.Product.ImageURL3;
                        break;
                }
                IEnumerable<ProductImage> tmpImgs = from item in ObjCSVDataCollections.ProductImages
                                                    where item.ImageUrl == prod.GetField(ImageField, "").ToString()
                                                    select item;
                if (tmpImgs.ToList().Count == 0 && prod.GetField(ImageField, "").ToString() != String.Empty)
                {
                    ProductImage objProductImage = new ProductImage();
                    objProductImage.ProdProgId = DocAttribs.ProdProgId.ToString();
                    objProductImage.ImageUrl = prod.GetField(ImageField, "").ToString();
                    objProductImage.ImageNumToUse = DocAttribs.ProgSettings.ImageNumToUse.ToString();
                    objProductImage.ImageId = prod.GetField(Constants.Product.ImageID,"").ToString();
                    objProductImage.ProgramID = DocAttribs.ProgSettings.ProgramID;
                    ObjCSVDataCollections.ProductImages.Add(objProductImage);
                }
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in BaseProcessor.AddImageObject()ProdProgId:" + DocAttribs.ProdProgId + " Artikel No." + prod.GetField(Constants.Product.ArtikelNumber, ""), ex);
            }
        }
        private bool IsDuplicateImage(Product prod, out String duplicateImgId)
        {
            bool retVal = false;
            duplicateImgId = "";

            if (ImagesForDailyReview.ContainsKey(prod.GetImageUrl(DocAttribs)))
            {
                duplicateImgId = ImagesForDailyReview[prod.GetImageUrl(DocAttribs)];
                retVal = true;
            }
            return retVal;
        }
        /// <summary>
        /// Gets the next product id.
        /// </summary>
        /// <returns></returns>
        private long GetNextProductId(GetNextIdEnum nextIdType, int noOfIds)
        {
            try
            {
                if (noOfIds > 0)
                {
                    DataTable dtNextIds = ObjProductDao.GetNextId(DocAttribs.ProdProgId, nextIdType, noOfIds);
                    long minId = 0;
                    if (dtNextIds == null || dtNextIds.Rows[0]["NextId"] == DBNull.Value)
                        throw new AffiliGenericException(
                            "BaseProcessor.GetNextProductId() Error while fetching new Product Ids: NextId is returned null");
                    long.TryParse(dtNextIds.Rows[0]["NextId"].ToString(), out minId);
                    return minId;
                }
                return 0;
            }catch(Exception ex)
            {
                throw new AffiliGenericException("BaseProcessor.GetNextProductId() Error while fetching new Product Ids",ex);
            }
        }

       public void WriteToDeletedImageFile()
       {
           StreamWriter swDelImgCsv = null;
           try
           {
               int deleteFileNumber = 1;
               int deletedIamgesInFile = 0;
               foreach (String img in ObjCSVDataCollections.DeletedImages)
               {
                   if (deletedIamgesInFile == 0 || deletedIamgesInFile >= MaxLinesInCSV)
                   {//Close file and open a new file
                       CloseFile(swDelImgCsv);
                       string fileName = DocAttribs.FileFullPath + "." + deleteFileNumber.ToString() + Constants.FileNaming.ImageDeleteCsvExtention;
                       bool appendFile = false;// File.Exists(fileName);
                       swDelImgCsv = new StreamWriter(fileName, appendFile, Encoding.Unicode);
                       //if (!appendFile)
                       swDelImgCsv.WriteLine(Constants.ProductImage.ImageID + Constants.Generic.DestFieldSeperator + Constants.ProductImage.ImageURL);//Write CSV header
                       deletedIamgesInFile = 0;
                       deleteFileNumber++;
                   }

                   swDelImgCsv.WriteLine(Constants.Generic.DestFieldQualifier + img + Constants.Generic.DestFieldQualifier); //Write Product row to insert product file
                   deletedIamgesInFile++;
               }
               ObjCSVDataCollections.DeletedImages.Clear(); //Remove object as already written to file
           }
           catch (AffiliBaseException)
           {
               throw;
           }
           catch (Exception ex)
           {
               throw new AffiliGenericException("Error in BaseProcessor.WriteToDeletedProductFile()", ex);
           }
           finally
           {
               CloseFile(swDelImgCsv);
           }
       }

        /// <summary>
        /// Writes to deleted product file.
        /// </summary>
        public void WriteToDeletedProductFile()
        {
            StreamWriter swDelProdCsv = null;
            try
            {
                int deleteFileNumber = 1;
                int deletedProductsInFile = 0;
                foreach (String prod in ObjCSVDataCollections.DeletedProducts)
                {
                    if (deletedProductsInFile == 0 || deletedProductsInFile >= MaxLinesInCSV)
                    {//Close file and open a new file
                        CloseFile(swDelProdCsv);
                        string fileName = DocAttribs.FileFullPath + "." + deleteFileNumber.ToString() + Constants.FileNaming.ProductDeleteCsvExtention;
                        bool appendFile = false;// File.Exists(fileName);
                        swDelProdCsv = new StreamWriter(fileName, appendFile, Encoding.Unicode);
                        //if (!appendFile)
                        swDelProdCsv.WriteLine(Constants.Product.ID);//Write CSV header
                        deletedProductsInFile = 0;
                        deleteFileNumber++;
                    }

                    swDelProdCsv.WriteLine(Constants.Generic.DestFieldQualifier + prod + Constants.Generic.DestFieldQualifier); //Write Product row to insert product file
                    deletedProductsInFile++;
                }
                ObjCSVDataCollections.DeletedProducts.Clear(); //Remove object as already written to file
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in BaseProcessor.WriteToDeletedProductFile()", ex);
            }
            finally
            {
                CloseFile(swDelProdCsv);
            }
        }

        /// <summary>
        /// Closes the file.
        /// </summary>
        private void CloseFile(StreamWriter swProdCsv)
        {
            if (swProdCsv != null)
            {
                swProdCsv.Flush();
                swProdCsv.Close();
                swProdCsv.Dispose();
                swProdCsv = null;
            }
        }
        /// <summary>
        /// This method initiate the processing of source CSV/XMl documemt.
        /// </summary>
        public abstract void ProcessDocumemt();
        /// <summary>
        /// This is a function to create the property XML that is later stored in the Properties column of
        /// the Product Table.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="value">The value.</param>
        /// <param name="isNumeric">if set to <c>true</c> [is numeric].</param>
        /// <param name="objProduct">The obj product.</param>
        /// <returns></returns>
        public string CreatePropertyNode(string title, string value, bool isNumeric, Product objProduct )
        {
            String strProperties = string.Empty;
            try
            {
                value = value.Replace(Constants.Generic.DestFieldQualifier, Constants.Generic.DestFieldQualifierAlternate);
                string strXmlTag = Utilities.GetAppSettingValue(Constants.AppSettings.PropertyXmlTag);
                string strTitle = Utilities.GetAppSettingValue(Constants.AppSettings.PropertyTitleAttrib);
                string strNumber = Utilities.GetAppSettingValue(Constants.AppSettings.PropertyNumberAttrib);
                string strText = Utilities.GetAppSettingValue(Constants.AppSettings.PropertyTextAttrib);
                strProperties = "<" + strXmlTag + " ";
                strProperties += strTitle + "='" + title.Trim() + "' ";
                if (isNumeric)
                    strProperties += strNumber + "='" + Convert.ToDecimal(value.Trim()) + "' ";
                else
                    strProperties += strText + "='" + HttpUtility.HtmlEncode(value.Trim()) + "' ";

                strProperties += " />";

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strProperties);//Validate XML, lest it will generate erros during SSIS import 

            }
            catch (Exception ex)
            {//No throw as it will abondon complete product line
                new AffiliGenericException(
                    "BaseProcessor.CreatePropertyNode() invalid properties XML, value=" + value +
                    ":Program:" + objProduct.ProdProgId + ":Article:" + objProduct.ArtNumber, ex,
                    (int)ApplicationEventsEnum.DocProcessing);
                strProperties = "";
            }
            return strProperties;
        }
       
        public void WriteToProductImagesFiles()
        {
            StreamWriter swProdImgCsv = null;
            try
            {
                string fileName = DocAttribs.FileFullPath + Constants.FileNaming.ProductImageFileExtention;
                bool appendFile = File.Exists(fileName);
                swProdImgCsv = new StreamWriter(fileName, appendFile, Encoding.Unicode);
                if(!appendFile)
                    swProdImgCsv.WriteLine(ObjCSVDataCollections.ProductImageColHeaders);

                foreach (ProductImage img in ObjCSVDataCollections.ProductImages)
                {
                        swProdImgCsv.WriteLine(img.GetLine); //Write Product image row to product images file
                }
                ObjCSVDataCollections.ProductImages.Clear(); //Remove objects as already written to file
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in BaseProcessor.WriteToProductFiles()", ex);
            }
            finally
            {
                CloseFile(swProdImgCsv);
            }
        }
        public void ImportImagesToProductDB()
        {
            Application app = null;
            Package package = null;
            try
            {
                        DTSExecResult results;

                        string csvImageURL = DocAttribs.FileFullPath + Constants.FileNaming.ProductImageFileExtention;

                        app = new Application();

                        string pkgLocation = Utilities.GetAppSettingValue(Constants.AppSettings.LoadImagesSSIS);
                        package = app.LoadPackage(pkgLocation, null);

                        //Set SSIS Package's Global Variables
                        Variables vars = package.Variables;

                        //Set file names and path
                        vars[Constants.SSISParameters.CsvImageURL].Value = csvImageURL.Replace("/",@"\");

                        vars[Constants.SSISParameters.ProdProgId].Value = DocAttribs.ProdProgId;

                        //This value is picked up from ProductData connection string.
                        vars[Constants.SSISParameters.DataSource].Value = Utilities.GetConnectionString(Affilinet.Data.Access.Constants.DBConnections.ProductSSIS);
                

                        lock (thisLock)
                        {
                            results = package.Execute();
                        }
                        string strDtsErrors = "-";
                        foreach(DtsError dtsErr in package.Errors)
                        {
                            strDtsErrors += dtsErr.Description + ":";
                        }
                        Utilities.CreateInfoLog(
                            "SSIS Image data import execute result PPID:" + DocAttribs.ProdProgId + " :" + results.ToString() + " Package:" + pkgLocation + " file:" + vars[Constants.SSISParameters.CsvImageURL].Value //+ " Data server: " + vars[Constants.SSISParameters.DataSource].Value
                            + " Error desc=" + strDtsErrors,
                            ApplicationEventsEnum.DocProcessInit);

            }
            catch (AffiliGenericException aex)
            {
                aex.CreateLog();
            }
            catch (Exception ex)
            {
                AffiliGenericException aex = new AffiliGenericException(
                    "Error in BaseProcessor.ImportImagesToProductDB() Launcing SSIS ProdProgId:" + DocAttribs.ProdProgId, ex,
                    (int)ApplicationEventsEnum.SsisImportStart);
                aex.CreateLog();
            }
            finally
            {
                if (package != null) package.Dispose();
            }
        }
        public void SetImageIds()
        {
            foreach(Product prod in ObjCSVDataCollections.Products)
            {
                ProductImage objProductImage = null;
                if (ObjCacheManager.IsImageInCache(prod, prod.ImageUrlHash, prod.ImageUrl, out objProductImage))//Check in cache
                {
                    if (objProductImage != null && !string.IsNullOrEmpty(objProductImage.ImageId) && objProductImage.ImageId.Trim() != "0")
                        prod.SetField(Constants.Product.ImageID, objProductImage.ImageId);
                }
            }
        }
        #endregion
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
                    if (SrCsvSource != null)
                    {
                        SrCsvSource.Close();
                        SrCsvSource = null;
                        if (ObjProductDao != null)
                            ObjProductDao.Dispose();
                        ObjProductDao = null;
                        if(DtProduct!=null)
                            DtProduct.Clear();
                        DtProduct = null;
                        DocAttribs = null;
                    }
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
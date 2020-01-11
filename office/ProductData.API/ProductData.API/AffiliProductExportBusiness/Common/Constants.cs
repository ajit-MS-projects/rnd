using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Affilinet.Business.ProductExport.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class Constants
    {

        public struct PublisherSettingsColumns
        {
            public const String ProductProgram_ID = "ProductProgram_ID";
            public const String shippingCosts = "shippingCosts";
        }
        public struct ExportXmlTagGroupsTable
        {
            public const String Position = "Position";
            public const String XmlTag = "XmlTag";
        }
        public struct ProdExpConfig
        {
            public const String ProductTableColumn = "ProductTableColumn";
            public const String ExportCsvColumn = "ExportCsvColumn";
            public const String ExportXmlColumn = "ExportXmlColumn";
            public const String XmlGroup = "XmlGroup";
            public const String ComputedColumns = "ComputedColumns";
            public const String ColumnsDefault = "ColumnsDefault";
            public const String ColumnValue = "ColumnValue";
            public const String IsCalculated = "IsCalculated";
            public const String IsDefault = "IsDefault";
            public const String IsActive = "IsActive";
            public const String FieldLength = "FieldLength";
            public const String DataType = "DataType";
        }
        public struct OutputHTTPHeaders
        {
            public const String HeaderContentDisposition = "Content-Disposition";
            public const String HeaderContentDispositionFileAttach = "attachment; filename={0}";
            public const String ContentTypeZip = "application/gzip";
            public const String ContentTypeGZip = "application/gzip";
            public const String ContentTypeXml = "text/xml";
            public const String ContentTypeCsv = "text/csv";
            public const String HeaderLastModified = "Last-Modified";
        }
        public struct ExportFileExtensions
        {
            public const String GZip = ".gz";
            public const String Zip = ".zip";
            public const String CSV = ".csv";
            public const String XML = ".xml";
        }
        public struct ExportXMLTags
        {
            public const String XMLHeader = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
            public const String ProductsStartTag = "<Products ProgramID=\"{0}\" ShopID=\"{1}\">";
            public const String ProductsEndTag = "</Products>";
            public const String ProductStartTag = "<Product ProgramID=\"{0}\" ArticleNumber=\"{1}\" xmlns:contentDetection=\"cd:extension\">";
            public const String ProductEndTag = "</Product>";
            public const String ImgTag = "<Img Name=\"{0}\">";
            public const String ImgUrlTag = "<URL>{0}</URL>";
            public const String ImgWidthTag = "<Width>{0}</Width>";
            public const String ImgHeightTag = "<Height>{0}</Height>";
            public const String PropertiesRootTag = "<Properties>{0}</Properties>";  
        }
        public struct ExportXMLTagGroups
        {
            public const String XmlTagGroups = "XmlTagGroups";
            public const String CategoryPath = "CategoryPath";
            public const String Price = "Price";
            public const String Deeplinks = "Deeplinks";
            public const String Details = "Details";
            public const String Date = "Date";
            public const String Images = "Images";
            public const String Shipping = "Shipping";
            public const String Properties = "Properties";
        }
        /// <summary>
        /// Export CSV/XML files's columns
        /// </summary>
        public struct ExportAppSettingConfigurationPrefixes
        {
            public const String ColumnValue = "ColumnValue";
            public const String CombineColumnsFor = "CombineColumnsFor";
            public const String ColumnsDefaultWith = "ColumnsDefaultWith";
        }

        /// <summary>
        /// ProductProgram table's & fn_drpLists's columns which represent all settings of program
        /// </summary>
        public struct ProgramSettings
        {
            public const string ImportSchemaID = "ID";
            public const string ProgramID = "ProgramID";
            public const string Title = "Title";
            public const string Description = "Description";
            public const string ImportType = "ImportType";
            public const string FieldSeperator = "FldSep";//"FieldSeperator";
            public const string LineSeperator = "LineSeperator";
            public const string Headlines = "Headlines";
            public const string CSVHeadline = "CSVHeadline";
            public const string TextQualifier = "TextQualifier";
            public const string DateFormat = "DateFormat";
            public const string DecimalSymbol = "DecimalSymbol";
            public const string DigitGroupingSymbol = "DigitGroupingSymbol";
            public const string CategorySeperator = "CategorySeperator";
            public const string Active = "Active";
            public const string DefaultAffiliCat = "DefaultAffiliCat";
            public const string ImageNb = "ImageNb";
            public const string Encoding = "EncodingString";
            public const string XmlProductTag = "XmlProductTag";
            public const string PlatformID = "PlatformID";
            public const string DiURL = "diURL";
            public const string TextLinkNb = "TextLinkNb";
        }
          /// <summary>
        /// Export CSV/XML files's columns
        /// </summary>
        public struct ExportProduct
        {
            public const String ArtNumber = "ArtNumber";
            public const String Title = "Title";
            public const String Description_Short = "Description_Short";
            public const String DisplayPrice = "DisplayPrice";
            public const String Img_Url = "Img_url";
            public const String Deeplink1 = "Deeplink1";
            public const String ProductCategoryID = "ProductCategoryID";
            public const String ProductCategoryName = "ProductCategoryName";
            public const String ProgramID = "ProgramID";
            public const String ShopCategoryID = "ShopCategoryID";
            public const String PricePrefix = "PricePrefix";
            public const String Price = "Price";
            public const String PriceSuffix = "PriceSuffix";
            public const String Currency_Symbol = "Currency_Symbol";
            public const String Price_old = "Price_old";
            public const String Description = "Description";
            public const String AffilinetProductCategoryID = "AffilinetProductCategoryID";
            public const String AffilinetProductCategoryName = "AffilinetProductCategoryName";
            public const String img30_url = "Img30_url";
            public const String img60_url = "Img60_url";
            public const String img90_url = "Img90_url";
            public const String img120_url = "Img120_url";
            public const String img180_url = "Img180_url";
            public const String Deeplink2 = "Deeplink2";
            public const String Keywords = "Keywords";
            public const String Manufacturer = "Manufacturer";
            public const String Brand = "Brand";
            public const String Distributor = "Distributor";
            public const String update_date  ="update_date"; 
            public const String valid_from  ="valid_from"; 
            public const String valid_to  ="valid_to"; 
            public const String EAN  ="EAN";
            public const String ShippingPrefix  = "ShippingPrefix";
            public const String Shipping = "Shipping"; 
            public const String ShippingSuffix  ="ShippingSuffix"; 
            public const String Shipping_Currency_Symbol  ="Shipping_Currency_Symbol"; 
            public const String DisplayShipping  ="DisplayShipping"; 
            public const String ProductProgramID  ="ProductProgramID";
            public const String ProductID  ="ProductID"; 
            public const String ImageID ="ImageID";
            public const string ImgWidth = "ImgWidth";
            public const string ImgHeight = "ImgHeight";
            public const string Img30Width = "Img30Width";
            public const string Img30Height = "Img30Height";
            public const string Img60Width = "Img60Width";
            public const string Img60Height = "Img60Height";
            public const string Img90Width = "Img90Width";
            public const string Img90Height = "Img90Height";
            public const string Img120Width = "Img120Width";
            public const string Img120Height = "Img120Height";
            public const string Img180Width = "Img180Width";
            public const string Img180Height = "Img180Height";
            public const string Properties = "Properties";


            // BasePrice
            public const string DisplayBasePrice = "DisplayBasePrice";
            public const string BasePrice = "BasePrice";
            public const string BasePricePrefix = "BasePricePrefix";
            public const string BasePriceSuffix = "BasePriceSuffix";
        }

        /// <summary>
        /// Product table's columns
        /// </summary>
        public struct Product
        {
            public const String ProdCatID = "ProductCategoryID";
            public const String ArtikelNumber = "ArtikelNumber";
            public const String ProductProgramID = "ProductProgramID";
            public const String affiliProductCategoryID = "affiliProductCategoryID";
            public const String ID = "ID";
            public const String ImportDate = "ImportDate";
            public const String Title = "Title";
            public const String Description_short = "Description_short";
            public const String Description = "Description";
            public const String ProductCategoryID = "ProductCategoryID";
            public const String ProductCategoryText = "ProductCategoryText";
            public const String affiliProductCategoryText = "affiliProductCategoryText";
            public const String Price = "Price";
            public const String Price_old = "Price_old";
            public const String Currency_Symbol = "Currency_Symbol";
            public const String ImageURL1 = "ImageURL1";
            public const String ImageURL2 = "ImageURL2";
            public const String ImageURL3 = "ImageURL3";
            public const String DeepLink1 = "DeepLink1";
            public const String DeepLink2 = "DeepLink2";
            public const String Keywords = "Keywords";
            public const String Manufacturer = "Manufacturer";
            public const String Brand = "Brand";
            public const String Distributor = "Distributor";
            public const String ImgWidth1 = "ImgWidth1";
            public const String ImgHeight1 = "ImgHeight1";
            public const String ImgWidth2 = "ImgWidth2";
            public const String ImgHeight2 = "ImgHeight2";
            public const String ImgWidth3 = "ImgWidth3";
            public const String ImgHeight3 = "ImgHeight3";
            public const String update_date = "update_date";
            public const String valid_from = "valid_from";
            public const String valid_to = "valid_to";
            public const String EAN = "EAN";
            public const String ImageID = "ImageID";
            public const String HashCode = "HashCode";
            public const String PricePrefix = "PricePrefix";
            public const String PriceSuffix = "PriceSuffix";
            public const String Shipping = "Shipping";
            public const String ISBN = "ISBN";
            public const String ShippingPrefix = "ShippingPrefix";
            public const String ShippingSuffix = "ShippingSuffix";
            public const String SearchText = "SearchText";
            public const String Properties = "Properties";
            public const String PropertyHash = "PropertyHash";
            public const string ImageOK = "ImageOK"; 
        }
        /// <summary>
        /// Stored procedures that reads data from DB but do not make any DML changes
        /// </summary>
        public struct ReadOnlyStoredProcs
        {
            public const String ReadPublisherAndFileLocationSettings = "spReadPublisherAndFileLocationSettings";
            public const String ReadExportconfiguration = "spReadExportconfiguration";
            public const String GetProdExportProgramFileForDelete = "spSelectProdExportProgramFilesForDelete";
            public const String GetProdExportProgramFileForCopy = "spSelectProdExportProgramFileForCopy";
        }

        /// <summary>
        /// Stored procedures that executes DML SQLs to modify data
        /// </summary>
        public struct DmlStoredProcs
        {
            public const String SpUpdateProdExportProgramFilesCache = "spUpdateProdExportProgramFilesCache";
            public const String SPDeleteProdExportProgramFilesCache = "spDeleteProdExportProgramFilesCache";
            public const String SPResetProdExportProgramFilesCache = "spResetProdExportProgramFilesCache";
            public const String InsertProdExportProgramFilesCacheException = "spInsertProdExportProgramFilesCacheException";            
        }

        /// <summary>
        /// Application configuration settings
        /// </summary>
        public struct AppSettings
        {

            public const String ExportServers = "ExportServers";
            public const String ExportFilesLocation = "ExportFilesLocation";
            public const String ExportCleanUpTimeInervalInMilliSecs = "ExportCleanUpTimeInervalInMilliSecs";
            public const String DistributedPath = "DistributedPath";
            public const String CsvFilesPath = "CsvFilesPath";
            public const String ExportCleanUpEndTime = "ExportCleanUpEndTime";
            public const String ExportCleanUpStartTime = "ExportCleanUpStartTime";
            public const String ExportCopyTimeInervalInMilliSecs = "ExportCopyTimeInervalInMilliSecs";
            public const String WaitForCopyExportFiles = "WaitForCopyExportFiles";
            public const String MaxCopyAttempts = "MaxCopyAttempts";
            public const String AbsoluteExpirationMinutesForExportConfiguration = "AbsoluteExpirationMinutesForExportConfiguration";
        }
        
        /// <summary>
        /// ProdExportProgramFilesCache table's fields
        /// </summary>
        public struct ProdExportProgramFilesCache
        {
            public const String ProdProgId = "ProdProgId";
            public const String ExportTimeStamp = "ExportTimeStamp";
            public const String MaxFileNum = "MaxFileNum";
            public const String LastModified = "ModifiedTime";
            public const String CounterAttempts = "CounterAttempts";
        }

        /// <summary>
        /// Possible values for the FileStatus field from the ProdExportProgramFilesCache table
        /// </summary>
        public struct ProdExportProgramFilesCacheFileStatus
        { 
            public const String Error = "error";
            public const String Ready4Delete = "ready4delete";
            public const String Copied = "copied";
            public const String New = "new";            
        }

        /// <summary>
        /// PartnerCSVFormat table's fields
        /// </summary>
        public struct PartnerCSVFormat
        {
            public const String CSVFields = "CSVFields";
            public const String FieldSeparator = "FieldSeparator";
            public const String TextQualifier = "TextQualifier";
            public const String DecimalSymbol = "DecimalSymbol";
            public const String DateFormat = "DateFormat";
            public const String Nulls = "Nulls";
            public const String ImagesWithSize = "ImagesWithSize";
        }

        /// <summary>
        /// All regular expressions used in project
        /// </summary>
        public struct RegExp
        {
            public const String LineFieldQualifiedSeperator = @"{0}\s*\{1}\s*{0}";
            public const String LineFieldSeperator = @"\s*\{0}\s*";
            public const String UrlValidator = @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
        }

        /// <summary>
        /// Processed CSV file extensions
        /// </summary>
        public struct FileNaming
        {
            public const String ProductInsertCsvExtention = ".product.insert.csv";
            public const String ProductUpdateCsvExtention = ".product.update.csv";
            public const String ProductDeleteCsvExtention = ".product.delete.csv";
            public const String ProductImageFileExtention = ".product.images.csv";
            public const String ProductNotchangedCsvExtention = ".product.notChanged.csv";
            public const String DtsxFileExtension = ".dtsx";
        }
        
        /// <summary>
        /// Common constants
        /// </summary>
        public struct Generic
        {
            public const String DestFieldSeperator = ";";
            public const String DestFieldQualifier = "~\"";//"~";
            public const int ImageIdLastCharsToGenImageUrls= 2;
            public const String ClickAspPrefix = "click.asp?ref=";
            public const String CacheKeyClickUrls = "ClickUrls";
            public const String CacheKeyColumnConfig = "ColumnConfig";
            public const String CacheKeyXmlTagGroups = "XmlTagGroups";
            public const String PropertiesXmlNumericAttributeName = "NUMBER";
            public const String SourceCsvPropertyTag = "ProductProperty";
            public const String TargetXmlPropertyTag = "Property";
        }
        /// <summary>
        /// Prefixed for image urls
        /// </summary>
        public struct ImageUrlPrefixesAndSuffixes
        {
            public const String images30 = "http://images30.affili.net/";
            public const String images60 = "http://images60.affili.net/";
            public const String images90 = "http://images90.affili.net/";
            public const String images120 = "http://images120.affili.net/";
            public const String images180 = "http://images180.affili.net/";
            public const String Suffix404 = "404.gif";
        }
        /// <summary>
        /// Columns of table ProductClickURL
        /// </summary>
        public struct ProductClickURL
        {
            public const String PlatformID = "PlatformID";
            public const String ClickURL = "ClickURL";
        }
    }
}

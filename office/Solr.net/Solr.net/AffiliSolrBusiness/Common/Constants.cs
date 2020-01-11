using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AffiliSolrBusiness.Common
{
    public class Constants
    {
        public struct ReadOnlyStoredProcs
        {
            public const string GetProductSchema = "solr_GetProductSchema";
            public const string GetProducts = "solr_GetProducts";
        }
        public struct DmlStoredProcs
        {
        }
        public struct AppSettings
        {
            public const string DefaultDecimalCulture = "DefaultDecimalCulture";
        }
        public struct ProductProgram
        {
            public const string SourceURL = "SourceURL";
            public const string PordProgId = "ProductProgramID";
            public const string DestinationURL = "URL";// e.g. in DB files/3778_digitalo.csv
            public const string Encoding = "EncodingString";
            public const string LoginName = "LoginName";
            public const string Password = "Password";
            public const string ProgramName = "Name";
            public const string FileType = "FileType";
            public const string ImportStatus = "ImportStatus";
            public const string ManualImport = "ManualImport";
            public const string AutoUpdateNext = "AutoUpdateNext";
            public const string AutoUpdateIntervalType = "AutoUpdateIntervallType";
            public const string AutoUpdateInterval = "AutoUpdateIntervall";
            public const string LinesChecked = "LinesChecked";
            public const string FileCheckSum = "FileCheckSum";
            public const string AdditionalImportStatus = "AdditionalImportStatus";
            public const string AccountManager = "AccountManager";
            public const string AccountManagerEmail = "AccountManagerEmail";
        }
        /// <summary>
        /// Product table's columns
        /// </summary>
        public struct Product
        {
            public const string ProdCatID = "ProductCategoryID";
            public const string ArtikelNumber = "ArtikelNumber";
            public const string ProductProgramID = "ProductProgramID";
            public const string affiliProductCategoryID = "affiliProductCategoryID";
            public const string ID = "ID";
            public const string ImportDate = "ImportDate";
            public const string Title = "Title";
            public const string Description_short = "Description_short";
            public const string Description = "Description";
            public const string ProductCategoryID = "ProductCategoryID";
            public const string ProductCategoryText = "ProductCategoryText";
            public const string affiliProductCategoryText = "affiliProductCategoryText";
            public const string Price = "Price";
            public const string Price_old = "Price_old";
            public const string Currency_Symbol = "Currency_Symbol";
            public const string ImageURL1 = "ImageURL1";
            public const string ImageURL2 = "ImageURL2";
            public const string ImageURL3 = "ImageURL3";
            public const string DeepLink1 = "DeepLink1";
            public const string DeepLink2 = "DeepLink2";
            public const string Keywords = "Keywords";
            public const string Manufacturer = "Manufacturer";
            public const string Brand = "Brand";
            public const string Distributor = "Distributor";
            public const string ImgWidth1 = "ImgWidth1";
            public const string ImgHeight1 = "ImgHeight1";
            public const string ImgWidth2 = "ImgWidth2";
            public const string ImgHeight2 = "ImgHeight2";
            public const string ImgWidth3 = "ImgWidth3";
            public const string ImgHeight3 = "ImgHeight3";
            public const string update_date = "update_date";
            public const string valid_from = "valid_from";
            public const string valid_to = "valid_to";
            public const string EAN = "EAN";
            public const string ImageID = "ImageID";
            public const string HashCode = "HashCode";
            public const string PricePrefix = "PricePrefix";
            public const string PriceSuffix = "PriceSuffix";
            public const string Shipping = "Shipping";
            public const string ISBN = "ISBN";
            public const string ShippingPrefix = "ShippingPrefix";
            public const string ShippingSuffix = "ShippingSuffix";
            public const string SearchText = "SearchText";
            public const string Properties = "Properties";
            public const string PropertyHash = "PropertyHash";

            //Start: Extra columns for export only
            public const string ImageOK = "ImageOK";  //- Part of DB.Product table
            public const string ImgWidth = "ImgWidth";//-Note: Not part of DB.Product table
            public const string ImgHeight = "ImgHeight";//-Note: Not part of DB.Product table
            public const string Img30Width = "Img30Width";//-Note: Not part of DB.Product table
            public const string Img30Height = "Img30Height";//-Note: Not part of DB.Product table
            public const string Img60Width = "Img60Width";//-Note: Not part of DB.Product table
            public const string Img60Height = "Img60Height";//-Note: Not part of DB.Product table
            public const string Img90Width = "Img90Width";//-Note: Not part of DB.Product table
            public const string Img90Height = "Img90Height";//-Note: Not part of DB.Product table
            public const string Img120Width = "Img120Width";//-Note: Not part of DB.Product table
            public const string Img120Height = "Img120Height";//-Note: Not part of DB.Product table
            public const string Img180Width = "Img180Width";//-Note: Not part of DB.Product table
            public const string Img180Height = "Img180Height";//-Note: Not part of DB.Product table
            //End: Extra columns for export only

            public const string ImageName = "ImageName";

            // BasePrice
            public const string BasePrice = "BasePrice";
            public const string BasePricePrefix = "BasePricePrefix";
            public const string BasePriceSuffix = "BasePriceSuffix";
        }
    }
}

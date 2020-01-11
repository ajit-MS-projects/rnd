using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Affilinet.Business.ProductImport.Common;

namespace Affilinet.Business.ProductImport.Entity
{
    /// <summary>
    /// Product entity class, that represent a product instance
    /// </summary>
    public class Product : IDisposable
    {
        private string hashCode;
        private string decimalCulture;
        private string hashCodeProperty;
        public string ProdProgId
        {
            get
            {
                return productRow[Constants.Product.ProductProgramID] == null
                           ? string.Empty
                           : productRow[Constants.Product.ProductProgramID].ToString();
            }
        }
        public string CategoryId
        {
            get
            {
                return productRow[Constants.Product.ProdCatID] == null
                           ? string.Empty
                           : productRow[Constants.Product.ProdCatID].ToString();
            }
        }
        public string ArtNumber
        {
            get
            {
                return productRow[Constants.Product.ArtikelNumber] == DBNull.Value
                           ? string.Empty
                           : productRow[Constants.Product.ArtikelNumber].ToString();
            }
        }
        internal string CategoryText { get; set; }//Not part of product line
        internal string MerchantCat { get; set; }//Not part of product line
        private DataRow productRow;
        private DataTable dtProduct;
        public string GetKey
        {
            get
            {
                string prodProgId = productRow[Constants.Product.ProductProgramID] == DBNull.Value
                                        ? string.Empty
                                        : productRow[Constants.Product.ProductProgramID].ToString();
                string categoryId = productRow[Constants.Product.ProdCatID] == DBNull.Value
                                        ? string.Empty
                                        : productRow[Constants.Product.ProdCatID].ToString();
                string artNumber = productRow[Constants.Product.ArtikelNumber] == DBNull.Value
                                       ? string.Empty
                                       : productRow[Constants.Product.ArtikelNumber].ToString();

                return prodProgId + "," + categoryId + "," + artNumber;
            }
        }
        public string GetLine
        {
            get
            {
                StringBuilder line = new StringBuilder(); ;
                string tmpVal;

                try
                {
                    for (int i = 0; i < dtProduct.Columns.Count; i++)
                    {
                        string strFldValue = productRow[i].ToString();

                        #region "Machine culture independent decimal seperator calculation"

                        if (dtProduct.Columns[i].DataType == Type.GetType("System.Decimal"))
                        {
                            decimal dTmp = -1;
                            if (decimal.TryParse(strFldValue, out dTmp))
                            {
                                CultureInfo cInf = CultureInfo.CreateSpecificCulture(decimalCulture);
                                strFldValue = string.Format(cInf, " {0:0.00}", dTmp).Trim();
                            }
                        }

                        #endregion

                        tmpVal = productRow[i] == DBNull.Value ? String.Empty : strFldValue;
                        productRow[i] = DBNull.Value;
                        line.Append(Constants.Generic.DestFieldQualifier + tmpVal + Constants.Generic.DestFieldQualifier);
                        if (i < dtProduct.Columns.Count - 1)
                            line.Append(Constants.Generic.DestFieldSeperator);
                        strFldValue = null;

                    }

                }
                catch (Exception ex) { throw ex; }
                finally
                {
                    tmpVal = null;
                    productRow = null;
                    dtProduct = null;
                }
                return line.ToString();
            }
        }
        public string ImageUrlHash { get; set; }//Not part of product line
        public string ImageUrl { get; set; }

        public Product(DataTable dtProd, int prodProgId)
        {
            if (dtProd != null)
            {
                productRow = dtProd.NewRow();
            }
            Initialize(dtProd, prodProgId, false);
        }
        private void Initialize(DataTable dtProd, int prodProgId, bool clean)
        {
            if(clean)CleanProduct();
            if (dtProd != null)
            {
                this.dtProduct = dtProd;
                SetField(Constants.Product.ProductProgramID, prodProgId.ToString());
                SetField(Constants.Product.ProductCategoryID, "0");
            }
            decimalCulture = Utilities.GetAppSettingValue(Constants.AppSettings.DefaultDecimalCulture);
        }

        public void Initialize(DataTable dtProd, int prodProgId)
        {
            Initialize(dtProd, prodProgId, true);
        }
        public void SetField(String fieldName, String fieldValue)
        {
            SetField(fieldName, fieldValue, false);
        }
        public void SetField(String fieldName, String fieldValue, bool add)
        {
            if (dtProduct != null && dtProduct.Columns.Contains(fieldName) && !string.IsNullOrEmpty(fieldValue))
            {
                if (add)
                    productRow[fieldName] = productRow[fieldName] + fieldValue;
                else
                    productRow[fieldName] = fieldValue;
            }
        }

        public void SetField(String fieldName, int fieldValue)
        {
            if (dtProduct != null && dtProduct.Columns.Contains(fieldName) )
            {
                    productRow[fieldName] = fieldValue;
            }
        }

        public void SetFieldToNull(String fieldName)
        {
            if (dtProduct != null && dtProduct.Columns.Contains(fieldName))
            {
                productRow[fieldName] = DBNull.Value;
            }
        }
        public object GetField(String fieldName, object defaultVal)
        {
            object retVal = defaultVal;
            if (dtProduct != null && dtProduct.Columns.Contains(fieldName) &&
                productRow[fieldName] != DBNull.Value && productRow[fieldName] != null)
            {
                retVal = productRow[fieldName].ToString();
            }
            return retVal;
        }
        public object GetField(String fieldName)
        {
            return GetField(fieldName, null);
        }
        #region Product Hash
        public string HashCode
        {
            get
            {
                if (string.IsNullOrEmpty(hashCode)) ReCaluculateHash();
                return hashCode;
            }
        }
        public ProductActionsEnum DbAction { get; set; }
        public void ReCaluculateHash()
        {
            StringBuilder strInput = new StringBuilder();
            //strInput.AppendLine(GetField(Constants.Product.Title, "").ToString());
            //strInput.AppendLine(GetField(Constants.Product.Description_short, "").ToString());
            //strInput.AppendLine(GetField(Constants.Product.Description, "").ToString());
            //strInput.AppendLine(GetField(Constants.Product.PricePrefix, "").ToString());
            //string price = GetField(Constants.Product.Price, "0").ToString();
            //if (price == "" || price == "0")
            //    price = GetField(Constants.Product.Price_old, "0").ToString();

            //strInput.AppendLine(price);
            //strInput.AppendLine(GetField(Constants.Product.PriceSuffix, "").ToString());
            //strInput.AppendLine(GetField(Constants.Product.ShippingPrefix, "").ToString());
            //strInput.AppendLine(GetField(Constants.Product.Shipping, "").ToString());
            //strInput.AppendLine(GetField(Constants.Product.ShippingSuffix, "").ToString());
            //strInput.AppendLine(GetField(Constants.Product.DeepLink1, "").ToString());
            //strInput.AppendLine(GetField(Constants.Product.ImageURL1, "").ToString());
            foreach (DataColumn dc in dtProduct.Columns)
            {
                strInput.AppendLine(GetField(dc.ColumnName, "").ToString());
            }

            hashCode = Utilities.GetMd5Hash(strInput.ToString());
            hashCode = hashCode.Replace(Constants.Generic.DestFieldQualifier, "?");//Replace qualifier from hash generated

        }
        public String GetImageUrl(DocumentAttributes DocAttribs)
        {
            string ImageField = string.Empty;
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
            if (!string.IsNullOrEmpty(ImageField) && GetField(ImageField) != null)
                return GetField(ImageField, "").ToString();
            else
                return "";
        }
        #endregion
        #region Property Hash
        public string HashCodeProperty
        {
            get
            {
                if (string.IsNullOrEmpty(hashCodeProperty)) ReCaluculateHashProperty();
                return hashCodeProperty;
            }
        }
        public void ReCaluculateHashProperty()
        {
            hashCodeProperty = Utilities.GetMd5Hash(GetField(Constants.Product.Properties, "").ToString());
        }
        #endregion
        #region Dispose
        public void CleanProduct()
        {
            if (productRow != null)
            {
                for (int i = 0; i < productRow.ItemArray.Count();i++ )
                {
                    productRow[i] = DBNull.Value;
                }
                PropertyInfo[] arrPi = this.GetType().GetProperties();
                foreach (PropertyInfo pi in arrPi)
                {
                    if(pi.CanWrite)
                        pi.SetValue(this,null,null);
                }
                hashCode = null;
                decimalCulture = null;
                hashCodeProperty = null;
                CategoryText = null;
                MerchantCat = null;
            }
        }
        private bool disposed;
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!(this.disposed))
            {
                if (disposing)
                {
                    if (productRow != null)
                    {
                        CleanProduct();
                        productRow = null;
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
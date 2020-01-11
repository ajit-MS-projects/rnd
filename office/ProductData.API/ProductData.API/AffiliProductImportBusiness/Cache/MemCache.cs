using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Affilinet.Business.ProductImport.Cache;
using Affilinet.Business.ProductImport.Common;
using Affilinet.Business.ProductImport.DAO;
using Affilinet.Business.ProductImport.Entity;
using Affilinet.Exceptions;
using System.Data;

namespace Affilinet.Business.ProductImport.Cache
{
    /// <summary>
    /// /// Stores cahce data in RAM in datatables and provides search functionality.
    /// </summary>
    internal class MemCache : CacheManager
    {
        private DataTable DtHashData;
        private DataTable DtCategoryData;
        private DataTable DtImageData;
        private DataTable DtAffiliCatData;

        public override int ShopCategoryCount
        {
            get
            {
                if (DtCategoryData != null)
                {
                    return DtCategoryData.Rows.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemCache"/> class.
        /// </summary>
        public MemCache()
        {
            ObjProductDao = new ProductDAO();
        }
        /// <summary>
        /// Initializes the local cache.
        /// </summary>
        public override void InitLocalCache()
        {
            DtHashData = new DataTable();
            DtCategoryData = new DataTable();
            DtImageData = new DataTable();
        }
        /// <summary>
        /// Loads the image local cache.
        /// </summary>
        /// <param name="ProdProgId">The prod prog id.</param>
        public override void LoadImageLocalCache(int ProdProgId)
        {
            try
            {
                if (DtImageData != null) DtImageData.Clear();
                Utilities.FreeMemory();
                DtImageData = ObjProductDao.GetProductImageForLC(ProdProgId);
                DtImageData.PrimaryKey = new DataColumn[]
                                             {
                                                 DtImageData.Columns[Constants.ProductImage.ProductProgramID],
                                                 DtImageData.Columns[Constants.ProductImage.ImageURL]
                                             };

                //DtImageData.PrimaryKey = new DataColumn[]
                //                            {
                //                                DtImageData.Columns[Constants.ProductImage.HashCode],
                //                                DtImageData.Columns[Constants.ProductImage.ProductProgramID]
                //                            };
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                new AffiliGenericException("Error in MemCache.LoadImageLocalCache", ex).CreateLog();
            }
        }
        /// <summary>
        /// Loads the local cache.
        /// </summary>
        /// <param name="ProdProgId">The prod prog id.</param>
        public override void LoadLocalCache(int ProdProgId)
        {
            DtHashData = ObjProductDao.GetProductHashForLC(ProdProgId);
            DtCategoryData = ObjProductDao.GetProductCatForLC(ProdProgId);
            base.CheckAndFixNullHashValues(ProdProgId);
            LoadImageLocalCache(ProdProgId);
            DtAffiliCatData = ObjProductDao.GetAffilicatForLC();
            DtHashData.PrimaryKey = new DataColumn[]
                                        {
                                            DtHashData.Columns[Constants.Product.ProductProgramID],
                                            DtHashData.Columns[Constants.Product.ProductCategoryID],
                                            DtHashData.Columns[Constants.Product.ArtikelNumber]
                                        };
            DtCategoryData.PrimaryKey = new DataColumn[]
                                            {
                                                DtCategoryData.Columns[Constants.ProductCategory.CatName],
                                                DtCategoryData.Columns[Constants.ProductCategory.ProductProgramID]
                                            };
            DtAffiliCatData.PrimaryKey = new DataColumn[]
                                             {
                                                 DtAffiliCatData.Columns[Constants.ProductCategory.ID]
                                             };
        }

        /// <summary>
        /// Adds the new category to cache.
        /// </summary>
        /// <param name="ProdProgId">The prod prog id.</param>
        /// <param name="CategoryText">The category text.</param>
        /// <param name="objProdCat">The obj prod cat.</param>
        public override void AddNewCategoryToLC(int ProdProgId, string CategoryText, ProductCategory objProdCat)
        {
            DataRow dr = DtCategoryData.NewRow();
            dr[Constants.ProductCategory.ProductProgramID] = ProdProgId;
            dr[Constants.ProductCategory.CatName] = CategoryText;
            dr[Constants.ProductCategory.ID] = objProdCat.ShopCategoryId;
            dr[Constants.ProductCategory.CatPathText] = objProdCat.CatPathText;

            DtCategoryData.Rows.Add(dr);
        }
        /// <summary>
        /// Determines whether searched item is in cahce.
        /// Also sets properties to null if it is not updated
        /// </summary>
        /// <returns>
        /// 	
        /// </returns>
        public override ProductActionsEnum GetProductHashStatus(Product objProduct)
        {


            Object[] arrKey = { objProduct.ProdProgId, objProduct.CategoryId, objProduct.ArtNumber };
            DataRow drHash = DtHashData.Rows.Find(arrKey);

            //////Check for property hash
            ////if (drHash != null && drHash[Constants.Product.PropertyHash] != DBNull.Value &&
            ////        objProduct.HashCodeProperty == drHash[Constants.Product.PropertyHash].ToString())
            ////{//Since nothing is changed in properties set both fields to null
            ////    objProduct.SetFieldToNull(Constants.Product.Properties);
            ////    objProduct.SetFieldToNull(Constants.Product.PropertyHash);
            ////}

            //Check for product hash
            if (drHash == null)//NEW article
            {
                DataRow dr = DtHashData.NewRow();
                dr[Constants.Product.ProductProgramID] = objProduct.ProdProgId;
                dr[Constants.Product.ProductCategoryID] = objProduct.CategoryId;
                dr[Constants.Product.ArtikelNumber] = objProduct.ArtNumber;
                dr[Constants.Product.HashCode] = objProduct.HashCode;
                dr[Constants.Generic.ProductHashActionField] = "I";
                DtHashData.Rows.Add(dr);

                return ProductActionsEnum.INSERT;
            }
            else if (drHash[Constants.Product.HashCode] != DBNull.Value &&
                     objProduct.HashCode == drHash[Constants.Product.HashCode].ToString())
            {
                if (drHash[Constants.Generic.ProductHashActionField].ToString() == "D")
                {
                    drHash[Constants.Generic.ProductHashActionField] = "N";
                    objProduct.SetField(Constants.Product.ID, drHash[Constants.Product.ID].ToString());
                    return ProductActionsEnum.NOT_CHANGED_ACTION;
                }
                else
                    return ProductActionsEnum.IGNORE_ACTION;
            }
            else
            {
                objProduct.SetField(Constants.Product.ID, drHash[Constants.Product.ID].ToString());
                if (drHash[Constants.Generic.ProductHashActionField].ToString() == "D")
                {
                    drHash[Constants.Generic.ProductHashActionField] = "U";
                    drHash[Constants.Product.ID] = DBNull.Value;
                    return ProductActionsEnum.UPDATE;
                }
                else
                    return ProductActionsEnum.IGNORE_ACTION;
            }
        }
        /// <summary>
        /// Determines whether [is category in cache].
        /// </summary>
        /// <param name="objProduct">The object of Product class.</param>
        /// <param name="objProdCat">The object if Prodcategory entity class.</param>
        /// <returns>
        /// 	<c>true</c> if [is category in cache]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsCategoryInCache(Product objProduct, out ProductCategory objProdCat)
        //public override bool IsCategoryInCache(Product objProduct, out string catId)
        {
            Object[] arrKey = { objProduct.CategoryText, objProduct.ProdProgId };
            DataRow drCat = DtCategoryData.Rows.Find(arrKey);
            if (drCat != null && drCat[Constants.ProductCategory.ID] != DBNull.Value && drCat[Constants.ProductCategory.ID] != null)
            {
                objProdCat = new ProductCategory()
                                 {
                                     ShopCategoryId = drCat[Constants.ProductCategory.ID].ToString(),
                                     AffiliCategoryId = drCat[Constants.ProductCategory.AffilinetCatID] == DBNull.Value ? "" : drCat[Constants.ProductCategory.AffilinetCatID].ToString(),
                                     CatPathText = drCat[Constants.ProductCategory.CatPathText] == DBNull.Value ? "" : drCat[Constants.ProductCategory.CatPathText].ToString()
                                 };
                return true;
            }
            objProdCat = null;
            return false;
        }


        /// <summary>
        /// Determines whether [is affili category in cache] [the specified affili cat id].
        /// </summary>
        /// <param name="affiliCatId">The affili cat id.</param>
        /// <param name="affiliCatText">The affili cat text.</param>
        /// <returns>
        /// 	<c>true</c> if [is affili category in cache] [the specified affili cat id]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsAffiliCategoryInCache(String affiliCatId, out String affiliCatText)
        {
            Object[] arrKey = { affiliCatId };
            affiliCatText = string.Empty;
            DataRow drCat = DtAffiliCatData.Rows.Find(arrKey);
            if (drCat != null && drCat[Constants.ProductCategory.ID] != DBNull.Value && drCat[Constants.ProductCategory.ID] != null)
            {
                affiliCatText = drCat[Constants.ProductCategory.AffiliCatPathText] == DBNull.Value
                                    ? ""
                                    : drCat[Constants.ProductCategory.AffiliCatPathText].ToString();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the products to delete.
        /// </summary>
        /// <param name="ProdProgId">The prod prog id.</param>
        /// <param name="objCsvDataCollections">The obj CSV data.</param>
        /// <param name="dtProduct">The dt product.</param>
        public override void GetProductsToDelete(int ProdProgId, CSVDataCollections objCsvDataCollections, DataTable dtProduct)
        {
            DataRow[] prodRows = DtHashData.Select(Constants.Generic.ProductHashActionField + " = 'D'");
            if (prodRows != null)
            {
                //Product objProduct;
                foreach (DataRow prod in prodRows)
                {
                    //objProduct = new Product(dtProduct, ProdProgId);
                    //if (prod[Constants.Product.ProductCategoryID] != DBNull.Value && prod[Constants.Product.ProductCategoryID] != null)
                    //    objProduct.SetField(Constants.Product.ProductCategoryID, prod[Constants.Product.ProductCategoryID].ToString());
                    //if (prod[Constants.Product.ArtikelNumber] != DBNull.Value && prod[Constants.Product.ArtikelNumber] != null)
                    //    objProduct.SetField(Constants.Product.ArtikelNumber, prod[Constants.Product.ArtikelNumber].ToString());
                    //objProduct.SetField(Constants.Product.ID, prod[Constants.Product.ID].ToString());
                    //objCsvDataCollections.DeletedProducts.Add(objProduct);
                    objCsvDataCollections.DeletedProducts.Add(prod[Constants.Product.ID].ToString());
                }
                DtHashData.Clear();
            }
        }
        public override void GetImagessToDelete(CSVDataCollections objCsvDataCollections)
        {

        }

        /// <summary>
        /// Determines whether [is image in cache] [the specified obj product].
        /// </summary>
        /// <param name="objProduct">The obj product.</param>
        /// <param name="imgUrlHash">The img URL hash.</param>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="objProductImage">The obj product image.</param>
        /// <returns>
        /// 	<c>true</c> if [is image in cache] [the specified obj product]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsImageInCache(Product objProduct, string imgUrlHash, string imageUrl, out ProductImage objProductImage)
        {
            objProductImage = null;
            bool retVal = false;

            if (DtImageData.PrimaryKey != null && DtImageData.PrimaryKey.Length > 0)
            {
                Object[] arrKey = {objProduct.ProdProgId, imageUrl};
                DataRow drImage = DtImageData.Rows.Find(arrKey);
                if (drImage != null && drImage[Constants.ProductImage.ImageID] != DBNull.Value &&
                    drImage[Constants.ProductImage.ImageID] != null)
                {
                    objProductImage = new ProductImage();
                    objProductImage.ImageId = drImage[Constants.ProductImage.ImageID].ToString();
                    objProductImage.ImageHeight = drImage[Constants.ProductImage.ImgHeight].ToString();
                    objProductImage.ImageWidth = drImage[Constants.ProductImage.ImgWidth].ToString();
                    retVal = true;
                }
            }
            else
            {
                string filterExp = Constants.ProductImage.ProductProgramID + "='" + objProduct.ProdProgId + "'" +
                                   " and  " + Constants.ProductImage.ImageURL + "='" + imageUrl + "'";
                DataRow[] drImages = DtImageData.Select(filterExp);
                if (drImages != null && drImages.Length > 0 &&
                    drImages[0][Constants.ProductImage.ImageID] != DBNull.Value &&
                    drImages[0][Constants.ProductImage.ImageID] != null)
                {
                    objProductImage = new ProductImage();
                    objProductImage.ImageId = drImages[0][Constants.ProductImage.ImageID].ToString();
                    objProductImage.ImageHeight = drImages[0][Constants.ProductImage.ImgHeight].ToString();
                    objProductImage.ImageWidth = drImages[0][Constants.ProductImage.ImgWidth].ToString();
                    retVal = true;
                }
                DocAttribs.Report.ImageError = Constants.Generic.DuplicateImageUrlError;
            }
            /*For image hashcode implementation
                 *  if (!string.IsNullOrEmpty(imgUrlHash))
                    {
                 Object[] arrKey = { imgUrlHash, objProduct.ProdProgId };
                DataRow drImage = DtImageData.Rows.Find(arrKey);
                if (drImage != null && drImage[Constants.ProductImage.ImageID] != DBNull.Value &&
                    drImage[Constants.ProductImage.ImageID] != null)
                {
                    imageId = drImage[Constants.ProductImage.ImageID].ToString();
                    retVal = true;
                }
                  -------------------
                string filterExp = Constants.ProductImage.HashCode + "='" + imgUrlHash + "' and  " +
                                   Constants.ProductImage.ProductProgramID + "='" + objProduct.ProdProgId + "'";
                DataRow[] drImage = DtImageData.Select(filterExp);
                if (drImage != null && drImage.Length > 0 &&
                    drImage[0][Constants.ProductImage.ImageID] != DBNull.Value &&
                    drImage[0][Constants.ProductImage.ImageID] != null)
                {
                    imageId = drImage[0][Constants.ProductImage.ImageID].ToString();
                    retVal = true;
                }
            }
                 */

                return retVal;
        }
        /// <summary>
        /// Adds the new image to Local Cache.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="imageId">The image id.</param>
        /// <param name="imgUrlHash">The img URL hash.</param>
        /// <param name="imageUrl">The image URL.</param>
        public override void AddNewImageToLC(string prodProgId, string imageId, string imgUrlHash,string imageUrl)
        {
            DataRow dr = DtImageData.NewRow();
            dr[Constants.ProductImage.ProductProgramID] = prodProgId;
            dr[Constants.ProductImage.ImageID] = imageId;
            //dr[Constants.ProductImage.HashCode] = imgUrlHash;
            dr[Constants.ProductImage.ImageURL] = imageUrl;

            DtImageData.Rows.Add(dr);
        }
        /// <summary>
        /// Unloads the local cache.
        /// </summary>
        public override void UnloadLocalCache()
        {
           //FreeMemory();
        }
        /// <summary>
        /// Unoads program's resorces from memory.
        /// </summary>
        //public override void FreeMemory()
        //{
        //    if (DtHashData!=null) DtHashData.Clear();
        //    if (DtCategoryData != null) DtCategoryData.Clear();
        //    if (DtImageData != null) DtImageData.Clear();
        //    if (DtAffiliCatData != null) DtAffiliCatData.Clear();
        //    Utilities.CreateDebugLog("MemCache.FreeMemory()",ApplicationEventsEnum.DocProcessing);
        //}
        #region Dispose

        private bool disposed;
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        public override void Dispose(bool disposing)
        {
            if (!(this.disposed))
            {
                if (disposing)
                {
                    if (DtHashData != null) DtHashData.Clear();
                    if (DtCategoryData != null) DtCategoryData.Clear();
                    if (DtImageData != null) DtImageData.Clear();
                    if (DtAffiliCatData != null) DtAffiliCatData.Clear();
                    DtHashData = null;
                    DtCategoryData = null;
                    DtImageData = null;
                    DtAffiliCatData = null;
                    if (ObjProductDao != null)
                        ObjProductDao.Dispose();
                    ObjProductDao = null;
                    Utilities.CreateDebugLog("MemCache.FreeMemory()", ApplicationEventsEnum.DocProcessing);
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// It also suppresses finalization
        /// </summary>
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);//This call is to makes sure that disposed object does not get put on the GC's finalize queue.
        }
        /// <summary>
        /// Finalizes this instance, if dispose is not called explicitly
        /// </summary>
        protected override void Finalize()
        {
            Dispose(false);



        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Affilinet.Business.ProductImport.Cache;
using Affilinet.Business.ProductImport.Common;
using Affilinet.Business.ProductImport.DAO;
using Affilinet.Business.ProductImport.Entity;
using Affilinet.Exceptions;
using Microsoft.SqlServer.Dts.Runtime;

namespace Affilinet.Business.ProductImport.Cache
{
    /// <summary>
    /// Stores cahce data in SQL server and provides search functionality.
    /// </summary>
    internal class SqlCache : CacheManager
    {
        private int imagesOnly = 0;
        public override int ShopCategoryCount
        {
            get { return 0; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCache"/> class.
        /// </summary>
        public SqlCache()
        {
            throw new NotImplementedException("SqlCache is not implemented and tested.");
            ObjProductDao = new ProductDAO();
        }
        /// <summary>
        /// Initializes the local cache.
        /// </summary>
        public override void InitLocalCache()
        {
            try
            {
                if (!ObjProductDao.InitLocalCache())
                    Utilities.CreateWarningLog("FileProcessor.EmptyLocalCache :- Can not initialise local cache DB", ApplicationEventsEnum.DocProcessInit);
            }
            catch (Exception ex)
            {
                new AffiliGenericException("FileProcessor.EmptyLocalCache :- Can not initialise local cache DB", ex).CreateLog();
            }
        }

        /// <summary>
        /// Loads the image local cache.
        /// </summary>
        /// <param name="ProdProgId">The prod prog id.</param>
        public override void LoadImageLocalCache(int ProdProgId)
        {
            imagesOnly = 1;
            LoadLocalCache(ProdProgId);
            imagesOnly = 0;
        }
        
        /// <summary>
        /// Loads the local cache.
        /// </summary>
        /// <param name="ProdProgId">The prod prog id.</param>
        public override void LoadLocalCache(int ProdProgId)
        {

            Application app = null;
            Package package = null;
            DTSExecResult results;
            try
            {
                base.CheckAndFixNullHashValues(ProdProgId);
                app = new Application();

                string pkgLocation = Utilities.GetAppSettingValue(Constants.AppSettings.LoadLocalCacheSSIS);
                package = app.LoadPackage(pkgLocation, null);

                //Set SSIS Package's Global Variables
                Variables vars = package.Variables;

                //Set Program id
                vars[Constants.SSISParameters.ProdProgId].Value = ProdProgId;
                vars[Constants.SSISParameters.ImagesOnly].Value = imagesOnly;

                //Sql server where localCache db is installed
                //This value is picked up from LocalCache connection string.
                vars[Constants.SSISParameters.DataSource].Value =
                    Utilities.GetConnectionString(Affilinet.Data.Access.Constants.DBConnections.ProductSSIS);
                vars[Constants.SSISParameters.LocalConnectionString].Value =
                    Utilities.GetConnectionString(Affilinet.Data.Access.Constants.DBConnections.LocalCacheSSIS);


                lock (thisLock)
                {
                    results = package.Execute();
                }
                string strDtsErrors = "-";
                foreach (DtsError dtsErr in package.Errors)
                {
                    strDtsErrors += dtsErr.Description + ":";
                }
                Utilities.CreateInfoLog("SSIS Load Local Cache execute result PPID:" + ProdProgId + " :" + results.ToString() + ":Errors:" + strDtsErrors, ApplicationEventsEnum.DocProcessInit);
            }
            catch
            {
                throw;//TODO
            }
            finally
            {
                if (package != null) package.Dispose();
            }
        }

        /// <summary>
        /// Adds the new category to cache.
        /// </summary>
        /// <param name="ProdProgId">The prod prog id.</param>
        /// <param name="CategoryText">The category text.</param>
        /// <param name="objProdCat">The obj prod cat.</param>
        public override void AddNewCategoryToLC(int ProdProgId, string CategoryText, ProductCategory objProdCat)
        {
            throw new NotImplementedException();
            //ObjProductDao.AddNewCategoryToLC(ProdProgId, CategoryText, catId);
        }

        /// <summary>
        /// Determines whether searched item is in cahce.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is in cahce]; otherwise, <c>false</c>.
        /// </returns>
        public override ProductActionsEnum GetProductHashStatus(Product objProduct)
        {
            string productId = "";
            string prodAction = ObjProductDao.GetProductHashStatus(objProduct.ProdProgId, objProduct.ArtNumber, objProduct.CategoryId, objProduct.HashCode,out productId );
            switch (prodAction.ToUpper())
            {
                case "I":
                    return ProductActionsEnum.INSERT;
                case "N":
                    return ProductActionsEnum.NOT_CHANGED_ACTION;
                case "U":
                    objProduct.SetField(Constants.Product.ID, productId);
                    return ProductActionsEnum.UPDATE;
                default:
                    return ProductActionsEnum.NOT_CHANGED_ACTION;
            }
        }
        public override void GetImagessToDelete(CSVDataCollections objCsvDataCollections)
        {

        }
        /// <summary>
        /// Determines whether [is category in cache].
        /// </summary>
        /// <param name="objProduct"></param>
        /// <param name="objProdCat"></param>
        /// <returns>
        /// 	<c>true</c> if [is category in cache]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsCategoryInCache(Product objProduct, out ProductCategory objProdCat)
        //public override bool IsCategoryInCache(Product objProduct, out string catId)
        {//TODO: implement IsCategoryInCache
            objProdCat = null;
            //catId = ObjProductDao.GetCategoryId(objProduct.ProdProgId, objProduct.CategoryText);
            //if (string.IsNullOrEmpty(catId))
                return false;
            //else
            //    return true;
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
        {//TODO: implement IsAffiliCategoryInCache
           affiliCatText = string.Empty;
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
            DataTable dtDelProds = ObjProductDao.GetProductsToDelete(ProdProgId);
            //Product objProduct;
            foreach (DataRow dr in dtDelProds.Rows)
            {
                //objProduct = new Product(dtProduct, ProdProgId);
                //if (dr[Constants.Product.ProductCategoryID] != DBNull.Value && dr[Constants.Product.ProductCategoryID] != null)
                //    objProduct.SetField(Constants.Product.ProductCategoryID, dr[Constants.Product.ProductCategoryID].ToString());
                //if (dr[Constants.Product.ArtikelNumber] != DBNull.Value && dr[Constants.Product.ArtikelNumber] != null)
                //    objProduct.SetField(Constants.Product.ArtikelNumber, dr[Constants.Product.ArtikelNumber].ToString());
                //objProduct.SetField(Constants.Product.ID, dr[Constants.Product.ID].ToString());
                //objCsvDataCollections.DeletedProducts.Add(objProduct);
                objCsvDataCollections.DeletedProducts.Add(dr[Constants.Product.ID].ToString());
            }
            dtDelProds.Clear();
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
            throw new NotImplementedException("IsImageInCache is nnot implemented in SQL cache");
            //imageId = ObjProductDao.GetImageId(objProduct.ProdProgId, imgUrlHash, imageUrl);
            //if (string.IsNullOrEmpty(imageId))
            //    return false;
            //else
                return true;
        }

        /// <summary>
        /// Adds the new image to Local Cache.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="imageId">The image id.</param>
        /// <param name="imgUrlHash">The img URL hash.</param>
        public override void AddNewImageToLC(string prodProgId, string imageId, string imgUrlHash, string imageUrl)
        {
            ObjProductDao.AddNewImageToLC(prodProgId, imageId, imgUrlHash, imageUrl);
        }
        /// <summary>
        /// Unloads the local cache.
        /// </summary>
        public override void UnloadLocalCache()
        {
            ObjProductDao.UnloadLocalCache();
        }
        /// <summary>
        /// Unoads program's resorces from memory.
        /// </summary>
        //public override void FreeMemory(){}
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
                    if (ObjProductDao != null)
                        ObjProductDao.Dispose();
                    ObjProductDao = null;
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
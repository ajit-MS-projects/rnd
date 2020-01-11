using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Affilinet.Business.ProductImport.Common;
using Affilinet.Business.ProductImport.DAO;
using Affilinet.Business.ProductImport.Entity;
using Microsoft.SqlServer.Dts.Runtime;
using Affilinet.Exceptions;
using PIMaintenanceTasks;

namespace Affilinet.Business.ProductImport.Cache
{
    /// <summary>
    /// Base class that manages load of all data into local cache for category, new and updated products.
    /// </summary>
    public abstract class CacheManager : IDisposable
    {
        /// <summary>
        /// This object instance is used to create thread synchronisation.
        /// </summary>
        protected Object thisLock = new Object();
        public DocumentAttributes DocAttribs { get; set; }

        /// <summary>
        /// Static instance to keep single instance of CacheManager
        /// </summary>
        //private static CacheManager objCacheManager = null;
        /// <summary>
        /// Gets or sets the obj product DAO.
        /// </summary>
        /// <value>The obj product DAO.</value>
        protected ProductDAO ObjProductDao { get; set; }

        public abstract int ShopCategoryCount { get; }
        /// <summary>
        /// Initializes the local cache.
        /// </summary>
        public abstract void InitLocalCache();
        /// <summary>
        /// Loads the local cache.
        /// </summary>
        /// <param name="ProdProgId">The prod prog id.</param>
        public abstract void LoadLocalCache(int ProdProgId);
        /// <summary>
        /// Unloads the local cache.
        /// </summary>
        public abstract void UnloadLocalCache();
        /// <summary>
        /// Unoads program's resorces from memory.
        /// </summary>
        //public abstract void FreeMemory();
        /// <summary>
        /// Determines whether searched product is in cahce.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is in cahce]; otherwise, <c>false</c>.
        /// </returns>
        public abstract ProductActionsEnum GetProductHashStatus(Product objProduct);

        /// <summary>
        /// Determines whether [is category in cache].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is category in cache]; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsCategoryInCache(Product objProduct, out ProductCategory objProdCat);


        /// <summary>
        /// Determines whether [is affili category in cache] [the specified affili cat id].
        /// </summary>
        /// <param name="affiliCatId">The affili cat id.</param>
        /// <param name="affiliCatText">The affili cat text.</param>
        /// <returns>
        /// 	<c>true</c> if [is affili category in cache] [the specified affili cat id]; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsAffiliCategoryInCache(String affiliCatId, out String affiliCatText);

        /// <summary>
        /// Adds the new category to cache.
        /// </summary>
        /// <param name="ProdProgId">The prod prog id.</param>
        /// <param name="CategoryText">The category text.</param>
        /// <param name="objProdCat">The obj prod cat.</param>
        public abstract void AddNewCategoryToLC(int ProdProgId, string CategoryText, ProductCategory objProdCat);

        /// <summary>
        /// Gets the cache manager.
        /// </summary>
        /// <returns></returns>
        public static CacheManager GetCacheManager()
        {
            CacheManager objCacheManager = null;
            //if (objCacheManager == null)
            {
                string cacheType = Utilities.GetAppSettingValue(Constants.AppSettings.Cache);
                if (cacheType.ToUpper() == "SQL")
                    objCacheManager = new SqlCache();
                else if ((cacheType.ToUpper() == "DATA_TABLE_MEMORY"))
                    objCacheManager = new MemCache();
                else
                    objCacheManager = new DictionaryMemCache();
            }
            return objCacheManager;
        }

        /// <summary>
        /// Gets the products to delete.
        /// </summary>
        /// <param name="ProdProgId">The prod prog id.</param>
        /// <param name="objCsvDataCollections">The obj CSV data.</param>
        /// <param name="dtProduct">The dt product.</param>
        public abstract void GetProductsToDelete(int ProdProgId, CSVDataCollections objCsvDataCollections, DataTable dtProduct);

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
        public abstract bool IsImageInCache(Product objProduct, string imgUrlHash, string imageUrl, out ProductImage objProductImage);

        /// <summary>
        /// Adds the new image to Local Cache.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="imageId">The image id.</param>
        /// <param name="imgUrlHash">The img URL hash.</param>
        /// <param name="imageUrl">The image URL.</param>
        public abstract void AddNewImageToLC(string prodProgId, string imageId, string imgUrlHash, string imageUrl);
        /// <summary>
        /// Checks the and fix null hash values.
        /// This method create and inserts hash code in DB for images where it is null
        /// This is needed as we need to compare hashcode to get imageId
        /// This must be 1 time process for each program.
        /// Note: Use maintenance utility to generate hash for all programs
        /// </summary>
        /// <param name="ProdProgId">The product program id.</param>
        protected void CheckAndFixNullHashValues(int ProdProgId)
        {
            try
            {
                /////**********Commented as image table has no hash column for now*******
                //ImageMaintenance objImgMain = new ImageMaintenance();
                //if (objImgMain.GetCountOfImagesForHashing(ProdProgId) > 0)
                //{
                //    if (Utilities.GetAppSettingValue(Constants.AppSettings.GenerateImageHash) == "1")
                //        objImgMain.StartImageMaintenance(ProdProgId);
                //    else
                //        throw new AffiliGenericException("Image Table has null values for HashCode column for program: " + ProdProgId + "Please set GenerateImageHash to 1 in app.config or generate hash using hash generate utility.");
                //}

            }
            catch (AffiliGenericException) { throw; }
            catch (Exception ex)
            {
                throw new AffiliGenericException("An error occured while generating hash for program: " + ProdProgId, ex);
            }
        }

        /// <summary>
        /// Loads the image local cache.
        /// </summary>
        /// <param name="ProdProgId">The prod prog id.</param>
        public abstract void LoadImageLocalCache(int ProdProgId);


        /// <summary>
        /// Gets the Images to delete.
        /// </summary>
        /// <param name="objCsvDataCollections">The obj CSV data.</param>
        public abstract void GetImagessToDelete(CSVDataCollections objCsvDataCollections);
        #region Dispose

        private bool disposed;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        public abstract void Dispose(bool disposing);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// It also suppresses finalization
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Finalizes this instance, if dispose is not called explicitly
        /// </summary>
        protected abstract void Finalize();

        #endregion
    }
}
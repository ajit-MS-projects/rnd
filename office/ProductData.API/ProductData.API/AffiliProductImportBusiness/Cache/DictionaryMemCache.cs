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
using System.Globalization;

namespace Affilinet.Business.ProductImport.Cache
{
    class DictionaryMemCache: CacheManager
    {
        private Dictionary<string, ProductHash> dicHashData;
        private Dictionary<string, ProductCategory> dicCategoryData;
        private Dictionary<string, ProductImage> dicImageData;
        private Dictionary<string, string> dicAffiliCatData;
        public override int ShopCategoryCount
        {
            get
            {
                if(dicCategoryData!=null)
                {
                    return dicCategoryData.Count;
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
        public DictionaryMemCache()
        {
            ObjProductDao = new ProductDAO();
            dicHashData = new Dictionary<string, ProductHash>();
            dicCategoryData = new Dictionary<string, ProductCategory>();
            dicImageData = new Dictionary<string, ProductImage>();
            dicAffiliCatData= new Dictionary<string, string>();
        }
        /// <summary>
        /// Initializes the local cache.
        /// </summary>
        public override void InitLocalCache()
        {
        }
        /// <summary>
        /// Loads the image local cache.
        /// </summary>
        /// <param name="ProdProgId">The prod prog id.</param>
        public override void LoadImageLocalCache(int ProdProgId)
        {
            try
            {
                DataTable dtImageData = ObjProductDao.GetProductImageForLC(ProdProgId);
                LoadImageDataToDictionary(dtImageData);
                dtImageData.Clear();
                dtImageData.Dispose();
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
            DataTable dtHashData = ObjProductDao.GetProductHashForLC(ProdProgId);
            LoadProductHashDataToDictionary(dtHashData);
            dtHashData.Clear();
            dtHashData.Dispose();

            DataTable dtCategoryData = ObjProductDao.GetProductCatForLC(ProdProgId);
            LoadCategoryHashDataToDictionary(dtCategoryData);
            dtCategoryData.Clear();
            dtCategoryData.Dispose();

            LoadImageLocalCache(ProdProgId);

            DataTable dtAffiliCategoryData = ObjProductDao.GetAffilicatForLC();
            LoadAffiliCategoryHashDataToDictionary(dtAffiliCategoryData);
            dtAffiliCategoryData.Clear();
            dtAffiliCategoryData.Dispose();

        }

        /// <summary>
        /// Adds the new category to cache.
        /// </summary>
        /// <param name="ProdProgId">The prod prog id.</param>
        /// <param name="CategoryText">The category text.</param>
        /// <param name="objProdCat">The obj prod cat.</param>
        public override void AddNewCategoryToLC(int ProdProgId, string CategoryText, ProductCategory objProdCat)
        {
            dicCategoryData.Add(CategoryText, objProdCat);
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
            string key = objProduct.ArtNumber + objProduct.ProdProgId + objProduct.CategoryId;
            //Check for product hash
            if (!dicHashData.ContainsKey(key))//NEW article
            {
                dicHashData.Add(key,new ProductHash(objProduct.HashCode,"I",null,0,DateTime.Now.ToShortDateString()) );
                objProduct.SetField(Constants.Product.ImageOK, 0);
                return ProductActionsEnum.INSERT;
            }
            else if (!string.IsNullOrEmpty(dicHashData[key].HashCode) &&
                     objProduct.HashCode == dicHashData[key].HashCode)
            {
                if (dicHashData[key].InsertUpdateDelete == "D")
                {
                    dicHashData[key].InsertUpdateDelete = "N";
                    objProduct.SetField(Constants.Product.ID, dicHashData[key].ProductId);
                    objProduct.SetField(Constants.Product.ImageOK, dicHashData[key].ImageOk);
                    objProduct.SetField(Constants.Product.update_date, dicHashData[key].UpdateDate);
                    return ProductActionsEnum.NOT_CHANGED_ACTION;
                }
                else
                    return ProductActionsEnum.IGNORE_ACTION;
            }
            else
            {
                objProduct.SetField(Constants.Product.ID, dicHashData[key].ProductId);
                objProduct.SetField(Constants.Product.ImageOK, dicHashData[key].ImageOk);
                if (dicHashData[key].InsertUpdateDelete == "D")
                {
                    dicHashData[key].InsertUpdateDelete = "U";
                    
                    // added a fixed culture because of problems on german test-machines                                        
                    objProduct.SetField(Constants.Product.update_date, DateTime.Now.ToString(CultureInfo.CreateSpecificCulture("en-US")));
                    return ProductActionsEnum.UPDATE;
                }
                else
                    return ProductActionsEnum.IGNORE_ACTION;
            }
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
        {
            if (dicCategoryData.ContainsKey(objProduct.CategoryText))
            {
                objProdCat = dicCategoryData[objProduct.CategoryText];
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
            affiliCatText = string.Empty;
            if (dicAffiliCatData.ContainsKey(affiliCatId))
            {
                affiliCatText = dicAffiliCatData[affiliCatId];
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
            IEnumerable<KeyValuePair<string, ProductHash>> tmpProds = from item in dicHashData
                                                                      where item.Value.InsertUpdateDelete == "D"
                                                                      select item;
            foreach (KeyValuePair<string, ProductHash> prodHash in tmpProds)
            {
                    objCsvDataCollections.DeletedProducts.Add(prodHash.Value.ProductId);
            }
            //foreach (KeyValuePair<string, ProductHash> prodHash in dicHashData)
            //{
            //    if(prodHash.Value.InsertUpdateDelete == "D")
            //        objCsvDataCollections.DeletedProducts.Add(prodHash.Value.ProductId);
            //}
            dicHashData.Clear();
        }

        /// <summary>
        /// Gets the Images to delete.
        /// </summary>
        /// <param name="objCsvDataCollections">The obj CSV data.</param>
        public override void GetImagessToDelete(CSVDataCollections objCsvDataCollections)
        {
            IEnumerable<KeyValuePair<string, ProductImage>> tmpImgss = from item in dicImageData
                                                                      where item.Value.Action == "D"
                                                                      select item;
            foreach (KeyValuePair<string, ProductImage> prodImg in tmpImgss)
            {
                objCsvDataCollections.DeletedImages.Add(prodImg.Value.ImageId + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator + Constants.Generic.DestFieldQualifier + prodImg.Key);
            }
            dicImageData.Clear();
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
            if (dicImageData.ContainsKey(imageUrl))
            {
                objProductImage = dicImageData[imageUrl];
                objProductImage.Action = "N";
                retVal = true;
            }
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
        //    if (dicHashData != null) dicHashData.Clear();
        //    if (dicCategoryData != null) dicCategoryData.Clear();
        //    if (dicImageData != null) dicImageData.Clear();
        //    if (dicAffiliCatData != null) dicAffiliCatData.Clear();
        //    Utilities.CreateDebugLog("MemCache.FreeMemory()",ApplicationEventsEnum.DocProcessing);
        //}

        /// <summary>
        /// Loads the product hash data to dictionary.
        /// </summary>
        /// <param name="dtHashData">The dt hash data.</param>
         private void LoadProductHashDataToDictionary(DataTable dtHashData)
         {
             foreach(DataRow dr in dtHashData.Rows)
             {
                 dicHashData.Add(dr[Constants.Product.ArtikelNumber].ToString() + 
                                 dr[Constants.Product.ProductProgramID].ToString() +
                                 dr[Constants.Product.ProductCategoryID].ToString(),
                                 new ProductHash(dr[Constants.Product.HashCode].ToString(), 
                                     "D", 
                                     dr[Constants.Product.ID].ToString(),
                                     dr[Constants.Product.ImageOK] == DBNull.Value ? 0 :(dr[Constants.Product.ImageOK].ToString()=="True"?1:0),
                                     dr[Constants.Product.update_date] == DBNull.Value ? "" : dr[Constants.Product.update_date].ToString()));
             }
         }
         /// <summary>
         /// Loads the category hash data to dictionary.
         /// </summary>
         /// <param name="dtCategoryData">The dt category data.</param>
         private void LoadCategoryHashDataToDictionary(DataTable dtCategoryData)
         {
             foreach (DataRow dr in dtCategoryData.Rows)
             {
                 dicCategoryData.Add(dr[Constants.ProductCategory.CatName].ToString(),
                                 new ProductCategory()
                                 {
                                     ShopCategoryId = dr[Constants.ProductCategory.ID].ToString(),
                                     AffiliCategoryId = dr[Constants.ProductCategory.AffilinetCatID] == DBNull.Value ? "" : dr[Constants.ProductCategory.AffilinetCatID].ToString(),
                                     CatPathText = dr[Constants.ProductCategory.CatPathText] == DBNull.Value ? "" : dr[Constants.ProductCategory.CatPathText].ToString(),
                                 });
             }
         }

         /// <summary>
         /// Loads the affili category hash data to dictionary.
         /// </summary>
         /// <param name="dtCategoryData">The dt category data.</param>
         private void LoadAffiliCategoryHashDataToDictionary(DataTable dtCategoryData)
         {
             foreach (DataRow dr in dtCategoryData.Rows)
             {
                 dicAffiliCatData.Add(dr[Constants.ProductCategory.ID].ToString(),
                                      dr[Constants.ProductCategory.CatName].ToString());
             }
         }
         /// <summary>
         /// Loads the image data to dictionary.
         /// </summary>
         /// <param name="dtImageData">The dt image data.</param>
         private void LoadImageDataToDictionary(DataTable dtImageData)
         {
             foreach (DataRow dr in dtImageData.Rows)
             {
                 dicImageData.Add(dr[Constants.ProductImage.ImageURL].ToString(),
                                  new ProductImage()
                                      {
                                          ImageId = dr[Constants.ProductImage.ImageID].ToString(),
                                          ImageHeight = dr[Constants.ProductImage.ImgHeight].ToString(),
                                          ImageWidth = dr[Constants.ProductImage.ImgWidth].ToString(),
                                          //Start: for produc export only
                                          Img30Width = dr[Constants.ProductImage.Img30Width] == DBNull.Value ? "" : dr[Constants.ProductImage.Img30Width].ToString(),
                                          Img30Height = dr[Constants.ProductImage.Img30Height] == DBNull.Value ? "" : dr[Constants.ProductImage.Img30Height].ToString(),
                                          Img60Width = dr[Constants.ProductImage.Img60Width] == DBNull.Value ? "" : dr[Constants.ProductImage.Img60Width].ToString(),
                                          Img60Height = dr[Constants.ProductImage.Img60Height] == DBNull.Value ? "" : dr[Constants.ProductImage.Img60Height].ToString(),
                                          Img90Width = dr[Constants.ProductImage.Img90Width] == DBNull.Value ? "" : dr[Constants.ProductImage.Img90Width].ToString(),
                                          Img90Height = dr[Constants.ProductImage.Img90Height] == DBNull.Value ? "" : dr[Constants.ProductImage.Img90Height].ToString(),
                                          Img120Width = dr[Constants.ProductImage.Img120Width] == DBNull.Value ? "" : dr[Constants.ProductImage.Img120Width].ToString(),
                                          Img120Height = dr[Constants.ProductImage.Img120Height] == DBNull.Value ? "" : dr[Constants.ProductImage.Img120Height].ToString(),
                                          Img180Width = dr[Constants.ProductImage.Img180Width] == DBNull.Value ? "" : dr[Constants.ProductImage.Img180Width].ToString(),
                                          Img180Height = dr[Constants.ProductImage.Img180Height] == DBNull.Value ? "" : dr[Constants.ProductImage.Img180Height].ToString(),
                                          //End: for produc export only
                                          Action = Constants.Generic.DeleteAction
                                      }
                                 );
             }
         }

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
                     if (dicHashData != null) dicHashData.Clear();
                     if (dicCategoryData != null) dicCategoryData.Clear();
                     if (dicImageData != null) dicImageData.Clear();
                     if (dicAffiliCatData != null) dicAffiliCatData.Clear();
                     dicHashData = null;
                     dicCategoryData = null;
                     dicImageData = null;
                     dicAffiliCatData = null;
                     if (ObjProductDao!=null)
                         ObjProductDao.Dispose();
                     ObjProductDao = null;
                     Utilities.CreateDebugLog("DicMemCache.FreeMemory()", ApplicationEventsEnum.DocProcessing);
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
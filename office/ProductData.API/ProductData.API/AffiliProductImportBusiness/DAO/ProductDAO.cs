using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Affilinet.Business.ProductImport.Common;
using Affilinet.Data.Access;
using System.Data;
using Constants=Affilinet.Business.ProductImport.Common.Constants;

namespace Affilinet.Business.ProductImport.DAO
{
    /// <summary>
    /// Represents data access object for Product abstraction.
    /// </summary>
    public class ProductDAO : IDisposable
    {
        /// <summary>
        /// Refers to Live product DB
        /// </summary>
        private IAffiliDatabase _productDBdatabase;
        /// <summary>
        /// Refers to DB to be used as temporary Cache
        /// </summary>
        private IAffiliDatabase _localDatabase;
        /// <summary>
        /// Refers to a read only connection to live DB, used to read program fields and settings
        /// </summary>
        private IAffiliDatabase _liveDatabase;
        private IAffiliDatabase productDBdatabase
        {
            get
            {
                if(_productDBdatabase==null)
                    _productDBdatabase = new AffiliGenericDataBase(DatabaseConnectionsEnum.ProductData);
                return _productDBdatabase;
            }
        }
        private IAffiliDatabase localDatabase
        {
            get
            {
                if (_localDatabase == null)
                    _localDatabase = new AffiliGenericDataBase(DatabaseConnectionsEnum.LocalCache);
                return _localDatabase;
            }
        }
         private IAffiliDatabase   liveDatabase
         {
             get
             {
                 if (_liveDatabase == null)
                     _liveDatabase = new AffiliGenericDataBase(DatabaseConnectionsEnum.ProductDataLiveReadOnly);
                 return _liveDatabase;
             }
         }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDAO"/> class.
        /// </summary>
        public ProductDAO()
        {
            
             
        }
        
        #region Product DB
        /// <summary>
        /// Gets the programs scheduled for download.
        /// </summary>
        /// <returns></returns>
        public DataTable GetScheduledPrograms(bool forDownload)
        {
            return GetScheduledPrograms(forDownload,false);
        }

        /// <summary>
        /// Gets the programs scheduled for download.
        /// </summary>
        /// <returns></returns>
        public DataTable GetScheduledPrograms(bool forDownload, bool isHourly)
        {
            DataTable dtRetVal = null;
            int order = Utilities.GetAppSettingValue(Constants.AppSettings.ProdProgProcessOrder).ToUpper() == "DESC" ? 1 : 0;
            int top = 0;
            int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ProgsToProcessPerCycle), out top);

            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetScheduledPrograms);

            productDBdatabase.AddInParameter("OrderByDesc", DbType.Int16, order);
            productDBdatabase.AddInParameter("Top", DbType.Int16, top);
            productDBdatabase.AddInParameter("ForDownload", DbType.Int16, forDownload ? 1 : 0);
            if (isHourly) productDBdatabase.AddInParameter("IsHourly", DbType.Int16, 1);

            dtRetVal = productDBdatabase.ExecuteReaderProcessed();

            return dtRetVal;
        }
        /// <summary>
        /// Gets the scheduled programs for ssis import.
        /// </summary>
        /// <param name="ManuallySchedOnly">if set to <c>true</c> [manually sched only].</param>
        /// <returns></returns>
        public DataTable GetScheduledProgramsForSsisImport(bool ManuallySchedOnly)
        {
            DataTable dtRetVal = null;
            int top = 0;
            int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ProgsToProcessPerCycle), out top);

            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetScheduledProgramsForSsisImport);

            productDBdatabase.AddInParameter("ManuallySchedOnly", DbType.Int16, ManuallySchedOnly ? 1 : 0);
            productDBdatabase.AddInParameter("Top", DbType.Int16, top);

            dtRetVal = productDBdatabase.ExecuteReaderProcessed();

            return dtRetVal;
        }
        /// <summary>
        /// Gets the program settings required for processing of files.
        /// </summary>
        /// <param name="programIds">Comma seperated program ids.</param>
        /// <returns></returns>
        public DataSet GetProgramSettings(string programIds, bool isXml)
        {
            DataSet dsRetVal = null;

            liveDatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetProgramSettings);

            liveDatabase.AddInParameter("ProgramIds", DbType.String, programIds);
            liveDatabase.AddInParameter("xmlFields", DbType.String, isXml ? 1 : 0);

            dsRetVal = liveDatabase.ExecuteDataset();

            return dsRetVal;
        }
        /// <summary>
        /// Inserts the new category.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="categorySeperator">The category seperator.</param>
        /// <param name="categoryText">The category text.</param>
        /// <param name="MerchantCategoryID">The merchant category ID.</param>
        /// <returns></returns>
        public DataTable InsertNewCategory(int prodProgId, string categorySeperator, string categoryText, string MerchantCategoryID)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.Sp_getCategoryID);

            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("CatSep", DbType.String, categorySeperator);
            productDBdatabase.AddInParameter("Category", DbType.String, categoryText);
            productDBdatabase.AddInParameter("MerchantID", DbType.String, MerchantCategoryID);

            return productDBdatabase.ExecuteReaderProcessed();
        }
        /// <summary>
        /// Gets the product columns.
        /// </summary>
        /// <returns></returns>
        public DataTable GetProductColumns()
        {
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetProductColumns);

            return productDBdatabase.ExecuteDataset().Tables[0];
        }
        /// <summary>
        /// Gets the product hash for LC.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <returns></returns>
        public DataTable GetProductHashForLC(int prodProgId)
        {
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetProductHashForLC);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            
            return productDBdatabase.ExecuteDataset().Tables[0];
        }
        /// <summary>
        /// Gets the product cat for LC.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <returns></returns>
        public DataTable GetProductCatForLC(int prodProgId)
        {
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetProductCatForLC);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);

            return productDBdatabase.ExecuteDataset().Tables[0];
        }

        /// <summary>
        /// Gets all affili categories for LC.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAffilicatForLC()
        {
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetAffilicatForLC);

            return productDBdatabase.ExecuteDataset().Tables[0];
        }

        /// <summary>
        /// Gets the product image for LC.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <returns></returns>
        public DataTable GetProductImageForLC(int prodProgId)
        {
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetProductImageForLC);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);

            return productDBdatabase.ExecuteDataset().Tables[0];
        }

        /// <summary>
        /// Updates the product program status.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="importStatus">The import status.</param>
        public void UpdateProductProgramStatus(int prodProgId, int importStatus)
        {
            UpdateProductProgramStatus(prodProgId, importStatus, -1);
        }

        /// <summary>
        /// Updates the product program status.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="importStatus">The import status.</param>
        /// <param name="errorProducts">The error products.</param>
        public void UpdateProductProgramStatus(int prodProgId, int importStatus, int errorProducts)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.UpdateProductProgramStatus);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("importStatus", DbType.Int32, importStatus);
            productDBdatabase.AddInParameter("importErrors", DbType.Int32, errorProducts);

            productDBdatabase.ExecuteNonQuery();
        }
        /// <summary>
        /// Adds the new image to product db.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="ProgramID">The program ID.</param>
        /// <param name="ImageURL">The image URL.</param>
        /// <param name="ImgWidth">Width of the img.</param>
        /// <param name="ImgHeight">Height of the img.</param>
        /// <param name="ImageNb">The image nb.</param>
        /// <param name="imageUrlHash">The image URL hash.</param>
        /// <returns></returns>
        public string AddNewImageToProductDb(string prodProgId, string ProgramID, string ImageURL, string ImgWidth, string ImgHeight, int ImageNb, string imageUrlHash)
        {
            string retVal = string.Empty;

            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.AddNewImageToProductDB);

            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("ProgramID", DbType.Int32, ProgramID);
            productDBdatabase.AddInParameter("ImageURL", DbType.String, ImageURL);
            productDBdatabase.AddInParameter("ImgWidth", DbType.Int32,
                                             string.IsNullOrEmpty(ImgWidth) ? DBNull.Value : (object) ImgWidth);
            productDBdatabase.AddInParameter("ImgHeight", DbType.Int32,
                                             string.IsNullOrEmpty(ImgHeight) ? DBNull.Value : (object) ImgHeight);
            productDBdatabase.AddInParameter("ImageNb", DbType.Int32, ImageNb);
            productDBdatabase.AddInParameter("HashCode", DbType.String, imageUrlHash);

            object objTmp = productDBdatabase.ExecuteScalar();
            if (objTmp != DBNull.Value)
                retVal = objTmp.ToString();


            return retVal;
        }
        /// <summary>
        /// Updates the product import protocol.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="actionMessage">The action message.</param>
        /// <param name="affectedRows">The affected rows.</param>
        /// <returns></returns>
        public DataTable UpdateProductImportProtocol(int prodProgId,string actionMessage,int affectedRows)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.UpdateProductImportProtocol);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("Action", DbType.String, actionMessage);
            productDBdatabase.AddInParameter("AffectedRows", DbType.Int32, affectedRows);
            return productDBdatabase.ExecuteDataset().Tables[0];
        }
        /// <summary>
        /// Updates the product program.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="AutoUpdateNext">The auto update next.</param>
        /// <param name="importErrors">The import errors.</param>
        /// <param name="productsChanged">if set to <c>true</c> products are deemed changed and ProductProgram.ProductChangeDate field is updated to current date.</param>
        public void UpdateProductProgram(int prodProgId, string AutoUpdateNext, int importErrors, bool productsChanged)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.UpdateProductProgram);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("AutoUpdateNext", DbType.String, AutoUpdateNext);
            productDBdatabase.AddInParameter("importErrors", DbType.Int32, importErrors);
            productDBdatabase.AddInParameter("ProductsChanged", DbType.Int32, productsChanged?1:0);

            productDBdatabase.ExecuteNonQuery();
        }

        public void UpdateProductProgramColumn(int prodProgId, string colName, string ColValue)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.UpdateProductProgramColumn);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("colName", DbType.String, colName);
            productDBdatabase.AddInParameter("ColValue", DbType.Int32, ColValue);

            productDBdatabase.ExecuteNonQuery();
        }

        /// <summary>
        /// Resets the product program(s) which were aborted due to errors or any reason.
        /// </summary>
        public void ResetProductProgram(int resetFromStatus,bool isHourly)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.ResetProductProgram);
            productDBdatabase.AddInParameter("ResetFromStatus", DbType.Int32, resetFromStatus);
            if (isHourly) productDBdatabase.AddInParameter("IsHourly", DbType.Int32, 1);
            productDBdatabase.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the product count.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        public void UpdateProductCount(string prodProgId)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.UpdateProductCount);
            productDBdatabase.AddInParameter("productProgramId", DbType.Int32, prodProgId);
            productDBdatabase.ExecuteNonQuery();
        }
        /// <summary>
        /// Updates the product count.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        public int GetProductProgramStatus(int prodProgId)
        {
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetProductProgramStatus);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            object retVal= productDBdatabase.ExecuteScalar();
            return retVal != DBNull.Value && retVal != null ? (int) retVal : 0;
        }


        /// <summary>
        /// Gets the product program file checksum.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <returns></returns>
        public DataTable GetProductProgramFileChecksum(int prodProgId)
        {
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetProductProgramFileChecksum);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            return productDBdatabase.ExecuteReaderProcessed();
        }

        /// <summary>
        /// Updates the product program additional import status.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="additionalImportStatus">The additional import status.</param>
        /// <returns></returns>
        public void UpdateProductProgramAdditionalImportStatus(int prodProgId, String additionalImportStatus)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.UpdateProductProgramAdditionalImportStatus);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("AdditionalImportStatus", DbType.String, additionalImportStatus);
            productDBdatabase.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the product program file checksum.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="fileChecksum">The file checksum.</param>
        /// <returns></returns>
        public void UpdateProductProgramFileChecksum(int prodProgId, String fileChecksum)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.UpdateProductProgramFileChecksum);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("FileCheckSum", DbType.String, fileChecksum);
            productDBdatabase.ExecuteNonQuery();
        }
        #endregion
        #region Local Cache
        /// <summary>
        /// Inits the local SQL cache.
        /// </summary>
        /// <returns></returns>
        public bool InitLocalCache()
        {
            object dsRetVal = false;

            localDatabase.SetupCommand(Constants.LocalCacheStoredProcs.InitLocalCache);

            dsRetVal = localDatabase.ExecuteScalar();

            return dsRetVal != DBNull.Value && dsRetVal.ToString() == "1" ? true : false;
        }

        /// <summary>
        /// Gets the category id from SQL cache.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="categoryText">The category text.</param>
        /// <returns></returns>
        public string GetCategoryId(string prodProgId, string categoryText)
        {
            object dsRetVal = null;

            localDatabase.SetupCommand(Constants.LocalCacheStoredProcs.GetCategoryId);

            localDatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            localDatabase.AddInParameter("CategoryText", DbType.String, categoryText);

            dsRetVal = localDatabase.ExecuteScalar();
            if (dsRetVal != DBNull.Value && dsRetVal != null)
                return dsRetVal.ToString();
            else
                return string.Empty;
        }
        /// <summary>
        /// Gets the image id from SQL cache.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="imageUrlHash">The image URL hash.</param>
        /// <returns></returns>
        public string GetImageId(string prodProgId, string imageUrlHash, string imageUrl)
        {
            object dsRetVal = null;

            localDatabase.SetupCommand(Constants.LocalCacheStoredProcs.GetImageId);

            localDatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            //localDatabase.AddInParameter("HashCode", DbType.String, imageUrlHash);
            localDatabase.AddInParameter("ImageUrl", DbType.String, imageUrlHash);

            dsRetVal = localDatabase.ExecuteScalar();
            if (dsRetVal != DBNull.Value && dsRetVal != null && dsRetVal.ToString() != "0")
                return dsRetVal.ToString();
            else
                return string.Empty;
        }
        /// <summary>
        /// Adds the new category to SQL cache.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="categoryText">The category text.</param>
        /// <param name="catId">The cat id.</param>
        public void AddNewCategoryToLC(int prodProgId, string categoryText, string catId)
        {

            localDatabase.SetupCommand(Constants.LocalCacheStoredProcs.AddNewCategoryToLC);

            localDatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            localDatabase.AddInParameter("CategoryText", DbType.String, categoryText);
            localDatabase.AddInParameter("CatId", DbType.Int32, catId);

            localDatabase.ExecuteNonQuery();
        }
        /// <summary>
        /// Gets the product hash status from SQL cache.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="articleNum">The article num.</param>
        /// <param name="catId">The cat id.</param>
        /// <param name="hashCode">The hash code.</param>
        /// <returns></returns>
        public string GetProductHashStatus(string prodProgId, string articleNum, string catId, string hashCode, out string productId)
        {

            localDatabase.SetupCommand(Constants.LocalCacheStoredProcs.GetProductHashStatus);

            localDatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            localDatabase.AddInParameter("ArticleNum", DbType.Int32, articleNum);
            localDatabase.AddInParameter("CategoryId", DbType.Int32, catId);
            localDatabase.AddInParameter("HashCode", DbType.String, hashCode);

            DataTable results = localDatabase.ExecuteReaderProcessed();
            string retval = "";
            productId = "";
            if (results != null && results.Rows.Count > 0)
            {
                if(results.Rows[0][Constants.Product.ID]!=DBNull.Value) productId = results.Rows[0][Constants.Product.ID].ToString();
                if (results.Rows[0][Constants.Generic.InsUpdDel] != DBNull.Value) retval = results.Rows[0][Constants.Generic.InsUpdDel].ToString();
            }

            return retval;
        }
        /// <summary>
        /// Gets the products to delete from SQL cache.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <returns></returns>
        public DataTable GetProductsToDelete(int prodProgId)
        {

            localDatabase.SetupCommand(Constants.LocalCacheStoredProcs.GetProductsToDelete);

            localDatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);

            return localDatabase.ExecuteDataset().Tables[0];
        }
        /// <summary>
        /// Adds the new image to local SQL cache.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="imageId">The image id.</param>
        /// <param name="imageUrlHash">The image URL hash.</param>
        public void AddNewImageToLC(string prodProgId, string imageId, string imageUrlHash, string imageUrl)
        {

            localDatabase.SetupCommand(Constants.LocalCacheStoredProcs.AddNewImageToLC);

            localDatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            localDatabase.AddInParameter("ImageID", DbType.String, imageId);
            //localDatabase.AddInParameter("HashCode", DbType.String, imageUrlHash);
            localDatabase.AddInParameter("ImageUrl", DbType.String, imageUrl);

            localDatabase.ExecuteNonQuery();
        }
        /// <summary>
        /// Unloads the local cache.
        /// </summary>
        public void UnloadLocalCache()
        {
            localDatabase.SetupCommand(Constants.LocalCacheStoredProcs.UnloadLocalCache);

            localDatabase.ExecuteNonQuery();
        }
        #endregion

        #region Export
        /// <summary>
        /// Gets the next id for new products or images.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="getNextIdType">Type of the get next id.</param>
        /// <param name="numberOfIds">The number of ids. id set to 0 returns Last result, -1 return 10000 ids, +ve no. return exact no. of ids</param>
        /// <returns></returns>
        public DataTable GetNextId(int prodProgId, GetNextIdEnum getNextIdType, int numberOfIds)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.SpGetNextID);
            productDBdatabase.AddInParameter("ProdProgID", DbType.Int32, prodProgId);
            if (getNextIdType == GetNextIdEnum.Product)
                productDBdatabase.AddInParameter("Type", DbType.String, Constants.Generic.GetNextIdProduct);
            else
                productDBdatabase.AddInParameter("Type", DbType.String, Constants.Generic.GetNextIdImage);
            if(numberOfIds <= 0 )
                productDBdatabase.AddInParameter("Number", DbType.Int32,DBNull.Value);//Returns 10,000 ids
            else
                productDBdatabase.AddInParameter("Number", DbType.Int32, numberOfIds);
            return productDBdatabase.ExecuteDataset().Tables[0];
        }

        /// <summary>
        /// Insert the ProdExportProgramFilesCache table.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        public void AddToProdExportProgramFilesCache(int prodProgId, string exportTimeStamp)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.SpInsertProdExportProgramFilesCache);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("ExportTimeStamp", DbType.String, exportTimeStamp);            
            productDBdatabase.ExecuteNonQuery();
        }

        /// <summary>
        /// Return the ProgramImportStatus
        /// </summary>
        /// <param name="prodProgID"></param>
        /// <returns></returns>
        internal int GetProgramImportStatus(int prodProgID)
        {
            int programImportStatus = 0;

            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetProgramImportStatus);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgID);

            object dsRetVal = productDBdatabase.ExecuteScalar();
            if (dsRetVal != DBNull.Value && dsRetVal != null)
                programImportStatus = Convert.ToInt32(dsRetVal);
            
            return programImportStatus;
        }
        #endregion

        #region Image Import
        /// <summary>
        /// Insert the ProdExportProgramFilesCache table.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="exportTimeStamp">The export time stamp.</param>
        public void AddToImageImportProgramFilesCache(int prodProgId, string exportTimeStamp)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.spInsertImageImportProgramFilesCache);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("ImageImportTimeStamp", DbType.String, exportTimeStamp);
            productDBdatabase.ExecuteNonQuery();
        }
        #endregion

        #region 
        /// <summary>
        /// Updates the last exception in Pa_ProductProgramManagement table.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="exceptionText">The exception text.</param>
        public bool UpdateLastExceptionAndGetHeaderCheckEnabledFlag(int prodProgId, String exceptionText)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.UpdateLastExceptionAndGetHeaderCheck);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("ExceptionText", DbType.String, exceptionText);
            object retVal = productDBdatabase.ExecuteScalar();
            if (retVal == DBNull.Value || retVal == null)
                retVal = 0;

            return (int)retVal == 1;
        }
        public void UpdateErrorIndex(int prodProgId, String errorIndexName, int issueOccurred)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.UpdateErrorIndex);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("errorIndexName", DbType.String, errorIndexName);
            productDBdatabase.AddInParameter("issueOccurred", DbType.Int32, issueOccurred);

            productDBdatabase.ExecuteNonQuery();
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
                    _productDBdatabase = null;
                    _localDatabase = null;
                    _liveDatabase = null;
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
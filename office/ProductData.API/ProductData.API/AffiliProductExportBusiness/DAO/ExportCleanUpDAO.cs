using System;
using Affilinet.Data.Access;
using System.Data;
using Constants=Affilinet.Business.ProductExport.Common.Constants;

namespace Affilinet.Business.ProductExport.DAO
{
    /// <summary>
    /// Represents data access object for Clean Up tasks.
    /// </summary>
    public class ExportCleanUpDAO : IDisposable
    {
        /// <summary>
        /// Refers to Live product DB
        /// </summary>
        private IAffiliDatabase _productDBdatabase;

        private IAffiliDatabase productDBdatabase
        {
            get 
            {
                if (_productDBdatabase == null)
                    _productDBdatabase = _productDBdatabase = new AffiliGenericDataBase(DatabaseConnectionsEnum.ProductData);

                return _productDBdatabase;
            }        
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCleanUpDAO"/> class.
        /// </summary>
        public ExportCleanUpDAO()
        { }

        /// <summary>
        /// Return the rows with the timestamp and prodprogId which can be deleted from filesystem.
        /// </summary>
        /// <returns></returns>
        public DataTable GetExportFilesForDelete()
        {
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetProdExportProgramFileForDelete);
            return productDBdatabase.ExecuteDataset().Tables[0];
        }

        /// <summary>
        /// Updates the ProdExportProgramFilesCache table.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        public void UpdateProdExportProgramFilesCache(string prodProgId, string exportTimeStamp)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.SpUpdateProdExportProgramFilesCache);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("ExportTimeStamp", DbType.String, exportTimeStamp);
            productDBdatabase.ExecuteNonQuery();
        }
                
        /// <summary>
        /// Deletes one entry from ProdExportProgramFilesCache table
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="exportTimeStamp">The exportTimeStamp.</param>
        internal void DeleteExpFileFromDB(string prodProgId, string exportTimeStamp)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.SPDeleteProdExportProgramFilesCache);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("ExportTimeStamp", DbType.String, exportTimeStamp);
            productDBdatabase.ExecuteNonQuery();
        }

        /// <summary>
        /// Return one row with the timestamp and prodprogId which can be copied.
        /// </summary>
        /// <returns></returns>
        internal DataTable GetExportFilesForCopy(int maxAttempts, int waitForCopyExpFiles)
        {
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetProdExportProgramFileForCopy);
            productDBdatabase.AddInParameter("maxAttempts", DbType.Int32, maxAttempts);
            productDBdatabase.AddInParameter("waitForCopyExpFiles", DbType.Int32, waitForCopyExpFiles);
            return productDBdatabase.ExecuteReaderProcessed();
        }

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


        /// <summary>
        /// Reset the CounterAttempts in ProdExportProgramFilesCache table
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="exportTimeStamp">The exportTimeStamp.</param>
        internal void ResetCounterAttepts(string prodProgId, string exportTimeStamp)
        {            
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.SPResetProdExportProgramFilesCache);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("ExportTimeStamp", DbType.String, exportTimeStamp);
            productDBdatabase.ExecuteNonQuery();
        }

        /// <summary>
        /// Increase the CounterAttempts and write the ExceptionMessage into ProdExportProgramFilesCache table 
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <param name="exportTimeStamp">The exportTimeStamp.</param>
        /// <param name="exMessage">The exception message.</param>
        internal void InsertProdExportProgramFilesCacheException(string prodProgId, string exportTimeStamp, string exMessage, int maxAttempts, string fileStatus)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.InsertProdExportProgramFilesCacheException);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("ExportTimeStamp", DbType.String, exportTimeStamp);
            productDBdatabase.AddInParameter("ExMessage", DbType.String, exMessage);
            productDBdatabase.AddInParameter("MaxAttempts", DbType.String, maxAttempts);
            if(fileStatus.Length > 0)
                productDBdatabase.AddInParameter("FileStatus", DbType.String, fileStatus);
            productDBdatabase.ExecuteNonQuery();
        }

        internal void InsertProdExportProgramFilesCacheException(string prodProgId, string exportTimeStamp, string exMessage, int maxAttempts)
        {
            InsertProdExportProgramFilesCacheException(prodProgId, exportTimeStamp, exMessage, maxAttempts, string.Empty);
        }
    }
}

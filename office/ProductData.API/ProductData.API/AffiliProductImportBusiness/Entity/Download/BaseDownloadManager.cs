using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Affilinet.Business.ProductImport.DAO;
using Affilinet.Business.ProductImport.Entity;

namespace Affilinet.Business.ProductImport.Download
{
    /// <summary>
    /// Abstract base class for managing download of files (FTP/HTTP).
    /// </summary>
    public abstract class BaseDownloadManager : IDownloadManager
    {
        #region Delegates and Events
        /// <summary>
        /// Delegate to register download complete event. 
        /// </summary>
        public delegate void DownloadComplete(BaseDownloadManager sender);
        /// <summary>
        /// Occurs when [download of all files completes].
        /// </summary>
        public event DownloadComplete DownloadCompleteEvent;
        /// <summary>
        /// Private delgate for asynchronous download process.
        /// </summary>
        protected delegate void BeginDownloadDocuments();
        #endregion
        #region Implementation of IDownloadManager

        /// <summary>
        /// Gets or sets the document attribute list objects, this list use to itrate though all files and download them.
        /// </summary>
        /// <value>The doc attrib list.</value>
        public abstract List<DocumentAttributes> DocAttribList { get; set; }
        /// <summary>
        /// Gets or sets the obj product Data Access Object.
        /// </summary>
        /// <value>The obj product DAO.</value>
        public ProductDAO ObjProductDao { get; set; }


        /// <summary>
        /// Public Method internally reads a file from a URI and writes to the disk.
        /// </summary>
        /// <returns></returns>
        public abstract void DownloadDocuments();
        /// <summary>
        /// Downloads the documents asynchronously.
        /// </summary>
        public abstract void DownloadDocumentsAsynch();
        #endregion

        /// <summary>
        /// Is called when Asynchronous callback occurs. Raises download complete event.
        /// </summary>
        /// <param name="ar">The return object of asynch call</param>
        protected void DownloadCompleteResults(IAsyncResult ar)
        {
            DownloadCompleteEvent(this);
        }


    }
}
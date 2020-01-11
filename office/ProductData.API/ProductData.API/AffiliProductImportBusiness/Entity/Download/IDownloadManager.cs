using System.Collections.Generic;
using Affilinet.Business.ProductImport.Entity;

namespace Affilinet.Business.ProductImport.Download
{
    /// <summary>
    /// Represents common behaviour foe various file download manager implementations.
    /// </summary>
    public interface IDownloadManager
    {
        /// <summary>
        /// Gets or sets the document attribute list objects, this list use to itrate though all files and download them.
        /// </summary>
        /// <value>The doc attrib list.</value>
        List<DocumentAttributes> DocAttribList { get; set; }

        /// <summary>
        /// Public Method internally reads a file from a URI and writes to the disk.
        /// </summary>
        /// <returns></returns>
        void DownloadDocuments();

        /// <summary>
        /// Downloads the documents asynchronously.
        /// </summary>
        void DownloadDocumentsAsynch();

    }
}
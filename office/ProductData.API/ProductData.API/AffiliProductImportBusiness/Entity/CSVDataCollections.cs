using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Affilinet.Business.ProductImport.Entity;
using Affilinet.Business.ProductImport.Common;

namespace Affilinet.Business.ProductImport.Entity
{
    /// <summary>
    /// Represents product collections and common properties to collections
    /// </summary>
    public class CSVDataCollections : IDisposable
    {
        public String ProductColHeaders { get; set; }
        public List<Product> Products { get; set; }
        public List<String> DeletedProducts { get; set; }
        public List<String> DeletedImages { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public String ProductImageColHeaders { get; set; }
        public CSVDataCollections()
        {
            Products = new List<Product>();
            DeletedProducts = new List<String>();
            DeletedImages = new List<String>();
            ProductImages = new List<ProductImage>();
            //Properties = new List<Property>();
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
                    ProductColHeaders = null;
                    ProductImageColHeaders = null;
                    if (Products != null)
                    {
                        Products.Clear();
                        Products = null;
                    }
                    if (DeletedProducts != null)
                    {
                        DeletedProducts.Clear();
                        DeletedProducts = null;
                    }
                    if (ProductImages != null)
                    {
                        ProductImages.Clear();
                        ProductImages = null;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Affilinet.Business.ProductImport.Common;

namespace Affilinet.Business.ProductImport.Entity
{
    /// <summary>
    /// Represents the relevent image of products to be stored in ProductImage table
    /// </summary>
    public class ProductImage : IDisposable
    {
        public string ImageId { get; set; }
        public string ProdProgId { get; set; }
        public string ProgramID { get; set; }
        public string ImageUrl { get; set; }
        public string ImageUrlHash { get; set; }
        public string ImageHeight { get; set; }
        public string ImageWidth { get; set; }
        public string ImageNumToUse { get; set; }

        //Start: added for product export only
        public string Img30Width{ get; set; }
        public string Img30Height{ get; set; }
        public string Img60Width  {get; set; }
        public string Img60Height { get; set; }
        public string Img90Width { get; set; }
        public string Img90Height { get; set; }
        public string Img120Width { get; set; }
        public string Img120Height { get; set; }
        public string Img180Width { get; set; }
        public string Img180Height { get; set; }
        //End: added for product export only

        //Start:Added for image import
        public string Action { get; set; }
        //End:Added for image import

        /// <summary>
        /// Gets the get line to be stored in CSV.
        /// </summary>
        /// <value>The get line.</value>
        public string GetLine
        {
            get
            {
                StringBuilder line = new StringBuilder();
                line.Append(Constants.Generic.DestFieldQualifier + ImageId + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + ProdProgId + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + ProgramID + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + ImageUrl + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                //line.Append(Constants.Generic.DestFieldQualifier + ImageUrlHash + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                //line.Append(Constants.Generic.DestFieldQualifier + ImageHeight + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                //line.Append(Constants.Generic.DestFieldQualifier + ImageWidth + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + ImageNumToUse + Constants.Generic.DestFieldQualifier);
                //ImageUrl = null;
                //ProdProgId = null;
                return line.ToString();
            }
        }

        #region Dispose
        
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
                    ImageId = null;
                    ProdProgId = null;
                    ProgramID = null;
                    ImageUrl = null;
                    ImageUrlHash = null;
                    ImageHeight = null;
                    ImageWidth = null;
                    ImageNumToUse = null;
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

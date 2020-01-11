using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Affilinet.Business.ImageImport.Common;
using Affilinet.Exceptions;

namespace Affilinet.Business.ImageImport.Entity
{
    public class ProductImage : IDisposable
    {
        public string ImageId { get; set; }
        public string ProdProgId { get; set; }
        public string ProgramID { get; set; }
        public string ImageUrl { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public string ImageNumToUse { get; set; }
        public int Img30Width { get; set; }
        public int Img30Height { get; set; }
        public int Img60Width { get; set; }
        public int Img60Height { get; set; }
        public int Img90Width { get; set; }
        public int Img90Height { get; set; }
        public int Img120Width { get; set; }
        public int Img120Height { get; set; }
        public int Img180Width { get; set; }
        public int Img180Height { get; set; }
        public bool IsValidImage { get; set; }
        public bool IsImageChanged { get; set; }
        public int ErrorCode { get; set; }
        public DateTime AdvUpdatedDate { get; set; }
        public bool IsNewImage
        {
            get
            {
                return ImageId == null;
            }
        }

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
                line.Append(Constants.Generic.DestFieldQualifier + ImageHeight + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + ImageWidth + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + ImageNumToUse + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + Img30Width + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + Img30Height + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + Img60Width + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + Img60Height + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + Img90Width + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + Img90Height + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + Img120Width + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + Img120Height + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + Img180Width + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + Img180Height + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + ErrorCode + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + AdvUpdatedDate + Constants.Generic.DestFieldQualifier);
                return line.ToString();
            }
        }

        public string GetBrokenImageLine
        {
            get
            {
                StringBuilder line = new StringBuilder();
                line.Append(Constants.Generic.DestFieldQualifier + ImageId + Constants.Generic.DestFieldQualifier + Constants.Generic.DestFieldSeperator);
                line.Append(Constants.Generic.DestFieldQualifier + ErrorCode + Constants.Generic.DestFieldQualifier);
                return line.ToString();
            }
        }

        public string GetNotChangedImageLine
        {
            get
            {
                StringBuilder line = new StringBuilder();
                line.Append(Constants.Generic.DestFieldQualifier + ImageId + Constants.Generic.DestFieldQualifier);
                return line.ToString();
            }
        }

        public static String GetBrokenImageHeader()
        {
            try
            {
                String ProductImageColHeaders = "";
                ProductImageColHeaders = Constants.ProductImage.ImageID + Constants.Generic.DestFieldSeperator;
                ProductImageColHeaders += Constants.ProductImage.ErrorCode;
                return ProductImageColHeaders;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in BaseProcessor.PrepareHeader()", ex);
            }
        }

        public static String GetNotChangedImageHeader()
        {
            try
            {
                String ProductImageColHeaders = "";
                ProductImageColHeaders = Constants.ProductImage.ImageID;
                return ProductImageColHeaders;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in BaseProcessor.PrepareHeader()", ex);
            }
        }
            /// <summary>
        /// Create a delimiter seperated header line for processed CSV files.
        /// </summary>
        public static String GetImageHeader()
        {
            try
            {
                String ProductImageColHeaders = "";
                ProductImageColHeaders = Constants.ProductImage.ImageID + Constants.Generic.DestFieldSeperator; 
                ProductImageColHeaders += Constants.ProductImage.ProductProgramID + Constants.Generic.DestFieldSeperator;
                ProductImageColHeaders += Constants.ProductImage.ProgramID + Constants.Generic.DestFieldSeperator;
                ProductImageColHeaders += Constants.ProductImage.ImageURL + Constants.Generic.DestFieldSeperator;
                ProductImageColHeaders += Constants.ProductImage.ImgHeight + Constants.Generic.DestFieldSeperator;
                ProductImageColHeaders += Constants.ProductImage.ImgWidth + Constants.Generic.DestFieldSeperator;
                ProductImageColHeaders += Constants.ProductImage.ImageNb+ Constants.Generic.DestFieldSeperator;
                ProductImageColHeaders += Constants.ProductImage.Img30Width + Constants.Generic.DestFieldSeperator;
                ProductImageColHeaders += Constants.ProductImage.Img30Height + Constants.Generic.DestFieldSeperator; 
                ProductImageColHeaders += Constants.ProductImage.Img60Width + Constants.Generic.DestFieldSeperator;
                ProductImageColHeaders += Constants.ProductImage.Img60Height + Constants.Generic.DestFieldSeperator; 
                ProductImageColHeaders += Constants.ProductImage.Img90Width + Constants.Generic.DestFieldSeperator;
                ProductImageColHeaders += Constants.ProductImage.Img90Height + Constants.Generic.DestFieldSeperator; 
                ProductImageColHeaders += Constants.ProductImage.Img120Width + Constants.Generic.DestFieldSeperator;
                ProductImageColHeaders += Constants.ProductImage.Img120Height + Constants.Generic.DestFieldSeperator; 
                ProductImageColHeaders += Constants.ProductImage.Img180Width + Constants.Generic.DestFieldSeperator;
                ProductImageColHeaders += Constants.ProductImage.Img180Height + Constants.Generic.DestFieldSeperator;
                ProductImageColHeaders += Constants.ProductImage.ErrorCode + Constants.Generic.DestFieldSeperator;
                ProductImageColHeaders += Constants.ProductImage.AdvUpdatedDate;
                return ProductImageColHeaders;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in BaseProcessor.PrepareHeader()", ex);
            }
        }

        public void ReuseObject()
        {
            this.disposed = false;
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
                    ImageHeight = 0;
                    ImageWidth = 0;
                    ImageNumToUse = null;
                    Img30Width = 0;
                    Img30Height = 0;
                    Img60Width = 0;
                    Img60Height = 0;
                    Img90Width = 0;
                    Img90Height = 0;
                    Img120Width = 0;
                    Img120Height = 0;
                    Img180Width = 0;
                    Img180Height = 0;
                    IsValidImage = false;
                    IsImageChanged = false;
                    ErrorCode = 0;
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

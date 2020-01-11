using System;
using System.IO;
using System.Net;
using System.Drawing;

namespace Affilinet.Business.ImageImport
{
    public class Thumbnail : IDisposable
    {
        public Image Image;
        public DateTime AdvUpdatedDate;
        private string ImageName;
        public Thumbnail(string ImageURL)
        {
            this.ImageName = ImageURL.Substring(ImageURL.LastIndexOf("/") + 1);
            this.Image = this.getImageFromURL(ImageURL);
        }

        public string getImageName()
        {
            return this.ImageName.Substring(0, this.ImageName.Length - 4);
        }

        public bool AbortThumbnailGeneration()
        {
            return false;
        }

        public Image GetThumbnail(Image imgFullSize, int nWidth, int nHeight)
        {
            Image.GetThumbnailImageAbort cb = new Image.GetThumbnailImageAbort(AbortThumbnailGeneration);
            return imgFullSize.GetThumbnailImage(nWidth, nHeight, cb, IntPtr.Zero);
        }

        public Image GetThumbnail(Image imgFullSize, int nPixelSize)
        {
            int nImageWidth = imgFullSize.Width;
            int nImageHeight = imgFullSize.Height;
            int nScalePercentage = 0;
            if (nImageHeight > nImageWidth)
            {
                nScalePercentage = (int)(nPixelSize * 100.0 / (double)nImageHeight);
                nImageWidth = (int)((double)nImageWidth * ((double)nScalePercentage / 100.0));
                nImageHeight = nPixelSize;
            }
            else
            {
                nScalePercentage = (int)(nPixelSize * 100.0 / (double)nImageWidth);
                nImageHeight = (int)((double)nImageHeight * ((double)nScalePercentage / 100.0));
                nImageWidth = nPixelSize;
            }
            return GetThumbnail(imgFullSize, nImageWidth, nImageHeight);
        }

        public Image GetThumbnail(int nPixelSize)
        {
            int nImageWidth = this.Image.Width;
            int nImageHeight = this.Image.Height;
            int nScalePercentage = 0;
            if (nImageHeight > nImageWidth)
            {
                nScalePercentage = (int)(nPixelSize * 100.0 / (double)nImageHeight);
                nImageWidth = (int)((double)nImageWidth * ((double)nScalePercentage / 100.0));
                nImageHeight = nPixelSize;
            }
            else
            {
                nScalePercentage = (int)(nPixelSize * 100.0 / (double)nImageWidth);
                nImageHeight = (int)((double)nImageHeight * ((double)nScalePercentage / 100.0));
                nImageWidth = nPixelSize;
            }
            return GetThumbnail(this.Image, nImageWidth, nImageHeight);
        }

        private Image getImageFromURL(string URL)
        {
            WebClient client = new WebClient();
            MemoryStream image;
            Image myImage;

            image = new MemoryStream(client.DownloadData(URL));
            myImage = System.Drawing.Image.FromStream(image);

            String lastUpdDate = client.ResponseHeaders.GetValues("Last-Modified") == null || client.ResponseHeaders.GetValues("Last-Modified").Length<=0? "" : client.ResponseHeaders.GetValues("Last-Modified")[0];
            DateTime.TryParse(lastUpdDate, out AdvUpdatedDate);

            return myImage;
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
                    if (Image != null)
                    {
                        Image.Dispose();
                        Image = null;
                    }
                    ImageName = null;
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

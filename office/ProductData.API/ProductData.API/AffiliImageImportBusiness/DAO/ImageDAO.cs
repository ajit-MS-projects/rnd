using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Affilinet.Business.ImageImport.Common;
using Affilinet.Business.ImageImport.Entity;
using Affilinet.Data.Access;
using Constants = Affilinet.Business.ImageImport.Common.Constants;

namespace Affilinet.Business.ImageImport.DAO
{
    public partial class ImageDAO
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
                    _productDBdatabase = new AffiliGenericDataBase(DatabaseConnectionsEnum.ProductData);
                return _productDBdatabase;
            }
        }

        #region Product DB
        private static object ReadImageImportFileInfoThreadLock = new object();//private static object variable to protect data common to all instances.
        public DataTable ReadImageImportFileInfo(ImageStatusEnum ImageStatus)
        {
            lock (ReadImageImportFileInfoThreadLock)
            {
                productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.ImageImportFileInfo);
                switch (ImageStatus)
                {
                    case ImageStatusEnum.New:
                        productDBdatabase.AddInParameter("FileStatus", DbType.String, Constants.ImageStatus.New);
                        break;
                    case ImageStatusEnum.Ready4delete:
                        productDBdatabase.AddInParameter("FileStatus", DbType.String, Constants.ImageStatus.Ready4delete);
                        break;
                    case ImageStatusEnum.Processed:
                        productDBdatabase.AddInParameter("FileStatus", DbType.String, Constants.ImageStatus.Processed);
                        break;
                }

                return productDBdatabase.ExecuteReaderProcessed();
            }
        }
        public void DeleteImgFileFromDB(String prodProgId, String imageTimeStamp)
        {
            productDBdatabase.SetupCommand(Constants.DMLStoredProcs.ImageImportDeleteFiles);
            productDBdatabase.AddInParameter("prodProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("imageTimeStamp", DbType.String, imageTimeStamp);
            productDBdatabase.ExecuteNonQuery();
        }

        public void UpdateImageImportProgramFilesCache(String prodProgId, String timeStamp, ProdImageFilesCacheStatusEnum prodImageFilesCacheStatus)
        {
                UpdateImageImportProgramFilesCache(prodProgId, timeStamp, prodImageFilesCacheStatus, String.Empty, false);
        }

        public void UpdateImageImportProgramFilesCache(String prodProgId, String timeStamp, ProdImageFilesCacheStatusEnum prodImageFilesCacheStatus, string exceptionMess)
        {
            UpdateImageImportProgramFilesCache(prodProgId, timeStamp, prodImageFilesCacheStatus, exceptionMess, false);
        }

        private static object ImageImportProgramFilesCacheThreadLock = new object();
        public void UpdateImageImportProgramFilesCache(String prodProgId, String timeStamp, ProdImageFilesCacheStatusEnum prodImageFilesCacheStatus, String exceptionMess, bool updateImageCount)
        {
            lock (ImageImportProgramFilesCacheThreadLock)
            {
                productDBdatabase.SetupCommand(Constants.DMLStoredProcs.UpdateImageImportProgramFilesCache);
                String status = String.Empty;
                switch (prodImageFilesCacheStatus)
                {
                    case ProdImageFilesCacheStatusEnum.New:
                        status = Constants.ProdImageFilesCacheStatus.New;
                        break;
                    case ProdImageFilesCacheStatusEnum.Processed:
                        status = Constants.ProdImageFilesCacheStatus.Processed;
                        break;
                    case ProdImageFilesCacheStatusEnum.Processing:
                        status = Constants.ProdImageFilesCacheStatus.Processing;
                        break;
                    case ProdImageFilesCacheStatusEnum.Error:
                        status = Constants.ProdImageFilesCacheStatus.Error;
                        break;
                    case ProdImageFilesCacheStatusEnum.SsisError:
                        status = Constants.ProdImageFilesCacheStatus.SsisError;
                        break;
                    case ProdImageFilesCacheStatusEnum.Ready4delete:
                        status = Constants.ProdImageFilesCacheStatus.Ready4Delete;
                        break;
                    case ProdImageFilesCacheStatusEnum.SsisImportProcessing:
                        status = Constants.ProdImageFilesCacheStatus.SsisImportProcessing;
                        break;
                }
                productDBdatabase.AddInParameter("prodProgId", DbType.Int32, prodProgId);
                productDBdatabase.AddInParameter("ImageImportTimeStamp", DbType.String, timeStamp);
                productDBdatabase.AddInParameter("Status", DbType.String, status);
                productDBdatabase.AddInParameter("exceptionMess", DbType.String, exceptionMess);
                productDBdatabase.AddInParameter("updateImageCount", DbType.Boolean, updateImageCount);
                productDBdatabase.ExecuteNonQuery();
            }
        }
        public void UpdateProductImageCheckedStatus(String prodProgId)
        {
            productDBdatabase.SetupCommand(Constants.DMLStoredProcs.UpdateProductImageCheckedStatus);
            productDBdatabase.AddInParameter("prodProgId", DbType.Int32, prodProgId);
            productDBdatabase.ExecuteNonQuery();
        }

        public DataSet GetImagesToReview()
        {
            return GetImagesToReview(0, "0");
        }

        private static object ImagesToReviewThreadLock = new object();
        public DataSet GetImagesToReview(long imageId, String prodProgId)
        {
            lock (ImagesToReviewThreadLock)
            {
                productDBdatabase.SetupCommand(Constants.DMLStoredProcs.GetImagesToReview);
                productDBdatabase.AddInParameter("ImageId", DbType.Int64, imageId);
                productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);

                return productDBdatabase.ExecuteDataset();
            }
        }
        public DataSet GetImagesForManualReview()
        {
            return GetImagesForManualReview(0, "0");
        }

        private static object ImagesForManualReviewThreadLock = new object();
        public DataSet GetImagesForManualReview(long imageId, String prodProgId)
        {
            lock (ImagesForManualReviewThreadLock)
            {
                productDBdatabase.SetupCommand(Constants.DMLStoredProcs.GetImagesForManualReview);
                productDBdatabase.AddInParameter("ImageId", DbType.Int64, imageId);
                productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);

                return productDBdatabase.ExecuteDataset();
            }
        }

        private static object UpdateImageProgramStatusThreadLock = new object();
        public DataTable UpdateImageProgramStatus(ImageProgramStatusEnum imageProgramStatus, String prodProgId)
        {
            lock (UpdateImageProgramStatusThreadLock)
            {
                productDBdatabase.SetupCommand(Constants.DMLStoredProcs.UpdateImageProgramStatus);
                String status = String.Empty;
                switch (imageProgramStatus)
                {
                    case ImageProgramStatusEnum.Reviewing:
                        status = Constants.ImageProgramStatus.Reviewing;
                        break;
                    case ImageProgramStatusEnum.ReviewComplete:
                        status = Constants.ImageProgramStatus.ReviewComplete;
                        break;
                    case ImageProgramStatusEnum.SsisImportComplete:
                        status = Constants.ImageProgramStatus.SsisImportComplete;
                        break;
                    case ImageProgramStatusEnum.Null:
                        status = Constants.ImageProgramStatus.Null;
                        break;
                    case ImageProgramStatusEnum.Error:
                        status = Constants.ImageProgramStatus.Error;
                        break;
                    case ImageProgramStatusEnum.SsisImportProcessing:
                        status = Constants.ImageProgramStatus.SsisImportProcessing;
                        break;
                }
                productDBdatabase.AddInParameter("Status", DbType.String, status);
                productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);

                return productDBdatabase.ExecuteReaderProcessed();
            }
        }
        public void UpdateSingleImageInDB(ProductImage objImage)
        {
            productDBdatabase.SetupCommand(Constants.DMLStoredProcs.UpdateSingleImage);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, objImage.ProdProgId);
            productDBdatabase.AddInParameter(Constants.ProductImage.ImageURL, DbType.String, objImage.ImageUrl);
            productDBdatabase.AddInParameter(Constants.ProductImage.ImgWidth, DbType.Int32, objImage.ImageWidth == 0 ? (object)DBNull.Value : (object)objImage.ImageWidth);
            productDBdatabase.AddInParameter(Constants.ProductImage.ImgHeight, DbType.Int32, objImage.ImageHeight == 0 ? (object)DBNull.Value : (object)objImage.ImageHeight);
            productDBdatabase.AddInParameter(Constants.ProductImage.Img30Width, DbType.Int32, objImage.Img30Width == 0 ? (object)DBNull.Value : (object)objImage.Img30Width);
            productDBdatabase.AddInParameter(Constants.ProductImage.Img30Height, DbType.Int32, objImage.Img30Height == 0 ? (object)DBNull.Value : (object)objImage.Img30Height);
            productDBdatabase.AddInParameter(Constants.ProductImage.Img60Width, DbType.Int32, objImage.Img60Width == 0 ? (object)DBNull.Value : (object)objImage.Img60Width);
            productDBdatabase.AddInParameter(Constants.ProductImage.Img60Height, DbType.Int32, objImage.Img60Height == 0 ? (object)DBNull.Value : (object)objImage.Img60Height);
            productDBdatabase.AddInParameter(Constants.ProductImage.Img90Width, DbType.Int32, objImage.Img90Width == 0 ? (object)DBNull.Value : (object)objImage.Img90Width);
            productDBdatabase.AddInParameter(Constants.ProductImage.Img90Height, DbType.Int32, objImage.Img90Height == 0 ? (object)DBNull.Value : (object)objImage.Img90Height);
            productDBdatabase.AddInParameter(Constants.ProductImage.Img120Width, DbType.Int32, objImage.Img120Width == 0 ? (object)DBNull.Value : (object)objImage.Img120Width);
            productDBdatabase.AddInParameter(Constants.ProductImage.Img120Height, DbType.Int32, objImage.Img120Height == 0 ? (object)DBNull.Value : (object)objImage.Img120Height);
            productDBdatabase.AddInParameter(Constants.ProductImage.Img180Width, DbType.Int32, objImage.Img180Width == 0 ? (object)DBNull.Value : (object)objImage.Img180Width);
            productDBdatabase.AddInParameter(Constants.ProductImage.Img180Height, DbType.Int32, objImage.Img180Height == 0 ? (object)DBNull.Value : (object)objImage.Img180Height);
            productDBdatabase.AddInParameter(Constants.ProductImage.AdvUpdatedDate, DbType.DateTime, objImage.AdvUpdatedDate == DateTime.MinValue ? (object)DBNull.Value : objImage.AdvUpdatedDate);

            productDBdatabase.ExecuteNonQuery();
        }

        public void IncrSingleImageBrokenCounter(String prodProgId, String imageUrl)
        {
            productDBdatabase.SetupCommand(Constants.DMLStoredProcs.IncrSingleImageBrokenCounter);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter(Constants.ProductImage.ImageURL, DbType.String, imageUrl);

            productDBdatabase.ExecuteNonQuery();
        }
        public DataTable GetImageReviewFilesToDelete()
        {
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetImageReviewFilesToDelete);
            return productDBdatabase.ExecuteReaderProcessed();
        }
        /// <summary>
        /// Gets the image program status.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        internal String GetImageProgramStatus(String prodProgId)
        {
            object retVal = null;
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetImageProgramStatus);
            productDBdatabase.AddInParameter("prodProgId", DbType.String, prodProgId);
            retVal = productDBdatabase.ExecuteScalar();
            if (retVal != null && retVal != DBNull.Value)
                return retVal.ToString();
            else
                return String.Empty;
        }

        /// <summary>
        /// Gets the image delete status from ProdImageFilesCache
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        internal String GetImageDeleteStatus(String prodProgId, string imageImportTimeStamp)
        {
            object retVal = null;
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetImageDeleteStatus);
            productDBdatabase.AddInParameter("ProdProgId", DbType.String, prodProgId);
            productDBdatabase.AddInParameter("ImageImportTimeStamp", DbType.String, imageImportTimeStamp);
            retVal = productDBdatabase.ExecuteScalar();
            if (retVal != null && retVal != DBNull.Value)
                return retVal.ToString();
            else
                return String.Empty;
        }

        /// <summary>
        /// Gets the daily image status.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        internal String GetDailyImageStatus(String prodProgId)
        {
            object retVal = null;
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetDailyImageStatus);
            productDBdatabase.AddInParameter("prodProgId", DbType.String, prodProgId);
            retVal = productDBdatabase.ExecuteScalar();
            if (retVal != null && retVal != DBNull.Value)
                return retVal.ToString();
            else
                return String.Empty;
        }

        /// <summary>
        /// Return a program where images can be deleted from FileSystem
        /// </summary>
        /// <returns></returns>
        internal DataTable GetProdProgForImageDeleteFromFS()
        {
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetProdProgForImageDeleteFromFS);
            return productDBdatabase.ExecuteReaderProcessed();
        }

        /// <summary>
        /// Return a program where images can be deleted from db
        /// </summary>
        /// <returns></returns>
        internal DataTable GetProdProgForImageDeleteFromDb()
        {
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetProdProgForImageDeleteFromDB);
            return productDBdatabase.ExecuteReaderProcessed();
        }

        #endregion

        

        internal void ResetImageProgramFilesCacheDeleteStatus(ProdImageFilesCacheDeleteStatusEnum oldStatus, ProdImageFilesCacheDeleteStatusEnum newStatus)
        {
            productDBdatabase.SetupCommand(Constants.DMLStoredProcs.ResetImageFilesCacheDeleteStatus);
            productDBdatabase.AddInParameter("OldImageDeleteStatus", DbType.String, GetProdImageFilesCacheDeleteStatus(oldStatus));
            productDBdatabase.AddInParameter("NewImageDeleteStatus", DbType.String, GetProdImageFilesCacheDeleteStatus(newStatus));
            productDBdatabase.ExecuteNonQuery();
        }

        /// <summary>
        /// Helper function to convert ProdImageFilesCacheStatusEnum to string from constants
        /// </summary>
        /// <param name="statusEnum"></param>
        /// <returns></returns>
        internal string GetProdImageFilesCacheDeleteStatus(ProdImageFilesCacheDeleteStatusEnum statusEnum)
        {
            String status = String.Empty;
            switch (statusEnum)
            {
                case ProdImageFilesCacheDeleteStatusEnum.new2delete:
                    status = Constants.ProdImageFilesCacheDeleteStatus.new2delete;
                    break;
                case ProdImageFilesCacheDeleteStatusEnum.processingfsdelete:
                    status = Constants.ProdImageFilesCacheDeleteStatus.processingfsdelete;
                    break;
                case ProdImageFilesCacheDeleteStatusEnum.processingssisdelete:
                    status = Constants.ProdImageFilesCacheDeleteStatus.processingssisdelete;
                    break;
                case ProdImageFilesCacheDeleteStatusEnum.ready4dbdelete:
                    status = Constants.ProdImageFilesCacheDeleteStatus.ready4dbdelete;
                    break;
                case ProdImageFilesCacheDeleteStatusEnum.ready4delete:
                    status = Constants.ProdImageFilesCacheDeleteStatus.ready4delete;
                    break;
                case ProdImageFilesCacheDeleteStatusEnum.error:
                    status = Constants.ProdImageFilesCacheDeleteStatus.error;
                    break;
            }

            return status;
        }

        internal void UpdateImageProgramFilesCacheDeleteStatus(string prodProgId, String imageTimeStamp, ProdImageFilesCacheDeleteStatusEnum status )
        {

            UpdateImageProgramFilesCacheDeleteStatus(prodProgId, imageTimeStamp, status, string.Empty);
        }

        public void UpdateImageProgramFilesCacheDeleteStatus(String prodProgId, String timeStamp, ProdImageFilesCacheDeleteStatusEnum prodImgDelFilesCacheStatus, string exceptionMess)
        {
            productDBdatabase.SetupCommand(Constants.DMLStoredProcs.UpdateImageFilesCacheDeleteStatus);
            productDBdatabase.AddInParameter("ProdProgId", DbType.String, prodProgId);
            productDBdatabase.AddInParameter("Status", DbType.String, GetProdImageFilesCacheDeleteStatus(prodImgDelFilesCacheStatus));
            productDBdatabase.AddInParameter("ImageImportTimeStamp", DbType.String, timeStamp);
            productDBdatabase.AddInParameter("exceptionMess", DbType.String, exceptionMess);
            productDBdatabase.ExecuteNonQuery();
        }

        internal void UpdateProductImageBrokenCounter(string productImageId, int brokenCounter)
        {
            productDBdatabase.SetupCommand(Constants.DMLStoredProcs.UpdateProductImageBrokenCounter);
            productDBdatabase.AddInParameter("Id", DbType.String, productImageId);
            productDBdatabase.AddInParameter("BrokenCounter", DbType.Int32, brokenCounter);
            productDBdatabase.ExecuteNonQuery();
        }
    }
}

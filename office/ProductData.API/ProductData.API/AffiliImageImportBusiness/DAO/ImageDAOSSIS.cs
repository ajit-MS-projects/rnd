using System;
using System.Data;
using Affilinet.Business.ImageImport.Common;
using Constants = Affilinet.Business.ImageImport.Common.Constants;

namespace Affilinet.Business.ImageImport.DAO
{
    public partial class ImageDAO
    {
        /// <summary>
        /// Return the next prodProgId and the timestamp for update.
        /// </summary>
        /// <returns></returns>
        internal DataTable GetProdProgIdForSsisImport()
        {
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetProdProgIdForSsisImport);
            return productDBdatabase.ExecuteReaderProcessed();
        }

        internal void UpdateImageProgramAfterSsisComplete(string prodProgId, string status, string errMessage)
        {
            productDBdatabase.SetupCommand(Constants.DMLStoredProcs.UpdateImageProgramSsisCompleted);
            productDBdatabase.AddInParameter("Status", DbType.String, status);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("ErrorMessage", DbType.String, errMessage);
            productDBdatabase.ExecuteNonQuery();
        }

        /// <summary>
        /// Return the next prodProgId for image import review.
        /// </summary>
        /// <returns></returns>
        internal string GetProdProgIdForReviewSsisImport(bool isManualImport)
        {
            string retVal = string.Empty;

            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetProdProgIdForReviewImageSsisImport);
            productDBdatabase.AddInParameter("manualImport", DbType.Boolean, isManualImport);
            object objTmp = productDBdatabase.ExecuteScalar();

            if (objTmp != null && objTmp != DBNull.Value)
                    retVal = objTmp.ToString();

            return retVal;
        }

        /// <summary>
        /// Return Images from ProductImage table where BrokenCounter >= maxBrokenImageCounter for deletion
        /// </summary>
        /// <param name="maxBrokenImageCounter"></param>
        /// <returns></returns>
        internal DataTable GetBrokenImages(int maxBrokenImageCounter)
        {
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetBrokenImagesForDelete);
            productDBdatabase.AddInParameter("BrokenCounter", DbType.Int32, maxBrokenImageCounter);
            return productDBdatabase.ExecuteReaderProcessed();
        }

        /// <summary>
        /// Update the FileStatus in ProdImageFilesCache and set it from OldStatus to NewStatus
        /// </summary>
        /// <returns></returns>
        internal void ResetImageProgramFilesCacheStatus(ProdImageFilesCacheStatusEnum oldStatus, ProdImageFilesCacheStatusEnum newStatus)
        {
            productDBdatabase.SetupCommand(Constants.DMLStoredProcs.ResetImageFilesCacheStatus);
            productDBdatabase.AddInParameter("OldFileStatus", DbType.String, GetProdImageFilesCacheStatus(oldStatus));
            productDBdatabase.AddInParameter("NewFileStatus", DbType.String, GetProdImageFilesCacheStatus(newStatus));
            productDBdatabase.ExecuteNonQuery();
        }

        /// <summary>
        /// Update the Status in ImageProgram and set it from OldStatus to NewStatus
        /// </summary>
        /// <returns></returns>
        internal void ResetImageProgramStatus(ImageProgramStatusEnum oldStatus, ImageProgramStatusEnum newStatus, bool manualImport)
        {
            productDBdatabase.SetupCommand(Constants.DMLStoredProcs.ResetImageProgramStatus);
            productDBdatabase.AddInParameter("OldStatus", DbType.String, GetImageProgramStatus(oldStatus));
            productDBdatabase.AddInParameter("NewStatus", DbType.String, GetImageProgramStatus(newStatus));
            productDBdatabase.AddInParameter("ManualImport", DbType.Int32, manualImport?1:0);
            productDBdatabase.ExecuteNonQuery();
        }

        /// <summary>
        /// Helper function to convert ProdImageFilesCacheStatusEnum to string from constants
        /// </summary>
        /// <param name="statusEnum"></param>
        /// <returns></returns>
        internal string GetProdImageFilesCacheStatus(ProdImageFilesCacheStatusEnum statusEnum)
        {
            String status = String.Empty;
            switch (statusEnum)
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

            return status;
        }

        /// <summary>
        /// Helper function to convert ImageProgramStatusEnum to string from constants
        /// </summary>
        /// <param name="statusEnum"></param>
        /// <returns></returns>
        internal string GetImageProgramStatus(ImageProgramStatusEnum statusEnum)
        {
            String status = String.Empty;
            switch (statusEnum)
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

            return status;
        }


    }
}

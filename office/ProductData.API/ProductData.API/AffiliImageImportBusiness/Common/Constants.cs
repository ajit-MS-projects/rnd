using System;

namespace Affilinet.Business.ImageImport.Common
{
    public  class Constants
    {
        public  struct ReadOnlyStoredProcs
        {
            public const String ImageImportFileInfo = "spImageImportFileInfo";
            public const String GetProdProgIdForSsisImport = "sp_SelectProdProgForImageImport";
            public const String GetProdProgIdForReviewImageSsisImport = "sp_SelectProdProgForReviewImageImport";
            public const String GetBrokenImagesForDelete = "sp_SelectBrokenImagesForDelete";
            public const String GetImageReviewFilesToDelete = "spGetImageReviewFilesToDelete";
            public const String GetImageProgramStatus = "spGetImageProgramStatus";
            public const String GetDailyImageStatus = "spGetDailyImageStatus";
            public const String GetProdProgForImageDeleteFromFS = "spGetProdProgForImageDeleteFromFS";
            public const String GetProdProgForImageDeleteFromDB = "spGetProdProgForImageDeleteFromDb";
            public const String GetImageDeleteStatus = "spGetImageDeleteStatus";
        }
        public  struct DMLStoredProcs
        {
            public const String GetImagesForManualReview = "spGetImagesForManualReview";
            public const String GetImagesToReview = "spGetImagesToReview";
            public const String UpdateImageImportProgramFilesCache = "spUpdateImageImportProgramFilesCache";
            public const String UpdateProductImageCheckedStatus = "spUpdateProductImageCheckedStatus";
            public const String ImageImportDeleteFiles = "spImageImportDeleteFiles";
            public const String UpdateImageProgramStatus = "spUpdateImageProgramStatus";
            public const String UpdateSingleImage = "spUpdateSingleImage";
            public const String IncrSingleImageBrokenCounter = "spIncrSingleImageBrokenCounter";
            public const String UpdateImageProgramSsisCompleted = "sp_UpdateImageProgramSsisComplete";
            public const String ResetImageFilesCacheStatus = "spUpdateProdImageFilesCacheResetStatus";
            public const String ResetImageProgramStatus = "spUpdateImageProgramResetStatus";
            public const String ResetImageFilesCacheDeleteStatus = "spResetProdImageFilesCacheResetDeleteStatus";
            public const String UpdateImageFilesCacheDeleteStatus = "spUpdateProdImageFilesCacheDeleteStatus";
            public const String UpdateProductImageBrokenCounter = "spUpdateProductImageBrokenCounter";
            
        }
        public struct ImageStatus
        {
            public const String New = "new";
            public const String Processed = "processed";
            public const String Ready4delete = "ready4delete";
            public const String Error = "error";
        }

        public struct ProdImageFilesCache
        {
            public const String ImageImportTimeStamp = "ImageImportTimeStamp";
            public const String ProdProgId = "ProdProgId";
        }

        public struct AppSettings
        {
            public const String ImagesSaveLocation = "ImagesSaveLocation";
            public const String ImgCsvSaveLocation = "ImgCsvSaveLocation";
            public const String ImgBrokenCsvSaveLocation = "ImgBrokenCsvSaveLocation";
            public const String ImgCsvMaxLines = "ImgCsvMaxLines";
            public const String ImageImportDeleteImageSsis = "ImageImportDeleteImageSSIS";
            public const String ImageImportUpdateImageSsis = "ImageImportUpdateImageSSIS";
            public const String CheckForImageChangesOnImageServer = "CheckForImageChangesOnImageServer";
            public const String DailyImageImportStartTime = "DailyImageImportStartTime";
            public const String DailyImageImportEndTime = "DailyImageImportEndTime";
            public const String DailyImageImportTimeInervalInMilliSecs = "DailyImageImportTimeInervalInMilliSecs";
            public const String ImageImportSsisFolder = "ImageImportSsisFolder";
            public const String ServiceTimeInervalInMilliSecs = "ServiceTimeInervalInMilliSecs";
            public const string MaxBrokenCounter = "MaxBrokenCounter";
            public const string MaxThreads = "MaxThreads";
            public const string DownloadImagesInDebugMode = "DownloadImagesInDebugMode";
            public const string DownloadImagesDebugModeImageUrl = "DownloadImagesDebugModeImageUrl";
            public const string MultiThreadingEnabled = "MultiThreadingEnabled";
            public const string DailyImageServiceStartTime = "DailyImageServiceStartTime";
            public const string DailyImageServiceEndHours = "DailyImageServiceEndHours";
            public const string ManualImageServiceStartTime = "ManualImageServiceStartTime";
            public const string ManualImageServiceEndHours = "ManualImageServiceEndHours";
            public const string ImageReviewServiceStartTime = "ImageReviewServiceStartTime";
            public const string ImageReviewServiceEndHours = "ImageReviewServiceEndHours";
            public const string CheckAbortAfterImages = "CheckAbortAfterImages";
            public const string ImageDeleteFromFsStartTime = "ImageDeleteFromFsStartTime";
            public const string ImageDeleteFromFsEndTime = "ImageDeleteFromFsEndTime";
            public const string ImageDeleteFromDbEndTime = "ImageDeleteFromDbEndTime";
            public const string ImageDeleteFromDbStartTime = "ImageDeleteFromDbStartTime";
            public const String ImageDeleteFromFsTimeInervalInMilliSecs = "ImageDeleteFromFsTimeInervalInMilliSecs";
            public const String ImageDeleteFromDbTimeInervalInMilliSecs = "ImageDeleteFromDbTimeInervalInMilliSecs";
            public const String CheckAbortAfterImagesToDelete = "CheckAbortAfterImagesToDelete";
            public const String ImagesTempSaveLocation = "ImagesTempSaveLocation";
        }

        /// <summary>
        /// Parameters of various SSIS packages executed
        /// </summary>
        public  struct SSISParameters
        {
            public const string DataSource = "ConnectionString";
            public const string ProductDbCon = "ProductDbCon";
            public const string ImagesOnly = "ImagesOnly";
            public const string CsvFilePath = "CsvFilePath";
        }

        public struct ImageProgramStatus
        {
            public const string Reviewing = "reviewing";
            public const string ReviewComplete = "reviewcomplete";
            public const string SsisImportComplete = "ssisimportcomplete";
            public const string Null = "0";
            public const string Error = "error";
            public const string SsisError = "SsisError";
            public const string SsisImportProcessing = "ssisimportprocessing";
            public const string AbortRequestedByAdmin = "abortrequestedbyadmin";
        }
        public struct ProdImageFilesCacheStatus
        {
            public const string New = "new";
            public const string Processing = "processing";
            public const string Processed = "processed";
            public const string Error = "error";
            public const string SsisImportComplete = "ssisimportcomplete";
            public const string SsisError = "ssiserror";
            public const string Ready4Delete = "ready4delete";
            public const string SsisImportProcessing = "ssisimportprocessing";
            public const string AbortRequestedByAdmin = "abortrequestedbyadmin";
        }
        public struct ProdImageFilesCacheDeleteStatus
        {
            public const string new2delete = "new2delete";
            public const string processingfsdelete = "processingfsdelete";
            public const string ready4dbdelete = "ready4dbdelete";
            public const string processingssisdelete = "processingssisdelete";
            public const string ready4delete = "ready4delete";
            public const string ssiserror = "ssiserror";
            public const string error = "error";
            public const string abortRequestedByAdmin = "abortrequestedbyadmin";
        }
        /// <summary>
        /// Common constants
        /// </summary>
        public  struct Generic
        {
            public const string DestFieldSeperator = ";";
            public const string DestFieldQualifier = "~\"";//"~";
            public const string ProductHashActionField = "action";
            public const string InsUpdDel = "InsUpdDel";
            public const string DestFieldQualifierAlternate = "-";
            public const string DuplicateImageUrlError = "Duplicate images exists, processing of this program is slow";
            public const string Tab = "\t";
            public const string TabReplacementFieldSeperator1 = "~";
            public const string TabReplacementFieldSeperator2 = "^";
            public const string TabReplacementFieldSeperator3 = "`";
            public const int CategoryTextFldLength = 250;
            public const string DoubleQuoteReplaceChars = "´´";
            public const string SingleQuoteReplaceChars = "´";
            public const string DefaultCurrencySymbol = "EUR";
            public const string HtmlNewLine = "<BR>";
            public const string XmlCategorySeperator = ">>";
            public const string GetNextIdProduct = "Product";
            public const string GetNextIdImage = "Image";
            public const string DeleteAction = "D";
            public const string ImageReviewCsvPrefix = "ImageReview";
        }
        /// <summary>
        /// ProductImage table's columns
        /// </summary>
        public struct ProductImage
        {
            public const string DBID = "ID";
            public const string ImageID = "ImageID";
            public const string ProductProgramID = "ProductProgramID";
            public const string ImageURL = "ImageURL";
            public const string HashCode = "HashCode";
            public const string ImageNb = "ImageNb";
            public const string ImgWidth = "ImgWidth";
            public const string ImgHeight = "ImgHeight";
            public const string ProgramID = "ProgramID";
            //Start: Columns for export only
            public const string Img30Width = "Img30Width";
            public const string Img30Height = "Img30Height";
            public const string Img60Width = "Img60Width";
            public const string Img60Height = "Img60Height";
            public const string Img90Width = "Img90Width";
            public const string Img90Height = "Img90Height";
            public const string Img120Width = "Img120Width";
            public const string Img120Height = "Img120Height";
            public const string Img180Width = "Img180Width";
            public const string Img180Height = "Img180Height";
            public const string ErrorCode = "ErrorCode";
            public const string AdvUpdatedDate = "AdvUpdatedDate";
            public const string BrokenCounter = "BrokenCounter";
            //End: Columns for export only
            //Start: Colums for ProductImage delete
            public const string ErrorMessage = "ErrorMessage";
            public const string ImageFilePath = "ImageFilePath";
            //End: Columns for ProductImage delete
        }
        /// <summary>
        /// Processed CSV file extensions
        /// </summary>
        public  struct FileNaming
        {
            //public const string PropertyCsvExtention = ".property.csv";
            public const string ProductInsertCsvExtention = ".product.insert.csv";
            public const string ProductUpdateCsvExtention = ".product.update.csv";
            public const string ProductDeleteCsvExtention = ".product.delete.csv";
            public const string ProductNotchangedCsvExtention = ".product.notChanged.csv";
            public const string ImageDeleteCsvExtention = ".image.delete.csv";
            public const string ImageUpdateCsvExtention = ".image.update.csv";
            public const string ImageBrokenCsvExtention = ".image.broken.csv";
            public const string ImageNotChangedCsvExtention = ".image.notchanged.csv";
            public const string DtsxFileExtension = ".dtsx";
            public const string ImageDeleteWorkingCsvExtention = ".image.delete.successful.csv";
            public const string ImageDeleteErrorCsvExtention = ".image.delete.error.csv";
            // public const string ImageDeleteBrokenOkCsvExtention = ".image.broken.delete.csv";
        }

        /// <summary>
        /// Provides a list of system list ids for multilingual support.
        /// </summary>
        public struct SystemListIds
        {
            public const string ProductImportReportParams = "A91E266F-8A52-42AC-8EF0-BC4035658DB8";
            public const string ImageImportSsisReportParams = "1EC0234C-E0DA-468D-BACD-F8F598AC39F1";
        }
    }
}

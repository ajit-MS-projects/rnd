

namespace Affilinet.Business.ImageImport.Common
{

    public enum ImageStatusEnum
    {
        New,
        Processed,
        Ready4delete
    }
    public enum ImageImportTypesEnum
    {
        ProductImport,
        ImageReview,
        ManualImport,
    }
    public enum ImageProgramStatusEnum
    {
        Null,
        Reviewing,
        ReviewComplete,
        SsisImportComplete,
        Error,
        SsisError,
        SsisImportProcessing
    }
    public enum ProdImageFilesCacheStatusEnum
    {
        New,
        Processing,
        Processed,
        Error,
        Ready4delete,
        SsisImportComplete,
        SsisError,
        SsisImportProcessing
    }

    public enum ProdImageFilesCacheDeleteStatusEnum
    {
        new2delete,
        processingfsdelete,
        ready4dbdelete,
        processingssisdelete,
        ready4delete,
        ssiserror,
        error,
        abortrequestedbyadmin
    }

    /// <summary>
    /// Import process Report parameters to log
    /// </summary>
    public enum ImageImportReportParamsEnum
    {
        
    }

    /// <summary>
    /// Image Import Ssis process Report parameters to log, used from the SsisManger
    /// </summary>
    public enum ImageImportReportSsisParamsEnum
    {
        ImageDeleteError = 1,
        SsisImageImportStartTime = 2,
        SsisImageImportEndTime = 3,
        SsisImageImportStatus = 4,
        SsisImageImportError = 5
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Affilinet.Utility.Logging;
using Affilinet.Business.ProductImport.Common;
using Constants = Affilinet.Business.ProductImport.Common.Constants;

namespace Affilinet.Business.ProductImport.Entity
{
    /// <summary>
    /// Entity for maintaining report log parameters
    /// </summary>
    public class DownloadReportingInfo
    {
        public string DownloadError { get; set; }
        public string FileDownloadStatus { get; set; }
        public string DownloadStart { get; set; }
        public string DownloadEnd { get; set; }
        public string DownloadDuration { get; set; }

    
        /// <summary>
        /// Gets the report logs. Loops through properties using reflection.
        /// </summary>
        /// <param name="productProgramId">The product program id.</param>
        /// <returns></returns>
        public List<ReportLog> GetReportLogs(string productProgramId)
        {
            List<ReportLog> logs = new List<ReportLog>();
            PropertyInfo[] arrPi = this.GetType().GetProperties();
            foreach (PropertyInfo pi in arrPi)
            {
                ReportLog log = new ReportLog();
                log.Id = productProgramId;
                log.SystemListId = Constants.SystemListIds.ProductImportReportParams;
                log.Value = pi.GetValue(this, null) == null ? "" : pi.GetValue(this, null).ToString();
                log.ListEnum = GetListEnum(pi.Name);
                logs.Add(log);
            }
            return logs;
        }
        /// <summary>
        /// Gets the list enum.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public int GetListEnum(string propertyName)
        {
            int retVal = 0;
            switch (propertyName)
            {
                case "FileDownloadStatus":
                    retVal = (int)ProductImportReportParamsEnum.FileDownloadStatus;
                    break;
                case "DownloadStart":
                    retVal = (int)ProductImportReportParamsEnum.DownloadStart;
                    break;
                case "DownloadEnd":
                    retVal = (int)ProductImportReportParamsEnum.DownloadEnd;
                    break;
                case "DownloadError":
                    retVal = (int)ProductImportReportParamsEnum.DownloadError;
                    break;
                case "DownloadDuration":
                    retVal = (int)ProductImportReportParamsEnum.DownloadDuration;
                    break;
            }
            return retVal;
        }

    }
}

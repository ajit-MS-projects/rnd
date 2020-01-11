using System.Collections.Generic;
using System.Reflection;
using Affilinet.Business.ImageImport.Common;
using Affilinet.Utility.Logging;
using Const = Affilinet.Business.ImageImport.Common.Constants;

namespace Affilinet.Business.ImageImport.Entity
{
    /// <summary>
    /// Entity for maintaining report log parameters
    /// </summary>
    public class SsisImportReportingInfo
    {
        public string SsisImageImportError { get; set; }
        public string SsisImageImportStatus { get; set; }
        public string SsisImageImportStartTime { get; set; }
        public string SsisImageImportEndTime { get; set; }
        public string ImageDeleteError { get; set; }

        /// <summary>
        /// Gets the report logs. Loops through properties using reflection.
        /// </summary>
        /// <param name="productProgramId">The product program id.</param>
        /// <returns></returns>
        public List<ReportLog> GetReportLogs(string productProgramId)
        {
            List<ReportLog> logs = new List<ReportLog>();
            PropertyInfo[] arrPi = GetType().GetProperties();
            foreach (PropertyInfo pi in arrPi)
            {
                ReportLog log = new ReportLog();
                log.Id = productProgramId;
                log.SystemListId = Const.SystemListIds.ImageImportSsisReportParams;
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
                case "SsisImageImportStatus":
                    retVal = (int)ImageImportReportSsisParamsEnum.SsisImageImportStatus;
                    break;
                case "SsisImageImportError":
                    retVal = (int)ImageImportReportSsisParamsEnum.SsisImageImportError;
                    break;
                case "SsisImageImportStartTime":
                    retVal = (int)ImageImportReportSsisParamsEnum.SsisImageImportStartTime;
                    break;
                case "SsisImageImportEndTime":
                    retVal = (int)ImageImportReportSsisParamsEnum.SsisImageImportEndTime;
                    break;
                case "ImageDeleteError":
                    retVal = (int)ImageImportReportSsisParamsEnum.ImageDeleteError;
                    break;
                
            }
            return retVal;
        }

    }
}

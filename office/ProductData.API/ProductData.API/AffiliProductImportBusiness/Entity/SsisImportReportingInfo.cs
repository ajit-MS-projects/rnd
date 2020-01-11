using System.Collections.Generic;
using System.Reflection;
using Affilinet.Business.ProductImport.Common;
using Affilinet.Utility.Logging;
using Constants = Affilinet.Business.ProductImport.Common.Constants;

namespace Affilinet.Business.ProductImport.Entity
{
    /// <summary>
    /// Entity for maintaining report log parameters
    /// </summary>
    public class SsisImportReportingInfo
    {
        public string SsisError { get; set; }
        public string ImportStatus { get; set; }
        public string SsisImportStartTime { get; set; }
        public string SsisImportEndTime { get; set; }

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
                case "ImportStatus":
                    retVal = (int)ProductImportReportParamsEnum.ImportStatus;
                    break;
                case "SsisError":
                    retVal = (int)ProductImportReportParamsEnum.SsisError;
                    break;
                case "SsisImportStartTime":
                    retVal = (int)ProductImportReportParamsEnum.SsisImportStartTime;
                    break;
                case "SsisImportEndTime":
                    retVal = (int)ProductImportReportParamsEnum.SsisImportEndTime;
                    break;
            }
            return retVal;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using Affilinet.Business.ProductImport.Common;
using Affilinet.Exceptions;

namespace Affilinet.Business.ProductImport
{
    public class ThreadManager
    {
        #region Hourly import
          protected delegate void BeginHourlyProductImport(bool isHourly);
          protected Int32 HourlyThreadCounter { get; set; }
          protected object HourlyLockObject = new object();
          private int MaxThreads;
          private const int MaxThreadsDefault = 1;
          public void StartHourlyProductImportThreads()
          {
              if (IsServiceScheduled())
              {
                  MaxThreads = int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.HourlyProductImportMaxThreads), out MaxThreads) ? MaxThreads : MaxThreadsDefault;
                  while (HourlyThreadCounter < MaxThreads)
                  {
                      BeginHourlyProductImport objBeginHourlyProductImport = new ProductImportManager().AutoImport;
                      HourlyThreadCounter++;
                      objBeginHourlyProductImport.BeginInvoke(true, HourlyImageImportCompleteResults, new object());
                      Thread.Sleep(800);
                  }
              }
          }
          protected void HourlyImageImportCompleteResults(IAsyncResult ar)
          {
              lock (HourlyLockObject)
              {
                  HourlyThreadCounter--;
              }
          }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool IsServiceScheduled()
        {
              Utilities.WriteHealthCheckTimeStamp(WinServiceHealthCheckFileTypesEnum.HourlyProductImport);

              Utilities.ReloadConfigSection();
              DateTime impStartTime =
                  Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " +
                                     Utilities.GetAppSettingValue(Constants.AppSettings.ImportStartTime));
              DateTime impEndTime =
                  Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " +
                                     Utilities.GetAppSettingValue(Constants.AppSettings.ImportEndTime));
              DataTable dtScheduledPrograms = null;
            return (!(DateTime.Now.CompareTo(impStartTime) >= 0 && DateTime.Now.CompareTo(impEndTime) <= 0));
        }
        #endregion
    }
}

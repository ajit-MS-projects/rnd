using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Affili.ProductData.Common;
using Affilinet.Business.ImageImport.Common;
using Affilinet.Exceptions;
using Utilities=Affili.ProductData.Common.Utilities;

namespace Affilinet.Business.ImageImport
{
    public class ThreadManager
    {
        #region Daily import
        protected delegate void BeginDailyImageImport();
        protected Int32 DailyThreadCounter{get; set;}
        protected object DailyLockObject = new object();
        private int MaxThreads;
        private const int MaxThreadsDefault = 10;
        public void StartDailyThreads()
        {
            Utilities.WriteHealthCheckTimeStamp(WinServiceHealthCheckFileTypesEnum.DailyImageService);
            if (!Utilities.IsServiceScheduled(Constants.AppSettings.DailyImageServiceStartTime, Constants.AppSettings.DailyImageServiceEndHours)) return;
            MaxThreads = int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.MaxThreads), out MaxThreads) ? MaxThreads : MaxThreadsDefault;
            while (DailyThreadCounter < MaxThreads)
            {
                BeginDailyImageImport objBeginDailyImageImport = new ImageImportManager().StartDailyImageProcessing;
                DailyThreadCounter++;
                objBeginDailyImageImport.BeginInvoke(DailyImageImportCompleteResults, new object());
                Thread.Sleep(800);
            }
        }
        protected void DailyImageImportCompleteResults(IAsyncResult ar)
        {
            lock (DailyLockObject)
            {
                DailyThreadCounter--;
            }
        }
        #endregion
        #region Image Review 
        protected delegate void BeginImageReview();
        protected Int32 ImageReviewThreadCounter { get; set; }
        protected object ImageReviewLockObject = new object();
        public void StartImageReviewThreads()
        {

            Utilities.WriteHealthCheckTimeStamp(WinServiceHealthCheckFileTypesEnum.ImageReviewService);
            if (!Utilities.IsServiceScheduled(Constants.AppSettings.ImageReviewServiceStartTime, Constants.AppSettings.ImageReviewServiceEndHours)) return;
            MaxThreads = int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.MaxThreads), out MaxThreads) ? MaxThreads : MaxThreadsDefault;
            while (ImageReviewThreadCounter < MaxThreads)
            {
                BeginImageReview objBeginImageReview = new ImageImportManager().StartImageReviewProcessing;
                ImageReviewThreadCounter++;
                objBeginImageReview.BeginInvoke(ImageReviewImportCompleteResults, new object());
                Thread.Sleep(800);
            }
        }
        protected void ImageReviewImportCompleteResults(IAsyncResult ar)
        {
            lock (ImageReviewLockObject)
            {
                ImageReviewThreadCounter--;
            }
        }
        #endregion
        #region Manual Image Review
        protected delegate void BeginManualImageReview(bool isManual);
        protected Int32 ManualImageReviewThreadCounter { get; set; }
        protected object ManualImageReviewLockObject = new object();
        public void StartManualImageReviewThreads()
        {
            try
            {
                Utilities.CreateInfoLog("StartManualImageReviewThreads()", ApplicationEventsEnum.ServiceStart);
                Utilities.WriteHealthCheckTimeStamp(WinServiceHealthCheckFileTypesEnum.ManualImageService);
                Utilities.CreateInfoLog("StartManualImageReviewThreads(): finished writing healthcheck", ApplicationEventsEnum.ServiceStart);
                if (
                    !Utilities.IsServiceScheduled(Constants.AppSettings.ManualImageServiceStartTime,
                                                  Constants.AppSettings.ManualImageServiceEndHours)) return;
                Utilities.CreateInfoLog("StartManualImageReviewThreads(): Service is scheduled", ApplicationEventsEnum.ServiceStart);
                MaxThreads = int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.MaxThreads), out MaxThreads)
                                 ? MaxThreads
                                 : MaxThreadsDefault;
                Utilities.CreateInfoLog("StartManualImageReviewThreads(): maxthreads=" + MaxThreads, ApplicationEventsEnum.ServiceStart);
                while (ManualImageReviewThreadCounter < MaxThreads)
                {
                    Utilities.CreateInfoLog("StartManualImageReviewThreads(): thread loop initializing start imagereview ", ApplicationEventsEnum.ServiceStart);
                    BeginManualImageReview objBeginManualImageReview =
                        new ImageImportManager().StartImageReviewProcessing;
                    ManualImageReviewThreadCounter++;
                    Utilities.CreateInfoLog("StartManualImageReviewThreads(): thread loop calling start imagereview thread no." + ManualImageReviewThreadCounter, ApplicationEventsEnum.ServiceStart);
                    objBeginManualImageReview.BeginInvoke(true, ManualImageReviewImportCompleteResults, new object());
                    Thread.Sleep(800);
                }
            }catch(Exception ex)
            {
                new AffiliGenericException("Error in StartManualImageReviewThreads()", ex).CreateLog();
            }
        }
        protected void ManualImageReviewImportCompleteResults(IAsyncResult ar)
        {
            lock (ManualImageReviewLockObject)
            {
                ManualImageReviewThreadCounter--;
            }
        }
        #endregion
    }
}

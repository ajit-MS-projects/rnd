using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Affilinet.Business.ImageImport;
using Affilinet.Business.ImageImport.Common;
using CommonUtilities = Affili.ProductData.Common.Utilities;

namespace AffiliManualImageImportService
{
    public partial class AffiliManualImageImportService : ServiceBase
    {
        private ImageImportManager objImageImportManager = null;
        private ThreadManager objThreadManager = null;
        public AffiliManualImageImportService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            CommonUtilities.CreateInfoLog("AffiliManualImageImportService: Start", Affili.ProductData.Common.ApplicationEventsEnum.ServiceStart);
            objImageImportManager = new ImageImportManager();
            objImageImportManager.ResetImageProgramStatus(ImageProgramStatusEnum.Reviewing, ImageProgramStatusEnum.Null, true);
            objThreadManager = new ThreadManager();
            timManualImageImport.Enabled = true;
            CommonUtilities.CreateInfoLog("AffiliManualImageImportService: Start() method-Exit", Affili.ProductData.Common.ApplicationEventsEnum.ServiceStart);
        }

        protected override void OnStop()
        {
            timManualImageImport.Enabled = false;
            objImageImportManager = null;
        }

        private void timManualImageImport_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CommonUtilities.CreateInfoLog("AffiliManualImageImportService: timManualImageImport_Elapsed() method-Start", Affili.ProductData.Common.ApplicationEventsEnum.ServiceStart);
            int intVal = 60000;
            int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ServiceTimeInervalInMilliSecs), out intVal);
            CommonUtilities.CreateInfoLog("Elapsed(): ServiceTimeInervalInMilliSecs:" + intVal, Affili.ProductData.Common.ApplicationEventsEnum.ServiceStart);
            timManualImageImport.Interval = intVal;
            timManualImageImport.Enabled = false;
            if (Utilities.GetAppSettingValue(Constants.AppSettings.MultiThreadingEnabled) == "1")
                objThreadManager.StartManualImageReviewThreads();
            else
                objImageImportManager.StartImageReviewProcessing(true);
            timManualImageImport.Enabled = true;
            CommonUtilities.CreateInfoLog("AffiliManualImageImportService: timManualImageImport_Elapsed() method-Exit", Affili.ProductData.Common.ApplicationEventsEnum.ServiceStart);
        }
    }
}

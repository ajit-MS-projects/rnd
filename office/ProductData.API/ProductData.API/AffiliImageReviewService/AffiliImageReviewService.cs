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

namespace AffiliImageReviewService
{
    public partial class AffiliImageReviewService : ServiceBase
    {
        private ImageImportManager objImageImportManager = null;
        private ThreadManager objThreadManager = null;
        public AffiliImageReviewService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            objImageImportManager = new ImageImportManager();
            objImageImportManager.ResetImageProgramStatus(ImageProgramStatusEnum.Reviewing, ImageProgramStatusEnum.Null, false);
            objThreadManager = new ThreadManager();
            timImageReviewImport.Enabled = true;
        }

        protected override void OnStop()
        {
            timImageReviewImport.Enabled = false;
            objImageImportManager = null;
        }

        private void timImageReviewImport_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int intVal = 60000;
            int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ServiceTimeInervalInMilliSecs), out intVal);
            timImageReviewImport.Interval = intVal;
            timImageReviewImport.Enabled = false;
            if (Utilities.GetAppSettingValue(Constants.AppSettings.MultiThreadingEnabled) == "1")
                objThreadManager.StartImageReviewThreads();
            else
                objImageImportManager.StartImageReviewProcessing();
            timImageReviewImport.Enabled = true;
        }
    }
}

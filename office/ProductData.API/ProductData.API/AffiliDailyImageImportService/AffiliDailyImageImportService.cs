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

namespace AffiliDailyImageImportService
{
    public partial class AffiliDailyImageImportService : ServiceBase
    {
        private ImageImportManager objImageImportManager = null;
        private ThreadManager objThreadManager = null;
        public AffiliDailyImageImportService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            objImageImportManager = new ImageImportManager();
            objImageImportManager.ResetImageFilesCacheStatus(ProdImageFilesCacheStatusEnum.Processing, ProdImageFilesCacheStatusEnum.New);
            objThreadManager = new ThreadManager();
            timDailyImageImport.Enabled = true;
        }

        protected override void OnStop()
        {
            timDailyImageImport.Enabled = false;
            objImageImportManager = null;
        }

        private void timDailyImageImport_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int intVal = 60000;
            int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ServiceTimeInervalInMilliSecs), out intVal);
            timDailyImageImport.Interval = intVal;
            timDailyImageImport.Enabled = false;
            if (Utilities.GetAppSettingValue(Constants.AppSettings.MultiThreadingEnabled) == "1")
                objThreadManager.StartDailyThreads();
            else
                objImageImportManager.StartDailyImageProcessing();
            timDailyImageImport.Enabled = true;
        }
    }
}

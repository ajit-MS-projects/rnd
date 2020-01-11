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

namespace AffiliImageCsvFileCleanUpService
{
    public partial class AffiliImageCsvFileCleanUpService : ServiceBase
    {
         private ImageFilesManager objImageFilesManager = null;
         public AffiliImageCsvFileCleanUpService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            objImageFilesManager = new ImageFilesManager();
        }

        protected override void OnStop()
        {
            objImageFilesManager = null;
        }

        private void timImageImportCleanUp_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int intVal = 60000;
            int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ServiceTimeInervalInMilliSecs), out intVal);
            timImageImportCleanUp.Interval = intVal;
            timImageImportCleanUp.Enabled = false;
            objImageFilesManager.StartCleanUp();
            timImageImportCleanUp.Enabled = true;
        }
    }
}

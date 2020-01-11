using System.ServiceProcess;
using Affilinet.Business.ImageImport;
using Affilinet.Business.ImageImport.Common;

namespace ImageImportSsisMgmtService
{
    partial class AffiliImageImportSsisMgmtService : ServiceBase
    {
        private SsisManager SsisMgmt;
        public AffiliImageImportSsisMgmtService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            SsisMgmt  = new SsisManager();
            SsisMgmt.ResetImageFilesCacheStatus(ProdImageFilesCacheStatusEnum.Processed,ProdImageFilesCacheStatusEnum.SsisImportProcessing);
            SsisMgmt.ResetImageProgramStatus(ImageProgramStatusEnum.ReviewComplete, ImageProgramStatusEnum.SsisImportProcessing, true);//resets manual program
            SsisMgmt.ResetImageProgramStatus(ImageProgramStatusEnum.ReviewComplete, ImageProgramStatusEnum.SsisImportProcessing, false);//resets automatic reviewed programs
            timDailyImageImport.Enabled = true;
        }

        protected override void OnStop()
        {
            timDailyImageImport.Enabled = false;
            SsisMgmt = null;
        }

        private void timDailyImageImport_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int intVal;
            if(!int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.DailyImageImportTimeInervalInMilliSecs), out intVal))
                intVal = 300000; // 5min

            timDailyImageImport.Interval = intVal;
            timDailyImageImport.Enabled = false;
            SsisMgmt.StartSsisImageImport();
            timDailyImageImport.Enabled = true;
        }
    }
}

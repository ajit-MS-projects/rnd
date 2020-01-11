using System.ServiceProcess;
using Affilinet.Business.ImageImport;
using Affilinet.Business.ImageImport.Common;

namespace AffiliImageDeleteService
{
    public partial class AffiliImageDeleteService : ServiceBase
    {
        private ImageDeleteManager imgDelManager; 
        public AffiliImageDeleteService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            imgDelManager = new ImageDeleteManager();
            imgDelManager.ResetImageFilesCacheImageDeleteStatus(ProdImageFilesCacheDeleteStatusEnum.new2delete, ProdImageFilesCacheDeleteStatusEnum.processingfsdelete);
            imgDelManager.ResetImageFilesCacheImageDeleteStatus(ProdImageFilesCacheDeleteStatusEnum.ready4dbdelete, ProdImageFilesCacheDeleteStatusEnum.processingssisdelete);
            timImageDeleteFromFS.Enabled = true;
            timImageDeleteFromDB.Enabled = true;
        }

        protected override void OnStop()
        {
            timImageDeleteFromFS.Enabled = false;
            timImageDeleteFromDB.Enabled = false;
            imgDelManager = null;
        }

        private void timImageDeleteFromFS_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int intVal;
            if (!int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ImageDeleteFromFsTimeInervalInMilliSecs), out intVal))
                intVal = 10000; // 10sec

            timImageDeleteFromFS.Interval = intVal;
            timImageDeleteFromFS.Enabled = false;
            imgDelManager.DeleteFromFs();
            timImageDeleteFromFS.Enabled = true;
        }

        private void timImageDeleteFromDB_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int intVal;
            if (!int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ImageDeleteFromDbTimeInervalInMilliSecs), out intVal))
                intVal = 10000; // 10sec

            timImageDeleteFromDB.Interval = intVal;
            timImageDeleteFromDB.Enabled = false;
            imgDelManager.DeleteFromDb();
            timImageDeleteFromDB.Enabled = true;
        }
    }
}

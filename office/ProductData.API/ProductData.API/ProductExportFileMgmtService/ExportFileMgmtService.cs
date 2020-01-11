using System.ServiceProcess;
using Affilinet.Business.ProductExport.Common;
using Affilinet.Business.ProductExport;

namespace ProductExportFileMgmtService
{
    public partial class AffilinetExportFileMgmtService : ServiceBase
    {
        public AffilinetExportFileMgmtService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            
        }

        protected override void OnStop()
        {      
        }


        private void timListCleanUp_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ExportFileManager objExportFileManager = new ExportFileManager();
            int intVal = 300000; // 5min
            int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ExportCleanUpTimeInervalInMilliSecs), out intVal);
            timListCleanUp.Interval = intVal;
            timListCleanUp.Enabled = false;            
            objExportFileManager.CleanUpProdExportProgramFiles();                        
            timListCleanUp.Enabled = true;
        }

        private void timCopyExportFiles_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ExportFileManager objExportFileManager = new ExportFileManager();
            int intVal = 30000; //30sec
            int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ExportCopyTimeInervalInMilliSecs), out intVal);
            timCopyExportFiles.Interval = intVal;
            timCopyExportFiles.Enabled = false;            
            objExportFileManager.CopyProdExportProgramFiles();            
            timCopyExportFiles.Enabled = true;

        }
    }
}

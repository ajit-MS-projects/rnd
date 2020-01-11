using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Affilinet.Business.ProductImport;
using Affilinet.Business.ProductImport.Common;

namespace ProductFileDownloadService
{
    public partial class AffilinetProductFileDownloadService : ServiceBase
    {
        public AffilinetProductFileDownloadService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ProductImportManager objProdImpMan = new ProductImportManager();
            objProdImpMan.ResetProductProgram(ProgramImportStatusEnum.DOWLOADING);
            objProdImpMan.Dispose();
            objProdImpMan = null;
            timFileDownload.Enabled = true;
        }

        protected override void OnStop()
        {
            timFileDownload.Enabled = false;
            ProductImportManager objProdImpMan = new ProductImportManager();
            objProdImpMan.ResetProductProgram(ProgramImportStatusEnum.DOWLOADING);
            objProdImpMan.Dispose();
            objProdImpMan = null;
        }

        private void timFileDownload_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int intVal = 60000;
            int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ServiceTimeInervalInMilliSecs), out intVal);
            timFileDownload.Interval = intVal;
            timFileDownload.Enabled = false;
            ProductImportManager objProdImpMan = new ProductImportManager();
            objProdImpMan.StartCsvDownloading();
            objProdImpMan.Dispose();
            objProdImpMan = null;
            timFileDownload.Enabled = true;
        }
    }
}

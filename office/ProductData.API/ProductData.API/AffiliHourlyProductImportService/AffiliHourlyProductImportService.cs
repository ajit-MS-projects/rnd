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

namespace AffiliHourlyProductImportService
{
    public partial class AffiliHourlyProductImportService : ServiceBase
    {
        private ProductImportManager objProdImpMan = null;
        private ThreadManager objThreadManager = null;
        public AffiliHourlyProductImportService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            objProdImpMan= new ProductImportManager();
            objProdImpMan.ResetProductProgram(ProgramImportStatusEnum.PROCESSING_CSV, true);
            objThreadManager = new ThreadManager();
            timHourlyProductImport.Enabled = true;
        }

        protected override void OnStop()
        {
            timHourlyProductImport.Enabled = false;
            if (objProdImpMan != null)
            {
                objProdImpMan.ResetProductProgram(ProgramImportStatusEnum.PROCESSING_CSV, true);
            }
            objProdImpMan = null;
        }

        private void timHourlyProductImport_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int intVal = 0;
            if (!int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ServiceTimeInervalInMilliSecs), out intVal))
                intVal = 1000;
            timHourlyProductImport.Interval = intVal;
            timHourlyProductImport.Enabled = false;
            if (Utilities.GetAppSettingValue(Constants.AppSettings.MultiThreadingEnabled) == "1")
                objThreadManager.StartHourlyProductImportThreads();
            else
                objProdImpMan.AutoImport(true);
            timHourlyProductImport.Enabled = true;
        }
    }
}

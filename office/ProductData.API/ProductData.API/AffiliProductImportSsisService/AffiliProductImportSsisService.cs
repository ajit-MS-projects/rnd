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

namespace AffiliProductImportSsisService
{
    public partial class AffiliProductImportSsisService : ServiceBase
    {
        private ProductImportManager objProdImpMan = null;

        public AffiliProductImportSsisService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            objProdImpMan = new ProductImportManager();
            objProdImpMan.ResetProductProgram(ProgramImportStatusEnum.IMPORTING_CSV);
            timInitImport.Enabled = true;
        }

        protected override void OnStop()
        {
            timInitImport.Enabled = false;
            if (objProdImpMan != null)
            {
                objProdImpMan.ResetProductProgram(ProgramImportStatusEnum.IMPORTING_CSV);
            }
            objProdImpMan = null;
        }

        private void timInitImport_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int intVal = 60000;
            int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ServiceTimeInervalInMilliSecs), out intVal);
            timInitImport.Interval = intVal;
            timInitImport.Enabled = false;
            objProdImpMan.AutoImportSsis();
            timInitImport.Enabled = true;
        }
    }
}

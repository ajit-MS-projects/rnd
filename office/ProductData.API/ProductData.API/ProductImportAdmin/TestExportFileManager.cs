using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Affilinet.Business.ProductExport;

namespace ProductImportAdmin
{
    public partial class TestExportFileManager : Form
    {
        private ExportFileManager objExpManager = null;

        public TestExportFileManager()
        {
            InitializeComponent();
            objExpManager = new ExportFileManager();

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            objExpManager.CleanUpProdExportProgramFiles();
        }

        /// <summary>
        /// test the copy process to one ore more servers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartCopy_Click(object sender, EventArgs e)
        {
            objExpManager.CopyProdExportProgramFiles();
        }


    }
}

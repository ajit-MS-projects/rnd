using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PIMaintenanceTasks;

namespace HashGenUtility
{
    public partial class frmHashGenerator : Form
    {
        public frmHashGenerator()
        {
            InitializeComponent();
            cmbProgramId.SelectedIndex = 0;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            MaintenanceManager mainMgr = new MaintenanceManager();
            int progId = 0;
            if (cmbProgramId.Text.ToUpper() != "ALL")
                progId = int.Parse(cmbProgramId.Text);
            mainMgr.StartMaintenanceTasks(progId);
            lblTotalImages.Text = "Total Images processed = " + ImageMaintenance.RowCount.ToString();
        }

        private void frmHashGenerator_Load(object sender, EventArgs e)
        {
            lblConn.Text = MUtilities.GetDataSource(Affilinet.Data.Access.Constants.DBConnections.ProductData);
        }
    }
}

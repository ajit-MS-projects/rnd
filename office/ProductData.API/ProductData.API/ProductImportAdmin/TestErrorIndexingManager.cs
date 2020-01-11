using System;
using System.Windows.Forms;
using AffiliErrorIndexing;

namespace ProductImportAdmin
{
    public partial class TestErrorIndexingManager : Form
    {
        private ErrorIndexingManager _errorIndexingManager;
        public TestErrorIndexingManager()
        {
            InitializeComponent();
            _errorIndexingManager = new ErrorIndexingManager();
        }

        private void btnStartErrorIndexCalculation_Click(object sender, EventArgs e)
        {
            _errorIndexingManager.StartErrorIndexCalculation(txtProdProgId.Text.Trim());
        }


    }
}

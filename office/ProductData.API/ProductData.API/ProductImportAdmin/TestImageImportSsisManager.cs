using System;
using System.Windows.Forms;
using Affilinet.Business.ImageImport;
using Affilinet.Business.ImageImport.Common;

namespace ProductImportAdmin
{
    public partial class TestImageImportSsisManager : Form
    {
        private readonly SsisManager Manager; 
        public TestImageImportSsisManager()
        {
            InitializeComponent();
            Manager = new SsisManager();
            Manager.ResetImageFilesCacheStatus(ProdImageFilesCacheStatusEnum.Processed, ProdImageFilesCacheStatusEnum.SsisImportProcessing);
            Manager.ResetImageProgramStatus(ImageProgramStatusEnum.ReviewComplete, ImageProgramStatusEnum.SsisImportProcessing, true);
            Manager.ResetImageProgramStatus(ImageProgramStatusEnum.ReviewComplete, ImageProgramStatusEnum.SsisImportProcessing, false);
        }

        private void btnStartSsisImageImport_Click(object sender, EventArgs e)
        {
            Manager.StartSsisImageImport();
        }


    }
}

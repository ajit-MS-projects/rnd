using System;
using System.Windows.Forms;
using Affilinet.Business.ImageImport;
using Affilinet.Business.ImageImport.Common;

namespace ProductImportAdmin
{
    public partial class TestImageDeleteManager : Form
    {
        private readonly ImageDeleteManager imgDelManager;   
        public TestImageDeleteManager()
        {
            InitializeComponent();
            imgDelManager = new ImageDeleteManager();
            imgDelManager.ResetImageFilesCacheImageDeleteStatus(ProdImageFilesCacheDeleteStatusEnum.new2delete, ProdImageFilesCacheDeleteStatusEnum.processingfsdelete);
            imgDelManager.ResetImageFilesCacheImageDeleteStatus(ProdImageFilesCacheDeleteStatusEnum.ready4dbdelete, ProdImageFilesCacheDeleteStatusEnum.processingssisdelete);
        }

        private void btnImageDeleteFromFS_Click(object sender, EventArgs e)
        {
            imgDelManager.DeleteFromFs();
        }

        private void btnImageDeleteFromDB_Click(object sender, EventArgs e)
        {
            imgDelManager.DeleteFromDb();
        }

    }
}

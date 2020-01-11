using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Affilinet.Business.ImageImport;
using Affilinet.Business.ImageImport.Common;
using Affilinet.Business.ProductImport;
using Constants=Affilinet.Business.ProductImport.Common.Constants;
using Utilities=Affilinet.Business.ProductImport.Common.Utilities;
using CommonUtilities = Affili.ProductData.Common.Utilities;
using ImageConstants = Affilinet.Business.ImageImport.Common.Constants;
using ThreadManager = Affilinet.Business.ImageImport.ThreadManager;

namespace ProductImportAdmin
{
    public partial class Service : Form
    {
        private ProductImportManager objProdImpMan = null;
        public Service()
        {
            InitializeComponent();
            objProdImpMan = new ProductImportManager();
            //objProdImpMan.ResetProductProgram();
        }

        private ProductImportManager pim;
        private void Service_Load(object sender, EventArgs e)
        {
            pim = new ProductImportManager();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            pim.AutoImport();
        }

        private void btnStartImport_Click(object sender, EventArgs e)
        {
            pim.StartCsvDownloading();
        }

        private void timInitImport_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int intVal = 60000;
            int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ServiceTimeInervalInMilliSecs), out intVal);
            timInitImport.Interval = intVal;
            objProdImpMan.AutoImport();
        }

        private void timFileDownload_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int intVal = 60000;
            int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ServiceTimeInervalInMilliSecs), out intVal);
            timFileDownload.Interval = intVal;
            timFileDownload.Enabled = false;
            objProdImpMan.StartCsvDownloading();
           // timFileDownload.Enabled = true;
        }

        private void btnImageImport_Click(object sender, EventArgs e)
        {
            if (chkMultiThreaded.Checked)
            {
                ThreadManager objThreadManager = new ThreadManager();
                objThreadManager.StartDailyThreads();
            }
            else
            {
                ImageImportManager objImg = new ImageImportManager();
                objImg.StartDailyImageProcessing();
            }
        }

        private void btnDelImgs_Click(object sender, EventArgs e)
        {
            ImageFilesManager obj = new  ImageFilesManager();
            obj.StartCleanUp();
        }

        private void btnImageReview_Click(object sender, EventArgs e)
        {
            if (chkMultiThreaded.Checked)
            {
                ThreadManager objThreadManager = new ThreadManager();
                objThreadManager.StartImageReviewThreads();
            }
            else
            {
                ImageImportManager objImg = new ImageImportManager();
                objImg.StartImageReviewProcessing();
            }
        }

        private void btnManImgImport_Click(object sender, EventArgs e)
        {
            if (chkMultiThreaded.Checked)
            {
                ThreadManager objThreadManager = new ThreadManager();
                objThreadManager.StartManualImageReviewThreads();
            }
            else
            {
                ImageImportManager objImg = new ImageImportManager();
                objImg.StartImageReviewProcessing(true);
            }
        }

        private void btndwnSingImg_Click(object sender, EventArgs e)
        {
            ImageImportManager objImg = new ImageImportManager();
            String mess = "";
            objImg.DownloadSingleImage(txtLidtId.Text, txtImgUrl.Text, out mess);
            MessageBox.Show(mess);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            pim.AutoImportSsis();
        }
        private ImageImportManager objImageImportManager = new ImageImportManager();
        private ThreadManager objThreadManager = new ThreadManager();

        private void timManualImageImport_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CommonUtilities.CreateInfoLog("AffiliManualImageImportService: timManualImageImport_Elapsed() method-Start", Affili.ProductData.Common.ApplicationEventsEnum.ServiceStart);
            int intVal = 60000;
            int.TryParse(Utilities.GetAppSettingValue(Constants.AppSettings.ServiceTimeInervalInMilliSecs), out intVal);
            timManualImageImport.Interval = intVal;
            timManualImageImport.Enabled = false;
            if (Utilities.GetAppSettingValue(ImageConstants.AppSettings.MultiThreadingEnabled) == "1")
                objThreadManager.StartManualImageReviewThreads();
            else
                objImageImportManager.StartImageReviewProcessing(true);
            timManualImageImport.Enabled = true;
            CommonUtilities.CreateInfoLog("AffiliManualImageImportService: timManualImageImport_Elapsed() method-Exit", Affili.ProductData.Common.ApplicationEventsEnum.ServiceStart);
        }
    }
}

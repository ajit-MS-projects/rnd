namespace AffiliImageReviewService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AffiliImageReviewServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.AffiliImageReviewService = new System.ServiceProcess.ServiceInstaller();
            // 
            // AffiliImageReviewServiceProcessInstaller
            // 
            this.AffiliImageReviewServiceProcessInstaller.Password = null;
            this.AffiliImageReviewServiceProcessInstaller.Username = null;
            // 
            // AffiliImageReviewService
            // 
            this.AffiliImageReviewService.Description = "Compares images with Advertiser server and Downloads them periodically.";
            this.AffiliImageReviewService.DisplayName = "AffiliImageReviewService";
            this.AffiliImageReviewService.ServiceName = "AffiliImageReviewService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.AffiliImageReviewServiceProcessInstaller,
            this.AffiliImageReviewService});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller AffiliImageReviewServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller AffiliImageReviewService;
    }
}
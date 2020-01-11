namespace ImageCsvFileCleanUpService
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
            this.ImageCsvFileCleanUpServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.AffiliImageCsvFileCleanUpService = new System.ServiceProcess.ServiceInstaller();
            // 
            // ImageCsvFileCleanUpServiceProcessInstaller
            // 
            this.ImageCsvFileCleanUpServiceProcessInstaller.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.AffiliImageCsvFileCleanUpService});
            this.ImageCsvFileCleanUpServiceProcessInstaller.Password = null;
            this.ImageCsvFileCleanUpServiceProcessInstaller.Username = null;
            // 
            // AffiliImageCsvFileCleanUpService
            // 
            this.AffiliImageCsvFileCleanUpService.Description = "Deleted obsolete files of daily image import process.";
            this.AffiliImageCsvFileCleanUpService.DisplayName = "AffiliImageCsvFileCleanUpService";
            this.AffiliImageCsvFileCleanUpService.ServiceName = "AffiliImageCsvFileCleanUpService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ImageCsvFileCleanUpServiceProcessInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ImageCsvFileCleanUpServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller AffiliImageCsvFileCleanUpService;
    }
}
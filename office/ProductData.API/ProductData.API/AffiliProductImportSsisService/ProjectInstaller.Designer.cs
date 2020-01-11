namespace AffiliProductImportSsisService
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
            this.AffiliProductImportSsisServiceInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.AffiliProductImportSsisService = new System.ServiceProcess.ServiceInstaller();
            // 
            // AffiliProductImportSsisServiceInstaller
            // 
            this.AffiliProductImportSsisServiceInstaller.Password = null;
            this.AffiliProductImportSsisServiceInstaller.Username = null;
            // 
            // AffiliProductImportSsisService
            // 
            this.AffiliProductImportSsisService.Description = "Affilinet Product Import Ssis Service";
            this.AffiliProductImportSsisService.DisplayName = "AffiliProductImportSsisService";
            this.AffiliProductImportSsisService.ServiceName = "AffiliProductImportSsisService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.AffiliProductImportSsisServiceInstaller,
            this.AffiliProductImportSsisService});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller AffiliProductImportSsisServiceInstaller;
        private System.ServiceProcess.ServiceInstaller AffiliProductImportSsisService;
    }
}
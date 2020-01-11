namespace ImageImportSsisMgmtService
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
            this.AffiliImageImportMgmtServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.AffiliImageImportSsisMgmtService = new System.ServiceProcess.ServiceInstaller();
            // 
            // AffiliImageImportMgmtServiceProcessInstaller
            // 
            this.AffiliImageImportMgmtServiceProcessInstaller.Password = null;
            this.AffiliImageImportMgmtServiceProcessInstaller.Username = null;
            // 
            // AffiliImageImportSsisMgmtService
            // 
            this.AffiliImageImportSsisMgmtService.Description = "Runs SSIS for image import (Update, delete from db and delete broken images from " +
                "filesystem).";
            this.AffiliImageImportSsisMgmtService.DisplayName = "AffiliImageImportSsisMgmtService";
            this.AffiliImageImportSsisMgmtService.ServiceName = "AffiliImageImportSsisMgmtService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.AffiliImageImportMgmtServiceProcessInstaller,
            this.AffiliImageImportSsisMgmtService});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller AffiliImageImportMgmtServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller AffiliImageImportSsisMgmtService;
    }
}
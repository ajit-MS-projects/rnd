namespace AffiliManualImageImportService
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
            this.AffiliManualImageImportServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.AffiliManualImageImportService = new System.ServiceProcess.ServiceInstaller();
            // 
            // AffiliManualImageImportServiceProcessInstaller
            // 
            this.AffiliManualImageImportServiceProcessInstaller.Password = null;
            this.AffiliManualImageImportServiceProcessInstaller.Username = null;
            // 
            // AffiliManualImageImportService
            // 
            this.AffiliManualImageImportService.Description = "Downloads all images for manually scheduled program.";
            this.AffiliManualImageImportService.DisplayName = "AffiliManualImageImportService";
            this.AffiliManualImageImportService.ServiceName = "AffiliManualImageImportService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.AffiliManualImageImportServiceProcessInstaller,
            this.AffiliManualImageImportService});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller AffiliManualImageImportServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller AffiliManualImageImportService;
    }
}
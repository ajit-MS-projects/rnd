namespace ProductExportFileMgmtService
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
            this.AffilinetExportFileMgmtServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.AffilinetExportFileMgmtServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // AffilinetExportFileMgmtServiceProcessInstaller
            // 
            this.AffilinetExportFileMgmtServiceProcessInstaller.Password = null;
            this.AffilinetExportFileMgmtServiceProcessInstaller.Username = null;
            // 
            // AffilinetExportFileMgmtServiceInstaller
            // 
            this.AffilinetExportFileMgmtServiceInstaller.Description = "Affilinet Export File Management Service";
            this.AffilinetExportFileMgmtServiceInstaller.DisplayName = "Affilinet Export File Management Service";
            this.AffilinetExportFileMgmtServiceInstaller.ServiceName = "AffilinetExportFileMgmtService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.AffilinetExportFileMgmtServiceProcessInstaller,
            this.AffilinetExportFileMgmtServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller AffilinetExportFileMgmtServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller AffilinetExportFileMgmtServiceInstaller;
    }
}
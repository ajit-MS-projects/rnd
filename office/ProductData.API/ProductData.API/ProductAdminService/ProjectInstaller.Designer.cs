namespace ProductAdminService
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
            this.ProductImportServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ProductImportServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ProductImportServiceProcessInstaller
            // 
            this.ProductImportServiceProcessInstaller.Password = null;
            this.ProductImportServiceProcessInstaller.Username = null;
            // 
            // ProductImportServiceInstaller
            // 
            this.ProductImportServiceInstaller.DisplayName = "Affilinet Product Import Service";
            this.ProductImportServiceInstaller.ServiceName = "AffilinetProductImportService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ProductImportServiceProcessInstaller,
            this.ProductImportServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ProductImportServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller ProductImportServiceInstaller;
    }
}
namespace ProductFileDownloadService
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
            this.FileDownloadServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.FileDownloadserviceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // FileDownloadServiceProcessInstaller
            // 
            this.FileDownloadServiceProcessInstaller.Password = null;
            this.FileDownloadServiceProcessInstaller.Username = null;
            // 
            // FileDownloadserviceInstaller
            // 
            this.FileDownloadserviceInstaller.DisplayName = "Affilinet Product File Download Service";
            this.FileDownloadserviceInstaller.ServiceName = "AffilinetProductFileDownloadService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.FileDownloadServiceProcessInstaller,
            this.FileDownloadserviceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller FileDownloadServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller FileDownloadserviceInstaller;
    }
}
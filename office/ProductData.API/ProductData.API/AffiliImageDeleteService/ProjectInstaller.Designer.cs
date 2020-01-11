namespace AffiliImageDeleteService
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
            this.AffiliImageDeleteServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.AffiliImageDeleteServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // AffiliImageDeleteServiceProcessInstaller
            // 
            this.AffiliImageDeleteServiceProcessInstaller.Password = null;
            this.AffiliImageDeleteServiceProcessInstaller.Username = null;
            // 
            // AffiliImageDeleteServiceInstaller
            // 
            this.AffiliImageDeleteServiceInstaller.Description = "Affili Image Delete Service";
            this.AffiliImageDeleteServiceInstaller.DisplayName = "AffiliImageDeleteService";
            this.AffiliImageDeleteServiceInstaller.ServiceName = "AffiliImageDeleteService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.AffiliImageDeleteServiceProcessInstaller,
            this.AffiliImageDeleteServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller AffiliImageDeleteServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller AffiliImageDeleteServiceInstaller;
    }
}
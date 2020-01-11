namespace WcfWinService
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
            this.WcfWinServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.WcfWinServiceServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // WcfWinServiceProcessInstaller
            // 
            this.WcfWinServiceProcessInstaller.Password = null;
            this.WcfWinServiceProcessInstaller.Username = null;
            // 
            // WcfWinServiceServiceInstaller
            // 
            this.WcfWinServiceServiceInstaller.Description = "Win Service that also host a WCF service";
            this.WcfWinServiceServiceInstaller.DisplayName = "WcfWinService";
            this.WcfWinServiceServiceInstaller.ServiceName = "WcfWinService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.WcfWinServiceProcessInstaller,
            this.WcfWinServiceServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller WcfWinServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller WcfWinServiceServiceInstaller;
    }
}
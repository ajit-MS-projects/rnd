namespace winServiceTest
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
            this.winServiceTestProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.winServiceTestInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // winServiceTestProcessInstaller
            // 
            this.winServiceTestProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.winServiceTestProcessInstaller.Password = null;
            this.winServiceTestProcessInstaller.Username = null;
            // 
            // winServiceTestInstaller
            // 
            this.winServiceTestInstaller.DisplayName = "winServiceTest";
            this.winServiceTestInstaller.ServiceName = "winServiceTest";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.winServiceTestProcessInstaller,
            this.winServiceTestInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller winServiceTestProcessInstaller;
        private System.ServiceProcess.ServiceInstaller winServiceTestInstaller;
    }
}
namespace ProductExportFileMgmtService
{
    partial class AffilinetExportFileMgmtService
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
            this.timListCleanUp = new System.Timers.Timer();
            this.timCopyExportFiles = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timListCleanUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timCopyExportFiles)).BeginInit();
            // 
            // timListCleanUp
            // 
            this.timListCleanUp.Enabled = true;
            this.timListCleanUp.Interval = 1000;
            this.timListCleanUp.Elapsed += new System.Timers.ElapsedEventHandler(this.timListCleanUp_Elapsed);
            // 
            // timCopyExportFiles
            // 
            this.timCopyExportFiles.Enabled = true;
            this.timCopyExportFiles.Interval = 1000;
            this.timCopyExportFiles.Elapsed += new System.Timers.ElapsedEventHandler(this.timCopyExportFiles_Elapsed);
            // 
            // AffilinetExportFileMgmtService
            // 
            this.ServiceName = "AffilinetExportFileMgmtService";
            ((System.ComponentModel.ISupportInitialize)(this.timListCleanUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timCopyExportFiles)).EndInit();

        }

        #endregion

        private System.Timers.Timer timListCleanUp;
        private System.Timers.Timer timCopyExportFiles;
    }
}

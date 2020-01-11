namespace ImageImportSsisMgmtService
{
    partial class AffiliImageImportSsisMgmtService
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
            this.timDailyImageImport = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timDailyImageImport)).BeginInit();
            // 
            // timDailyImageImport
            // 
            this.timDailyImageImport.Interval = 1000;
            this.timDailyImageImport.Elapsed += new System.Timers.ElapsedEventHandler(this.timDailyImageImport_Elapsed);
            // 
            // AffiliImageImportSsisMgmtService
            // 
            this.ServiceName = "AffiliImageImportSsisMgmtService";
            ((System.ComponentModel.ISupportInitialize)(this.timDailyImageImport)).EndInit();

        }

        #endregion

        private System.Timers.Timer timDailyImageImport;
    }
}

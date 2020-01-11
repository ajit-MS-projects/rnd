namespace AffiliImageCsvFileCleanUpService
{
    partial class AffiliImageCsvFileCleanUpService
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
            this.timImageImportCleanUp = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timImageImportCleanUp)).BeginInit();
            // 
            // timImageImportCleanUp
            // 
            this.timImageImportCleanUp.Enabled = true;
            this.timImageImportCleanUp.Interval = 1000;
            this.timImageImportCleanUp.Elapsed += new System.Timers.ElapsedEventHandler(this.timImageImportCleanUp_Elapsed);
            // 
            // AffiliImageCsvFileCleanUpService
            // 
            this.ServiceName = "AffiliImageCsvFileCleanUpService";
            ((System.ComponentModel.ISupportInitialize)(this.timImageImportCleanUp)).EndInit();

        }

        #endregion

        private System.Timers.Timer timImageImportCleanUp;
    }
}

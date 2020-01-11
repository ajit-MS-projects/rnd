namespace AffiliProductImportSsisService
{
    partial class AffiliProductImportSsisService
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
            this.timInitImport = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timInitImport)).BeginInit();
            // 
            // timInitImport
            // 
            this.timInitImport.Interval = 5000;
            this.timInitImport.Elapsed += new System.Timers.ElapsedEventHandler(this.timInitImport_Elapsed);
            // 
            // AffiliProductImportSsisService
            // 
            this.CanHandleSessionChangeEvent = true;
            this.ServiceName = "AffiliProductImportSsisService";
            ((System.ComponentModel.ISupportInitialize)(this.timInitImport)).EndInit();

        }

        #endregion

        private System.Timers.Timer timInitImport;
    }
}

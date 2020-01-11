namespace AffiliManualImageImportService
{
    partial class AffiliManualImageImportService
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
            this.timManualImageImport = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timManualImageImport)).BeginInit();
            // 
            // timManualImageImport
            // 
            this.timManualImageImport.Interval = 60000;
            this.timManualImageImport.Elapsed += new System.Timers.ElapsedEventHandler(this.timManualImageImport_Elapsed);
            // 
            // AffiliManualImageImportService
            // 
            this.ServiceName = "AffiliManualImageImportService";
            ((System.ComponentModel.ISupportInitialize)(this.timManualImageImport)).EndInit();

        }

        #endregion

        private System.Timers.Timer timManualImageImport;
    }
}

namespace AffiliImageDeleteService
{
    partial class AffiliImageDeleteService
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
            this.timImageDeleteFromFS = new System.Timers.Timer();
            this.timImageDeleteFromDB = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timImageDeleteFromFS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timImageDeleteFromDB)).BeginInit();
            // 
            // timImageDeleteFromFS
            // 
            this.timImageDeleteFromFS.Interval = 1000;
            this.timImageDeleteFromFS.Elapsed += new System.Timers.ElapsedEventHandler(this.timImageDeleteFromFS_Elapsed);
            // 
            // timImageDeleteFromDB
            // 
            this.timImageDeleteFromDB.Interval = 1000;
            this.timImageDeleteFromDB.Elapsed += new System.Timers.ElapsedEventHandler(this.timImageDeleteFromDB_Elapsed);
            // 
            // AffiliImageDeleteService
            // 
            this.ServiceName = "Service1";
            ((System.ComponentModel.ISupportInitialize)(this.timImageDeleteFromFS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timImageDeleteFromDB)).EndInit();

        }

        #endregion

        private System.Timers.Timer timImageDeleteFromFS;
        private System.Timers.Timer timImageDeleteFromDB;
    }
}

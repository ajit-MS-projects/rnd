﻿namespace ProductFileDownloadService
{
    partial class AffilinetProductFileDownloadService
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
            this.timFileDownload = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timFileDownload)).BeginInit();
            // 
            // timFileDownload
            // 
            this.timFileDownload.Interval = 1000;
            this.timFileDownload.Elapsed += new System.Timers.ElapsedEventHandler(this.timFileDownload_Elapsed);
            // 
            // AffilinetProductFileDownloadService
            // 
            this.ServiceName = "AffilinetProductFileDownloadService";
            ((System.ComponentModel.ISupportInitialize)(this.timFileDownload)).EndInit();

        }

        #endregion

        private System.Timers.Timer timFileDownload;
    }
}

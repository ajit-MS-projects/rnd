namespace AffiliImageReviewService
{
    partial class AffiliImageReviewService
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
            this.timImageReviewImport = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timImageReviewImport)).BeginInit();
            // 
            // timImageReviewImport
            // 
            this.timImageReviewImport.Interval = 1000;
            this.timImageReviewImport.Elapsed += new System.Timers.ElapsedEventHandler(this.timImageReviewImport_Elapsed);
            // 
            // AffiliImageReviewService
            // 
            this.ServiceName = "AffiliImageReviewService";
            ((System.ComponentModel.ISupportInitialize)(this.timImageReviewImport)).EndInit();

        }

        #endregion

        private System.Timers.Timer timImageReviewImport;
    }
}

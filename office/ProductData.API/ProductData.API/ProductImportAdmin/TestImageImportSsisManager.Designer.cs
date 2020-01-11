namespace ProductImportAdmin
{
    partial class TestImageImportSsisManager
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStartSsisImageImport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStartSsisImageImport
            // 
            this.btnStartSsisImageImport.Location = new System.Drawing.Point(27, 38);
            this.btnStartSsisImageImport.Name = "btnStartSsisImageImport";
            this.btnStartSsisImageImport.Size = new System.Drawing.Size(141, 23);
            this.btnStartSsisImageImport.TabIndex = 0;
            this.btnStartSsisImageImport.Text = "Start Ssis ImageImport";
            this.btnStartSsisImageImport.UseVisualStyleBackColor = true;
            this.btnStartSsisImageImport.Click += new System.EventHandler(this.btnStartSsisImageImport_Click);
            // 
            // TestImageImportSsisManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(198, 161);
            this.Controls.Add(this.btnStartSsisImageImport);
            this.Name = "TestImageImportSsisManager";
            this.Text = "TestImageImportSsisManager";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStartSsisImageImport;
    }
}
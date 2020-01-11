namespace ProductImportAdmin
{
    partial class TestImageDeleteManager
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
            this.btnImageDeleteFromFS = new System.Windows.Forms.Button();
            this.btnImageDeleteFromDB = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnImageDeleteFromFS
            // 
            this.btnImageDeleteFromFS.Location = new System.Drawing.Point(65, 58);
            this.btnImageDeleteFromFS.Name = "btnImageDeleteFromFS";
            this.btnImageDeleteFromFS.Size = new System.Drawing.Size(161, 23);
            this.btnImageDeleteFromFS.TabIndex = 0;
            this.btnImageDeleteFromFS.Text = "ImageDeleteFromFS";
            this.btnImageDeleteFromFS.UseVisualStyleBackColor = true;
            this.btnImageDeleteFromFS.Click += new System.EventHandler(this.btnImageDeleteFromFS_Click);
            // 
            // btnImageDeleteFromDB
            // 
            this.btnImageDeleteFromDB.Location = new System.Drawing.Point(65, 132);
            this.btnImageDeleteFromDB.Name = "btnImageDeleteFromDB";
            this.btnImageDeleteFromDB.Size = new System.Drawing.Size(161, 23);
            this.btnImageDeleteFromDB.TabIndex = 1;
            this.btnImageDeleteFromDB.Text = "ImageDeleteFromDB";
            this.btnImageDeleteFromDB.UseVisualStyleBackColor = true;
            this.btnImageDeleteFromDB.Click += new System.EventHandler(this.btnImageDeleteFromDB_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Test for AffiliImageDeleteService";
            // 
            // TestImageDeleteManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 194);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnImageDeleteFromDB);
            this.Controls.Add(this.btnImageDeleteFromFS);
            this.Name = "TestImageDeleteManager";
            this.Text = "TestImageDeleteManager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnImageDeleteFromFS;
        private System.Windows.Forms.Button btnImageDeleteFromDB;
        private System.Windows.Forms.Label label1;
    }
}
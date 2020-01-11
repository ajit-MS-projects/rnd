namespace ProductImportAdmin
{
    partial class TestErrorIndexingManager
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
            this.btnStartErrorIndexCalculation = new System.Windows.Forms.Button();
            this.txtProdProgId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStartErrorIndexCalculation
            // 
            this.btnStartErrorIndexCalculation.Location = new System.Drawing.Point(25, 79);
            this.btnStartErrorIndexCalculation.Name = "btnStartErrorIndexCalculation";
            this.btnStartErrorIndexCalculation.Size = new System.Drawing.Size(142, 23);
            this.btnStartErrorIndexCalculation.TabIndex = 0;
            this.btnStartErrorIndexCalculation.Text = "StartErrorIndexCalculation";
            this.btnStartErrorIndexCalculation.UseVisualStyleBackColor = true;
            this.btnStartErrorIndexCalculation.Click += new System.EventHandler(this.btnStartErrorIndexCalculation_Click);
            // 
            // txtProdProgId
            // 
            this.txtProdProgId.AcceptsReturn = true;
            this.txtProdProgId.Location = new System.Drawing.Point(25, 33);
            this.txtProdProgId.MaxLength = 5;
            this.txtProdProgId.Name = "txtProdProgId";
            this.txtProdProgId.Size = new System.Drawing.Size(100, 20);
            this.txtProdProgId.TabIndex = 1;
            this.txtProdProgId.Text = "974";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "ProductProgramId";
            // 
            // TestErrorIndexingManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 139);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtProdProgId);
            this.Controls.Add(this.btnStartErrorIndexCalculation);
            this.Name = "TestErrorIndexingManager";
            this.Text = "TestErrorIndexingManager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartErrorIndexCalculation;
        private System.Windows.Forms.TextBox txtProdProgId;
        private System.Windows.Forms.Label label1;
    }
}
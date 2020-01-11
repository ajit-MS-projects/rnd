namespace DatabaseTests
{
    partial class Form1
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
            this.btnSqlMdfFile = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSqlMdfFile
            // 
            this.btnSqlMdfFile.Location = new System.Drawing.Point(98, 117);
            this.btnSqlMdfFile.Name = "btnSqlMdfFile";
            this.btnSqlMdfFile.Size = new System.Drawing.Size(144, 23);
            this.btnSqlMdfFile.TabIndex = 0;
            this.btnSqlMdfFile.Text = "Sql MDF File";
            this.btnSqlMdfFile.UseVisualStyleBackColor = true;
            this.btnSqlMdfFile.Click += new System.EventHandler(this.btnSqlMdfFile_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(24, 13);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(322, 98);
            this.txtOutput.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 296);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnSqlMdfFile);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSqlMdfFile;
        private System.Windows.Forms.TextBox txtOutput;
    }
}


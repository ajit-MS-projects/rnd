namespace RenameFiles
{
    partial class frmFileRenamer
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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.txtSelectedPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblInfoGreen = new System.Windows.Forms.Label();
            this.lblInfoRed = new System.Windows.Forms.Label();
            this.panelPics = new System.Windows.Forms.Panel();
            this.dataGridViewFiles = new System.Windows.Forms.DataGridView();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblFilesInfo = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnRefresh2 = new System.Windows.Forms.Button();
            this.rdRemove = new System.Windows.Forms.RadioButton();
            this.rdAppend = new System.Windows.Forms.RadioButton();
            this.btnRenameText = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRenameText = new System.Windows.Forms.TextBox();
            this.txtFileExtension = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFiles)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.HelpRequest += new System.EventHandler(this.folderBrowserDialog1_HelpRequest);
            // 
            // txtSelectedPath
            // 
            this.txtSelectedPath.Location = new System.Drawing.Point(77, 18);
            this.txtSelectedPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSelectedPath.Name = "txtSelectedPath";
            this.txtSelectedPath.Size = new System.Drawing.Size(1057, 22);
            this.txtSelectedPath.TabIndex = 0;
            this.txtSelectedPath.Text = "d:\\temp";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Folder";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtFileExtension);
            this.panel1.Controls.Add(this.btnSelectFolder);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtSelectedPath);
            this.panel1.Location = new System.Drawing.Point(16, 15);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1619, 66);
            this.panel1.TabIndex = 3;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(1165, 18);
            this.btnSelectFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(151, 28);
            this.btnSelectFolder.TabIndex = 2;
            this.btnSelectFolder.Text = "Select Folder";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblInfoGreen);
            this.panel2.Controls.Add(this.lblInfoRed);
            this.panel2.Controls.Add(this.panelPics);
            this.panel2.Controls.Add(this.dataGridViewFiles);
            this.panel2.Controls.Add(this.btnRefresh);
            this.panel2.Controls.Add(this.lblFilesInfo);
            this.panel2.Location = new System.Drawing.Point(16, 107);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(2353, 654);
            this.panel2.TabIndex = 4;
            // 
            // lblInfoGreen
            // 
            this.lblInfoGreen.AutoSize = true;
            this.lblInfoGreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoGreen.ForeColor = System.Drawing.Color.Green;
            this.lblInfoGreen.Location = new System.Drawing.Point(743, 14);
            this.lblInfoGreen.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfoGreen.Name = "lblInfoGreen";
            this.lblInfoGreen.Size = new System.Drawing.Size(139, 39);
            this.lblInfoGreen.TabIndex = 8;
            this.lblInfoGreen.Text = "File Info";
            // 
            // lblInfoRed
            // 
            this.lblInfoRed.AutoSize = true;
            this.lblInfoRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoRed.ForeColor = System.Drawing.Color.Orange;
            this.lblInfoRed.Location = new System.Drawing.Point(400, 14);
            this.lblInfoRed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfoRed.Name = "lblInfoRed";
            this.lblInfoRed.Size = new System.Drawing.Size(139, 39);
            this.lblInfoRed.TabIndex = 7;
            this.lblInfoRed.Text = "File Info";
            // 
            // panelPics
            // 
            this.panelPics.Location = new System.Drawing.Point(7, 95);
            this.panelPics.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelPics.Name = "panelPics";
            this.panelPics.Size = new System.Drawing.Size(2343, 140);
            this.panelPics.TabIndex = 6;
            // 
            // dataGridViewFiles
            // 
            this.dataGridViewFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFiles.Location = new System.Drawing.Point(7, 242);
            this.dataGridViewFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridViewFiles.Name = "dataGridViewFiles";
            this.dataGridViewFiles.Size = new System.Drawing.Size(2343, 407);
            this.dataGridViewFiles.TabIndex = 4;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(7, 4);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(96, 55);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblFilesInfo
            // 
            this.lblFilesInfo.AutoSize = true;
            this.lblFilesInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilesInfo.Location = new System.Drawing.Point(111, 14);
            this.lblFilesInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFilesInfo.Name = "lblFilesInfo";
            this.lblFilesInfo.Size = new System.Drawing.Size(139, 39);
            this.lblFilesInfo.TabIndex = 2;
            this.lblFilesInfo.Text = "File Info";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnRefresh2);
            this.panel3.Controls.Add(this.rdRemove);
            this.panel3.Controls.Add(this.rdAppend);
            this.panel3.Controls.Add(this.btnRenameText);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.txtRenameText);
            this.panel3.Location = new System.Drawing.Point(16, 793);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1339, 74);
            this.panel3.TabIndex = 5;
            // 
            // btnRefresh2
            // 
            this.btnRefresh2.Location = new System.Drawing.Point(516, 4);
            this.btnRefresh2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRefresh2.Name = "btnRefresh2";
            this.btnRefresh2.Size = new System.Drawing.Size(96, 66);
            this.btnRefresh2.TabIndex = 9;
            this.btnRefresh2.Text = "Refresh";
            this.btnRefresh2.UseVisualStyleBackColor = true;
            this.btnRefresh2.Click += new System.EventHandler(this.btnRefresh2_Click);
            // 
            // rdRemove
            // 
            this.rdRemove.AutoSize = true;
            this.rdRemove.Location = new System.Drawing.Point(844, 33);
            this.rdRemove.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdRemove.Name = "rdRemove";
            this.rdRemove.Size = new System.Drawing.Size(81, 21);
            this.rdRemove.TabIndex = 8;
            this.rdRemove.Text = "Remove";
            this.rdRemove.UseVisualStyleBackColor = true;
            this.rdRemove.CheckedChanged += new System.EventHandler(this.rdRemove_CheckedChanged);
            // 
            // rdAppend
            // 
            this.rdAppend.AutoSize = true;
            this.rdAppend.Checked = true;
            this.rdAppend.Location = new System.Drawing.Point(709, 33);
            this.rdAppend.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdAppend.Name = "rdAppend";
            this.rdAppend.Size = new System.Drawing.Size(78, 21);
            this.rdAppend.TabIndex = 7;
            this.rdAppend.TabStop = true;
            this.rdAppend.Text = "Append";
            this.rdAppend.UseVisualStyleBackColor = true;
            this.rdAppend.CheckedChanged += new System.EventHandler(this.rdAppend_CheckedChanged);
            // 
            // btnRenameText
            // 
            this.btnRenameText.Location = new System.Drawing.Point(1057, 33);
            this.btnRenameText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRenameText.Name = "btnRenameText";
            this.btnRenameText.Size = new System.Drawing.Size(259, 28);
            this.btnRenameText.TabIndex = 6;
            this.btnRenameText.Text = "Rename";
            this.btnRenameText.UseVisualStyleBackColor = true;
            this.btnRenameText.Click += new System.EventHandler(this.btnRenameText_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 33);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Rename Text";
            // 
            // txtRenameText
            // 
            this.txtRenameText.Location = new System.Drawing.Point(124, 33);
            this.txtRenameText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRenameText.Name = "txtRenameText";
            this.txtRenameText.Size = new System.Drawing.Size(361, 22);
            this.txtRenameText.TabIndex = 4;
            this.txtRenameText.TextChanged += new System.EventHandler(this.txtRenameText_TextChanged);
            // 
            // txtFileExtension
            // 
            this.txtFileExtension.Location = new System.Drawing.Point(1349, 18);
            this.txtFileExtension.Name = "txtFileExtension";
            this.txtFileExtension.Size = new System.Drawing.Size(100, 22);
            this.txtFileExtension.TabIndex = 3;
            this.txtFileExtension.Text = ".jpg";
            // 
            // frmFileRenamer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1906, 881);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmFileRenamer";
            this.Text = "Rename Files in a Foler";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFiles)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txtSelectedPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblFilesInfo;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dataGridViewFiles;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnRenameText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRenameText;
        private System.Windows.Forms.Panel panelPics;
        private System.Windows.Forms.RadioButton rdRemove;
        private System.Windows.Forms.RadioButton rdAppend;
        private System.Windows.Forms.Button btnRefresh2;
        private System.Windows.Forms.Label lblInfoGreen;
        private System.Windows.Forms.Label lblInfoRed;
        private System.Windows.Forms.TextBox txtFileExtension;
    }
}


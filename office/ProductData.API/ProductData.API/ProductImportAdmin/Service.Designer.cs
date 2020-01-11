namespace ProductImportAdmin
{
    partial class Service
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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStartImport = new System.Windows.Forms.Button();
            this.timInitImport = new System.Timers.Timer();
            this.timFileDownload = new System.Timers.Timer();
            this.btnImageImport = new System.Windows.Forms.Button();
            this.btnDelImgs = new System.Windows.Forms.Button();
            this.btnImageReview = new System.Windows.Forms.Button();
            this.btnManImgImport = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkMultiThreaded = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLidtId = new System.Windows.Forms.TextBox();
            this.txtImgUrl = new System.Windows.Forms.TextBox();
            this.btndwnSingImg = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.timManualImageImport = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timInitImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timFileDownload)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timManualImageImport)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnStart.Location = new System.Drawing.Point(242, 17);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(116, 22);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start Sanitization";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStartImport
            // 
            this.btnStartImport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnStartImport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnStartImport.Location = new System.Drawing.Point(242, 42);
            this.btnStartImport.Margin = new System.Windows.Forms.Padding(2);
            this.btnStartImport.Name = "btnStartImport";
            this.btnStartImport.Size = new System.Drawing.Size(116, 22);
            this.btnStartImport.TabIndex = 1;
            this.btnStartImport.Text = "Start Download";
            this.btnStartImport.UseVisualStyleBackColor = true;
            this.btnStartImport.Click += new System.EventHandler(this.btnStartImport_Click);
            // 
            // timInitImport
            // 
            this.timInitImport.Interval = 5000;
            this.timInitImport.SynchronizingObject = this;
            this.timInitImport.Elapsed += new System.Timers.ElapsedEventHandler(this.timInitImport_Elapsed);
            // 
            // timFileDownload
            // 
            this.timFileDownload.Interval = 1000;
            this.timFileDownload.SynchronizingObject = this;
            this.timFileDownload.Elapsed += new System.Timers.ElapsedEventHandler(this.timFileDownload_Elapsed);
            // 
            // btnImageImport
            // 
            this.btnImageImport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnImageImport.Location = new System.Drawing.Point(507, 87);
            this.btnImageImport.Margin = new System.Windows.Forms.Padding(2);
            this.btnImageImport.Name = "btnImageImport";
            this.btnImageImport.Size = new System.Drawing.Size(116, 22);
            this.btnImageImport.TabIndex = 2;
            this.btnImageImport.Text = "Daily Image Import";
            this.btnImageImport.UseVisualStyleBackColor = true;
            this.btnImageImport.Click += new System.EventHandler(this.btnImageImport_Click);
            // 
            // btnDelImgs
            // 
            this.btnDelImgs.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnDelImgs.Location = new System.Drawing.Point(507, 142);
            this.btnDelImgs.Margin = new System.Windows.Forms.Padding(2);
            this.btnDelImgs.Name = "btnDelImgs";
            this.btnDelImgs.Size = new System.Drawing.Size(116, 22);
            this.btnDelImgs.TabIndex = 3;
            this.btnDelImgs.Text = "Delete Image CSVs";
            this.btnDelImgs.UseVisualStyleBackColor = true;
            this.btnDelImgs.Click += new System.EventHandler(this.btnDelImgs_Click);
            // 
            // btnImageReview
            // 
            this.btnImageReview.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnImageReview.Location = new System.Drawing.Point(507, 61);
            this.btnImageReview.Margin = new System.Windows.Forms.Padding(2);
            this.btnImageReview.Name = "btnImageReview";
            this.btnImageReview.Size = new System.Drawing.Size(116, 22);
            this.btnImageReview.TabIndex = 4;
            this.btnImageReview.Text = "Image Review";
            this.btnImageReview.UseVisualStyleBackColor = true;
            this.btnImageReview.Click += new System.EventHandler(this.btnImageReview_Click);
            // 
            // btnManImgImport
            // 
            this.btnManImgImport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnManImgImport.Location = new System.Drawing.Point(507, 34);
            this.btnManImgImport.Name = "btnManImgImport";
            this.btnManImgImport.Size = new System.Drawing.Size(116, 22);
            this.btnManImgImport.TabIndex = 5;
            this.btnManImgImport.Text = "Manual Image Import";
            this.btnManImgImport.UseVisualStyleBackColor = true;
            this.btnManImgImport.Click += new System.EventHandler(this.btnManImgImport_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkMultiThreaded);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtLidtId);
            this.groupBox1.Controls.Add(this.txtImgUrl);
            this.groupBox1.Controls.Add(this.btndwnSingImg);
            this.groupBox1.Controls.Add(this.btnManImgImport);
            this.groupBox1.Controls.Add(this.btnImageImport);
            this.groupBox1.Controls.Add(this.btnDelImgs);
            this.groupBox1.Controls.Add(this.btnImageReview);
            this.groupBox1.Location = new System.Drawing.Point(12, 131);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(628, 179);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image Review/Import";
            // 
            // chkMultiThreaded
            // 
            this.chkMultiThreaded.AutoSize = true;
            this.chkMultiThreaded.Location = new System.Drawing.Point(128, 87);
            this.chkMultiThreaded.Margin = new System.Windows.Forms.Padding(2);
            this.chkMultiThreaded.Name = "chkMultiThreaded";
            this.chkMultiThreaded.Size = new System.Drawing.Size(97, 17);
            this.chkMultiThreaded.TabIndex = 10;
            this.chkMultiThreaded.Text = "Multi Threaded";
            this.chkMultiThreaded.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "ListId";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Img URL:";
            // 
            // txtLidtId
            // 
            this.txtLidtId.Location = new System.Drawing.Point(38, 114);
            this.txtLidtId.Name = "txtLidtId";
            this.txtLidtId.Size = new System.Drawing.Size(31, 20);
            this.txtLidtId.TabIndex = 8;
            this.txtLidtId.Text = "620";
            // 
            // txtImgUrl
            // 
            this.txtImgUrl.Location = new System.Drawing.Point(128, 114);
            this.txtImgUrl.Name = "txtImgUrl";
            this.txtImgUrl.Size = new System.Drawing.Size(373, 20);
            this.txtImgUrl.TabIndex = 7;
            this.txtImgUrl.Text = "http://www.lmweb.de/thumbs/0.jpg";
            // 
            // btndwnSingImg
            // 
            this.btndwnSingImg.Location = new System.Drawing.Point(507, 114);
            this.btndwnSingImg.Name = "btndwnSingImg";
            this.btndwnSingImg.Size = new System.Drawing.Size(116, 23);
            this.btndwnSingImg.TabIndex = 6;
            this.btndwnSingImg.Text = "Single Image";
            this.btndwnSingImg.UseVisualStyleBackColor = true;
            this.btndwnSingImg.Click += new System.EventHandler(this.btndwnSingImg_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Controls.Add(this.btnStart);
            this.groupBox2.Controls.Add(this.btnStartImport);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(623, 100);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Product Import";
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(242, 68);
            this.btnImport.Margin = new System.Windows.Forms.Padding(2);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(116, 19);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // timManualImageImport
            // 
            this.timManualImageImport.Interval = 1000;
            this.timManualImageImport.SynchronizingObject = this;
            this.timManualImageImport.Elapsed += new System.Timers.ElapsedEventHandler(this.timManualImageImport_Elapsed);
            // 
            // Service
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 319);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Service";
            this.Text = "Service";
            this.Load += new System.EventHandler(this.Service_Load);
            ((System.ComponentModel.ISupportInitialize)(this.timInitImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timFileDownload)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.timManualImageImport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStartImport;
        private System.Timers.Timer timInitImport;
        private System.Timers.Timer timFileDownload;
        private System.Windows.Forms.Button btnImageImport;
        private System.Windows.Forms.Button btnDelImgs;
        private System.Windows.Forms.Button btnImageReview;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnManImgImport;
        private System.Windows.Forms.TextBox txtLidtId;
        private System.Windows.Forms.TextBox txtImgUrl;
        private System.Windows.Forms.Button btndwnSingImg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkMultiThreaded;
        private System.Windows.Forms.Button btnImport;
        private System.Timers.Timer timManualImageImport;
    }
}
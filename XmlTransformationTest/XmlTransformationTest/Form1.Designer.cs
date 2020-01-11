namespace XmlTransformationTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtSourceXml = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdVersion3 = new System.Windows.Forms.RadioButton();
            this.rdVersion2 = new System.Windows.Forms.RadioButton();
            this.rdVersion1 = new System.Windows.Forms.RadioButton();
            this.txtXslt1 = new System.Windows.Forms.TextBox();
            this.txtXslt3 = new System.Windows.Forms.TextBox();
            this.txtXslt2 = new System.Windows.Forms.TextBox();
            this.btnTransform = new System.Windows.Forms.Button();
            this.btnTransformFiletoFile = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSourceXml
            // 
            this.txtSourceXml.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtSourceXml.Location = new System.Drawing.Point(17, 31);
            this.txtSourceXml.Multiline = true;
            this.txtSourceXml.Name = "txtSourceXml";
            this.txtSourceXml.Size = new System.Drawing.Size(278, 246);
            this.txtSourceXml.TabIndex = 0;
            this.txtSourceXml.Text = resources.GetString("txtSourceXml.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Source Xml:";
            // 
            // txtOutput
            // 
            this.txtOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutput.Location = new System.Drawing.Point(304, 31);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(434, 246);
            this.txtOutput.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(304, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Output Xml:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSourceXml);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtOutput);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 391);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(751, 287);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Xml";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdVersion3);
            this.groupBox2.Controls.Add(this.rdVersion2);
            this.groupBox2.Controls.Add(this.rdVersion1);
            this.groupBox2.Controls.Add(this.txtXslt1);
            this.groupBox2.Controls.Add(this.txtXslt3);
            this.groupBox2.Controls.Add(this.txtXslt2);
            this.groupBox2.Location = new System.Drawing.Point(5, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(892, 373);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "XSLT";
            // 
            // rdVersion3
            // 
            this.rdVersion3.AutoSize = true;
            this.rdVersion3.Location = new System.Drawing.Point(601, 12);
            this.rdVersion3.Name = "rdVersion3";
            this.rdVersion3.Size = new System.Drawing.Size(69, 17);
            this.rdVersion3.TabIndex = 5;
            this.rdVersion3.Text = "Version 3";
            this.rdVersion3.UseVisualStyleBackColor = true;
            // 
            // rdVersion2
            // 
            this.rdVersion2.AutoSize = true;
            this.rdVersion2.Location = new System.Drawing.Point(311, 12);
            this.rdVersion2.Name = "rdVersion2";
            this.rdVersion2.Size = new System.Drawing.Size(69, 17);
            this.rdVersion2.TabIndex = 5;
            this.rdVersion2.Text = "Version 2";
            this.rdVersion2.UseVisualStyleBackColor = true;
            // 
            // rdVersion1
            // 
            this.rdVersion1.AutoSize = true;
            this.rdVersion1.Checked = true;
            this.rdVersion1.Location = new System.Drawing.Point(20, 13);
            this.rdVersion1.Name = "rdVersion1";
            this.rdVersion1.Size = new System.Drawing.Size(69, 17);
            this.rdVersion1.TabIndex = 5;
            this.rdVersion1.TabStop = true;
            this.rdVersion1.Text = "Version 1";
            this.rdVersion1.UseVisualStyleBackColor = true;
            // 
            // txtXslt1
            // 
            this.txtXslt1.Location = new System.Drawing.Point(17, 31);
            this.txtXslt1.Multiline = true;
            this.txtXslt1.Name = "txtXslt1";
            this.txtXslt1.Size = new System.Drawing.Size(278, 336);
            this.txtXslt1.TabIndex = 0;
            this.txtXslt1.Text = resources.GetString("txtXslt1.Text");
            // 
            // txtXslt3
            // 
            this.txtXslt3.Location = new System.Drawing.Point(601, 31);
            this.txtXslt3.Multiline = true;
            this.txtXslt3.Name = "txtXslt3";
            this.txtXslt3.Size = new System.Drawing.Size(278, 336);
            this.txtXslt3.TabIndex = 0;
            this.txtXslt3.Text = resources.GetString("txtXslt3.Text");
            // 
            // txtXslt2
            // 
            this.txtXslt2.Location = new System.Drawing.Point(311, 31);
            this.txtXslt2.Multiline = true;
            this.txtXslt2.Name = "txtXslt2";
            this.txtXslt2.Size = new System.Drawing.Size(278, 336);
            this.txtXslt2.TabIndex = 0;
            this.txtXslt2.Text = resources.GetString("txtXslt2.Text");
            // 
            // btnTransform
            // 
            this.btnTransform.Location = new System.Drawing.Point(769, 467);
            this.btnTransform.Name = "btnTransform";
            this.btnTransform.Size = new System.Drawing.Size(122, 37);
            this.btnTransform.TabIndex = 7;
            this.btnTransform.Text = "Transform";
            this.btnTransform.UseVisualStyleBackColor = true;
            this.btnTransform.Click += new System.EventHandler(this.btnTransform_Click);
            // 
            // btnTransformFiletoFile
            // 
            this.btnTransformFiletoFile.Location = new System.Drawing.Point(769, 526);
            this.btnTransformFiletoFile.Name = "btnTransformFiletoFile";
            this.btnTransformFiletoFile.Size = new System.Drawing.Size(122, 37);
            this.btnTransformFiletoFile.TabIndex = 7;
            this.btnTransformFiletoFile.Text = "Transform File to File";
            this.btnTransformFiletoFile.UseVisualStyleBackColor = true;
            this.btnTransformFiletoFile.Click += new System.EventHandler(this.btnTransformFiletoFile_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 685);
            this.Controls.Add(this.btnTransformFiletoFile);
            this.Controls.Add(this.btnTransform);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtSourceXml;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtXslt1;
        private System.Windows.Forms.TextBox txtXslt3;
        private System.Windows.Forms.TextBox txtXslt2;
        private System.Windows.Forms.RadioButton rdVersion3;
        private System.Windows.Forms.RadioButton rdVersion2;
        private System.Windows.Forms.RadioButton rdVersion1;
        private System.Windows.Forms.Button btnTransform;
        private System.Windows.Forms.Button btnTransformFiletoFile;
    }
}


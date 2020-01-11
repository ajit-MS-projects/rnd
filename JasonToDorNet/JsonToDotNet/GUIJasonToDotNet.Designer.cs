namespace JsonToDotNet
{
    partial class GUIJasonToDotNet
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
            this.btnReadJason = new System.Windows.Forms.Button();
            this.txtRawJason = new System.Windows.Forms.TextBox();
            this.txtJsonObject = new System.Windows.Forms.TextBox();
            this.btnReadJsonToEnt = new System.Windows.Forms.Button();
            this.btnReadUsingAffiliApi = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnReadJason
            // 
            this.btnReadJason.Location = new System.Drawing.Point(103, 536);
            this.btnReadJason.Name = "btnReadJason";
            this.btnReadJason.Size = new System.Drawing.Size(124, 23);
            this.btnReadJason.TabIndex = 0;
            this.btnReadJason.Text = "Read Json";
            this.btnReadJason.UseVisualStyleBackColor = true;
            this.btnReadJason.Click += new System.EventHandler(this.btnReadJason_Click);
            // 
            // txtRawJason
            // 
            this.txtRawJason.Location = new System.Drawing.Point(13, 13);
            this.txtRawJason.Multiline = true;
            this.txtRawJason.Name = "txtRawJason";
            this.txtRawJason.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRawJason.Size = new System.Drawing.Size(686, 240);
            this.txtRawJason.TabIndex = 1;
            // 
            // txtJsonObject
            // 
            this.txtJsonObject.Location = new System.Drawing.Point(12, 259);
            this.txtJsonObject.Multiline = true;
            this.txtJsonObject.Name = "txtJsonObject";
            this.txtJsonObject.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtJsonObject.Size = new System.Drawing.Size(686, 240);
            this.txtJsonObject.TabIndex = 2;
            // 
            // btnReadJsonToEnt
            // 
            this.btnReadJsonToEnt.Location = new System.Drawing.Point(268, 536);
            this.btnReadJsonToEnt.Name = "btnReadJsonToEnt";
            this.btnReadJsonToEnt.Size = new System.Drawing.Size(153, 23);
            this.btnReadJsonToEnt.TabIndex = 3;
            this.btnReadJsonToEnt.Text = "Read Json to Entities";
            this.btnReadJsonToEnt.UseVisualStyleBackColor = true;
            this.btnReadJsonToEnt.Click += new System.EventHandler(this.btnReadJsonToEnt_Click);
            // 
            // btnReadUsingAffiliApi
            // 
            this.btnReadUsingAffiliApi.Location = new System.Drawing.Point(453, 535);
            this.btnReadUsingAffiliApi.Name = "btnReadUsingAffiliApi";
            this.btnReadUsingAffiliApi.Size = new System.Drawing.Size(207, 23);
            this.btnReadUsingAffiliApi.TabIndex = 4;
            this.btnReadUsingAffiliApi.Text = "Read Using affili API";
            this.btnReadUsingAffiliApi.UseVisualStyleBackColor = true;
            this.btnReadUsingAffiliApi.Click += new System.EventHandler(this.btnReadUsingAffiliApi_Click);
            // 
            // GUIJasonToDotNet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 571);
            this.Controls.Add(this.btnReadUsingAffiliApi);
            this.Controls.Add(this.btnReadJsonToEnt);
            this.Controls.Add(this.txtJsonObject);
            this.Controls.Add(this.txtRawJason);
            this.Controls.Add(this.btnReadJason);
            this.Name = "GUIJasonToDotNet";
            this.Text = "Json To Dot Net";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReadJason;
        private System.Windows.Forms.TextBox txtRawJason;
        private System.Windows.Forms.TextBox txtJsonObject;
        private System.Windows.Forms.Button btnReadJsonToEnt;
        private System.Windows.Forms.Button btnReadUsingAffiliApi;
    }
}


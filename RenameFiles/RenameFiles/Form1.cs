using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RenameFiles
{
    public partial class frmFileRenamer : Form
    {
        private string fileExtension { get { return txtFileExtension.Text.ToUpper(); } }
       
        public frmFileRenamer()
        {
            InitializeComponent();
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtRenameText_TextChanged(sender, e);
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog(); 
            if(result==System.Windows.Forms.DialogResult.OK)
            {
                txtSelectedPath.Text = folderBrowserDialog1.SelectedPath;
                btnRefresh_Click(sender, e);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lblFilesInfo.Text = "Total Files: " + Directory.EnumerateFiles(txtSelectedPath.Text, "*"+fileExtension).Count().ToString();
            var data = new DataTable();
            data.Columns.Add("Source File");
            data.Columns.Add("Dest. File");
            int left = 0;
            int notAffectedFiles = 0;
            PictureBox picBox = null;
            foreach (Control cnt in panelPics.Controls)
            {
                panelPics.Controls.Remove(cnt);
            }
            foreach (var file in Directory.EnumerateFiles(txtSelectedPath.Text, "*"+fileExtension))
            {
                if (!string.IsNullOrWhiteSpace(txtRenameText.Text))
                {
                    if (rdAppend.Checked)
                    {
                        if (file.ToUpper().Contains(txtRenameText.Text.ToUpper()))
                        {
                            notAffectedFiles++;
                            continue;
                        }
                    }

                    if (rdRemove.Checked)
                    {
                        if (!file.ToUpper().Contains(txtRenameText.Text.ToUpper()))
                        {
                            notAffectedFiles++;
                            continue;
                        }
                    }
                }
             
                if (left < panelPics.Width - 150)
                {
                    picBox = new PictureBox();
                    picBox.ImageLocation = file;
                    picBox.Height = 100;
                    picBox.Width = 150;
                    picBox.SizeMode = PictureBoxSizeMode.Zoom;
                    picBox.BackColor = Color.Black;
                    panelPics.Controls.Add(picBox);
                    picBox.Left = left;
                    left += 151;
                }
                else 
                {
                    picBox.ImageLocation = file;
                }

                var rw = data.NewRow();
                rw[0] = file;
                rw[1] = GetNewFileName(file);
                data.Rows.Add(rw);
            }

            lblInfoRed.Text  = "Files Affected:" + data.Rows.Count ;
            lblInfoGreen.Text =  "Files Not Affected:" + notAffectedFiles;

            dataGridViewFiles.DataSource = data;
            dataGridViewFiles.AutoResizeColumns();

        }

        private string GetNewFileName(string file)  
        {
            if (!string.IsNullOrWhiteSpace(txtRenameText.Text))
            {
                if (rdAppend.Checked)
                {
                    return file.ToUpper().Replace(fileExtension, txtRenameText.Text.ToUpper() + fileExtension);
                }
                else if (rdRemove.Checked)
                {
                    return file.ToUpper().Replace(txtRenameText.Text.ToUpper(), "");
                }
            }
            return string.Empty;
        }
        
        private void rdAppend_CheckedChanged(object sender, EventArgs e)
        {
            btnRefresh_Click(sender, e);
        }

        private void rdRemove_CheckedChanged(object sender, EventArgs e)
        {
            btnRefresh_Click(sender, e);
        }

        private void btnRenameText_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(this, "Are you sure..?", "Confirm", MessageBoxButtons.OKCancel);
            if (result != System.Windows.Forms.DialogResult.OK)
                return;

            foreach (var file in Directory.EnumerateFiles(txtSelectedPath.Text, "*"+fileExtension))
            {
                File.Move(file, GetNewFileName(file));
            }

            txtRenameText.Text = "";
            btnRefresh_Click(sender, e);
        }

        private void btnRefresh2_Click(object sender, EventArgs e)
        {
            btnRefresh_Click(sender, e);
        }

        private void txtRenameText_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRenameText.Text))
            {
                btnRenameText.Enabled = false;
            }
            else
            {
                btnRenameText.Enabled = true;
            }
        }

    }
}

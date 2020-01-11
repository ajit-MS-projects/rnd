using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HashGenUtility
{
    public partial class frmFileSplitter : Form
    {
        public StreamReader SrCsvSource { get; set; }
        StreamWriter swDelProdCsv  { get; set; }
        public frmFileSplitter()
        {
            InitializeComponent();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog.ShowDialog();
            if(dr==DialogResult.OK)
            {
                txtFileName.Text = openFileDialog.FileName;
                int lst = txtFileName.Text.LastIndexOf(@"\");
                string dir = txtFileName.Text.Substring(0, lst);
                wbDirectoryView.Navigate(dir);
            }
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            OpenFileForReadWrite();
            string strLine = "";
            long start = long.Parse(txtStart.Text);
            long end = 0;
            long cnt = 0;
            if(cmbEnd.Text.ToUpper()!="EOF")
                end = long.Parse(cmbEnd.Text);
            writeLine(SrCsvSource.ReadLine());
            while ((strLine = SrCsvSource.ReadLine()) != null)
            {
                cnt++;
                if (cnt >= start && (cnt <= end || cmbEnd.Text.ToUpper() == "EOF"))
                        writeLine(strLine);
                if (!chkCountTotalRows.Checked && cnt > end) break;
            }
            txtTotalRows.Text = cnt.ToString();
            CloseFiles();
            MessageBox.Show("Done...!","...",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
        private void writeLine(string line)
        {
            swDelProdCsv.WriteLine(line);
        }
        private void OpenFileForReadWrite()
        {
            try
            {
                SrCsvSource = new StreamReader(txtFileName.Text);
                swDelProdCsv = new StreamWriter(txtFileName.Text + "split.csv", false, Encoding.Unicode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CloseFiles()
        {
            if (SrCsvSource != null)
            {
                SrCsvSource.Close();
                SrCsvSource.Dispose();
                SrCsvSource = null;
            }
            if (swDelProdCsv != null)
            {
                swDelProdCsv.Flush();
                swDelProdCsv.Close();
                swDelProdCsv.Dispose();
                swDelProdCsv = null;
            }
        }
    }
}

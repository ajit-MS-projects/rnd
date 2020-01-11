using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ImgDownloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();

            DirectoryInfo info = Directory.CreateDirectory(@"d:\dwnImages\" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
            label1.Text = "Working.........!";

            for (int i = 1; i < int.MaxValue; i++)
            {
                string fileName = string.Format(textBox1.Text, i);
                Uri uri = new Uri(fileName);
                try
                {
                    client.DownloadFile(fileName, info.FullName + @"\" + Path.GetFileName(uri.LocalPath));    
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    label1.Text = "Total images downloaded" + i + " " + ex;
                    break;
                 }

            }
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClientApp.ServiceReference1;

namespace ClientApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Service1Client client = new Service1Client("httpClient");
        Service1Client tcpClient = new Service1Client("tcpClient");
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = client.GetData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = tcpClient.GetData();
        }
    }
}

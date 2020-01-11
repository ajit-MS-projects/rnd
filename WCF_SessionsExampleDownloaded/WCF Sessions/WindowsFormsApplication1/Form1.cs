using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using System.Transactions;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        IserviceClient httpclient = new IserviceClient("b");
        IserviceClient tcpclient = new IserviceClient("c");
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            httpclient.dowork();
            label1.Text = httpclient.InnerChannel.SessionId;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tcpclient.dowork();
            label1.Text = tcpclient.InnerChannel.SessionId;
        }
    }
}

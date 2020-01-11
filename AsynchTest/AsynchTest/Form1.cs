using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AsynchTest
{
    public partial class Form1 : Form
    {
        private delegate string AsyncDelegate();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AsyncDelegate ad = new AsyncDelegate(LongMethod);
            ad.BeginInvoke(null,null);
        }

        private string LongMethod()
        {
            Thread.Sleep(5000);
            MessageBox.Show(DateTime.Now.ToString());
            return DateTime.Now.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AsyncDelegate ad = new AsyncDelegate(LongMethod2);
            ad.BeginInvoke(CallBack, null);
        }

        private void CallBack(IAsyncResult res)
        {
            MessageBox.Show(res.ToString());
        }
        private string LongMethod2()
        {
            Thread.Sleep(5000);
            return "2:"+DateTime.Now.ToString();
        }
    }
}

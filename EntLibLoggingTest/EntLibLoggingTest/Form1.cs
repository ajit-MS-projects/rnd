using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Solar.Common;
using Solar.Exceptions;

namespace EntLibLoggingTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int x = int.Parse(textBox1.Text);
                int y = int.Parse(textBox2.Text);
                textBox3.Text = (x/y).ToString();
                Utilities.CreateDebugLog("Divide result:" + textBox3.Text, ApplicationEventsEnum.None);
            }
            catch (Exception ex)
            {
                new SolarGenericException("Error while dividing." + ex).CreateLog();
            }
           
        }
    }
}

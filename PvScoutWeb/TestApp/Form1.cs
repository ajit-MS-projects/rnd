using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Solar.Pvscout.Business;
using Solar.Pvscout.Business.Entity;


namespace TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // new ObjectPostionCalculator().GetPvModulePosition(new PvModuleActual());
        }
    }
}

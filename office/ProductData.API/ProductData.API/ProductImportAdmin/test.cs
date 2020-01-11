using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Affilinet.Business.ProductImport.Common;

namespace ProductImportAdmin
{
    public partial class test : Form
    {
        public test()
        {
            InitializeComponent();
        }

        private void btnGenHash_Click(object sender, EventArgs e)
        {
            txtHash1.Text=Utilities.GetMd5Hash(txtURL1.Text);
            txtHash2.Text = Utilities.GetMd5Hash(txtURL2.Text);
            txtHash3.Text = Utilities.GetMd5Hash(txtURL3.Text);
            txtHash4.Text = Utilities.GetMd5Hash(txtURL4.Text);
        }
    }
}

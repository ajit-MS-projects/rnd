﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace XmlTransformationTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTransform_Click(object sender, EventArgs e)
        {
            try
            {
                TextReader tr1 = new StringReader(txtSourceXml.Text);
                XmlTextReader tr11 = new XmlTextReader(tr1);
                XPathDocument xPathDocument = new XPathDocument(tr11);

                //read XSLT
                TextReader tr2 = null;
                if (rdVersion1.Checked)
                    tr2 = new StringReader(txtXslt1.Text);
                if (rdVersion2.Checked)
                    tr2 = new StringReader(txtXslt2.Text);
                if (rdVersion3.Checked)
                    tr2 = new StringReader(txtXslt3.Text);
                XmlTextReader tr22 = new XmlTextReader(tr2);
                XslTransform xslt = new XslTransform();
                xslt.Load(tr22);

                //create the output stream
                StringBuilder sb = new StringBuilder();
                TextWriter tw = new StringWriter(sb);

                //xsl.Transform (doc, null, Console.Out);
                xslt.Transform(xPathDocument, null, tw);

                txtOutput.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnTransformFiletoFile_Click(object sender, EventArgs e)
        {
            XslTransform myXslTransform = new XslTransform();
            myXslTransform.Load(@"schiene\schiene.xsl");
            myXslTransform.Transform(@"schiene\01_testschiene.xml", @"schiene\01_testschiene_output.xml");
        }

    }
}

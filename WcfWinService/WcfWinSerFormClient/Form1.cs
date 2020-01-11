using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using WcfWinService.MetaDataServices;

namespace WcfWinSerFormClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private IMetaDataService metaDataServiceChannel = null;
        private void button1_Click(object sender, EventArgs e)
        {
            EndpointAddress productAddress = new EndpointAddress("net.tcp://localhost:9010/ProductService");
            metaDataServiceChannel = ChannelFactory<IMetaDataService>.CreateChannel(new NetTcpBinding(), productAddress);
            ServiceMetadata objServiceMetadata = metaDataServiceChannel.WhatAreYouDoing();

            textBox1.Text = objServiceMetadata.HealthCheckCounter.ToString() + Environment.NewLine;
            textBox1.Text += objServiceMetadata.LastStamp + Environment.NewLine;
            textBox1.Text += objServiceMetadata.LastMetaDataServiceFault + Environment.NewLine;
        }
    }
}

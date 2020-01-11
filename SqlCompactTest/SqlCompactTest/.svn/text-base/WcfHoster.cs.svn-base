using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WcfServices;

namespace SqlCompactTest
{
    public partial class WcfHoster : Form
    {
        public WcfHoster()
        {
            InitializeComponent();
        }

        private Thread m_thread = null;
        private void WcfHoster_Load(object sender, EventArgs e)
        {
            m_thread = new Thread(new ThreadStart(StartMetaDataService));
            m_thread.Start();
            String path = "";
            path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            webBrowser1.Navigate(path+@"\SilverLightClientTestPage.html");
        }
        private void StartMetaDataService()
        {
            ServiceHost productHost = null;
            ServiceHost crossDomainHost = null;
            try
            {
                productHost = new ServiceHost(typeof(ManufacturerService));
                productHost.Faulted += new EventHandler(ProductHost_Faulted);
                productHost.Open();

                crossDomainHost = new ServiceHost(typeof(CrossDomainService));
                crossDomainHost.Faulted += new EventHandler(ProductHost_Faulted);
                crossDomainHost.Open();
                Thread.CurrentThread.Suspend();
            }
            finally
            {
                if (productHost.State == CommunicationState.Faulted)
                {
                    productHost.Abort();
                }
                else
                {
                    productHost.Close();
                }
            }
        }
        private  void ProductHost_Faulted(object sender, EventArgs e)
        {
            if (sender != null && sender.GetType() == typeof(ServiceHost))
            {
                ServiceHost productHost = (ServiceHost)sender;
                productHost.Abort();
                productHost.Close();
            }
            //MetaDataService.objServiceMetadata.LastMetaDataServiceFault = "ProductHost_Faulted at: " + DateTime.Now + "::" + e.ToString();
            //WriteHealthCheckTimeStamp(MetaDataService.objServiceMetadata.LastMetaDataServiceFault);
            StartMetaDataService();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IManufacturerService serviceChannel = null;
            EndpointAddress productAddress = new EndpointAddress("http://localhost:9010/ProductService");
            serviceChannel = ChannelFactory<IManufacturerService>.CreateChannel(new BasicHttpBinding(), productAddress);
            List<Manufacturer> objServiceMetadata = serviceChannel.GetAllManufacturers();

            textBox1.Text = objServiceMetadata[0].ManufName;
        }

        private void WcfHoster_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(m_thread!=null)
            {
                m_thread.Resume();
                m_thread.Abort();
                m_thread = null;
            }
        }
    }
}

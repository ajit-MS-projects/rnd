using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Text;
using WcfWinService.MetaDataServices;

namespace WcfWinService
{
    public partial class WcfWinService : ServiceBase
    {
        public WcfWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            StartMetaDataService();
        }
        ServiceHost productHost = null;

        private void StartMetaDataService()
        {
            try
            {
                productHost = new ServiceHost(typeof (MetaDataService));
                //ServiceEndpoint productEndpoint = productHost.AddServiceEndpoint(typeof(IMetaDataService),
                //                                   new NetTcpBinding(), "net.tcp://localhost:9010/ProductService");
                //ServiceEndpoint productEndpoint = productHost.AddServiceEndpoint(typeof(IMetaDataService),
                //                              new WSHttpBinding(), "http://localhost:9010/ProductService");
                productHost.Faulted += new EventHandler(ProductHost_Faulted);
                productHost.Open();
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
        private void ProductHost_Faulted(object sender, EventArgs e)
        {
            if (sender!=null && sender.GetType() == typeof(ServiceHost))
            {
                ServiceHost productHost = (ServiceHost) sender;
                productHost.Abort();
                productHost.Close();
            }
            MetaDataService.objServiceMetadata.LastMetaDataServiceFault = "ProductHost_Faulted at: " + DateTime.Now + "::" + e.ToString();
            WriteHealthCheckTimeStamp(MetaDataService.objServiceMetadata.LastMetaDataServiceFault);
            StartMetaDataService();
        }

        protected override void OnStop()
        {
            productHost.Abort();
            productHost.Close();
        }

        private bool isServiceStarted = false;
        private void timWcfWinService_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            WriteHealthCheckTimeStamp();
            if (!isServiceStarted)
            {
                StartMetaDataService();
                isServiceStarted = true;
            }
        }
        public static void WriteHealthCheckTimeStamp()
        {
            StreamWriter swFile = null;
            try
            {
                String fileFullPath = @"c:\";
                String fileName = "WcfWinService.WinServiceHeathCheck.txt";
                swFile = new StreamWriter(fileFullPath + fileName, false, Encoding.Unicode);
                swFile.Write(DateTime.Now.ToString());
                MetaDataService.objServiceMetadata.HealthCheckCounter++;
                MetaDataService.objServiceMetadata.LastStamp = DateTime.Now.ToString();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (swFile != null)
                {
                    swFile.Close();
                    swFile.Dispose();
                }
            }
        }

        public static void WriteHealthCheckTimeStamp(String text)
        {
            StreamWriter swFile = null;
            try
            {
                String fileFullPath = @"c:\";
                String fileName = "WcfWinService.ErrorLog.txt";
                swFile = new StreamWriter(fileFullPath + fileName, false, Encoding.Unicode);
                swFile.Write(text);
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                if (swFile != null)
                {
                    swFile.Close();
                    swFile.Dispose();
                }
            }
        }

    }
}

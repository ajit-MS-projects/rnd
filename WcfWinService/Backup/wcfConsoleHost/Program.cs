using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using WcfWinService.MetaDataServices;

namespace wcfConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            MetaDataService.objServiceMetadata.LastStamp = DateTime.Now.ToString();
            MetaDataService.objServiceMetadata.HealthCheckCounter++;
            MetaDataService.objServiceMetadata.LastMetaDataServiceFault = "none";
            StartMetaDataService();
        }
        private static void StartMetaDataService()
        {
            ServiceHost productHost = null;
            ServiceHost inventoryHost = null;
            try
            {
                productHost = new ServiceHost(typeof(MetaDataService));
                //ServiceEndpoint productEndpoint = productHost.AddServiceEndpoint(typeof(IMetaDataService),
                //                                   new NetTcpBinding(), "net.tcp://localhost:9010/ProductService");
                productHost.Faulted += new EventHandler(ProductHost_Faulted);
                productHost.Open();
                Console.WriteLine("The Product service is running and is listening on:");
                //Console.WriteLine("{0} ({1})",productEndpoint.Address.ToString(),productEndpoint.Binding.Name);
                Console.WriteLine("\nPress any key to stop the service.");
                Console.ReadKey();
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
        private static void ProductHost_Faulted(object sender, EventArgs e)
        {
            if (sender != null && sender.GetType() == typeof(ServiceHost))
            {
                ServiceHost productHost = (ServiceHost)sender;
                productHost.Abort();
                productHost.Close();
            }
            MetaDataService.objServiceMetadata.LastMetaDataServiceFault = "ProductHost_Faulted at: " + DateTime.Now + "::" + e.ToString();
            //WriteHealthCheckTimeStamp(MetaDataService.objServiceMetadata.LastMetaDataServiceFault);
            StartMetaDataService();
        }
    }
}

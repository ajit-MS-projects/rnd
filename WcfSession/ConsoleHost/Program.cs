using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(WcfSession.Service1)))
            {
                host.Open();
                Console.WriteLine("Service Started...at http://localhost:62476/?wsdl");
                Console.WriteLine("Press enter to stop the service.");
                Console.ReadLine();
            }
        }
    }
}

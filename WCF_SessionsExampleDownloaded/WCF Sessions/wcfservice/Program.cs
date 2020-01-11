using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace wcfservice
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(wcfservice.serviceclass)))
            {                
                host.Open();
                Console.WriteLine("Service Started...");
                Console.WriteLine("Press enter to stop the service.");
                Console.ReadLine();
            }

        }
    }
}

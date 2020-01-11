using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.Web;

namespace WcfMultiInheritence
{
    public class PolymorphicServiceFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost host = new ServiceHost(serviceType, baseAddresses);
            ServiceEndpoint endpoint = host.AddServiceEndpoint(typeof(IService1), new BasicHttpBinding(), "");
            foreach (var operation in endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dcsob = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dcsob != null)
                {
                    dcsob.DataContractResolver = new DogResolver();
                }
            }

            return host;
        }
    }
}
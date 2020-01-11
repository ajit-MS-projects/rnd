using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;
using System.Data.SqlClient;
using System.Transactions;

namespace wcfservice
{
    [ServiceContract]
    public interface Iservice
    {
        [OperationContract]     
        void dowork();
    }


    public class serviceclass : Iservice
    {
        public void dowork()
        {
            Console.WriteLine("Instance Object : {0}", OperationContext.Current.InstanceContext.GetHashCode());
            Console.WriteLine("Service Object : {0}", OperationContext.Current.InstanceContext.GetServiceInstance().GetHashCode());
            Console.WriteLine("Session Id  : {0}", OperationContext.Current.SessionId);
            Console.WriteLine();
        }
    }
}

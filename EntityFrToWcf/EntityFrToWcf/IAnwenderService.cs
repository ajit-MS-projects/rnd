using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace EntityFrToWcf
{
    [ServiceContract]
    public interface IAnwenderService
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        Anwender LoadAnwender(int id);

        [OperationContract]
        void SaveAnwender(Anwender anwender);

   
    }

   
}

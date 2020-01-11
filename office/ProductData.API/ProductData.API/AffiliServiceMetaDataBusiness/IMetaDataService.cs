using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Affilinet.Business.ServiceMetaData
{
    [ServiceContract(Namespace = "http://affilinet.Service.MetaData")]
    public interface IMetaDataService
    {
        [OperationContract]
        Dictionary<String, String> RetrieveMetdaData();
    }
}

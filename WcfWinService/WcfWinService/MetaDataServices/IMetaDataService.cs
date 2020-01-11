using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Runtime.Serialization;

namespace WcfWinService.MetaDataServices
{
    [ServiceContract]
    public interface IMetaDataService
    {
        [OperationContract]
        ServiceMetadata WhatAreYouDoing();
    }
    [DataContract]
    public class ServiceMetadata
    {
        [DataMember]
        public long HealthCheckCounter = 0;
        [DataMember]
        public String LastStamp = "";
        [DataMember]
        public String LastMetaDataServiceFault = "";
    }
}

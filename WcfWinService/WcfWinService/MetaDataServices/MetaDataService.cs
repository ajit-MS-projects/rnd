using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcfWinService.MetaDataServices
{
    public class MetaDataService : IMetaDataService
    {
        public static ServiceMetadata objServiceMetadata = new ServiceMetadata();

        public ServiceMetadata WhatAreYouDoing()
        {
            ServiceMetadata objServiceMetadata1 = new ServiceMetadata();
            objServiceMetadata.HealthCheckCounter++;
            objServiceMetadata.LastMetaDataServiceFault = "aaaa777aaaaaa";
            return objServiceMetadata;
        }
    }
}

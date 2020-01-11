using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServices
{
    [ServiceContract]
    public interface IManufacturerService
    {

  
        [OperationContract]
        List<Manufacturer> GetAllManufacturers();

    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class Manufacturer
    {

        [DataMember]
        public int ManufId  {get;set;}

        [DataMember]
        public String ManufName { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public String Address { get; set; }
    }
}

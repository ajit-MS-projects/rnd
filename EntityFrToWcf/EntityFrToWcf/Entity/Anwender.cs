using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntityFrToWcf
{
    public interface IAnwender
    {
       // [DataMember]
        String AName { get; set; }

      //  [DataMember]
        Person Person { get; set; }

      //  [DataMember]
        List<BankInfo> BankInfos { get; set; }
    }

    [DataContract]
    public class Anwender : BasisObjekt, IAnwender
    {
        
        [DataMember]
        public String AName { get; set; }
        [DataMember]
        public Person Person { get; set; }
        [DataMember]
        public List<BankInfo> BankInfos { get; set; }
    }
}

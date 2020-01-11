using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntityFrToWcf
{
    public interface IBankInfo
    {
       // [DataMember]
        String BankName { get; set; }
    }

    [DataContract]
    public class BankInfo :BasisObjekt , IBankInfo
    {
        [DataMember]
        public String BankName { get; set; }
    }
}

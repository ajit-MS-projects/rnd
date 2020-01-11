using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntityFrToWcf
{
    public interface IPerson
    {
      //  [DataMember]
        String Name { get; set; }
    }

    [DataContract]
    public class Person : BasisObjekt, IPerson
    {

        [DataMember]
        public String Name { get; set; }
    }
}

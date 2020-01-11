using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Solr.Pvscout.Business.Entity
{
    [DataContract]
    public class PvModule
    {
        [DataMember]
        public PvModuleActual ObjPvModuleActual { get; set; }
        [DataMember]
        public PvModuleVirtual ObjPvModuleVirtual { get; set; }
        public PvModule(PvModuleActual objPvModuleActual)
        {
            ObjPvModuleActual = objPvModuleActual;
            ObjPvModuleVirtual = new PvModuleVirtual();
        }
    }
}


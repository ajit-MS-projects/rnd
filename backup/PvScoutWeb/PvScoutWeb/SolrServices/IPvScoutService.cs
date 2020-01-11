using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Solr.Pvscout.Business.Entity;
using Solr.Exceptions;
using Solr.Pvscout.Business;

namespace Solr.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPvScoutService" in both code and config file together.
    [ServiceContract]
    public interface IPvScoutService
    {
        [OperationContract]
        PvModule GetPvModulePosition(PvModuleActual objPvModuleActual);
    }
    //[DataContract]
    //public class PvModule
    //{
    //    [DataMember]
    //    public PvModuleActual ObjPvModuleActual { get; set; }
    //    [DataMember]
    //    public PvModuleVirtual ObjPvModuleVirtual { get; set; }
    //}
}

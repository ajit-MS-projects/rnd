using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Solar.Pvscout.Business.Entity;
using Solar.Exceptions;
using Solar.Pvscout.Business;

namespace Solar.Services
{
     //NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPvScoutService" in both code and config file together.
    [ServiceContract]
    public interface IPvScoutService
    {
    //    [OperationContract]
    //    PvModule GetPvModulePosition(PvModuleActual objPvModuleActual);
    //    [OperationContract]
    //    string Hello();
    //}
    //[DataContract]
    //public class PvModule
    //{
    //    [DataMember]
    //    public PvModuleActual ObjPvModuleActual { get; set; }
    //    [DataMember]
    //    public PvModuleVirtual ObjPvModuleVirtual { get; set; }
    }
}

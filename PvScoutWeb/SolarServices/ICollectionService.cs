using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Solar.Pvscout.Business.Entity;

namespace Solar.Services
{
    [ServiceContract]
    public interface ICollectionService
    {
        [OperationContract]
        IEnumerable<RoofType> GetAllRoofTypes();
        [OperationContract]
        RoofType GetRoofType(String roofId);
        [OperationContract]
        IEnumerable<PvModule> GetAllPvModules(String manufacturerId);
        [OperationContract]
        IEnumerable<Manufacturer> GetManufacturer();
        [OperationContract]
        PvModule GetPvModule(String pvscoutArticleNumber);
    }
}

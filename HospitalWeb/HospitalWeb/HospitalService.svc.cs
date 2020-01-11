using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace HospitalWeb
{
    public class HospitalService : DataService<HospitalContext>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("Patients", EntitySetRights.All);
            config.SetEntitySetAccessRule("LabResults", EntitySetRights.All);
            config.SetServiceOperationAccessRule("LoadPatient", ServiceOperationRights.AllRead);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }

        [WebGet]
        public IQueryable<Patient> LoadPatient(string name)
        {
            return CurrentDataSource.Patients.Where(p => p.Name.Contains(name));
        }
    }
}

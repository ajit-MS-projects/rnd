using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Solar.Data.Access;

namespace WcfServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class ManufacturerService : IManufacturerService
    {
        public List<Manufacturer> GetAllManufacturers()
        {
            SolarSqlCompactDataBase db = new SolarSqlCompactDataBase(DatabaseConnectionsEnum.PvScout);
            db.SetupCommand("select * from Manufacturers");
            DataTable dt = db.ExecuteReaderProcessed();
            List<Manufacturer> retVal = new List<Manufacturer>();
            foreach (DataRow dr in dt.Rows)
            {
                Manufacturer mf = new Manufacturer();
                mf.ManufId = int.Parse(dr["manufid"].ToString());
                mf.ManufName = dr["ManufName"].ToString();
                mf.Description = dr["Description"].ToString();
                mf.Address = dr["Address"].ToString();
                retVal.Add(mf);
            }
      

                return retVal;
        }
    }
}

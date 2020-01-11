using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Solar.Exceptions;
using Solar.Pvscout.Business.Common;
using Solar.Pvscout.Business.DAO;
using Solar.Pvscout.Business.Entity;

namespace Solar.Pvscout.Business
{
    public class Collections
    {
        public IEnumerable<RoofType> GetAllRoofTypes()
        {
            try
            {
                List<RoofType> roofTypes = new List<RoofType>();
                PvscoutDao objPvscoutDao = new PvscoutDao();
                DataTable dtRoofs = objPvscoutDao.GetAllRoofTypes();
                foreach (DataRow dr in dtRoofs.Rows)
                {
                    RoofType roofType = new RoofType(dr);
                    roofTypes.Add(roofType);
                }

                return roofTypes;
            }
            catch (Exception ex)
            {
                SolarGenericException sEx = new SolarGenericException("Excpetion in Collections.GeAlltRoofTypes()::", ex);
                sEx.CreateLog();
                throw sEx;
            }
        }
        public RoofType GetRoofType(String roofId)
         {
             try
             {
                 RoofType roofType = null;
                 PvscoutDao objPvscoutDao = new PvscoutDao();
                 DataTable dtRoofs = objPvscoutDao.GetRoofType(roofId);
                 if (dtRoofs.Rows.Count > 0)
                 {
                     roofType = new RoofType(dtRoofs.Rows[0]);
                 }
                 return roofType;
             }
             catch (Exception ex)
             {
                 SolarGenericException sEx = new SolarGenericException("Excpetion in Collections.GetRoofType(String roofId)::", ex);
                 sEx.CreateLog();
                 throw sEx;
             }
         }
        public IEnumerable<Manufacturer> GetManufacturers()
        {
            try
            {
                List<Manufacturer> roofTypes = new List<Manufacturer>();
                PvscoutDao objPvscoutDao = new PvscoutDao();
                DataTable dtRoofs = objPvscoutDao.GetManufacturers();
                foreach (DataRow dr in dtRoofs.Rows)
                {
                    Manufacturer roofType = new Manufacturer(dr);
                    roofTypes.Add(roofType);
                }

                return roofTypes;
            }
            catch (Exception ex)
            {
                SolarGenericException sEx = new SolarGenericException("Excpetion in Collections.GetManufacturers()::", ex);
                sEx.CreateLog();
                throw sEx;
            }
        }
        public IEnumerable<PvModule> GetAllPvModules(String manufacturerId)
        {
            try
            {
                List<PvModule> pvModules = new List<PvModule>();
                PvscoutDao objPvscoutDao = new PvscoutDao();
                DataTable dtRoofs = objPvscoutDao.GetAllPvModules(manufacturerId);
                foreach (DataRow dr in dtRoofs.Rows)
                {
                    PvModule pvModule = new PvModule(dr);
                    pvModules.Add(pvModule);
                }

                return pvModules;
            }
            catch (Exception ex)
            {
                SolarGenericException sEx = new SolarGenericException("Excpetion in Collections.GetAllPvModules(String manufacturerId)::", ex);
                sEx.CreateLog();
                throw sEx;
            }
        }
        public PvModule GetPvModule(String pvscoutArticleNumber)
        {
            try
            {
                PvModule pvModule = null;
                PvscoutDao objPvscoutDao = new PvscoutDao();
                DataTable dtPvModule = objPvscoutDao.GetPvModule(pvscoutArticleNumber);
                if (dtPvModule.Rows.Count > 0) pvModule = new PvModule(dtPvModule.Rows[0]);

                return pvModule;
            }
            catch (Exception ex)
            {
                SolarGenericException sEx = new SolarGenericException("Excpetion in Collections.GetPvModule(String pvscoutArticleNumber)::", ex);
                sEx.CreateLog();
                throw sEx;
            }
        }
    }
}

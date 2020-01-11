using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Solar.Exceptions;
using Solar.Pvscout.Business;
using Solar.Pvscout.Business.Entity;

namespace Solar.Services
{
    public class CollectionService : ICollectionService
    {
        public IEnumerable<RoofType> GetAllRoofTypes()
        {
            try
            {
                return new Collections().GetAllRoofTypes();
            }
            catch(SolarBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                SolarGenericException sEx = new SolarGenericException("Error in CollectionService.GeAllRoofTypes()",ex);
                sEx.CreateLog();
                throw sEx;
            }   
        }
        public RoofType GetRoofType(String roofId)
        {
            try
            {
                return new Collections().GetRoofType(roofId);
            }
            catch (SolarBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                SolarGenericException sEx = new SolarGenericException("Error in CollectionService.GetRoofTypes(int roofId)", ex);
                sEx.CreateLog();
                throw sEx;
            }
        }

        public IEnumerable<PvModule> GetAllPvModules(String manufacturerId)
        {
            try
            {
                return new Collections().GetAllPvModules(manufacturerId);
            }
            catch (SolarBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                SolarGenericException sEx = new SolarGenericException("Error in CollectionService.GetAllPvModules()", ex);
                sEx.CreateLog();
                throw sEx;
            } 
        }

        public IEnumerable<Manufacturer> GetManufacturer()
        {
            try
            {
                return new Collections().GetManufacturers();
            }
            catch (SolarBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                SolarGenericException sEx = new SolarGenericException("Error in CollectionService.GetManufacturer()", ex);
                sEx.CreateLog();
                throw sEx;
            } 
        }

        public PvModule GetPvModule(String pvscoutArticleNumber)
        {
            try
            {
                return new Collections().GetPvModule(pvscoutArticleNumber);
            }
            catch (SolarBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                SolarGenericException sEx = new SolarGenericException("Error in CollectionService.GetPvModule(String pvscoutArticleNumber)", ex);
                sEx.CreateLog();
                throw sEx;
            } 
        }


    }
}

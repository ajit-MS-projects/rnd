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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PvScoutService" in code, svc and config file together.
    public class PvScoutService : IPvScoutService
    {
        public PvModule GetPvModulePosition(PvModuleActual objPvModuleActual)
        {
            if (objPvModuleActual == null)
            {
                throw new SolrInvalidInputException("PvModuleActual object can not be null in call to PvScoutService.GetPvModulePosition()");
            }

            return new ObjectPostionCalculator().GetPvModulePosition(objPvModuleActual);
        }
    }
}

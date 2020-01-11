using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using  Solr.Pvscout.Business.Entity;

namespace Solr.Pvscout.Business
{
    public class ObjectPostionCalculator
    {
        public PvModule GetPvModulePosition(PvModuleActual objPvModuleActual)
        {
            PvModule objPvModule = new PvModule(objPvModuleActual);
            objPvModule.ObjPvModuleVirtual.Width = objPvModuleActual.Width + 10;  //todo implemenet menthod and delete test code
            objPvModule.ObjPvModuleVirtual.Height = objPvModuleActual.Height + 10;

            return objPvModule;
        }
    }
}

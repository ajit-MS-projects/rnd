using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Solar.Pvscout.Business.Common;

namespace Solar.Pvscout.Business.Entity
{
    [DataContract]
    public class PvModule
    {
        [DataMember]
        public PvModuleActual ObjPvModuleActual { get; set; }
        [DataMember]
        public PvModuleVirtual ObjPvModuleVirtual { get; set; }
        public PvModule(DataRow dr):this()
        {
            ObjPvModuleActual.Length = double.Parse(dr[Constants.PvModules.Length].ToString());
            ObjPvModuleActual.Width = double.Parse(dr[Constants.PvModules.Width].ToString());
            ObjPvModuleActual.Weight = float.Parse(dr[Constants.PvModules.Weight].ToString());
            ObjPvModuleActual.ImagePath = dr[Constants.PvModules.ImagePath].ToString();
            ObjPvModuleActual.ManufacturerId = dr[Constants.PvModules.ManufId].ToString();
            ObjPvModuleActual.PvScoutArticleNumber = dr[Constants.PvModules.PVscoutArticleNumber].ToString();
            ObjPvModuleActual.Thickness = int.Parse(dr[Constants.PvModules.Thickness].ToString());
            ObjPvModuleActual.ArticleNumber = dr[Constants.PvModules.ArticleNumber].ToString();
            ObjPvModuleActual.CellMaterial = dr[Constants.PvModules.CellMaterial].ToString();
            ObjPvModuleActual.CellTechnology = dr[Constants.PvModules.CellTechnology].ToString();
            //ObjPvModuleActual.Top todo: calculate
            //ObjPvModuleActual.Left 
            //ObjPvModuleVirtual.width todo: calculate
        }
        public PvModule(PvModuleActual objPvModuleActual)
        {
            ObjPvModuleActual = objPvModuleActual;
            ObjPvModuleVirtual = new PvModuleVirtual();
        }
        public PvModule():this(new PvModuleActual())
        {
        }
    }
}


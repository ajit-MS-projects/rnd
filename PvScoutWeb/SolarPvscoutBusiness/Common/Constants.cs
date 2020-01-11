using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solar.Pvscout.Business.Common
{
    public class Constants
    {
        public struct ReadOnlyStoredProcs
        {
            public const string GetRoofTypes = "GetRoofTypes";
            public const string GetPvModules = "GetPvModules";
            public const string GetManufacturers = "GetManufacturers";
        }
        public struct DmlStoredProcs
        {
     
        }
        public struct AppSettings
        {
        }
        #region tables
        public struct RoofTypes
        {
            public const string RoofTypeId = "RoofTypeId";
            public const string IsDefault = "IsDefault";
            public const string IsSelected = "IsSelected";
            public const string ImagePath = "ImagePath";
            public const string DisplayName = "DisplayName";
        }
        public struct PvModules
        {
            public const string PVscoutArticleNumber = "PVscoutArticleNumber";
            public const string ArticleNumber = "ArticleNumber";
            public const string ManufId = "ManufId";
            public const string ImagePath = "ImagePath";
            public const string Length = "Length";
            public const string Width = "Width";
            public const string Thickness = "Thickness";
            public const string Weight = "Weight";
            public const string CellTechnology = "CellTechnology";
            public const string CellMaterial = "CellMaterial";
        }
        #endregion
    }
}

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
    public class RoofType
    {
        #region Public Properties
        [DataMember]
        public int RoofTypeId{ get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public bool IsDefault { get; set; }
        [DataMember]
        public bool IsSelected { get; set; }
        [DataMember]
        public string ImagePath { get; set; }
        #endregion

        #region Constructors
        public RoofType(DataRow dr)
        {
            RoofTypeId = int.Parse(dr[Constants.RoofTypes.RoofTypeId].ToString());
            IsDefault = bool.Parse(dr[Constants.RoofTypes.IsDefault].ToString());
            IsSelected = bool.Parse(dr[Constants.RoofTypes.IsSelected].ToString());
            ImagePath = dr[Constants.RoofTypes.ImagePath].ToString();
            DisplayName = dr[Constants.RoofTypes.DisplayName].ToString();
        }
        public RoofType()
        {
            RoofTypeId = 0;
            this.DisplayName = "";           
            this.IsDefault = false;
            this.IsSelected = false;
            this.ImagePath = "";
        }
        #endregion
 }
}

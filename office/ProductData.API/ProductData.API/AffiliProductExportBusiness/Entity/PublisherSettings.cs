using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Affilinet.Business.ProductExport.Entity
{
    public class PublisherSettings
    {
        public String DeciSeperator { get; set; }
        public String FieldSeperator { get; set; }
        public String FieldQualifier  { get; set; }
        public bool ReplaceEmptyWithNull { get; set; }
        public bool ImageWithSizeOnly { get; set; }
        public List<String> Columns  { get; set; }
        public String DateFormat{ get; set; }
        public PublisherSettings()
        {
            Columns= new List<String>();
        }
    }
}

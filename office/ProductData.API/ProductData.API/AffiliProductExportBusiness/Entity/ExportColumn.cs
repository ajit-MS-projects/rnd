using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Affilinet.Business.ProductExport.Entity
{
    public class ExportColumn
    {
        public String ProductTableColumn { get; set;} 
        public String ExportCsvColumn{ get; set;} 
        public String ExportXmlColumn{ get; set;} 
        public String XmlGroup{ get; set;} 
        public List<String> ComputedColumns{ get; set;} 
        public List<String> ColumnsDefault{ get; set;} 
        public String ColumnValue{ get; set;}
        public bool IsCalculated { get; set; }
        public bool IsDefault { get; set; } 
        public bool IsActive{ get; set;}
        public int FieldLength{ get; set; } 
        public String DataType { get; set; }
        public ExportColumn()
        {
            ComputedColumns = new List<string>();
            ColumnsDefault = new List<string>();
        }
    }
}

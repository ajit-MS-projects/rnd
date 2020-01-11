using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Affilinet.Business.ProductExport.Common;

namespace Affilinet.Business.ProductExport.Entity
{
    public class ExportAttributes
    {
        public String PublisherId { get; set; }
        public PublisherSettings PubSettings { get; set; }
        public ProgramFilesList ProgramFiles { get; set; }
        public ProgramSettings ProgSettings { get; set; }
        public HttpContext HttpContext { get; set; }
        public Dictionary<int, String> PlateFormClickUrls { get; set; }
        public List<String> XmlTagGroups { get; set; }
        public List<ExportColumn> ExportColumns { get; set; }

        public ExportAttributes(String publisherId)
        {
            this.PublisherId = publisherId;
            PlateFormClickUrls = new Dictionary<int, String>();
            ExportColumns = new List<ExportColumn>();
            XmlTagGroups= new List<String>();
        }
    }
}

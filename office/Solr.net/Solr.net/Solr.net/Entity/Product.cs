using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SolrNet.Attributes;

namespace Solr.net.Entity
{
    public class Product
    {
        [SolrUniqueKey("id")]
        public int ProductId { get; internal set; }

        public string FileLocation { get; internal set; }

        [SolrField("title")]
        public string Title{ get; internal set; }
    }
}

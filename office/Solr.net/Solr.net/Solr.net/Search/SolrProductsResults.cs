using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolrNet.Attributes;

namespace Solr.net.Search
{
    public class SolrProductsResults
    {
        [SolrField("id")]
        public int ProductId { get; set; }
    }
}

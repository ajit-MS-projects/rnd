using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solr.net.Entity;

namespace Solr.net.Search
{
    public class ProductSearchResults
    {
        public IEnumerable<Product> Results { get; set; }
        public int QueryTime { get; set; }
        public int TotalResults { get; set; }
    }
}

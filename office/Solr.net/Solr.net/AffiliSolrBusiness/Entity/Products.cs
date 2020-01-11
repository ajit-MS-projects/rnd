using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AffiliSolrBusiness.Entity
{
    public class Products
    {
        public IEnumerable<Product> Results { get; set; }
        public int QueryTime { get; set; }
        public int TotalResults { get; set; }
    }
}

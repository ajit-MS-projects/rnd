using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolrNet.Attributes;

namespace AffiliSolrBusiness.Entity.Search
{
    public class SolrResultProduct
    {
        [SolrUniqueKey("ID")]
        public int ProductId { get; set; }
        [SolrField("ProductProgramId")]
        public int ProductProgramId { get; set; }
        // public String ArticlelNumber { get; set; }
    }
}

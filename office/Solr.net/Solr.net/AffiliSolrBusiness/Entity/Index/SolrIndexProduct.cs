using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolrNet.Attributes;

namespace AffiliSolrBusiness.Entity.Index
{
    public class SolrIndexProduct
    {
        [SolrUniqueKey("ID")]
        public int ID { get; set; }
        [SolrField("ArtikelNumber")]
        public String ArtikelNumber { get; set; }
        [SolrField("ProductProgramId")]
        public int ProductProgramId { get; set; }
        [SolrField("Title")]
        public String Title { get; set; }
        [SolrField("Description")]
        public String Description { get; set; }
        [SolrField("Price")]
        public decimal Price { get; set; }
        [SolrField("Shipping")]
        public decimal Shipping { get; set; }
    }
}

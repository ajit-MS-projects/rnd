using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolrNet;
using AffiliSolrBusiness.Entity.Search;

namespace AffiliSolrBusiness.CutomExtensions
{
    public static class Extensions
    {
        public static string ToDelimetedString(this IEnumerable<int> obj)
        {
            string[] array = obj.Where(n => n != null).Select(n => n.ToString()).ToArray();

            return string.Join(",", array);

        }
        public static string ToDelimetedString(this ISolrQueryResults<SolrResultProduct> obj)
        {
            string[] array = obj.Where(n => n != null).Select(n => n.ProductId.ToString()).ToArray();

            return string.Join(",", array);

        }
    }
}

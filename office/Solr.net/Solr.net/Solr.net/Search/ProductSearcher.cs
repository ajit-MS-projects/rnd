using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solr.net.Entity;
using Solr.net.SolrNetLayer;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace Solr.net.Search
{
    public class ProductSearcher
    {
        //private string connectionString;
        private string solrUrl;

        public ProductSearcher(string connectionString, string solrUrl)
        {
           // this.connectionString = connectionString;
            this.solrUrl = solrUrl;
        }

        public ProductSearchResults Search(string query, int resultsPerPage, int pageNumber)
        {
            var solrWorker = SolrOperationsCache<SolrProductsResults>.GetSolrOperations(this.solrUrl);

            QueryOptions options = new QueryOptions
            {
                Rows = resultsPerPage,
                Start = (pageNumber - 1) * resultsPerPage,
            };

            ISolrQueryResults<SolrProductsResults> results = solrWorker.Query(query, options);
            //var textFiles = new TextFileRepository(this.connectionString)
            //    .GetTextFiles(results.Select(r => r.FileID));
            //todo goto db & get products

            List<Product> products = new List<Product>();
            foreach (SolrProductsResults pr in results)
            {
                products.Add(new Product(){ProductId = pr.ProductId});
            }


            var searchResults = new ProductSearchResults
            {
                Results = products,
                QueryTime = results.Header.QTime,
                TotalResults = results.NumFound
            };

            return searchResults;
        }
    }
}

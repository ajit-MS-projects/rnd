using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AffiliSolrBusiness.Entity;
using AffiliSolrBusiness.Entity.Search;
using SolrNet;
using SolrNet.Commands.Parameters;
using Solr.net.SolrNetLayer;
using AffiliSolrBusiness.Common;
using System.Data;
using AffiliSolrBusiness.DAO;
using AffiliSolrBusiness.CutomExtensions;

namespace AffiliSolrBusiness.Searcher
{
    public class SolrProductSearcher
    {
         private string solrUrl;

         public SolrProductSearcher(string solrUrl)
         {
             this.solrUrl = solrUrl;
         }

        public Products Search(string query, int resultsPerPage, int pageNumber)
        {
            var solrWorker = SolrOperationsCache<SolrResultProduct>.GetSolrOperations(this.solrUrl);

            QueryOptions options = new QueryOptions
            {
                Rows = resultsPerPage,
                Start = (pageNumber - 1) * resultsPerPage,
            };

            ISolrQueryResults<SolrResultProduct> results = solrWorker.Query(query, options);//get data from solr

            ProductDao objProductDao = new ProductDao();
            StringBuilder productIds = new StringBuilder();

            DataTable dtProducts = objProductDao.GetProducts(results.ToDelimetedString());//get data from product DB


            DataTable dtProductSchema = objProductDao.GetProductSchema();
            List<Product> lstProducts = new List<Product>();
            foreach (SolrResultProduct pr in results)
            {
                Product dbProduct = new Product(dtProductSchema, 0);
                dbProduct.SetField(Constants.Product.ID, pr.ProductId);
                dbProduct.SetField(Constants.Product.ProductProgramID, pr.ProductProgramId);

                DataRow[] drs = dtProducts.Select(Constants.Product.ID + "=" + pr.ProductId);
                if (drs != null)
                {
                    foreach (DataColumn dc in dtProducts.Columns)
                    {
                        dbProduct.SetField(dc.ColumnName, drs[0][dc.ColumnName].ToString());
                    }
                }
                lstProducts.Add(dbProduct);
            }


            var searchResults = new Products
            {
                Results = lstProducts,
                QueryTime = results.Header.QTime,
                TotalResults = results.NumFound
            };

            return searchResults;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AffiliSolrBusiness.Entity;
using Microsoft.Practices.ServiceLocation;
using Solr.net.FileReaders;
using SolrNet;
using AffiliSolrBusiness.Entity.Index;

namespace AffiliSolrBusiness.Indexer
{
    public class SolrProductIndexer
    {
          private string productsFile;
        private string solrUrl;

        public SolrProductIndexer(string productsFile, string solrUrl)
        {
            this.productsFile = productsFile;
            this.solrUrl = solrUrl;
        }

        public void IndexFiles()
        {
            Startup.Init<SolrIndexProduct>(this.solrUrl);//todo call only once, causing exception
            var solrWorker = ServiceLocator.Current.GetInstance<ISolrOperations<SolrIndexProduct>>();
            IEnumerable<SolrIndexProduct> files = new CsvReader(productsFile).GetProducts();
            solrWorker.Add(files).Commit();
        }
    }
}

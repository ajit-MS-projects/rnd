using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Solr.net.Entity;
using Solr.net.FileReaders;
using SolrNet;

namespace Solr.net.Indexer
{
    public class ProductIndexer
    {
         private string productsFile;
        private string solrUrl;

        public ProductIndexer(string productsFile, string solrUrl)
        {
            this.productsFile = productsFile;
            this.solrUrl = solrUrl;
        }

        public void IndexFiles()
        {
            Startup.Init<Product>(this.solrUrl);//todo call only once
            var solrWorker = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();
            var files = new CsvReader(productsFile).GetProducts();
            solrWorker.Add(files).Commit();
        }
    }
}

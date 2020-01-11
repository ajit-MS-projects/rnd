using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolrNet.Impl;
using Microsoft.Practices.ServiceLocation;
using Solr.net.DBLayer;
using SolrNet;
using SolrNet.Commands;

namespace Solr.net.Indexer
{
    public class BasicIndexer
    {
        private string connectionString;
        private string solrUrl;

        public BasicIndexer(string connectionString, string solrUrl)
        {
            this.connectionString = connectionString;
            this.solrUrl = solrUrl;
        }

        public void IndexFiles()
        {
            Startup.Init<TextFile>(this.solrUrl);
            var solrWorker = ServiceLocator.Current.GetInstance<ISolrOperations<TextFile>>();
            var files = new TextFileRepository(this.connectionString).GetTextFiles();
            solrWorker.Add(files).Commit();
        }
    }
}

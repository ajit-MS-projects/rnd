using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolrNet;
using SolrNet.Impl;
using SolrNet.Commands.Parameters;
using Solr.net.SolrNetLayer;
using Solr.net.DBLayer;

namespace Solr.net.Search
{
    public class BasicSearcher
    {
        private string connectionString;
        private string solrUrl;

        public BasicSearcher(string connectionString, string solrUrl)
        {
            this.connectionString = connectionString;
            this.solrUrl = solrUrl;
        }

        public SearchResults Search(string query, int resultsPerPage, int pageNumber)
        {
            var solrWorker = SolrOperationsCache<FileIDResult>.GetSolrOperations(this.solrUrl);

            QueryOptions options = new QueryOptions
            {
                Rows = resultsPerPage,
                Start = (pageNumber - 1) * resultsPerPage,
            };

            ISolrQueryResults<FileIDResult> results = solrWorker.Query(query, options);
            var textFiles = new TextFileRepository(this.connectionString)
                .GetTextFiles(results.Select(r => r.FileID));


            var searchResults = new SearchResults
            {
                Results = textFiles,
                QueryTime = results.Header.QTime,
                TotalResults = results.NumFound
            };

            return searchResults;
        }
    }
}

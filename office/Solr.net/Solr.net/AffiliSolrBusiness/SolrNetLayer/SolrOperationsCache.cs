using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolrNet.Impl;
using SolrNet.Commands.Parameters;
using SolrNet;
using Microsoft.Practices.ServiceLocation;

namespace Solr.net.SolrNetLayer
{
    internal static class SolrOperationsCache<T>
       where T : new()
    {
        private static ISolrOperations<T> solrOperations;

        public static ISolrOperations<T> GetSolrOperations(string solrUrl)
        {
            if (solrOperations == null)
            {
                Startup.Init<T>(solrUrl);
                solrOperations = ServiceLocator.Current.GetInstance<ISolrOperations<T>>();
            }

            return solrOperations;
        }

    }
}

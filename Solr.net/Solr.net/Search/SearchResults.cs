using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solr.net.Search
{
    public class SearchResults
    {
        public IEnumerable<TextFile> Results { get; set; }
        public int QueryTime { get; set; }
        public int TotalResults { get; set; }
    }
}

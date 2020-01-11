using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolrNet.Attributes;

namespace Solr.net.Search
{
    internal class FileIDResult
    {
        [SolrField("fileid")]
        public int FileID { get; set; }
    }
}

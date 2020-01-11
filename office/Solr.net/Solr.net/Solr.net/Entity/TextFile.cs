using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolrNet.Attributes;
using System.IO;

namespace Solr.net
{
    public class TextFile
    {
        #region Members

        private string documentText;

        #endregion

        [SolrUniqueKey("fileid")]
        public int FileID { get; internal set; }

        public string FileLocation { get; internal set; }

        [SolrField("doctext")]
        public string DocumentText
        {
            get
            {
                if (this.documentText == null)
                {
                    this.documentText = File.ReadAllText(FileLocation);
                }
                return this.documentText;
            }

        }

        [SolrField("title")]
        public string Title { get; internal set; }

        [SolrField("datecreated")]
        public DateTime? DateCreated { get; internal set; }
    }
}

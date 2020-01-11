using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Solr.net.Indexer;
using Solr.net.Search;

namespace Solr.net
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string connectionString = @"Data Source=AJIT-PC\sqlexpress;Initial Catalog=solrDB;Integrated Security=True;Pooling=False";
        string solrUrl = "http://localhost:8983/solr/";
        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtResult.Text = "";
            BasicSearcher searcher = new BasicSearcher(connectionString, solrUrl);
            var results = searcher.Search(txtSearch.Text == "" ? "*:*" : txtSearch.Text, 10, 1);
            foreach (TextFile file in results.Results)
            {
               txtResult.Text +=  string.Format("FileID: {0}, Title: {1}", file.FileID, file.Title) + Environment.NewLine;
            }
        }

        private void btnIndex_Click(object sender, EventArgs e)
        {
            BasicIndexer indexer = new BasicIndexer(connectionString, solrUrl);
            indexer.IndexFiles();
        }
    }
}

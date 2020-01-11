using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Solr.net.Entity;
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
        string connectionString = @"Persist Security Info=False;server=ebes06;database=ProductExportDB;User ID=ProductASP;pwd=psatcudorp;";
        string solrUrl = "http://vebes04:8983/solr/";
        private string productFile = @"c:\files\Products.txt";
        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtResult.Text = "";
            BasicSearcher searcher = new BasicSearcher(connectionString, solrUrl);
            var results = searcher.Search(txtSearch.Text == "" ? "*:*" : txtSearch.Text, 10, 1);
            foreach (TextFile file in results.Results)
            {
                txtResult.Text += string.Format("FileID: {0}, Title: {1}", file.FileID, file.Title) + Environment.NewLine;
            }
        }

        private void btnIndex_Click(object sender, EventArgs e)
        {
            BasicIndexer indexer = new BasicIndexer(connectionString, solrUrl);
            indexer.IndexFiles();
        }

        private void btnSearchProducts_Click(object sender, EventArgs e)
        {
            txtResult.Text = "";
            ProductSearcher searcher = new ProductSearcher(connectionString, solrUrl);
            var results = searcher.Search(txtSearch.Text == "" ? "*:*" : txtSearch.Text, 10, 1);
            foreach (Product pr in results.Results)
            {
                txtResult.Text += string.Format("Product Id: {0}, Title: {1}", pr.ProductId, pr.Title) +
                                  Environment.NewLine;
            }
        }

        private void btnIndexProducts_Click(object sender, EventArgs e)
        {
            ProductIndexer indexer = new ProductIndexer(productFile, solrUrl);
            indexer.IndexFiles();
        }
    }
}

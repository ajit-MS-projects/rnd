using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AffiliSolrBusiness.Indexer;
using AffiliSolrBusiness.Searcher;
using AffiliSolrBusiness.Entity;
using AffiliSolrBusiness.Common;

namespace Solr.net
{
    public partial class ProductForm : Form
    {
        string solrUrl = "http://localhost:8983/solr/";
        private string productFile = @"D:\ajit\RND\Products.csv";
        public ProductForm()
        {
            InitializeComponent();
        }

        private void btnSearchProducts_Click(object sender, EventArgs e)
        {
            txtResult.Text = "";
            SolrProductSearcher searcher = new SolrProductSearcher(solrUrl);
            Products objProducts = searcher.Search(txtSearch.Text == "" ? "*:*" : txtSearch.Text, 10, 1);
            foreach (Product dbProduct in objProducts.Results)
            {
                txtResult.Text += string.Format("Product Id: {0}, Product Program Id: {1}, Title:{2}", dbProduct.GetField(Constants.Product.ID), dbProduct.GetField(Constants.Product.ProductProgramID), dbProduct.GetField(Constants.Product.Title)) +
                                  Environment.NewLine;
            }
        }

        private void btnIndexProducts_Click(object sender, EventArgs e)
        {
            SolrProductIndexer indexer = new SolrProductIndexer(productFile, solrUrl);
            indexer.IndexFiles();
        }
    }
}

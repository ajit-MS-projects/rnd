using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AffiliSolrBusiness.Entity;
using AffiliSolrBusiness.Entity.Index;

namespace Solr.net.FileReaders
{
    public class CsvReader
    {
        private String productsFile { get; set; }
        public CsvReader(String productsFile)
        {
            this.productsFile = productsFile;
        }
        public IEnumerable<SolrIndexProduct> GetProducts()
        {
            StreamReader srFile = new StreamReader(productsFile);
            List<SolrIndexProduct> lst = new List<SolrIndexProduct>();
            SolrIndexProduct prd = null;
            string line = null;
            srFile.ReadLine();
            while ((line = srFile.ReadLine()) != null)
            {
                String[] prLine = line.Split(new string[] { "~,~" }, StringSplitOptions.RemoveEmptyEntries);
                prd=new SolrIndexProduct(); 
                prd.ID = int.Parse(prLine[0].Replace("~",""));
                prd.ArtikelNumber = prLine[1].Replace("~", "");
                prd.ProductProgramId = int.Parse(prLine[2].Replace("~", ""));
                prd.Title = prLine[3].Replace("~", "");
                prd.Description = prLine[4].Replace("~", "");
                prd.Price = decimal.Parse(prLine[5].Replace("~", ""));
                prd.Shipping = decimal.Parse(prLine[6].Replace("~", ""));
                lst.Add(prd);
            }

            return lst;
        }
    }
}

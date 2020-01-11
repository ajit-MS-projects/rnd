using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Solr.net.Entity;

namespace Solr.net.FileReaders
{
    public class CsvReader
    {
        private String productsFile { get; set; }
        public CsvReader(String productsFile)
        {
            this.productsFile = productsFile;
        }
        public IEnumerable<Product> GetProducts()
        {
            StreamReader srFile = new StreamReader(productsFile);                      
            List<Product> lst = new List<Product>();
            Product prd = null;
            string line = null;
            while ((line = srFile.ReadLine()) != null)
            {
                String[] prLine = line.Split(';');
                prd=new Product();
                prd.ProductId = int.Parse(prLine[0]);
                prd.Title = prLine[1];
                lst.Add(prd);
            }

            return lst;
        }
    }
}

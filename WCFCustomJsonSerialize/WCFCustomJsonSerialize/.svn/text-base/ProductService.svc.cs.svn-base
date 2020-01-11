using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFCustomJsonSerialize
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProductService" in code, svc and config file together.
    public class ProductService : IProductService  , IProductServiceXml, IProductServiceJson
    {

        public ProductResponse SearchProducts(ProductRequest request)
        {

            ProductResponse retVal = new ProductResponse();
            retVal.Products= new List<Product>();
            Product prod = new Product();
            prod.ID=1111;
            prod.Description="Ipod bag 1 description";
            prod.Title="Ipod bag 1";
            prod.Price=20;
            prod.ProductType = ProductTypeEnum.ConsumerGood;
            retVal.Products.Add(prod);
            prod = new Product();
            prod.ID=2222;
            prod.Description="Ipod bag 2 description";
            prod.Title="Ipod bag 2";
            prod.Price=22;
            prod.ProductType = ProductTypeEnum.Hotel;
            retVal.Products.Add(prod);

            return retVal;
        }

        public ProductWebResponse SearchProducts(string query)
        {
            ProductRequest request = new ProductRequest();
            request.Query = query;
            return GetProductWebResponse(SearchProducts(request));
        }

        private ProductWebResponse GetProductWebResponse(ProductResponse productResponse)
        {
            ProductWebResponse retVal = new ProductWebResponse();
            retVal.Products = new List<Product>();
            foreach (Product p in productResponse.Products)
            {
                retVal.Products.Add(p);       
            }

            return retVal;
        }
    }
}

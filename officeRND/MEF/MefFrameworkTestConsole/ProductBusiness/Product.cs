using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using ProductContracts.Models;
using ProductContracts.Services;

namespace ProductBusiness
{
	[Export(typeof(IProduct))]
	public class Product : IProduct
	{
		public int Id { get; set; }
		public String ArticleNumber { get; set; }
		public String Title { get; set; }
		public double Price { get; set; }
		public double Shipping { get; set; }
	}

	[Export(typeof(IProductService))]
	public class ProductService : IProductService
	{
		public IProduct GetProduct(int id)
		{
			return new Product() {Id = 1,ArticleNumber = "A1234",Price = 10.15,Shipping = 2,Title="LG LCD"};
		}

		public List<IProduct> GetProducts()
		{
			var products = new List<IProduct>
			                          	{
			                          		new Product()
			                          			{Id = 1, ArticleNumber = "A1234", Price = 1000.15, Shipping = 52, Title = "LG LCD"},
			                          		new Product()
			                          			{Id = 2, ArticleNumber = "A234", Price = 1001.15, Shipping = 32, Title = "Sony LCD"},
			                          		new Product()
			                          			{Id = 3, ArticleNumber = "A34", Price = 1002.15, Shipping = 22, Title = "Samsung LCD"},
			                          		new Product()
			                          			{Id = 4, ArticleNumber = "A4", Price = 1003.15, Shipping = 21, Title = "Toshiba LCD"},
			                          		new Product()
			                          			{Id = 5, ArticleNumber = "A5", Price = 1004.15, Shipping = 20, Title = "Bla LCD"}
			                          	};

			return products;
		}
	}
}

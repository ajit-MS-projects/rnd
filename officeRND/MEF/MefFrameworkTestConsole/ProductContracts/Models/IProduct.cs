using System;

namespace ProductContracts.Models
{
	public interface IProduct
	{
		int Id { get; set; }
		String ArticleNumber { get; set; }
		String Title { get; set; }
		double Price { get; set; }
		double Shipping { get; set; }
	}
}
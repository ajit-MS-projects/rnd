using System.Collections.Generic;
using ProductContracts.Models;

namespace ProductContracts.Services
{
	public interface IProductService
	{
		IProduct GetProduct(int id);
		List<IProduct> GetProducts();
	}
}
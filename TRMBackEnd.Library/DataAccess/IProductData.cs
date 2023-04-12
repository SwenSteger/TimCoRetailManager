using System.Collections.Generic;
using TRMBackEnd.Library.Models;

namespace TRMBackEnd.Library.DataAccess
{
	public interface IProductData
	{
		List<ProductModel> GetProducts();
		ProductModel GetProductById(int productId);
	}
}
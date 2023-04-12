using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using TRMBackEnd.Library.Internal.DataAccess;
using TRMBackEnd.Library.Models;

namespace TRMBackEnd.Library.DataAccess
{
	public class ProductData : IProductData
	{
		private readonly ISqlDataAccess _sqlDataAccess;

		public ProductData(ISqlDataAccess sqlDataAccess)
		{
			_sqlDataAccess = sqlDataAccess;
		}

		public List<ProductModel> GetProducts() 
			=> _sqlDataAccess
				.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TRMData");

		public ProductModel GetProductById(int productId) 
			=> _sqlDataAccess
				.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = productId }, "TRMData").FirstOrDefault();
	}
}
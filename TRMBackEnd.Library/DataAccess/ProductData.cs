using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using TRMBackEnd.Library.Internal.DataAccess;
using TRMBackEnd.Library.Models;

namespace TRMBackEnd.Library.DataAccess
{
	public class ProductData
	{
		private readonly IConfiguration _config;

		public ProductData(IConfiguration config)
		{
			_config = config;
		}

		public List<ProductModel> GetProducts()
		{
			var sql = new SqlDataAccess(_config);
			var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TRMData");

			return output;
		}

		public ProductModel GetProductById(int productId)
		{
			var sql = new SqlDataAccess(_config);
			var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = productId }, "TRMData").FirstOrDefault();
			return output;
		}
	}
}
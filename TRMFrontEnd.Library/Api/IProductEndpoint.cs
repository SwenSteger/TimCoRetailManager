using System.Collections.Generic;
using System.Threading.Tasks;
using TRMFrontEnd.Library.Models;

namespace TRMFrontEnd.Library.Api
{
	public interface IProductEndpoint
	{
		Task<List<ProductModel>> GetAll();
	}
}
using System.Threading.Tasks;
using TRMFrontEnd.Library.Models;

namespace TRMFrontEnd.Library.Api
{
	public interface ISaleEndpoint
	{
		Task PostSale(SaleModel sale);
	}
}
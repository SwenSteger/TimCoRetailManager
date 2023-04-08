using System.Collections.Generic;
using System.Threading.Tasks;
using TRMFrontEnd.Library.Models;

namespace TRMFrontEnd.Library.Api
{
	public interface IUserEndpoint
	{
		Task<List<UserModel>> GetAll();
	}
}
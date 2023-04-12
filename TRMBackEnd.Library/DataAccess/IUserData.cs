using System.Collections.Generic;
using TRMBackEnd.Library.Models;

namespace TRMBackEnd.Library.DataAccess
{
	public interface IUserData
	{
		List<UserModel> GetUserById(string id);
	}
}
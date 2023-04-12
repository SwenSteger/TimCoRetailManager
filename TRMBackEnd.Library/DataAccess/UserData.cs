using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using TRMBackEnd.Library.Internal.DataAccess;
using TRMBackEnd.Library.Models;

namespace TRMBackEnd.Library.DataAccess
{
	public class UserData : IUserData
	{
		private readonly ISqlDataAccess _sqlDataAccess;

		public UserData(ISqlDataAccess sqlDataAccess)
		{
			_sqlDataAccess = sqlDataAccess;
		}

		public List<UserModel> GetUserById(string id)
		{
			return _sqlDataAccess
				.LoadData<UserModel, dynamic>("dbo.spUserLookup", new { Id = id }, "TRMData");
		}
	}
}
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using TRMBackEnd.Library.Internal.DataAccess;
using TRMBackEnd.Library.Models;

namespace TRMBackEnd.Library.DataAccess
{
	public class UserData
	{
		private readonly IConfiguration _config;

		public UserData(IConfiguration config)
		{
			_config = config;
		}

		public List<UserModel> GetUserById(string id)
		{
			var sql = new SqlDataAccess(_config);
			var p = new { Id = id };
			var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "TRMData");
			
			return output;
		}
	}
}
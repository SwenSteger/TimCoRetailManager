using System.Collections.Generic;
using TRMBackEnd.Library.Internal.DataAccess;
using TRMBackEnd.Library.Models;

namespace TRMBackEnd.Library.DataAccess
{
	public class UserData
	{
		public List<UserModel> GetUserById(string id)
		{
			var sql = new SqlDataAccess();
			var p = new { Id = id };
			var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "TRMData");
			
			return output;
		}
	}
}
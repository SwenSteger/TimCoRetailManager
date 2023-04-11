using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using TRMBackEnd.Library.Internal.DataAccess;
using TRMBackEnd.Library.Models;

namespace TRMBackEnd.Library.DataAccess
{
	public class InventoryData
	{
		private readonly IConfiguration _config;

		public InventoryData(IConfiguration config)
		{
			_config = config;
		}

		public List<InventoryModel> GetInventory()
		{
			SqlDataAccess sql = new SqlDataAccess(_config);
			var output = sql.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll",
								new { }, "TRMData");
			return output;
		}

		public void SaveInventoryRecord(InventoryModel item)
		{
			SqlDataAccess sql = new SqlDataAccess(_config);
			sql.SaveData("dbo.spInventory_Insert", item, "TRMData");
		}
	}
}
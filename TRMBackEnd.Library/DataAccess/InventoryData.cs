using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using TRMBackEnd.Library.Internal.DataAccess;
using TRMBackEnd.Library.Models;

namespace TRMBackEnd.Library.DataAccess
{
	public class InventoryData : IInventoryData
	{
		private readonly ISqlDataAccess _sqlDataAccess;

		public InventoryData(ISqlDataAccess sqlDataAccess)
		{
			_sqlDataAccess = sqlDataAccess;
		}

		public List<InventoryModel> GetInventory()
		{
			return _sqlDataAccess
				.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll",
								new { }, "TRMData");
		}

		public void SaveInventoryRecord(InventoryModel item)
		{
			_sqlDataAccess
				.SaveData("dbo.spInventory_Insert", item, "TRMData");
		}
	}
}
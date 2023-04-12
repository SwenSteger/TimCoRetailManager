using System.Collections.Generic;
using TRMBackEnd.Library.Models;

namespace TRMBackEnd.Library.DataAccess
{
	public interface IInventoryData
	{
		List<InventoryModel> GetInventory();
		void SaveInventoryRecord(InventoryModel item);
	}
}
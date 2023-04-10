using System.Collections.Generic;
using System.Web.Http;
using TRMBackEnd.Library.DataAccess;
using TRMBackEnd.Library.Models;

namespace TRMDataManager.Controllers
{
    [Authorize]
    public class InventoryController : ApiController
    {
	    [Authorize(Roles = "Manager,Admin")]
		public List<InventoryModel> Get()
	    {
			var data = new InventoryData();
			return data.GetInventory();
		}

		[Authorize(Roles = "Admin")]
		public void Post(InventoryModel item)
	    {
			var data = new InventoryData();
			data.SaveInventoryRecord(item);
		}
    }
}

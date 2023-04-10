using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRMBackEnd.Library.DataAccess;
using TRMBackEnd.Library.Models;

namespace TRMApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
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

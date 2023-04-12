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
	    private readonly IConfiguration _config;
	    private readonly IInventoryData _inventoryData;

	    public InventoryController(IConfiguration config, IInventoryData inventoryData)
	    {
		    _config = config;
		    _inventoryData = inventoryData;
	    }

		[HttpGet]
	    [Authorize(Roles = "Manager,Admin")]
		public List<InventoryModel> Get()
	    {
			return _inventoryData.GetInventory();
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public void Post(InventoryModel item)
	    {
			_inventoryData.SaveInventoryRecord(item);
		}
    }
}

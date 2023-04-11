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

	    public InventoryController(IConfiguration config)
	    {
		    _config = config;
	    }
	    
	    [Authorize(Roles = "Manager,Admin")]
		public List<InventoryModel> Get()
	    {
			var data = new InventoryData(_config);
			return data.GetInventory();
		}

		[Authorize(Roles = "Admin")]
		public void Post(InventoryModel item)
	    {
			var data = new InventoryData(_config);
			data.SaveInventoryRecord(item);
		}
    }
}

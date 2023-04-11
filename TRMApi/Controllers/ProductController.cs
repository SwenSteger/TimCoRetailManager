using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRMBackEnd.Library.DataAccess;
using TRMBackEnd.Library.Models;

namespace TRMApi.Controllers
{
	[Authorize(Roles = "Cashier")]
	[ApiController]
	[Route("api/[controller]")]
    public class ProductController : ControllerBase
	{
		private readonly IConfiguration _config;

		public ProductController(IConfiguration config)
		{
			_config = config;
		}

		[HttpGet]
	    public List<ProductModel> Get()
	    {
		    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		    var data = new ProductData(_config);

		    return data.GetProducts();
		}
	}
}

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
	    // GET: User/Details/5
	    public List<ProductModel> Get()
	    {
		    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		    var data = new ProductData();

		    return data.GetProducts();
		}
	}
}

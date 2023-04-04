using System.Collections.Generic;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
	//[Authorize]
	[Route("api/product")]
    public class ProductController : ApiController
	{
	    // GET: User/Details/5
	    public List<ProductModel> Get()
	    {
		    string userId = RequestContext.Principal.Identity.GetUserId();
		    var data = new ProductData();

		    return data.GetProducts();
		}
	}
}

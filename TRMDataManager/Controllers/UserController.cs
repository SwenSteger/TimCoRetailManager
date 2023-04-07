using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
	[Authorize]
	[Route("api/user")]
	public class UserController : ApiController
	{
        // GET: User/Details/5
        [HttpGet]
        public UserModel GetById()
        {
	        string userId = RequestContext.Principal.Identity.GetUserId();
            var data = new UserData();

            return data.GetUserById(userId).First();
        }
    }
}

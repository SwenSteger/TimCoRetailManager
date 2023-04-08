using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;
using TRMDataManager.Models;

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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/users/Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            var output = new List<ApplicationUserModel>();

	        using (var context = new ApplicationDbContext())
	        {
		        var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                
                var users = userManager.Users.ToList();
                var rols = context.Roles.ToList();

                foreach (var user in users)
                {
                    var userModel = new ApplicationUserModel
                    {
						Id = user.Id,
						Email = user.Email,
						Roles = new Dictionary<string, string>()
					};

                    foreach (var role in rols.Where(role => userManager.IsInRole(user.Id, role.Name)))
	                    userModel.Roles.Add(role.Id, role.Name);

                    output.Add(userModel);
                }
			}

	        return output;
        }
    }
}

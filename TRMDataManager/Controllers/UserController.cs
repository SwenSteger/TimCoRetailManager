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
        [Route("api/users/admin/getAllUsers")]
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/users/admin/getAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
	        using (var context = new ApplicationDbContext())
	        {
		        var roles = context.Roles.ToDictionary(x => x.Id, x => x.Name);
		        return roles;
	        }

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("api/users/admin/AddToRole")]
        public void AddToRole(UserRolePairModel userRolePair)
        {
	        using (var context = new ApplicationDbContext())
	        {
		        var userStore = new UserStore<ApplicationUser>(context);
		        var userManager = new UserManager<ApplicationUser>(userStore);

                userManager.AddToRole(userRolePair.UserId, userRolePair.RoleName);
	        }

		}
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("api/users/admin/RemoveFromRole")]
        public void RemoveFromRole(UserRolePairModel userRolePair)
        {
	        using (var context = new ApplicationDbContext())
	        {
		        var userStore = new UserStore<ApplicationUser>(context);
		        var userManager = new UserManager<ApplicationUser>(userStore);
		        userManager.RemoveFromRole(userRolePair.UserId, userRolePair.RoleName);
	        }

		}
	}
}

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TRMApi.Models;
using TRMBackEnd.Library.DataAccess;
using TRMBackEnd.Library.Models;
using ApplicationDbContext = TRMApi.Data.ApplicationDbContext;

namespace TRMApi.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IUserData _userData;

		public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IUserData userData)
		{
			_context = context;
			_userManager = userManager;
			_userData = userData;
		}
		
        [HttpGet]
        public UserModel GetById()
        {
	        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            return _userData.GetUserById(userId).First();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("admin/getAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
	        var output = new List<ApplicationUserModel>();

	        var users = _context.Users.ToList();
	        var userRoles =
		        from ur in _context.UserRoles
		        join r in _context.Roles on ur.RoleId equals r.Id
		        select new { ur.UserId, ur.RoleId, r.Name };

	        foreach (var user in users)
	        {
		        var userModel = new ApplicationUserModel
		        {
			        Id = user.Id,
			        Email = user.Email,
			        Roles = new Dictionary<string, string>()
		        };
		        userModel.Roles = userRoles
			        .Where(x => x.UserId == user.Id)
			        .ToDictionary(x => x.RoleId, x => x.Name);

		        output.Add(userModel);
	        }

			return output;
		}

		[HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("admin/getAllRoles")]
        public Dictionary<string, string> GetAllRoles() 
	        => _context.Roles.ToDictionary(x => x.Id, x => x.Name);

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("admin/AddToRole")]
        public async Task AddToRole(UserRolePairModel userRolePair)
        {
	        var user = await _userManager.FindByIdAsync(userRolePair.UserId);
	        await _userManager.AddToRoleAsync(user, userRolePair.RoleName);
		}
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("admin/RemoveFromRole")]
        public async Task RemoveFromRole(UserRolePairModel userRolePair)
        {
	        var user = await _userManager.FindByIdAsync(userRolePair.UserId);
	        await _userManager.RemoveFromRoleAsync(user, userRolePair.RoleName);
		}
	}
}

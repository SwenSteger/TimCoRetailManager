using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
		private readonly IConfiguration _config;

		public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IConfiguration config)
		{
			_context = context;
			_userManager = userManager;
			_config = config;
		}
		
        [HttpGet]
        public UserModel GetById()
        {
	        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var data = new UserData(_config);

            return data.GetUserById(userId).First();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/users/admin/getAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
	        var output = new List<ApplicationUserModel>();

	        var users = _context.Users.ToList();
	        var userRoles = from userRole in _context.UserRoles
											        join role in _context.Roles on userRole.RoleId equals role.Id
											        select new { userRole.UserId, role.Name };

	        foreach (var user in users)
	        {
		        var userModel = new ApplicationUserModel
		        {
			        Id = user.Id,
			        Email = user.Email,
			        Roles = userRoles
				        .Where(x => x.UserId == user.Id)
				        .ToDictionary(x => x.UserId, x => x.Name)
		        };

		        output.Add(userModel);
	        }

	        return output;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/users/admin/getAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
	        var roles = _context.Roles.ToDictionary(x => x.Id, x => x.Name);
	        return roles;
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("api/users/admin/AddToRole")]
        public async Task AddToRole(UserRolePairModel userRolePair)
        {
	        var user = await _userManager.FindByIdAsync(userRolePair.UserId);
	        await _userManager.AddToRoleAsync(user, userRolePair.RoleName);
		}
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("api/users/admin/RemoveFromRole")]
        public async Task RemoveFromRole(UserRolePairModel userRolePair)
        {
	        var user = await _userManager.FindByIdAsync(userRolePair.UserId);
	        await _userManager.RemoveFromRoleAsync(user, userRolePair.RoleName);
		}
	}
}

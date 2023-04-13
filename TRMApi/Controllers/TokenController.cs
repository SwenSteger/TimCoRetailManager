﻿using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TRMApi.Data;

namespace TRMApi.Controllers
{
	// [Route("api/[controller]")]
	// [ApiController]
	public class TokenController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IConfiguration _configuration;

		public TokenController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IConfiguration configuration)
		{
			_context = context;
			_userManager = userManager;
			_configuration = configuration;
		}

		[HttpPost]
		[Route("/token")]
		public async Task<IActionResult> Create(string username, string password, string grandt_type)
		{
			if (await IsValidUsernameAndPassword(username, password))
				return new ObjectResult(await GenerateToken(username));
			
			return BadRequest();
		}

		private async Task<dynamic> GenerateToken(string username)
		{
			var user = await _userManager.FindByNameAsync(username);
			var roles = from ur in _context.UserRoles
						join r in _context.Roles on ur.RoleId equals r.Id
						where ur.UserId == user.Id
						select new { ur.UserId, ur.RoleId, r.Name };

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, username),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
				new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
			};
			foreach (var role in roles) 
				claims.Add(new Claim(ClaimTypes.Role, role.Name));

			var secret = _configuration["Secrets:SecurityKey"];
			var token = new JwtSecurityToken(
				new JwtHeader(
					new SigningCredentials(
						new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
						SecurityAlgorithms.HmacSha256)),
				new JwtPayload(claims));

			var output = new
			{
				access_token = new JwtSecurityTokenHandler().WriteToken(token),
				UserName = username
			};

			return output;
		}
		private async Task<bool> IsValidUsernameAndPassword(string username, string password)
		{
			var user = await _userManager.FindByNameAsync(username);
			if (user == null)
			{
				return false;
			}
			var result = await _userManager.CheckPasswordAsync(user, password);
			return result;
		}
	}
}

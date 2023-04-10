using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TRMApi.Controllers
{
	[AllowAnonymous]
	[ApiController]
	[Route("api/[controller]")]
	public class PingController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get() => Ok("OK");
	}
}


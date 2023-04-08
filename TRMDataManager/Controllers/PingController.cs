using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
	[AllowAnonymous]
	[Route("api/ping")]
	public class PingController : ApiController
	{
		[HttpGet]
		public OkNegotiatedContentResult<string> Get() => Ok("OK");
	}
}


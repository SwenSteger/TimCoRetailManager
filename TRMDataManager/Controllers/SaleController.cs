using System;
using System.Web.Http;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
    [Authorize]
    public class SaleController : BaseApiController
	{
        // POST api/<controller>
        public void Post(SaleModel sale)
        {
	        Console.WriteLine();
        }
    }
}

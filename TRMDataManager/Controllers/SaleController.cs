using System;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
	{
        // POST api/<controller>
        public void Post(SaleModel sale)
        {
	        SaleData  data = new SaleData();
            var userId = RequestContext.Principal.Identity.GetUserId();

            data.SaveSale(sale, userId);
		}
    }
}

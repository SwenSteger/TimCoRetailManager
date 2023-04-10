using System.Collections.Generic;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using TRMBackEnd.Library.DataAccess;
using TRMBackEnd.Library.Models;

namespace TRMDataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
	{
		[Authorize(Roles = "Cashier")]
		public void Post(SaleModel sale)
        {
	        var  data = new SaleData();
            string userId = RequestContext.Principal.Identity.GetUserId();

            data.SaveSale(sale, userId);
		}

        [Route("GetSalesReport")]
        [Authorize(Roles = "Admin,Manager")]
		public List<SaleReportModel> GetSalesReport()
        {
	        if (RequestContext.Principal.IsInRole("Admin"))
	        {
				// Do admin stuff
	        }
			else if (RequestContext.Principal.IsInRole("Manager"))
	        {
				// Do manager stuff
	        }

			var data = new SaleData();
			return data.GetSaleReport();
		}
    }
}

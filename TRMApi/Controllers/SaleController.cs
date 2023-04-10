using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRMBackEnd.Library.DataAccess;
using TRMBackEnd.Library.Models;

namespace TRMApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
	{
		[Authorize(Roles = "Cashier")]
		public void Post(SaleModel sale)
        {
	        var  data = new SaleData();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            data.SaveSale(sale, userId);
		}

        [Route("GetSalesReport")]
        [Authorize(Roles = "Admin,Manager")]
		public List<SaleReportModel> GetSalesReport()
        {
	  //       if (RequestContext.Principal.IsInRole("Admin"))
	  //       {
			// 	// Do admin stuff
	  //       }
			// else if (RequestContext.Principal.IsInRole("Manager"))
	  //       {
			// 	// Do manager stuff
	  //       }

			var data = new SaleData();
			return data.GetSaleReport();
		}
    }
}

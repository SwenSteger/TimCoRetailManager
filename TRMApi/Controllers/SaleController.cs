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
		private readonly ISaleData _saleData;

		public SaleController(ISaleData saleData)
		{
			_saleData = saleData;
		}

		[HttpPost]
		[Authorize(Roles = "Cashier")]
		public void Post(SaleModel sale)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _saleData.SaveSale(sale, userId);
		}

		[HttpGet]
        [Route("GetSalesReport")]
        [Authorize(Roles = "Admin,Manager")]
		public List<SaleReportModel> GetSalesReport() 
			=> _saleData.GetSaleReport();
	}
}

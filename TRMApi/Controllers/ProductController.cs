using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRMBackEnd.Library.DataAccess;
using TRMBackEnd.Library.Models;

namespace TRMApi.Controllers
{
	[Authorize(Roles = "Cashier")]
	[ApiController]
	[Route("api/[controller]")]
    public class ProductController : ControllerBase
	{
		private readonly IProductData _productData;

		public ProductController(IProductData productData)
		{
			_productData = productData;
		}

		[HttpGet]
	    public List<ProductModel> Get() 
		    => _productData.GetProducts();
	}
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
	public class ProductEndpoint : IProductEndpoint
	{
		private readonly IApiHelper _apiHelper;

		public ProductEndpoint(IApiHelper apiHelper)
		{
			_apiHelper = apiHelper;
		}

		public async Task<List<ProductModel>> GetAll()
		{
			using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/product"))
			{
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsAsync<List<ProductModel>>();
					return result;
				}
				else
				{
					throw new Exception(response.ReasonPhrase);
				}
			}
		}
	}
}
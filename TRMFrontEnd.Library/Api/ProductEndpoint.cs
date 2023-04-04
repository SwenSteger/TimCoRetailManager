using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TRMFrontEnd.Library.Models;

namespace TRMFrontEnd.Library.Api
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
					var result = await response.Content.ReadFromJsonAsync<List<ProductModel>>();
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
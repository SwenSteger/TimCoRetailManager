using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
	public class SaleEndpoint : ISaleEndpoint
	{
		private readonly IApiHelper _apiHelper;

		public SaleEndpoint(IApiHelper apiHelper)
		{
			_apiHelper = apiHelper;
		}

		public async Task PostSale(SaleModel sale)
		{
			using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/sale", sale))
			{
				if (response.IsSuccessStatusCode)
				{
					// Log Success?
				}
				else
				{
					throw new Exception(response.ReasonPhrase);
				}
			}
		}
	}
}
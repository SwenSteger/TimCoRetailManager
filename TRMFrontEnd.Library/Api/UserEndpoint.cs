using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net.Http.Json;
using TRMFrontEnd.Library.Models;

namespace TRMFrontEnd.Library.Api
{
	public class UserEndpoint : IUserEndpoint
	{
		private readonly IApiHelper _apiHelper;

		public UserEndpoint(IApiHelper apiHelper )
		{
			_apiHelper = apiHelper;
		}

		public async Task<List<UserModel>> GetAll()
		{
			using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/users/admin/getallusers"))
			{
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadFromJsonAsync<List<UserModel>>();
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
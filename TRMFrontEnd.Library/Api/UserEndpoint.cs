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
			using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/user/admin/getallusers"))
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

		public async Task<Dictionary<string, string>> GetAllRoles()
		{
			using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/user/admin/getallroles"))
			{
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
					return result;
				}
				else
				{
					throw new Exception(response.ReasonPhrase);
				}
			}
		}

		public async Task AddUserToRole(string userId, string roleName)
		{
			var data = new { UserId = userId, RoleName = roleName };
			using (HttpResponseMessage response =
			       await _apiHelper.ApiClient.PostAsJsonAsync("api/user/admin/AddToRole", data))
			{
				if (!response.IsSuccessStatusCode)
					throw new Exception(response.ReasonPhrase);
			}
		}

		public async Task RemoveUserFromRole(string userId, string roleName)
		{
			var data = new { UserId = userId, RoleName = roleName };
			using (HttpResponseMessage response =
			       await _apiHelper.ApiClient.PostAsJsonAsync("api/user/admin/RemoveFromRole", data))
			{
				if (!response.IsSuccessStatusCode)
					throw new Exception(response.ReasonPhrase);
			}
		}
	}
}
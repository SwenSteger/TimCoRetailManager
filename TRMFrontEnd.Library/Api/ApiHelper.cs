using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TRMFrontEnd.Library.Exceptions;
using TRMFrontEnd.Library.Models;

namespace TRMFrontEnd.Library.Api
{
	public class ApiHelper : IApiHelper
	{
		private HttpClient _apiClient;
		private readonly ILoggedInUserModel _loggedInUser;
		private readonly Func<IConfiguration> _funcConfiguration;
		private IConfiguration Configuration => _funcConfiguration?.Invoke();

		public ApiHelper(ILoggedInUserModel loggedInUser, Func<IConfiguration> configuration)
		{
			_loggedInUser = loggedInUser;
			_funcConfiguration = configuration;
			InitializeClient();
		}

		public HttpClient ApiClient => _apiClient;

		public void InitializeClient()
		{
			string api = Configuration["api"];
			_apiClient = new HttpClient();
			_apiClient.BaseAddress = new Uri(api);
			_apiClient.DefaultRequestHeaders.Accept.Clear();
			_apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<AuthenticatedUser> Authenticate(string username, string password)
		{
			// TODO: Add error handling that throws proper HttpRequestExceptions
			var data = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("grant_type", "password"),
				new KeyValuePair<string, string>("username", username),
				new KeyValuePair<string, string>("password", password),
			});

			using (HttpResponseMessage response = await _apiClient.PostAsync("/token", data))
			{
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadFromJsonAsync<AuthenticatedUser>();

					return result;
				}
				else
					throw new Exception(response.ReasonPhrase);
			}
		}

		public void Logout() 
			=> _apiClient.DefaultRequestHeaders.Clear();

		public async Task GetLoggedInUserInfo(string token)
		{
			_apiClient.DefaultRequestHeaders.Clear();
			_apiClient.DefaultRequestHeaders.Accept.Clear();
			_apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			_apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

			using (HttpResponseMessage response = await _apiClient.GetAsync("/api/user"))
			{
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadFromJsonAsync<LoggedInUserModel>();
					_loggedInUser.CreatedDate = result.CreatedDate;
					_loggedInUser.EmailAddress = result.EmailAddress;
					_loggedInUser.FirstName = result.FirstName;
					_loggedInUser.Id = result.Id;
					_loggedInUser.LastName = result.LastName;
					_loggedInUser.Token = token;
				}
				else
				{
					throw new ApiException(response.ReasonPhrase);
				}
			}
		}
		public async Task<string> PingServer()
		{
			using (var response = await _apiClient.GetAsync("/api/ping"))
				return await response.Content.ReadAsStringAsync();
		}
	}
}
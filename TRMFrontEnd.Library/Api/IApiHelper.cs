﻿using System.Net.Http;
using System.Threading.Tasks;
using TRMFrontEnd.Library.Models;

namespace TRMFrontEnd.Library.Api
{
	public interface IApiHelper
	{
		HttpClient ApiClient { get; }
		Task<AuthenticatedUser> Authenticate(string username, string password);
		Task GetLoggedInUserInfo(string token);
	}
}
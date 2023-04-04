using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TRMDataManager
{
	public class WebApiApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		// protected void Application_BeginRequest(object sender, EventArgs e)
		// {
		// 	if (Request.HttpMethod == "OPTIONS")
		// 	{
		// 		Response.Headers.Add("Access-Control-Allow-Origin", "https://localhost:7030");
		// 		Response.Headers.Add("Access-Control-Allow-Headers", "Authorization,Content-Type");
		// 		Response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE");
		// 		Response.StatusCode = 200;
		// 		Response.End();
		// 	}
		// }

	}
}

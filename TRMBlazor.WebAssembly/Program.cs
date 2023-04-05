using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TRMBlazor.WebAssembly;
using TRMFrontEnd.Library.Api;
using TRMFrontEnd.Library.Helpers;
using TRMFrontEnd.Library.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
var configuration = new ConfigurationBuilder()
	.AddInMemoryCollection(new Dictionary<string, string>
	{
		{"api", "https://localhost:44361/"},
		{"taxRate", "8.75"}
	})
	.Build();

// LocalStorage
builder.Services.AddBlazoredLocalStorage(config =>
{
	config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
	config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
	config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
	config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
	config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
	config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
	config.JsonSerializerOptions.WriteIndented = false;
});
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddApiAuthorization();

// DI
builder.Services.AddSingleton<Func<IConfiguration>>(() => configuration);
builder.Services.AddSingleton<ILoggedInUserModel, LoggedInUserModel>();
builder.Services.AddSingleton<IApiHelper, ApiHelper>();
builder.Services.AddSingleton<IConfigHelper, ConfigHelper>();

await builder.Build().RunAsync();

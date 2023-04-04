using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using TRMDesktopUI.Helpers;
using TRMDesktopUI.ViewModels;
using TRMFrontEnd.Library.Api;
using TRMFrontEnd.Library.Helpers;
using TRMFrontEnd.Library.Models;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

namespace TRMDesktopUI
{
	public class Configuration : IConfiguration
	{
		private readonly IConfigurationRoot _configuration;

		public Configuration()
		{
			_configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(new Dictionary<string, string>
				{
					{"api", "https://localhost:44361/"},
					{"taxRate", "8.75"}
				})
				.Build();
		}
		public IConfigurationSection GetSection(string key)
		{
			return _configuration.GetSection(key);
		}

		public IEnumerable<IConfigurationSection> GetChildren()
		{
			return _configuration.GetChildren();
		}

		public IChangeToken GetReloadToken()
		{
			return _configuration.GetReloadToken();
		}

		public string this[string key]
		{
			get => _configuration[key];
			set => throw new NotImplementedException();
		}
	}
	public class Bootstrapper : BootstrapperBase
	{
		private readonly SimpleContainer _container = new SimpleContainer();
		private IConfigurationRoot _configuration;

		public Bootstrapper()
		{
			Initialize();

			ConventionManager.AddElementConvention<PasswordBox>(
				PasswordBoxHelper.BoundPasswordProperty,
				"Password",
				"PasswordChanged");
		}

		protected override void Configure()
		{

			_container.Instance(_container)
				.PerRequest<IProductEndpoint, ProductEndpoint>()
				.PerRequest<ISaleEndpoint, SaleEndpoint>();

			_container
				.Singleton<IWindowManager, WindowManager>()
				.Singleton<IEventAggregator, EventAggregator>()
				.Singleton<IConfiguration, Configuration>()
				.Singleton<ILoggedInUserModel, LoggedInUserModel>()
				.Singleton<IApiHelper, ApiHelper>()
				.Singleton<IConfigHelper, ConfigHelper>();

			_container.RegisterInstance(
				typeof(Func<IConfiguration>), 
				"IConfiguration", 
				new Func<IConfiguration>(() => new Configuration()));

			GetType().Assembly.GetTypes()
				.Where(type => type.IsClass)
				.Where(type => type.Name.EndsWith("ViewModel"))
				.ToList()
				.ForEach(viewModelType => _container.RegisterPerRequest(
					viewModelType,
					viewModelType.ToString(),
					viewModelType));
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewForAsync<ShellViewModel>();
		}

		protected override object GetInstance(Type service, string key)
		{
			return _container.GetInstance(service, key);
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return _container.GetAllInstances(service);
		}

		protected override void BuildUp(object instance)
		{
			_container.BuildUp(instance);
		}
	}
}
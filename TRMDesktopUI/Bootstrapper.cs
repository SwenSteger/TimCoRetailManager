using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AutoMapper;
using Caliburn.Micro;
using Microsoft.Extensions.Configuration;
using TRMDesktopUI.Helpers;
using TRMDesktopUI.Models;
using TRMDesktopUI.ViewModels;
using TRMFrontEnd.Library.Api;
using TRMFrontEnd.Library.Helpers;
using TRMFrontEnd.Library.Models;

namespace TRMDesktopUI
{
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

		private IMapper ConfigureAutoMapper()
		{
			var autoMapperConfig = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<ProductModel, ProductDisplayModel>();
				cfg.CreateMap<CartItemModel, CartItemDisplayModel>();
			});

			return autoMapperConfig.CreateMapper();
		}

		protected override void Configure()
		{
			var autoMapperConfig = ConfigureAutoMapper();

			_container.Instance(autoMapperConfig);

			_container.Instance(_container)
				.PerRequest<IProductEndpoint, ProductEndpoint>()
				.PerRequest<ISaleEndpoint, SaleEndpoint>()
				.PerRequest<IUserEndpoint, UserEndpoint>();

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
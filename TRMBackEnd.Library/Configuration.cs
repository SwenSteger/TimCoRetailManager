using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace TRMBackEnd.Library
{
	public class Configuration : IConfiguration
	{
		private readonly IConfigurationRoot _configuration;

		public Configuration()
		{
			_configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
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
}
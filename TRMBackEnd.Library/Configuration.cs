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
				.AddInMemoryCollection(new Dictionary<string, string>
				{
					{"api", "https://localhost:7035/"},
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
}
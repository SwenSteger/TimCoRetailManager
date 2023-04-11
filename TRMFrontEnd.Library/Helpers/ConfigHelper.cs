using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace TRMFrontEnd.Library.Helpers
{
	public class ConfigHelper : IConfigHelper
	{
		private readonly IConfiguration _configuration;

		public ConfigHelper(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public decimal GetTaxRate()
		{
			string rateText = _configuration["taxRate"];
			if (string.IsNullOrWhiteSpace(rateText))
				throw new ConfigurationErrorsException("The Tax rate is not set up properly");

			var isValidTaxRate = decimal.TryParse(rateText, out var output);
			if (!isValidTaxRate)
				throw new ConfigurationErrorsException("The Tax rate is not set up properly");

			return output;
		}
	}
}
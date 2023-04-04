using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace TRMFrontEnd.Library.Helpers
{
	public class ConfigHelper : IConfigHelper
	{
		private readonly IConfiguration _configuration;

		public ConfigHelper(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		
		// TODO: Move this from config to the API
		public decimal GetTaxRate()
		{
			string rateText = _configuration["taxRate"];
			if (rateText == null) 
				throw new ConfigurationErrorsException("The Tax rate is not set up properly");

			var isValidTaxRate = decimal.TryParse(rateText, out var output);
			if (!isValidTaxRate)
				throw new ConfigurationErrorsException("The Tax rate is not set up properly");

			return output;
		}
	}
}
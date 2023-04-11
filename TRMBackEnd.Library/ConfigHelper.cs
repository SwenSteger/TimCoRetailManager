using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace TRMBackEnd.Library
{
	public class ConfigHelper
	{
		private static IConfiguration _config;

		public static decimal GetTaxRate()
		{
			if (_config == null)
				_config = new Configuration();
			
			string rateText = _config["taxRate"];
			if (string.IsNullOrWhiteSpace(rateText))
				throw new ConfigurationErrorsException("The Tax rate is not set up properly");

			var isValidTaxRate = decimal.TryParse(rateText, out var output);
			if (!isValidTaxRate)
				throw new ConfigurationErrorsException("The Tax rate is not set up properly");

			return output;
		}
	}
}
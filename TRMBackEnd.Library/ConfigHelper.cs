using System.Configuration;

namespace TRMBackEnd.Library
{
	public class ConfigHelper
	{
		// TODO: Move this from config to the API
		public static decimal GetTaxRate()
		{
			string rateText = ConfigurationManager.AppSettings["taxRate"];
			if (rateText == null)
				throw new ConfigurationErrorsException("The Tax rate is not set up properly");

			var isValidTaxRate = decimal.TryParse(rateText, out var output);
			if (!isValidTaxRate)
				throw new ConfigurationErrorsException("The Tax rate is not set up properly");

			return output;
		}
	}
}
using System.Collections.Generic;
using TRMBackEnd.Library.Models;

namespace TRMBackEnd.Library.DataAccess
{
	public interface ISaleData
	{
		void SaveSale(SaleModel saleInfo, string cashierId);
		List<SaleReportModel> GetSaleReport();
	}
}
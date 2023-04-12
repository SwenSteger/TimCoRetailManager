using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using TRMBackEnd.Library.Internal.DataAccess;
using TRMBackEnd.Library.Models;

namespace TRMBackEnd.Library.DataAccess
{
	public class SaleData : ISaleData
	{
		private readonly IProductData _productData;
		private readonly ISqlDataAccess _sqlDataAccess;

		public SaleData(IConfiguration config, IProductData productData, ISqlDataAccess sqlDataAccess)
		{
			_productData = productData;
			_sqlDataAccess = sqlDataAccess;
		}

		public void SaveSale(SaleModel saleInfo, string cashierId)
		{
			var details = new List<SaleDetailDbModel>();
			var taxRate = ConfigHelper.GetTaxRate() / 100;

			foreach (var item in saleInfo.SaleDetails)
			{
				var detail = new SaleDetailDbModel
				{
					ProductId = item.ProductId,
					Quantity = item.Quantity,
				};

				// Get the info about this product
				var productInfo = _productData.GetProductById(item.ProductId);
				if (productInfo == null)
					throw new System.Exception(
						$"The product Id of {item.ProductId} could not be found in the database.");

				detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);
				if (productInfo.IsTaxable) 
					detail.Tax = detail.PurchasePrice * taxRate;

				details.Add(detail);
			}
			
			// Create the Sale model
			var sale = new SaleDbModel
			{
				SubTotal = details.Sum(x => x.PurchasePrice),
				Tax = details.Sum(x => x.Tax),
				CashierId = cashierId
			};
			sale.Total = sale.SubTotal + sale.Tax;

			// Save the Sale model
			try
			{
				_sqlDataAccess.StartTransaction("TRMData");
				_sqlDataAccess.SaveDataInTransaction<SaleDbModel>("dbo.spSale_Insert", sale);

				// Get the ID from the Sale model
				sale.Id = _sqlDataAccess.LoadDataInTransaction<int, dynamic>("dbo.spSale_Lookup",
						new { sale.CashierId, sale.SaleDate })
					.FirstOrDefault();

				// Finish filling in the sale detail models
				foreach (var item in details)
				{
					item.SaleId = sale.Id;
					// Save the sale detail models
					_sqlDataAccess.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
				}

				_sqlDataAccess.CommitTransaction();
			}
			catch
			{
				_sqlDataAccess.RollbackTransaction();
				throw;
			}
		}

		public List<SaleReportModel> GetSaleReport() 
			=> _sqlDataAccess.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", new { }, "TRMData");
	}
}
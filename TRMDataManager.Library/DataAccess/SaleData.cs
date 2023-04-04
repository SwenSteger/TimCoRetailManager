using System.Collections.Generic;
using System.Linq;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
	public class SaleData
	{
		public void SaveSale(SaleModel saleInfo, string cashierId)
		{
			// TODO: Make this SOLID/DRY/Better
			// Star filling in the sale detail models we will save to the database
			List<SaleDetailDbModel> details = new List<SaleDetailDbModel>();
			ProductData products = new ProductData();
			var taxRate = ConfigHelper.GetTaxRate() / 100;

			foreach (var item in saleInfo.SaleDetails)
			{
				var detail = new SaleDetailDbModel
				{
					ProductId = item.ProductId,
					Quantity = item.Quantity,
				};

				// Get the info about this product
				var productInfo = products.GetProductById(item.ProductId);
				if (productInfo == null)
					throw new System.Exception(
						$"The product Id of {item.ProductId} could not be found in the database.");

				detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);
				if (productInfo.IsTaxable) 
					detail.Tax = detail.PurchasePrice * taxRate;

				details.Add(detail);
			}
			
			// Create the Sale model
			SaleDbModel sale = new SaleDbModel
			{
				SubTotal = details.Sum(x => x.PurchasePrice),
				Tax = details.Sum(x => x.Tax),
				CashierId = cashierId
			};
			sale.Total = sale.SubTotal + sale.Tax;

			// Save the Sale model
			SqlDataAccess sql = new SqlDataAccess();
			sql.SaveData<SaleDbModel>("dbo.spSale_Insert", sale, "TRMData");

			// Get the ID from the Sale model
			//sale.Id = sql.LoadDataInTransaction<int, dynamic>("spSale_Lookup", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();
			// Finish filling in the sale detail models
			// Save the sale detail models
			
		}
	}
}
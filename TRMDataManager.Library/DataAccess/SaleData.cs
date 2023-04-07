﻿using System.Collections.Generic;
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
			using (SqlDataAccess sql = new SqlDataAccess())
			{
				try
				{
					sql.StartTransaction("TRMData");
					sql.SaveDataInTransaction<SaleDbModel>("dbo.spSale_Insert", sale);

					// Get the ID from the Sale model
					sale.Id = sql.LoadDataInTransaction<int, dynamic>("dbo.spSale_Lookup",
							new { sale.CashierId, sale.SaleDate })
						.FirstOrDefault();

					// Finish filling in the sale detail models
					foreach (var item in details)
					{
						item.SaleId = sale.Id;
						// Save the sale detail models
						sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
					}

					sql.CommitTransaction();
				}
				catch
				{
					sql.RollbackTransaction();
					throw;
				}
			}
		}

		public List<SaleReportModel> GetSaleReport()
		{
			SqlDataAccess sql = new SqlDataAccess();
			var output = sql.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", new { }, "TRMData");
			return output;
		}
	}
}
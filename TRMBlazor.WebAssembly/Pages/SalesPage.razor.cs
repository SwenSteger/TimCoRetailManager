using Microsoft.AspNetCore.Components;
using TRMFrontEnd.Library.Api;
using TRMFrontEnd.Library.Helpers;
using TRMFrontEnd.Library.Models;

namespace TRMBlazor.WebAssembly.Pages;

public partial class SalesPage : ComponentBase
{
	[Inject] public IProductEndpoint ProductEndpoint { get; set; }
	[Inject] public IConfigHelper ConfigHelper { get; set; }
	[Inject] public ISaleEndpoint SaleEndpoint { get; set; }

	public List<ProductModel> Products { get; set; } = new();
	public List<CartItemModel> Cart { get; } = new();

	public ProductModel SelectedProduct { get; set; }
	public CartItemModel SelectedCartItem { get; set; }

	public int ItemQuantity { get; set; } = 1;

	public decimal SubTotal => CalculateSubTotal();
	public decimal Tax => CalculateTotalTax();
	public decimal Total => CalculateSubTotal() + CalculateTotalTax();


	protected override async Task OnInitializedAsync()
	{
		await LoadProducts();
	}

	public async Task LoadProducts()
	{
		var productList = await ProductEndpoint.GetAll();
		Products = new List<ProductModel>(productList);
	}

	private decimal CalculateSubTotal() => Cart.Sum(item => (item.Product.RetailPrice * item.QuantityInCart));
	private decimal CalculateTotalTax()
	{
		decimal taxRate = ConfigHelper.GetTaxRate() / 100;
		return Cart
			.Where(item => item.Product.IsTaxable)
			.Sum(item => (item.Product.RetailPrice * item.QuantityInCart) * taxRate);
	}

	public bool CanAddToCart => ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity;
	public void AddToCart()
	{
		var existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
		if (existingItem != null)
		{
			existingItem.QuantityInCart += ItemQuantity;
		}
		else
		{
			var item = new CartItemModel
			{
				Product = SelectedProduct,
				QuantityInCart = ItemQuantity
			};

			Cart.Add(item);
		}

		SelectedProduct.QuantityInStock -= ItemQuantity;
		ItemQuantity = 1;
	}

	public bool CanRemoveFromCart => Cart.Any();
	public void RemoveFromCart(CartItemModel item)
	{
		item.Product.QuantityInStock += item.QuantityInCart;
		Cart.Remove(item);
	}

	public bool CanCheckOut => Cart.Any();
	public async Task CheckOut()
	{
		// create a SaleModel and post to api
		var sale = new SaleModel();
		foreach (var item in Cart)
		{
			sale.SaleDetails.Add(new SaleDetailModel
			{
				ProductId = item.Product.Id,
				Quantity = item.QuantityInCart
			});
		}

		await SaleEndpoint.PostSale(sale);
	}
}
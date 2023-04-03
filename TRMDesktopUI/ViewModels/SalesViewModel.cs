using Caliburn.Micro;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
	public class SalesViewModel : Screen
	{
		private readonly IProductEndpoint _productEndpoint;
		private readonly IConfigHelper _configHelper;

		public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper)
		{
			_productEndpoint = productEndpoint;
			_configHelper = configHelper;
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			await LoadProducts();
		}

		public async Task LoadProducts()
		{
			var productList = await _productEndpoint.GetAll();
			Products = new BindingList<ProductModel>(productList);
		}

		private BindingList<ProductModel> _products;
		public BindingList<ProductModel> Products
		{
			get => _products;
			set
			{
				_products = value;
				NotifyOfPropertyChange(() => Products);
			}
		}

		private ProductModel _selectedProduct;
		public ProductModel SelectedProduct
		{
			get => _selectedProduct;
			set
			{
				_selectedProduct = value;
				NotifyOfPropertyChange(() => SelectedProduct);
				NotifyOfPropertyChange(() => CanAddToCart);
				NotifyOfPropertyChange(() => Products);
			}
		}

		private int _itemQuantity = 1;
		public int ItemQuantity
		{
			get => _itemQuantity;
			set {
				_itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
		public BindingList<CartItemModel> Cart
		{
			get => _cart;
			set
			{
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}

		public string SubTotal => CalculateSubTotal().ToString("C");

		private decimal CalculateSubTotal()
		{
			decimal subTotal = 0;
			foreach (var item in Cart)
				subTotal += (item.Product.RetailPrice * item.QuantityInCart);
			return subTotal;
		}

		public string Tax => CalculateTotalTax().ToString("C2");

		private decimal CalculateTotalTax()
		{
			decimal taxAmount = 0;
			decimal taxRate = _configHelper.GetTaxRate() / 100;

			foreach (var item in Cart)
				if (item.Product.IsTaxable)
					taxAmount += (item.Product.RetailPrice * item.QuantityInCart) * taxRate;

			return taxAmount;
		}

		public string Total
		{
			get
			{
				decimal total = CalculateSubTotal() + CalculateTotalTax();
				return total.ToString("C2");
			}
		}

		public bool CanAddToCart 
			=> ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity;

		public void AddToCart()
		{
			var existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
			if (existingItem != null)
			{
				existingItem.QuantityInCart += ItemQuantity;
				var index = Cart.IndexOf(existingItem);
				Cart.Remove(existingItem);
				Cart.Insert(index, existingItem);
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
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);

			// Doesn't seem to update the QuantityInStock for the products list :(
			var selectedItem = Products.FirstOrDefault(x => x == SelectedProduct);
			if (selectedItem == null) return;
			selectedItem.QuantityInStock = SelectedProduct.QuantityInStock;
			NotifyOfPropertyChange(() => Products);
		}

		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;

				// Make sure something is selected

				NotifyOfPropertyChange(() => SubTotal);
				NotifyOfPropertyChange(() => Tax);
				NotifyOfPropertyChange(() => Total);
				return output;
			}
		}
		public void RemoveFromCart()
		{

		}

		public bool CanCheckOut
		{
			get
			{
				bool output = false;

				// Make sure something is in cart

				return output;
			}
		}
		public void CheckOut()
		{

		}
	}
}
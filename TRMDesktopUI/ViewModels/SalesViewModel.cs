using System;
using System.Collections.Generic;
using Caliburn.Micro;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using AutoMapper;
using TRMDesktopUI.Models;
using TRMFrontEnd.Library.Api;
using TRMFrontEnd.Library.Helpers;
using TRMFrontEnd.Library.Models;

namespace TRMDesktopUI.ViewModels
{
	public class SalesViewModel : Screen
	{
		private readonly IProductEndpoint _productEndpoint;
		private readonly IConfigHelper _configHelper;
		private readonly ISaleEndpoint _saleEndpoint;
		private readonly IMapper _mapper;
		private readonly StatusInfoViewModel _status;
		private readonly IWindowManager _window;

		public SalesViewModel(IProductEndpoint productEndpoint, 
			IConfigHelper configHelper, ISaleEndpoint saleEndpoint, IMapper mapper,
			StatusInfoViewModel status, IWindowManager window)
		{
			_productEndpoint = productEndpoint;
			_configHelper = configHelper;
			_saleEndpoint = saleEndpoint;
			_mapper = mapper;
			_status = status;
			_window = window;
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			try
			{
				await LoadProducts();
			}
			catch (Exception e)
			{
				dynamic settings = new ExpandoObject();
				settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				settings.ResizeMode = ResizeMode.NoResize;
				settings.Title = "System Error - Unauthorized Access";

				if (e.Message == "Unauthorized")
				{
					_status.UpdateMessage("Unauthorized Access", "You do not have permission to interact with the Sales form.");
					await _window.ShowDialogAsync(_status, null, settings);
				}
				else
				{
					_status.UpdateMessage("Fatal Exception", e.Message);
					await _window.ShowDialogAsync(_status, null, settings);
				}

				await TryCloseAsync();
			}
		}

		private async Task ResetSalesViewModel()
		{
			Cart = new BindingList<CartItemDisplayModel>();
			ItemQuantity = 1;
			await LoadProducts();

			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
		}
		public async Task LoadProducts()
		{
			var productList = await _productEndpoint.GetAll();
			var products = _mapper.Map<List<ProductDisplayModel>>(productList);
			Products = new BindingList<ProductDisplayModel>(products);
		}

		private BindingList<ProductDisplayModel> _products;
		public BindingList<ProductDisplayModel> Products
		{
			get => _products;
			set
			{
				_products = value;
				NotifyOfPropertyChange(() => Products);
			}
		}

		private ProductDisplayModel _selectedProduct;
		public ProductDisplayModel SelectedProduct
		{
			get => _selectedProduct;
			set
			{
				_selectedProduct = value;
				NotifyOfPropertyChange(() => SelectedProduct);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}
		private CartItemDisplayModel _selectedCartItem;
		public CartItemDisplayModel SelectedCartItem
		{
			get => _selectedCartItem;
			set
			{
				_selectedCartItem = value;
				NotifyOfPropertyChange(() => SelectedCartItem);
				NotifyOfPropertyChange(() => CanRemoveFromCart);
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

		private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
		public BindingList<CartItemDisplayModel> Cart
		{
			get => _cart;
			set
			{
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}

		public string SubTotal => CalculateSubTotal().ToString("C");
		public string Tax => CalculateTotalTax().ToString("C2");
		public string Total
		{
			get
			{
				decimal total = CalculateSubTotal() + CalculateTotalTax();
				return total.ToString("C2");
			}
		}

		private decimal CalculateSubTotal() 
			=> Cart.Sum(item => (item.Product.RetailPrice * item.QuantityInCart));

		private decimal CalculateTotalTax()
		{
			decimal taxRate = _configHelper.GetTaxRate() / 100;
			return Cart
				.Where(item => item.Product.IsTaxable)
				.Sum(item => (item.Product.RetailPrice * item.QuantityInCart) * taxRate);
		}

		public bool CanAddToCart 
			=> ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity;

		public void AddToCart()
		{
			var existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
			if (existingItem != null)
			{
				existingItem.QuantityInCart += ItemQuantity;
			}
			else
			{
				var item = new CartItemDisplayModel
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
			NotifyOfPropertyChange(() => CanCheckOut);
		}

		public bool CanRemoveFromCart => SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0;

		public void RemoveFromCart()
		{
			SelectedCartItem.Product.QuantityInStock += 1;

			if (SelectedCartItem.QuantityInCart > 1)
				SelectedCartItem.QuantityInCart -= 1;
			else
				Cart.Remove(SelectedCartItem);

			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
			NotifyOfPropertyChange(() => CanAddToCart);
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

			await _saleEndpoint.PostSale(sale);

			await ResetSalesViewModel();
		}
	}
}
using Caliburn.Micro;
using System.ComponentModel;

namespace TRMDesktopUI.ViewModels
{
	public class SalesViewModel : Screen
	{
		private BindingList<string> _products;
		public BindingList<string> Products
		{
			get => _products;
			set
			{
				_products = value;
				NotifyOfPropertyChange(() => Products);
			}
		}

		private string _itemQuantity;
		public string ItemQuantity
		{
			get => _itemQuantity;
			set {
				_itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
			}
		}

		private BindingList<string> _cart;
		public BindingList<string> Cart
		{
			get => _cart;
			set
			{
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}

		public string SubTotal
		{
			get
			{
				// TODO - Replace with calculation
				return "$0.00";
			}
		}
		public string Tax
		{
			get
			{
				// TODO - Replace with calculation
				return "$0.00";
			}
		}
		public string Total
		{
			get
			{
				// TODO - Replace with calculation
				return "$0.00";
			}
		}

		public bool CanAddToCart
		{
			get
			{
				bool output = false;

				// Make sure something is selected
				// Make sure there is an item quantity

				return output;
			}
		}
		public void AddToCart()
		{

		}

		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;

				// Make sure something is selected

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
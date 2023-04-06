using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TRMDesktopUI.Models
{
	public class CartItemDisplayModel : INotifyPropertyChanged
	{
		public ProductDisplayModel Product { get; set; }
		private int _quantityInCart;

		public int QuantityInCart
		{
			get => _quantityInCart;
			set
			{
				_quantityInCart = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(DisplayText));
			}
		}


		public string DisplayText => $"{Product.ProductName} ({QuantityInCart})";

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) 
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) return false;
			field = value;
			OnPropertyChanged(propertyName);
			return true;
		}
	}
}
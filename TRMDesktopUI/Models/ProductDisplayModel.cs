using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TRMDesktopUI.Models
{
	public class ProductDisplayModel : INotifyPropertyChanged
	{
		public int Id { get; set; }
		public string ProductName { get; set; }
		public string Description { get; set; }
		public decimal RetailPrice { get; set; }

		private int _quantityInStock;
		public int QuantityInStock
		{
			get => _quantityInStock;
			set
			{
				_quantityInStock = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(DisplayText));
			}

		}
		public bool IsTaxable { get; set; }

		public string DisplayText => $"{ProductName} ({QuantityInStock})";

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
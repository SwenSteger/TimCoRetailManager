using System.Reflection;
using System.Windows.Controls;
using System.Windows;

namespace TRMDesktopUI.Helpers
{
	public static class PasswordBoxHelper
	{
		public static readonly DependencyProperty BoundPasswordProperty =
			DependencyProperty.RegisterAttached("BoundPassword",
				typeof(string),
				typeof(PasswordBoxHelper),
				new FrameworkPropertyMetadata(string.Empty, OnBoundPasswordChanged));

		public static string GetBoundPassword(DependencyObject dependency)
		{
			if (!(dependency is PasswordBox passwordBox)) 
				return (string)dependency.GetValue(BoundPasswordProperty);

			// Ensure that we've hooked the PasswordChanged event once, and only once.
			passwordBox.PasswordChanged -= PasswordChanged;
			passwordBox.PasswordChanged += PasswordChanged;

			return (string)dependency.GetValue(BoundPasswordProperty);
		}

		public static void SetBoundPassword(DependencyObject dependency, string value)
		{
			if (string.Equals(value, GetBoundPassword(dependency)))
				return; // prevent infinite recursion

			dependency.SetValue(BoundPasswordProperty, value);
		}

		private static void OnBoundPasswordChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArgs)
		{
			if (!(dependency is PasswordBox passwordBox))
				return;

			passwordBox.Password = GetBoundPassword(dependency);
		}

		private static void PasswordChanged(object sender, RoutedEventArgs e)
		{
			var passwordBox = sender as PasswordBox;

			SetBoundPassword(passwordBox, passwordBox?.Password);

			// set cursor past the last character in the password box
			passwordBox?.GetType()
				.GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic)
				?.Invoke(passwordBox, new object[] { passwordBox.Password.Length, 0 });
		}
	}
}
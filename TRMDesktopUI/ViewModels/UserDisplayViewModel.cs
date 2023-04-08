using Caliburn.Micro;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System;
using TRMDesktopUI.Models;
using TRMFrontEnd.Library.Api;
using TRMFrontEnd.Library.Models;

namespace TRMDesktopUI.ViewModels
{
	public class UserDisplayViewModel : Screen
	{
		private readonly StatusInfoViewModel _status;
		private readonly IWindowManager _window;
		private readonly IUserEndpoint _userEndpoint;

		private BindingList<UserModel> _users;
		public BindingList<UserModel> Users
		{
			get => _users;
			set
			{
				_users = value;
				NotifyOfPropertyChange(() => Users);
			}
		}

		public UserDisplayViewModel(StatusInfoViewModel status, IWindowManager window, IUserEndpoint userEndpoint)
		{
			_status = status;
			_window = window;
			_userEndpoint = userEndpoint;
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			try
			{
				await LoadUsers();
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

		public async Task LoadUsers()
		{
			var productList = await _userEndpoint.GetAll();
			Users = new BindingList<UserModel>(productList);
		}
	}
}
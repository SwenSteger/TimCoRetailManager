using Caliburn.Micro;
using System.ComponentModel;
using System.Dynamic;
using System.Threading.Tasks;
using System.Windows;
using System;
using System.Linq;
using TRMFrontEnd.Library.Api;
using TRMFrontEnd.Library.Models;

namespace TRMDesktopUI.ViewModels
{
	public class UserDisplayViewModel : Screen
	{
		private readonly StatusInfoViewModel _status;
		private readonly IWindowManager _window;
		private readonly IUserEndpoint _userEndpoint;

		private BindingList<UserModel> _users = new BindingList<UserModel>();
		public BindingList<UserModel> Users
		{
			get => _users;
			set
			{
				_users = value;
				NotifyOfPropertyChange(() => Users);
			}
		}

		private UserModel _selectedUser;
		public UserModel SelectedUser
		{
			get => _selectedUser;
			set
			{
				_selectedUser = value;
				SelectedUserName = _selectedUser.Email;
				UserRoles.Clear();
				UserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
				LoadRoles();
				NotifyOfPropertyChange(() => SelectedUser);
			}
		}

		private string _selectedUserRole;
		public string SelectedUserRole
		{
			get => _selectedUserRole;
			set
			{
				_selectedUserRole = value;
				NotifyOfPropertyChange(() => SelectedUserRole);
			}
		}

		private string _selectedAvailableRole;
		public string SelectedAvailableRole
		{
			get => _selectedAvailableRole;
			set
			{
				_selectedAvailableRole = value;
				NotifyOfPropertyChange(() => SelectedAvailableRole);
			}
		}

		private string _selectedUserName;
		public string SelectedUserName
		{
			get => _selectedUserName;
			set
			{
				_selectedUserName = value;
				NotifyOfPropertyChange(() => SelectedUserName);
			}
		}

		private BindingList<string> _userRoles = new BindingList<string>();
		public BindingList<string> UserRoles
		{
			get => _userRoles;
			set
			{
				_userRoles = value;
				NotifyOfPropertyChange(() => UserRoles);
			}
		}

		private BindingList<string> _availableRoles = new BindingList<string>();
		public BindingList<string> AvailableRoles
		{
			get => _availableRoles;
			set
			{
				_availableRoles = value;
				NotifyOfPropertyChange(() => AvailableRoles);
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

		private async Task LoadRoles()
		{
			var roles = await _userEndpoint.GetAllRoles();

			foreach (var role in roles)
			{
				if (UserRoles.IndexOf(role.Value) < 0) 
					AvailableRoles.Add(role.Value);
			}
		}

		public async Task AddSelectedRole()
		{
			await _userEndpoint.AddUserToRole(SelectedUser.Id, SelectedAvailableRole);

			UserRoles.Add(SelectedAvailableRole);
			AvailableRoles.Remove(SelectedAvailableRole);
		}

		public async Task RemoveSelectedRole()
		{
			await _userEndpoint.RemoveUserFromRole(SelectedUser.Id, SelectedUserRole);

			UserRoles.Remove(SelectedUserRole);
			AvailableRoles.Add(SelectedUserRole);
		}
	}
}
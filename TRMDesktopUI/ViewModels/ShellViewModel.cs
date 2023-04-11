using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using TRMDesktopUI.EventModels;
using TRMFrontEnd.Library.Api;
using TRMFrontEnd.Library.Models;

namespace TRMDesktopUI.ViewModels
{
	public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
	{
		private readonly IEventAggregator _events;
		private readonly ILoggedInUserModel _user;
		private readonly IApiHelper _apiHelper;

		public ShellViewModel(IEventAggregator events, ILoggedInUserModel user, IApiHelper apiHelper)
		{
			_user = user;
			_apiHelper = apiHelper;
			_events = events;
			_events.SubscribeOnPublishedThread(this);
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			await ActivateLoginViewModel();
		}

		public bool IsLoggedIn => !string.IsNullOrWhiteSpace(_user.Token);

		public async Task LogOut()
		{
			_user.LogOffUser();
			_apiHelper.Logout();
			await ActivateLoginViewModel();
			NotifyOfPropertyChange(() => IsLoggedIn);
		}
		public void ExitApplication() 
			=> TryCloseAsync();

		public async Task ActivateLoginViewModel() 
			=> await ActivateItemAsync(IoC.Get<LoginViewModel>());
		public async Task UserManagement()
			=> await ActivateItemAsync(IoC.Get<UserDisplayViewModel>());

		public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken = default)
		{
			await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);
			NotifyOfPropertyChange(() => IsLoggedIn);
		}
	}
}
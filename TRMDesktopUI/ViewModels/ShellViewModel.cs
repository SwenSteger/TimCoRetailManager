using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using TRMDesktopUI.EventModels;

namespace TRMDesktopUI.ViewModels
{
	public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
	{
		private readonly IEventAggregator _events;
		private readonly SalesViewModel _salesViewModel;
		private readonly SimpleContainer _container;

		public ShellViewModel(IEventAggregator events, SalesViewModel salesViewModel, SimpleContainer container)
		{
			_salesViewModel = salesViewModel;
			_container = container;
			_events = events;
			_events.SubscribeOnPublishedThread(this);
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			await ActivateLoginViewModel();
		}

		public async Task ActivateLoginViewModel()
		{
			await ActivateItemAsync(_container.GetInstance<LoginViewModel>());
		}

		public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken = default)
		{
			await ActivateItemAsync(_salesViewModel, cancellationToken);
		}
	}
}
using Xamarin.Forms;

namespace FormsPushNotificationSample
{
	abstract class BasePage<TViewModel> : ContentPage where TViewModel : BaseViewModel, new()
	{
		#region Fields
		readonly TViewModel _viewModel;
		#endregion

		#region Constructor
		public BasePage()
		{
			_viewModel = new TViewModel();
			BindingContext = ViewModel;
		}
		#endregion

		#region Properties
		protected TViewModel ViewModel => _viewModel;
		#endregion

		#region Methods
		protected override void OnAppearing()
		{
			base.OnAppearing();

			SubscribeEventHanlders();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			UnsubscribeEventHanlders();
		}

		protected abstract void SubscribeEventHanlders();
		protected abstract void UnsubscribeEventHanlders();
		#endregion
	}
}

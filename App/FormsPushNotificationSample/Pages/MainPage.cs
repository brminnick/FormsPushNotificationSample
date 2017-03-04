using Xamarin.Forms;

namespace FormsPushNotificationSample
{
	class MainPage : ContentPage
	{
		public MainPage()
		{
			var viewModel = new MainViewModel();
			BindingContext = viewModel;

			var registerForPushNotificationsButton = new Button
			{
				Text = "Register For Push Notifications"
			};
			registerForPushNotificationsButton.SetBinding(Button.CommandProperty, nameof(viewModel.RegisterForPushNotificationsButtonCommand));

			var checkPushNotificationsSettingsButton = new Button
			{
				Text = "Check Push Notification Settings"
			};
			checkPushNotificationsSettingsButton.SetBinding(Button.CommandProperty, nameof(viewModel.CheckPushNotificationsSettingsButtonCommand));

			var requestPushNotificationButton = new Button
			{
				Text = "Request Push Notification"
			};
			requestPushNotificationButton.SetBinding(Button.CommandProperty, nameof(viewModel.RequestPushNotificationButtonCommand));

			viewModel.DisplayNotificationDialog += HandleDisplayNotificationDialog;

			Content = new StackLayout
			{
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				Children = {
					registerForPushNotificationsButton,
					checkPushNotificationsSettingsButton,
					requestPushNotificationButton
				}
			};
		}

		#region Methods
		async void HandleDisplayNotificationDialog(object sender, PushNotificationAlertEventArgs e)
		{
			if (string.IsNullOrEmpty(e.AcceptText))
				await DisplayAlert(e.TitleText, e.MessageText, e.CancelText);
		}
		#endregion
	}
}

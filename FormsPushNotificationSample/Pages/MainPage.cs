using System;
using Xamarin.Forms;

namespace FormsPushNotificationSample
{
	class MainPage : BasePage<MainViewModel>
	{
		#region Constructors
		public MainPage()
		{
			var registerForPushNotificationsButton = new Button
			{
				Text = "Register For Push Notifications"
			};
			registerForPushNotificationsButton.SetBinding(Button.CommandProperty, nameof(ViewModel.RegisterForPushNotificationsButtonCommand));

			var checkPushNotificationsSettingsButton = new Button
			{
				Text = "Check Push Notification Settings"
			};
			checkPushNotificationsSettingsButton.SetBinding(Button.CommandProperty, nameof(ViewModel.CheckPushNotificationsSettingsButtonCommand));

			var requestPushNotificationButton = new Button
			{
				Text = "Request Push Notification"
			};
			requestPushNotificationButton.SetBinding(Button.CommandProperty, nameof(ViewModel.RequestPushNotificationButtonCommand));

			var clearBadgeNotificationsButton = new Button
			{
				Text = "Clear Badge Notifications"
			};
			clearBadgeNotificationsButton.SetBinding(Button.CommandProperty, nameof(ViewModel.ClearBadgeNotificationsButton));

			Content = new StackLayout
			{
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				Children = {
					registerForPushNotificationsButton,
					checkPushNotificationsSettingsButton,
					requestPushNotificationButton,
					clearBadgeNotificationsButton
				}
			};
		}
		#endregion

		#region Methods
		protected override void SubscribeEventHanlders()
		{
			ViewModel.SuccessNotificationFired += HandleSuccessNotificationFired;
			ViewModel.OpenSettingsNotificationFired += HandleOpenSettingsNotificationFired;
			ViewModel.DeviceNotRegisteredNotificationFired += HandleDeviceNotRegisteredNotificationFired;
		}

		protected override void UnsubscribeEventHanlders()
		{
			ViewModel.SuccessNotificationFired -= HandleSuccessNotificationFired;
			ViewModel.OpenSettingsNotificationFired -= HandleOpenSettingsNotificationFired;
			ViewModel.DeviceNotRegisteredNotificationFired -= HandleDeviceNotRegisteredNotificationFired;
		}

		async void HandleOpenSettingsNotificationFired(object sender, NotificationAlertEventArgs e)
		{
			var didUserAccept = await DisplayAlert(e.TitleText, e.MessageText, e.AcceptText, e.CancelText);

			if (didUserAccept)
				DependencyService.Get<IPushNotificationServices>()?.OpenPushNotificationSettings();
		}

		async void HandleDeviceNotRegisteredNotificationFired(object sender, NotificationAlertEventArgs e)
		{
			var didUserAccept = await DisplayAlert(e.TitleText, e.MessageText, e.AcceptText, e.CancelText);

			if (didUserAccept)
				DependencyService.Get<IPushNotificationServices>()?.RegisterDeviceForPushNotifications();
		}

		async void HandleSuccessNotificationFired(object sender, NotificationAlertEventArgs e)
		{
			await DisplayAlert(e.TitleText, e.MessageText, e.CancelText);
		}
		#endregion
	}
}

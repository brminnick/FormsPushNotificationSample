using System;
using System.Windows.Input;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace FormsPushNotificationSample
{
	class MainViewModel : BaseViewModel
	{
		#region Fields
		ICommand _registerForPushNotificationsButtonCommand, _configurePushNotificationsSettingsButtonCommand,
			_requestPushNotificationButtonCommand, _clearBadgeNotificationsButton;
		#endregion

		#region Events
		public event EventHandler<NotificationAlertEventArgs> DeviceNotRegisteredNotificationFired;
		public event EventHandler<NotificationAlertEventArgs> OpenSettingsNotificationFired;
		public event EventHandler<NotificationAlertEventArgs> SuccessNotificationFired;
		#endregion

		#region Properties
		public ICommand RegisterForPushNotificationsButtonCommand => _registerForPushNotificationsButtonCommand ??
			(_registerForPushNotificationsButtonCommand = new Command(ExecuteRegisterForPushNotificationsButtonCommand));

		public ICommand CheckPushNotificationsSettingsButtonCommand => _configurePushNotificationsSettingsButtonCommand ??
			(_configurePushNotificationsSettingsButtonCommand = new Command(async () => await ExecuteCheckPushNotificationsSettingsButtonCommand()));

		public ICommand RequestPushNotificationButtonCommand => _requestPushNotificationButtonCommand ??
			(_requestPushNotificationButtonCommand = new Command(ExecuteRequestPushNotificationButtonCommand));

		public ICommand ClearBadgeNotificationsButton => _clearBadgeNotificationsButton ??
			(_clearBadgeNotificationsButton = new Command(ExecuteClearBadgeNotificationsButton));

		#endregion

		#region Methods
		void ExecuteRegisterForPushNotificationsButtonCommand()
		{
			DependencyService.Get<IPushNotificationServices>().RegisterDeviceForPushNotifications();
		}

		async Task ExecuteCheckPushNotificationsSettingsButtonCommand()
		{
			var isDeviceRegisteredForPushNotifications = await DependencyService.Get<IPushNotificationServices>().IsDeviceRegisteredForRemotePushNotifications();

			if (!isDeviceRegisteredForPushNotifications)
			{
				OnDeviceNotRegisteredNotificationFired("Warning", "Device Not Registered For Push Notifications", "Cancel", "Register");
			}
			else
			{
				var arePushNotificationsEnabledInSettings = await DependencyService.Get<IPushNotificationServices>().AreNotificationsEnabledInSettings();

				if (!arePushNotificationsEnabledInSettings)
					OnOpenSettingsNotificationFired("Warning", "Push Notifications Disabled In Settings", "Cancel", "Open Settings");
				else
					OnSuccessNotificationFired("Success!", "Device is Registered For Push Notifications", "Ok");
			}
		}

		void ExecuteRequestPushNotificationButtonCommand(object obj)
		{
			DependencyService.Get<IPushNotificationServices>().RequestPushNotification();
		}

		void ExecuteClearBadgeNotificationsButton(object obj)
		{
			DependencyService.Get<IPushNotificationServices>().ClearBadgeNotifications();
		}

		void OnDeviceNotRegisteredNotificationFired(string title, string message, string cancel, string accept)
		{
			DeviceNotRegisteredNotificationFired?.Invoke(this, new NotificationAlertEventArgs(title, message, cancel, accept));
		}

		void OnOpenSettingsNotificationFired(string title, string message, string cancel, string accept)
		{
			OpenSettingsNotificationFired?.Invoke(this, new NotificationAlertEventArgs(title, message, cancel, accept));
		}

		void OnSuccessNotificationFired(string title, string message, string cancel)
		{
			SuccessNotificationFired?.Invoke(this, new NotificationAlertEventArgs(title, message, cancel));
		}
		#endregion
	}
}

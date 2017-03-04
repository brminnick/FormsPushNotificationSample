using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace FormsPushNotificationSample
{
	class MainViewModel : BaseViewModel
	{
		#region Fields
		ICommand _registerForPushNotificationsButtonCommand, _configurePushNotificationsSettingsButtonCommand,
			_requestPushNotificationButtonCommand;
		#endregion

		#region Events
		public event EventHandler<PushNotificationAlertEventArgs> DisplayNotificationDialog;
		#endregion

		#region Properties
		public ICommand RegisterForPushNotificationsButtonCommand => _registerForPushNotificationsButtonCommand ??
			(_registerForPushNotificationsButtonCommand = new Command(ExecuteRegisterForPushNotificationsButtonCommand));

		public ICommand CheckPushNotificationsSettingsButtonCommand => _configurePushNotificationsSettingsButtonCommand ??
			(_configurePushNotificationsSettingsButtonCommand = new Command(ExecuteCheckPushNotificationsSettingsButtonCommand));

		public ICommand RequestPushNotificationButtonCommand => _requestPushNotificationButtonCommand ??
		(_requestPushNotificationButtonCommand = new Command(ExecuteRequestPushNotificationButtonCommand));

		#endregion

		#region Methods
		void ExecuteRegisterForPushNotificationsButtonCommand()
		{
			DependencyService.Get<IPushNotificationServices>().RegisterDeviceForPushNotifications();
		}

		void ExecuteCheckPushNotificationsSettingsButtonCommand()
		{
			var isDeviceRegisteredForPushNotifications = DependencyService.Get<IPushNotificationServices>().IsDeviceRegisteredForRemotePushNotifications();

			if (!isDeviceRegisteredForPushNotifications)
			{
				OnDisplayNotificationDialog("Warning", "Device Not Registered For Push Notifications", "Cancel", "Register");
			}
			else
			{
				var arePushNotificationsEnabledInSettings = DependencyService.Get<IPushNotificationServices>().AreNotificationsEnabledInSettings();

				if (!arePushNotificationsEnabledInSettings)
					OnDisplayNotificationDialog("Warning", "Push Notifications Disabled In Settings", "Cancel", "Open Settings");
				else
					OnDisplayNotificationDialog("Success", "Device Registered For Push Notifications", "Ok");
			}
		}

		void ExecuteRequestPushNotificationButtonCommand(object obj)
		{
			DependencyService.Get<IPushNotificationServices>().RequestPushNotification();
		}

		void OnDisplayNotificationDialog(string title, string message, string cancelText)
		{
			DisplayNotificationDialog?.Invoke(this, new PushNotificationAlertEventArgs(title, message, cancelText));
		}

		void OnDisplayNotificationDialog(string title, string message, string cancelText, string acceptText)
		{
			DisplayNotificationDialog?.Invoke(this, new PushNotificationAlertEventArgs(title, message, cancelText, acceptText));
		}
		#endregion
	}
}

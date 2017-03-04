using System;
using System.Threading.Tasks;

using UIKit;
using Foundation;
using UserNotifications;

using Xamarin.Forms;

using FormsPushNotificationSample.iOS;

[assembly: Dependency(typeof(PushNotificationServices_iOS))]
namespace FormsPushNotificationSample.iOS
{
	public class PushNotificationServices_iOS : IPushNotificationServices
	{
		public bool AreNotificationsEnabledInSettings()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
				return AreNotificationsEnabledInSettings_iOS10();
			
			return AreNotificationsEnabledInSettings_iOS9();
		}

		public bool IsDeviceRegisteredForRemotePushNotifications()
		{
			return UIApplication.SharedApplication.IsRegisteredForRemoteNotifications;
		}

		public void OpenPushNotificationSettings()
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
		}

		public void RegisterDeviceForPushNotifications()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
				RegiserForPushNotifications_IOS10();
			else
				RegisterForPushNotifications_IOS9();
		}

		public void RequestPushNotification()
		{
			throw new NotImplementedException();
		}

		void RegisterForPushNotifications_IOS9()
		{
			var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, new NSSet());

			UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
			UIApplication.SharedApplication.RegisterForRemoteNotifications();
		}

		void RegiserForPushNotifications_IOS10()
		{
			UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, (approved, err) =>
			{
				if (approved)
					UIApplication.SharedApplication.RegisterForRemoteNotifications();
			});
		}

		bool AreNotificationsEnabledInSettings_iOS9()
		{
			var notificationTypes = UIApplication.SharedApplication.CurrentUserNotificationSettings.Types;

			return !notificationTypes.Equals(UIUserNotificationType.None);
		}

		bool AreNotificationsEnabledInSettings_iOS10()
		{
			var tcs = new TaskCompletionSource<bool>();

			UNUserNotificationCenter.Current.GetNotificationSettings((settings) =>
			{
				var areNotificationsEnabledInSettings =
					(settings.AlertSetting == UNNotificationSetting.Enabled) ||
					(settings.BadgeSetting == UNNotificationSetting.Enabled) ||
					(settings.SoundSetting == UNNotificationSetting.Enabled);

				tcs.SetResult(areNotificationsEnabledInSettings);
			});

			return tcs.Task.Result;
		}
	}
}

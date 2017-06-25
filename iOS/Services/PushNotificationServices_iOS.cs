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
		public Task<bool> AreNotificationsEnabledInSettings()
		{
			var tcs = new TaskCompletionSource<bool>();

			UIApplication.SharedApplication.InvokeOnMainThread(async () =>
			{
				if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
					tcs.SetResult(await AreNotificationsEnabledInSettings_iOS10());
				else
					tcs.SetResult(AreNotificationsEnabledInSettings_iOS9());
			});

			return tcs.Task;
		}

		public Task<bool> IsDeviceRegisteredForRemotePushNotifications()
		{
			var tcs = new TaskCompletionSource<bool>();

			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				tcs.SetResult(UIApplication.SharedApplication.IsRegisteredForRemoteNotifications);
			});

			return tcs.Task;
		}

		public void OpenPushNotificationSettings()
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
					OpenPushNotificationSettings_iOS10();
				else
					OpenPushNotificationSettings_iOS9();
			});
		}

		public void RegisterDeviceForPushNotifications()
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
					RegiserForPushNotifications_iOS10();
				else
					RegisterForPushNotifications_iOS9();
			});
		}

		public void RequestPushNotification()
		{
			throw new NotImplementedException();
		}

		public void ClearBadgeNotifications()
		{
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}

		void RegisterForPushNotifications_iOS9()
		{
			var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, new NSSet());

			UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
			UIApplication.SharedApplication.RegisterForRemoteNotifications();
		}

		void RegiserForPushNotifications_iOS10()
		{
			UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, async (approved, err) =>
			{
				var isDeviceRegisteredForPushNotifications = await DependencyService.Get<IPushNotificationServices>().IsDeviceRegisteredForRemotePushNotifications();

				if (approved && !isDeviceRegisteredForPushNotifications)
					UIApplication.SharedApplication.InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
			});
		}

		bool AreNotificationsEnabledInSettings_iOS9()
		{
			var notificationTypes = UIApplication.SharedApplication.CurrentUserNotificationSettings.Types;

			return !notificationTypes.Equals(UIUserNotificationType.None);
		}

		Task<bool> AreNotificationsEnabledInSettings_iOS10()
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

			return tcs.Task;
		}

		void OpenPushNotificationSettings_iOS9()
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
		}

		void OpenPushNotificationSettings_iOS10()
		{
			var options = new UIApplicationOpenUrlOptions
			{
				SourceApplication = "com.minnick.formspushnotificationsample"
			};

			UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString), options, null);
		}
	}
}

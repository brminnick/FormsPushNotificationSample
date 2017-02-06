using UIKit;
using Foundation;

using Microsoft.WindowsAzure.MobileServices;
using System;
using UserNotifications;

namespace FormsPushNotificationSample.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		MobileServiceClient Client { get; set; }

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}

		void RegisterForPushNotifications()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				RegiserForPushNotificationsIOS10();
			}
			else
			{
				RegisterForPushNotificationsIOS9();
			}

		}

		void RegisterForPushNotificationsIOS9()
		{
			var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, new NSSet());
			UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
			UIApplication.SharedApplication.RegisterForRemoteNotifications();
		}

		void RegiserForPushNotificationsIOS10()
		{
			UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
			{
				UIApplication.SharedApplication.RegisterForRemoteNotifications();
			});
		}
	}
}

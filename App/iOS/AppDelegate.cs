using System;

using UIKit;
using Foundation;
using UserNotifications;

using Microsoft.WindowsAzure.MobileServices;

using Newtonsoft.Json.Linq;

namespace FormsPushNotificationSample.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		MobileServiceClient Client { get; set; }

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			CurrentPlatform.Init();

			LoadApplication(new App());

			RegisterForPushNotifications();

			return base.FinishedLaunching(app, options);
		}

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			var client = new MobileServiceClient(AzureConstants.AzureAppServiceUrl);

			var push = client.GetPush();
			push.RegisterAsync(deviceToken);
		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			DisplayAlert("Failed To Register Remote Notifications", error.ToString());
		}

		public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			DisplayAlert("Success!", "Push Notification Received");
		}

		void RegisterForPushNotifications()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				RegiserForPushNotifications_IOS10();
			}
			else
			{
				RegisterForPushNotifications_IOS9();
			}

		}

		void RegisterForPushNotifications_IOS9()
		{
			var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, new NSSet());

			InvokeOnMainThread(() =>
			{
				UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
				UIApplication.SharedApplication.RegisterForRemoteNotifications();
			});
		}

		void RegiserForPushNotifications_IOS10()
		{
			UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
			{
				if (approved)
					InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
			});
		}

		void DisplayAlert(string title, string message, Action completionHandler = null)
		{
			var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

			alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));

			ViewControllerHelpers.GetVisibleViewController().PresentViewController(alert, true, completionHandler);
		}
	}
}

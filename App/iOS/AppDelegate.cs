using System;

using UIKit;
using Foundation;

using Microsoft.WindowsAzure.MobileServices;

using FormsPushNotificationSample.Shared;

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

			return base.FinishedLaunching(app, options);
		}

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			var client = new MobileServiceClient(AzureConstants.AzureAppServiceUrl);

			var push = client.GetPush();
			push?.RegisterAsync(deviceToken);
		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			DisplayAlert("Failed To Register Remote Notifications", error.ToString());
		}

		public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			DisplayAlert("Success!", "Push Notification Received");
		}	

		void DisplayAlert(string title, string message, Action completionHandler = null)
		{
			var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

			alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));

			ViewControllerHelpers.GetVisibleViewController().PresentViewController(alert, true, completionHandler);
		}
	}
}

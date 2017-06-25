using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using UIKit;
using Foundation;

using Microsoft.WindowsAzure.MobileServices;

using FormsPushNotificationSample.Shared;

namespace FormsPushNotificationSample.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		#region Fields
		readonly MobileServiceClient _client = new MobileServiceClient(AzureConstants.AzureAppServiceUrl);
		#endregion

		public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
		{
			global::Xamarin.Forms.Forms.Init();

			CurrentPlatform.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(uiApplication, launchOptions);
		}

		public override async void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			await RegisterForAzurePushNotifications(deviceToken);
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

		async Task RegisterForAzurePushNotifications(NSData deviceToken)
		{
			if (deviceToken == null)
				return;


			var registrationId = deviceToken.Description
				.Trim('<', '>').Replace(" ", string.Empty).ToUpperInvariant();

			var push = _client.GetPush();

			var installation = new DeviceInstallation
			{
				InstallationId = _client.InstallationId,
				Platform = "apns",
				PushChannel = registrationId
			};

			// Set up tags to request
			installation.Tags.Add("topic:Sports");
			// Set up templates to request
			var genericTemplate = new PushTemplate
			{
				Body = "{\"aps\":{\"alert\":\"$(messageParam)\"}}"
			};
			installation.Templates.Add("genericTemplate", genericTemplate);

			try
			{
				// Register with NH
				var response = await _client.InvokeApiAsync<DeviceInstallation, DeviceInstallation>(
					$"/push/installations/{_client.InstallationId}",
					installation,
					HttpMethod.Put,
					new Dictionary<string, string>());
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Fail($"[iOSPlatformProvider]: Could not register with NH: {ex.Message}");
			}

		}
	}
}

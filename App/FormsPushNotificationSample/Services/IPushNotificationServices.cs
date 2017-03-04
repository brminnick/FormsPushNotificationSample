namespace FormsPushNotificationSample
{
	public interface IPushNotificationServices
	{
		void RegisterDeviceForPushNotifications();
		void RequestPushNotification();
		void OpenPushNotificationSettings();
		bool AreNotificationsEnabledInSettings();
		bool IsDeviceRegisteredForRemotePushNotifications();
	}
}

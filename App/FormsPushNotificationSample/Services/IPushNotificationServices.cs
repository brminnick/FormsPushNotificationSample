using System.Threading.Tasks;

namespace FormsPushNotificationSample
{
	public interface IPushNotificationServices
	{
		void RegisterDeviceForPushNotifications();
		void RequestPushNotification();
		void OpenPushNotificationSettings();
		Task<bool> AreNotificationsEnabledInSettings();
		Task<bool> IsDeviceRegisteredForRemotePushNotifications();
		void ClearBadgeNotifications();
	}
}

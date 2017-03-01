using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;

namespace FormsPushNotificationBackend
{
    public static class Notifications
    {
        #region Constant Fields
        const string _defaultFullSharedAccessSignature = @"Endpoint=sb://formspushnotificationsample.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=DWP3/YEY3fUjZf01BbJP5od3kGBq+iqHe8eUAVbsFSE=";
        const string _hubName = "FormsPushNotificationSampleNotificationHub";
        #endregion

        #region Fields
        static NotificationHubClient _hub;
        #endregion

        #region Properties
        public static NotificationHubClient Hub => _hub ?? 
                (_hub = NotificationHubClient.CreateClientFromConnectionString(_defaultFullSharedAccessSignature, _hubName));

        public static Task<NotificationOutcome> Instance { get; internal set; }
        #endregion
    }
}
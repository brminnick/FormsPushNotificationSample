using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

using FormsPushNotificationBackend.Models;

namespace FormsPushNotificationBackend.Controllers
{
    public class NotificationsController : ApiController
    {
        public async Task<HttpResponseMessage> Post(string pns, [FromBody]string message, string to_tag)
        {
            var user = HttpContext.Current.User.Identity.Name;
            string[] userTag = new string[2];
            userTag[0] = "username:" + to_tag;
            userTag[1] = "from:" + user;

            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
            HttpStatusCode ret = HttpStatusCode.InternalServerError;

            switch (pns.ToUpper())
            {
                case "WNS":
                    // Windows 8.1 / Windows Phone 8.1
                    var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" +
                                "From " + user + ": " + message + "</text></binding></visual></toast>";
                    outcome = await Notifications.Hub.SendWindowsNativeNotificationAsync(toast, userTag);
                    break;
                case "APNS":
                    // iOS
                    var alert = "{\"aps\":{\"alert\":\"" + "From " + user + ": " + message + "\"}}";
                    outcome = await Notifications.Hub.SendAppleNativeNotificationAsync(alert, userTag);
                    break;
                case "GCM":
                    // Android
                    var notif = "{ \"data\" : {\"message\":\"" + "From " + user + ": " + message + "\"}}";
                    outcome = await Notifications.Hub.SendGcmNativeNotificationAsync(notif, userTag);
                    break;
            }

            if (outcome != null && 
                (!((outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Abandoned) ||
                    (outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Unknown))))
                ret = HttpStatusCode.OK;
            
            return Request.CreateResponse(ret);
        }
    }
}

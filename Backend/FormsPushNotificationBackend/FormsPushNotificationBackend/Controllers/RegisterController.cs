using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;

namespace FormsPushNotificationBackend.Controllers
{
    public class RegisterController : ApiController
    {
        #region Methods
        // POST api/register
        // This creates a registration id
        public async Task<string> Post(string handle = null)
        {
            string newRegistrationId = null;

            // make sure there are no existing registrations for this push handle (used for iOS and Android)
            if (handle != null)
            {
                var registrations = await Notifications.Hub.GetRegistrationsByChannelAsync(handle, 100);

                foreach (RegistrationDescription registration in registrations)
                {
                    if (newRegistrationId == null)
                        newRegistrationId = registration.RegistrationId;
                    else
                        await Notifications.Hub.DeleteRegistrationAsync(registration);
                }
            }

            if (newRegistrationId == null)
                newRegistrationId = await Notifications.Hub.CreateRegistrationIdAsync();

            return newRegistrationId;
        }

        // PUT api/register/5
        // This creates or updates a registration (with provided channelURI) at the specified id
        public async Task<HttpResponseMessage> Put(string id, DeviceRegistration deviceUpdate)
        {
            RegistrationDescription registration = null;

            switch (deviceUpdate.Platform)
            {
                case "mpns":
                    registration = new MpnsRegistrationDescription(deviceUpdate.Handle);
                    break;
                case "wns":
                    registration = new WindowsRegistrationDescription(deviceUpdate.Handle);
                    break;
                case "apns":
                    registration = new AppleRegistrationDescription(deviceUpdate.Handle);
                    break;
                case "gcm":
                    registration = new GcmRegistrationDescription(deviceUpdate.Handle);
                    break;
                default:
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            registration.RegistrationId = id;
            var username = HttpContext.Current.User.Identity.Name;

            // add check if user is allowed to add these tags
            registration.Tags = new HashSet<string>(deviceUpdate.Tags);
            registration.Tags.Add("username:" + username);

            try
            {
                await Notifications.Hub.CreateOrUpdateRegistrationAsync(registration);
            }
            catch (MessagingException e)
            {
                ReturnGoneIfHubResponseIsGone(e);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/register/5
        public async Task<HttpResponseMessage> Delete(string id)
        {
            await Notifications.Hub.DeleteRegistrationAsync(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        static void ReturnGoneIfHubResponseIsGone(MessagingException e)
        {
            var webex = e.InnerException as WebException;
            if (webex?.Status != WebExceptionStatus.ProtocolError)
                return;

            var response = webex?.Response as HttpWebResponse;
            if (response?.StatusCode == HttpStatusCode.Gone)
                throw new HttpRequestException(HttpStatusCode.Gone.ToString());

        }
        #endregion 

        #region Classes
        public class DeviceRegistration
        {
            public string Platform { get; set; }
            public string Handle { get; set; }
            public string[] Tags { get; set; }
        }
        #endregion  
    }


}

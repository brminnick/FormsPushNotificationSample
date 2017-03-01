using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace FormsPushNotificationBackend
{
    public class AuthenticationTestHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            const string beginningAuthorizationHeaderText = "Basic ";

            var authorizationHeader = request.Headers.GetValues("Authorization").FirstOrDefault();
            var isHeaderValid = authorizationHeader?.StartsWith(beginningAuthorizationHeaderText, StringComparison.InvariantCultureIgnoreCase);

            if (isHeaderValid == false)
                return Unauthorized();
       
            string authorizationUserAndPwdBase64 = authorizationHeader.Substring(beginningAuthorizationHeaderText.Length);
            string authorizationUserAndPwd = Encoding.Default.GetString(Convert.FromBase64String(authorizationUserAndPwdBase64));
            string user = authorizationUserAndPwd.Split(':').FirstOrDefault();
            string password = authorizationUserAndPwd.Split(':').LastOrDefault();

            if (!verifyUserAndPwd(user, password))
                return Unauthorized();

            // Attach the new principal object to the current HttpContext object
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(user), new string[0]);
            Thread.CurrentPrincipal = HttpContext.Current.User;

            return base.SendAsync(request, cancellationToken);
        }

        private bool verifyUserAndPwd(string user, string password)
        {
            // This is not a real authentication scheme.
            return user == password;
        }

        private Task<HttpResponseMessage> Unauthorized()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }
    }
}
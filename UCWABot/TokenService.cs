using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Configuration;

namespace UCWABot
{
    public class TokenService
    {
        private static string tenant = ConfigurationManager.AppSettings["Tenant"].ToString();
        private static string username = ConfigurationManager.AppSettings["Username"].ToString();
        private static string password = ConfigurationManager.AppSettings["Password"].ToString();
        private static string clientId = ConfigurationManager.AppSettings["ClientId"].ToString();
        private static string sfbResourceId = "00000004-0000-0ff1-ce00-000000000000";
        private static string aadInstance = "https://login.microsoftonline.com/{0}";

        static public string AquireAADToken(string resourceId = "")
        {
            if (string.IsNullOrEmpty(resourceId))
                resourceId = sfbResourceId;
            else
                resourceId = new Uri(resourceId).Scheme + "://" + new Uri(resourceId).Host;

            var authContext = new AuthenticationContext(string.Format(aadInstance, tenant));

            var cred = new UserPasswordCredential(username, password);
            AuthenticationResult authenticationResult = null;

            try
            {
                authenticationResult = authContext.AcquireTokenAsync(resourceId, clientId, cred).Result;
            }
            catch (Exception ex)
            {
                throw;
            }

            return authenticationResult.AccessToken;
        }
    }
}

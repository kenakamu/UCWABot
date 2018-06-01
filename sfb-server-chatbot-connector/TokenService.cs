using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Linq;
using System.Configuration;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

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

        static public string AquireOnPremToken(string resourceId = "")
        {
            // You may want to consider cache the token as it only expired in 8 hours.
            // https://msdn.microsoft.com/en-us/skype/ucwa/authenticationinucwa
            using (HttpClient client = new HttpClient())
            {
                // Get OAuth service url
                var response = client.GetAsync(resourceId).Result;
                var wwwAuthenticate = response.Headers.WwwAuthenticate;
                var uri = wwwAuthenticate.Where(x => x.Scheme == "MsRtcOAuth").First().Parameter.Split(',').Where(y => y.Contains("href")).First().Split('=')[1].Replace("\"", "");

                // Obtain AccessToken
                response = client.PostAsync(uri, new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("grant_type", "password"), new KeyValuePair<string, string>("username", username), new KeyValuePair<string, string>("password", password) })).Result;
                return JToken.Parse(response.Content.ReadAsStringAsync().Result)["access_token"].ToString();
            }         
        }
    }
}

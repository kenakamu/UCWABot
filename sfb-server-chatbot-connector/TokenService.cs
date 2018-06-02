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
        private static string username = ConfigurationManager.AppSettings["SfbUsername"].ToString();
        private static string password = ConfigurationManager.AppSettings["SfbPassword"].ToString();

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

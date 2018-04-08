using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Web;
using Twendanishe.DataContracts;

namespace Twendanishe.Business
{
    /// <summary>
    /// This is the proxy for Where is Transport API.
    /// </summary>
    public class WhereIsMyTransportApiService
    {
        private readonly IConfiguration _configuration;
        private static readonly HttpClient _client = new HttpClient();
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        private string _token;

        public string Token
        {
            get { GetToken();  return _token; }
            set { _token = value; }
        }

        public DateTime TokenExpiresAt { get; set; }

        private bool _disableToken = true;
        public WhereIsMyTransportApiService(IConfiguration configuration) {
            _configuration = configuration;
            ClientId = _configuration.GetValue<string>("WIMTAId");
            ClientSecret = _configuration.GetValue<string>("WIMTASecret");

            // Configure HttpClient
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Content-Type", "application/json");

            // Can we get some token
            if (Token != null && !_disableToken)
            {
                _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Token);
                _disableToken = false;
            }
            
        }

        /// <summary>
        /// Processes an HttpRequest to obtain a token
        /// </summary>
        private async void GetToken() {
            _disableToken = false;
            if (Token == null || TokenExpiresAt == null || TokenExpiresAt <= DateTime.Now)
            {
                var data = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>( "client_id", ClientId ),
                    new KeyValuePair<string, string>( "client_secret", ClientSecret ),
                    new KeyValuePair<string, string>( "grant_type", "client_credentials" ),
                    new KeyValuePair<string, string>( "scope", "transitapi:all" )
                };

                var tokenRequest = await _client.PostAsync(
                    "https://identity.whereismytransport.com/connect/token", 
                    new StringContent(BuildJsonString(data)));

                if (tokenRequest.IsSuccessStatusCode)
                {
                    var tokenDataSerializer = new DataContractJsonSerializer(typeof(TokenResponse));
                    var tokenData = tokenDataSerializer.ReadObject(
                        await tokenRequest.Content.ReadAsStreamAsync()) as TokenResponse;
                    Token = tokenData.access_token;
                    TokenExpiresAt = DateTime.Now.AddSeconds(tokenData.expires_in);
                }
            }
        }

        /// <summary>
        /// An agency, or operator, is an organisation which provides and governs 
        /// a transport service which is available for use by either the general 
        /// public (in most cases) or through some private arrangement.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="agencies">
        /// A string of comma-separated agency identifiers which to filter the results by.
        /// </param>
        /// <param name="point">
        /// The point from where to search for nearby agencies. 
        /// Agencies will be returned in order of their distance from 
        /// this point (from closest to furthest).
        /// </param>
        /// <param name="radius">
        /// The distance in metres from the point to search for nearby agencies.
        /// This filter is optional.
        /// </param>
        /// <param name="bbox">
        /// The bounding box from where to retrieve agencies. 
        /// This will be ignored if a point is provided in the query.
        /// </param>
        /// <param name="exclude">
        /// A string of comma-separated object or collection names to exclude from the response.
        /// </param>
        /// <param name="limit">
        /// Limit for results
        /// </param>
        /// <param name="offset">Results start</param>
        /// <returns></returns>
        public async Task<string> Agencies(
            string id = null, string agencies = null, string point = null,
            string radius = null, string bbox = null, string exclude = null, 
            int limit = 100, int offset = 0)
        {
            string url = "https://platform.whereismytransport.com/api/agencies";
            if (id != null) url += "/" + id;

            string sep = null;

            if (agencies != null) url += GetSeparator(ref sep) + "agencies=" + agencies;

            if (point != null) url += GetSeparator(ref sep) + "point=" + point;

            if (radius != null) url += GetSeparator(ref sep) + "radius=" + radius;

            if (bbox != null) url += GetSeparator(ref sep) + "bbox=" + bbox;

            if (exclude != null) url += GetSeparator(ref sep) + "exclude=" + exclude;

            url += GetSeparator(ref sep) + "limit=" + limit.ToString();
            url += GetSeparator(ref sep) + "offset=" + offset.ToString();

            url = HttpUtility.UrlEncode(url);

            return await _client.GetStringAsync(url);
        }

        #region Helpers

        private string BuildJsonString(List<KeyValuePair<string, string>> data)
        {
            string results = "{";

            foreach (KeyValuePair<string, string> item in data)
            {
                results += $"{ item.Key }:{ item.Value },";
            }
            results += "}";
            return results;
        }

        private string GetSeparator(ref string sep)
        {
            if (sep == null) sep = "?";
            else sep = "&";
            return sep;
        }

        #endregion
    }
}

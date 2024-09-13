using Newtonsoft.Json.Linq;
using RestSharp;

namespace SpotifyApiClient.Services
{
    public class SpotifyApiClient
    {
        private readonly string clientId;
        private readonly string clientSecret;


        public SpotifyApiClient(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var client = new RestClient("https://accounts.spotify.com");
            var request = new RestRequest("api/token", Method.Post);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded",
                $"grant_type=client_credentials&client_id={this.clientId}&client_secret={this.clientSecret}",
                ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error retrieving token: {response.Content}");
            }

            dynamic resp = JObject.Parse(response.Content!);
            return resp.access_token;
        }
    }
}
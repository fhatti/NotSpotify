using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NotSpotify.Models;
using RestSharp;


namespace NotSpotify.ViewModels;

public class MainViewModel : ObservableObject
{
    private List<Track.Item>? songs;

    public List<Track.Item>? Songs
    {
        get => this.songs;
        set => this.SetProperty(ref this.songs, value);
    }
    public IAsyncRelayCommand LoadDataCommand { get; }

    public MainViewModel()
    {
        this.Songs = new List<Track.Item>();
        this.LoadDataCommand = new AsyncRelayCommand(this.populateUiAsync);
    }

    private static async Task<Track?> getDataFromApiAsync()
    {
        var id = Environment.GetEnvironmentVariable("ID");
        var secret = Environment.GetEnvironmentVariable("SECRET");

        // Get Access Token
        using var client = new RestClient("https://accounts.spotify.com");
        var request = new RestRequest("api/token", Method.Post);
        request.AddHeader("cache-control", "no-cache");
        request.AddHeader("content-type", "application/x-www-form-urlencoded");
        request.AddParameter("application/x-www-form-urlencoded",
            $"grant_type=client_credentials&client_id={id}&client_secret={secret}",
            ParameterType.RequestBody);
        var response = await client.ExecuteAsync(request);
        if (!response.IsSuccessful)
        {
            throw new Exception($"Error retrieving token: {response.Content}");
        }

        dynamic resp = JObject.Parse(response.Content!);
        string token = resp.access_token;

        // Fetch 
        using (var apiClient = new RestClient("https://api.spotify.com"))
        {
            var apiRequest = new RestRequest("v1/browse/new-releases", Method.Get);
            apiRequest.AddHeader("authorization", $"Bearer {token}");
            apiRequest.AddHeader("cache-control", "no-cache");
            apiRequest.AddHeader("Accept", "application/json");
            apiRequest.AddHeader("Content-Type", "application/json");

            var apiResponse = await apiClient.ExecuteAsync(apiRequest);
            if (apiResponse.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<Track>(apiResponse.Content!);
            }

            throw new Exception($"Error fetching new releases: {apiResponse.Content}");
        }
    }

    private async Task populateUiAsync()
    {
        var result = await getDataFromApiAsync();

        if (result?.Album?.Items == null)
        {
            throw new Exception("Failed to retrieve or parse the track data.");
        }
        this.Songs = result.Album.Items.ToList();
    }
}
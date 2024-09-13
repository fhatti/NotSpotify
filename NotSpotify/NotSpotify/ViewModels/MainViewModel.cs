using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using NotSpotify.Models;
using RestSharp;


namespace NotSpotify.ViewModels;

public class MainViewModel : ObservableObject
{
    private List<Track.Item>? songs;
    private readonly SpotifyApiClient.Services.SpotifyApiClient spotifyApiClient;

    public List<Track.Item>? Songs
    {
        get => this.songs;
        set => this.SetProperty(ref this.songs, value);
    }

    public IAsyncRelayCommand LoadDataCommand { get; }

    public MainViewModel()
    {
        var clientId = Environment.GetEnvironmentVariable("ID")
                       ?? throw new InvalidOperationException("Client ID not set in environment variables.");
        var clientSecret = Environment.GetEnvironmentVariable("SECRET")
                           ?? throw new InvalidOperationException("Client Secret not set in environment variables.");

        this.spotifyApiClient = new SpotifyApiClient.Services.SpotifyApiClient(clientId, clientSecret);

        this.songs = new List<Track.Item>();
        this.LoadDataCommand = new AsyncRelayCommand(this.loadNewReleasesAsync);
    }

    private async Task loadNewReleasesAsync()
    {
        var token = await this.spotifyApiClient.GetAccessTokenAsync();

        var client = new RestClient("https://api.spotify.com");
        var request = new RestRequest("v1/browse/new-releases", Method.Get);
        request.AddHeader("Authorization", $"Bearer {token}");
        request.AddHeader("cache-control", "no-cache");
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var response = await client.ExecuteAsync(request);
        if (response.IsSuccessful)
        {
            var result = JsonConvert.DeserializeObject<Track>(response.Content!);

            if (result?.Album?.Items == null)
            {
                throw new Exception("Failed to retrieve or parse the track data.");
            }

            this.Songs = result.Album.Items.ToList();
        }
        else
        {
            throw new Exception($"Error fetching new releases: {response.Content}");
        }
    }
}
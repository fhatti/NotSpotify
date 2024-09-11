using Newtonsoft.Json;

namespace NotSpotify.Models;

public class Artist
{
    [JsonProperty("external_urls")] public ExternalUrls? ExternalUrls { get; set; }

    [JsonProperty("followers")] public Followers? Followers { get; set; }

    [JsonProperty("genres")] public List<string>? Genres { get; set; }

    [JsonProperty("href")] public Uri? Href { get; set; }

    [JsonProperty("id")] public string? Id { get; set; }

    [JsonProperty("images")] public List<Image>? Images { get; set; }

    [JsonProperty("name")] public string? Name { get; set; }

    [JsonProperty("popularity")] public long Popularity { get; set; }

    [JsonProperty("type")] public string? Type { get; set; }

    [JsonProperty("uri")] public string? Uri { get; set; }
}

public class ExternalUrls
{
    [JsonProperty("spotify")] public Uri? Spotify { get; set; }
}

public class Followers
{
    [JsonProperty("href")] public object? Href { get; set; }

    [JsonProperty("total")] public long Total { get; set; }
}

public class Image
{
    [JsonProperty("url")] public Uri? Url { get; set; }

    [JsonProperty("height")] public long Height { get; set; }

    [JsonProperty("width")] public long Width { get; set; }
}
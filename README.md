# NotSpotify

NotSpotify is a .NET desktop application designed to fetch and display new music releases from Spotify. It demonstrates how to use MVVM architecture with the `CommunityToolkit.Mvvm` library, `RestSharp` for API requests, and `Newtonsoft.Json` for JSON handling.

## Features

- Fetch new music releases from Spotify using the Spotify Web API.
- Display a list of tracks in a WPF window with a clean, modern interface.
- Utilize MVVM pattern for separation of concerns and easy maintainability.

## Prerequisites

To run this project, ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 5.0 or later)
- [NuGet](https://www.nuget.org/) (for managing dependencies)
- A valid [Spotify API](https://developer.spotify.com/dashboard/) client ID and secret.

## Setup

1. **Clone the Repository**

   ```bash
   git clone https://github.com/fhatti/NotSpotify.git
   cd NotSpotify
   
2. **Install the required NuGet packages**
    ```bash
    dotnet restore
   
3. **Create a .env file in the root directory of your project with the following content:**
    ```bash
   ID=your_spotify_client_id
    SECRET=your_spotify_client_secret


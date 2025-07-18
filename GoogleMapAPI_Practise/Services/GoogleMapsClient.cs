using GoogleMapAPI_Practise.Models.Configuration;
using GoogleMapAPI_Practise.Models.DTOs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GoogleMapAPI_Practise.Services;

public interface IGoogleMapsClient
{
    Task<List<GoogleAutocompleteResult>> GetAutocompleteAsync(string input, double? latitude = null, double? longitude = null);
    Task<GoogleGeoResult?> GeocodeAsync(string address);
    Task<GoogleGeoResult?> ReverseGeocodeAsync(double latitude, double longitude);
    Task<List<GooglePlaceResult>> GetNearbyPlacesAsync(double latitude, double longitude, int radius, string[]? types = null);
    Task<GooglePlaceDetailResult?> GetPlaceDetailsAsync(string placeId);
}

public class GoogleMapsClient(
    HttpClient _client,
    IOptions<GoogleMapConfig> config
    ) : IGoogleMapsClient
{
    private readonly GoogleMapConfig _config = config.Value;

    public async Task<GoogleGeoResult?> GeocodeAsync(string address)
    {
        var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_config.ApiKey}";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GoogleGeocodeResponse>(json);

        var firstResult = result?.Results?.FirstOrDefault();
        if (firstResult != null)
        {
            return new GoogleGeoResult()
            {
                Latitude = firstResult.Geometry.Location.Lat,
                Longitude = firstResult.Geometry.Location.Lng,
                FormattedAddress = firstResult.FormattedAddress,
                PlaceId = firstResult.PlaceId,
                Types = firstResult.Types
            };
        }

        return null;
    }

    public async Task<GoogleGeoResult?> ReverseGeocodeAsync(double latitude, double longitude)
    {
        var url = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key={_config.ApiKey}";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GoogleGeocodeResponse>(json);

        var firstResult = result?.Results?.FirstOrDefault();
        if (firstResult != null)
        {
            return new GoogleGeoResult()
            {
                Latitude = firstResult.Geometry.Location.Lat,
                Longitude = firstResult.Geometry.Location.Lng,
                FormattedAddress = firstResult.FormattedAddress,
                PlaceId = firstResult.PlaceId,
                Types = firstResult.Types
            };
        }

        return null;
    }

    public async Task<List<GoogleAutocompleteResult>> GetAutocompleteAsync(string input, double? latitude = null, double? longitude = null)
    {
        var url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={Uri.EscapeDataString(input)}&key={_config.ApiKey}";

        if (latitude.HasValue && longitude.HasValue)
        {
            url += $"&location={Uri.EscapeDataString($"{latitude.Value},{longitude.Value}")}";
            url += $"&radius={_config.AutocompleteRadiusMeters}";
        }

        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GoogleAutocompleteResponse>(json);

        return result?.Predictions.Select(p => new GoogleAutocompleteResult
        {
            PlaceId = p.PlaceId,
            Description = p.Description,
            MainText = p.StructuredFormatting?.MainText ?? "",
            SecondaryText = p.StructuredFormatting?.SecondaryText ?? ""
        }).ToList() ?? [];
    }

    public async Task<List<GooglePlaceResult>> GetNearbyPlacesAsync(double latitude, double longitude, int radius, string[]? types = null)
    {
        var url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latitude},{longitude}&radius={radius}&key={_config.ApiKey}";

        var typeQuery = types != null ? string.Join("&type=", types) : "";
        if (!string.IsNullOrEmpty(typeQuery))
            url += $"&type={typeQuery}";

        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GoogleNearbySearchResponse>(json);

        return result?.Results.Select(p => new GooglePlaceResult
        {
            PlaceId = p.PlaceId,
            Name = p.Name,
            Address = p.Vicinity,
            Latitude = p.Geometry.Location.Lat,
            Longitude = p.Geometry.Location.Lng,
            Rating = p.Rating,
            Types = p.Types,
            IsOpen = p.OpeningHours?.OpenNow ?? false,
        }).ToList() ?? [];
    }

    public async Task<GooglePlaceDetailResult?> GetPlaceDetailsAsync(string placeId)
    {
        var url = $"https://maps.googleapis.com/maps/api/place/details/json?place_id={placeId}&fields=place_id,name,geometry,formatted_address,types&key={_config.ApiKey}";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GooglePlaceDetailResponse>(json);
        if (result == null || result.Status != GoogleApiResponseStatus.OK)
        {
            return null;
        }

        var detail = result.Result;
        return new GooglePlaceDetailResult()
        {
            PlaceId = detail.PlaceId,
            Name = detail.Name,
            FormattedAddress = detail.FormattedAddress,
            Geometry = detail.Geometry,
            Types = detail.Types,
        };
    }
}

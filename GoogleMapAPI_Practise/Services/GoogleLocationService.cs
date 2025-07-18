using GoogleMapAPI_Practise.Models.Configuration;
using GoogleMapAPI_Practise.Models.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net;

namespace GoogleMapAPI_Practise.Services;

public interface IGoogleLocationService
{
    Task<List<GoogleAutocompleteResult>> GetAutocompleteAsync(string input, double? lat = null, double? lng = null);
    Task<GoogleGeoResult?> GeocodeAsync(string address);
    Task<GoogleGeoResult?> ReverseGeocodeAsync(double latitude, double longitude);
    Task<List<GooglePlaceResult>> GetNearbyPlacesAsync(double latitude, double longitude, int? radius = null, string[]? types = null);
    Task<GooglePlaceDetailResult?> GetPlaceDetailsAsync(string placeId);
}

public class GoogleLocationService(
    IGoogleMapsClient _googleMapsClient,
    IMemoryCache _cache,
    IOptions<GoogleMapConfig> config
    ) : IGoogleLocationService
{
    private readonly GoogleMapConfig _config = config.Value;

    public async Task<GoogleGeoResult?> GeocodeAsync(string address)
    {
        if (string.IsNullOrEmpty(address))
            return null;

        return await _googleMapsClient.GeocodeAsync(address);
    }

    public async Task<List<GoogleAutocompleteResult>> GetAutocompleteAsync(string input, double? lat = null, double? lng = null)
    {
        if (string.IsNullOrEmpty(input))
        {
            return [];
        }

        var cacheKey = $"autocomplete:{input.ToLower()}";
        if (_cache.TryGetValue(cacheKey, out List<GoogleAutocompleteResult>? cached))
        {

            return cached ?? [];
        }

        var result = await _googleMapsClient.GetAutocompleteAsync(input, lat, lng);
        if (result.Count > 0)
        {
            var cacheEntries = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(_config.AutocompleteCacheMinutes)).SetSize(1);
            _cache.Set(cacheKey, result, cacheEntries);
        }

        return result;
    }

    public async Task<List<GooglePlaceResult>> GetNearbyPlacesAsync(double latitude, double longitude, int? radius = null, string[]? types = null)
    {
        var safeTypes = types?.Where(t => GooglePlaceTypes.Allowed.Contains(t)).ToArray();
        var searchRadius = radius ?? _config.NearbySearchRadiusMeters;
        return await _googleMapsClient.GetNearbyPlacesAsync(latitude, longitude, searchRadius, safeTypes);
    }

    public async Task<GooglePlaceDetailResult?> GetPlaceDetailsAsync(string placeId)
    {
        if (string.IsNullOrEmpty(placeId))
            return null;

        return await _googleMapsClient.GetPlaceDetailsAsync(placeId);
    }

    public async Task<GoogleGeoResult?> ReverseGeocodeAsync(double latitude, double longitude)
    {
        return await _googleMapsClient.ReverseGeocodeAsync(latitude, longitude);
    }
}

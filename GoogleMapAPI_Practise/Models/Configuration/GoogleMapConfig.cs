namespace GoogleMapAPI_Practise.Models.Configuration;

public class GoogleMapConfig
{
    public string ApiKey { get; set; } = string.Empty;
    public int AutocompleteCacheMinutes { get; set; } = 30;
    public int AutocompleteRadiusMeters { get; set; } = 10000;
    public int NearbySearchRadiusMeters { get; set; } = 1000;
    public int MaxCacheEntries { get; set; } = 1000;
}

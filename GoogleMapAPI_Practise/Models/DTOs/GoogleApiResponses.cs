using Newtonsoft.Json;

namespace GoogleMapAPI_Practise.Models.DTOs;

public class GoogleApiResponse
{
    [JsonProperty("error_message")]
    public string ErrorMessage { get; set; } = string.Empty;
    public GoogleApiResponseStatus Status { get; set; }
}

public enum GoogleApiResponseStatus
{
    OK,
    ZERO_RESULTS,
    OVER_QUERY_LIMIT,
    REQUEST_DENIED,
    INVALID_REQUEST,
    UNKNOWN_ERROR,
    PERMISSION_DENIED,     // Maps JavaScript API only
    NOT_FOUND,             // e.g. Places Details for invalid place_id
    MAX_ROUTE_LENGTH_EXCEEDED // only for Directions API
}

public class GoogleAutocompleteResponse : GoogleApiResponse
{
    public GooglePrediction[] Predictions { get; set; } = Array.Empty<GooglePrediction>();
}

public class GoogleNearbySearchResponse : GoogleApiResponse
{
    public GoogleNearbyResult[] Results { get; set; } = Array.Empty<GoogleNearbyResult>();
}

public class GoogleGeocodeResponse : GoogleApiResponse
{
    public GoogleGeocodeResult[] Results { get; set; } = Array.Empty<GoogleGeocodeResult>();
}

public class GooglePlaceDetailResponse : GoogleApiResponse
{
    public GooglePlaceDetailResult Result { get; set; }
}

public class GooglePrediction
{
    [JsonProperty("place_id")]
    public string PlaceId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [JsonProperty("structured_formatting")]
    public StructuredFormatting? StructuredFormatting { get; set; }
}

public class StructuredFormatting
{
    [JsonProperty("main_text")]
    public string MainText { get; set; } = string.Empty;
    [JsonProperty("secondary_text")]
    public string SecondaryText { get; set; } = string.Empty;
}



public class GoogleGeocodeResult
{
    [JsonProperty("formatted_address")]
    public string FormattedAddress { get; set; } = string.Empty;
    [JsonProperty("place_id")]
    public string PlaceId { get; set; } = string.Empty;
    public string[] Types { get; set; } = Array.Empty<string>();
    public GoogleGeometry Geometry { get; set; }
}



public class GoogleNearbyResult
{
    [JsonProperty("place_id")]
    public string PlaceId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Vicinity { get; set; } = string.Empty;
    public GoogleGeometry Geometry { get; set; } = new();
    public double? Rating { get; set; }
    public string[] Types { get; set; } = Array.Empty<string>();

    [JsonProperty("opening_hours")]
    public GoogleOpeningHours? OpeningHours { get; set; }
}

public class GoogleGeometry
{
    public GoogleLocation Location { get; set; } = new();
}

public class GoogleLocation
{
    public double Lat { get; set; }
    public double Lng { get; set; }
}

public class GoogleOpeningHours
{
    [JsonProperty("open_now")]
    public bool OpenNow { get; set; }
}
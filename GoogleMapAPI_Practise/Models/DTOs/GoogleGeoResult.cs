namespace GoogleMapAPI_Practise.Models.DTOs;

public class GoogleGeoResult
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string FormattedAddress { get; set; } = string.Empty;
    public string PlaceId { get; set; } = string.Empty;
    public string[] Types { get; set; } = Array.Empty<string>();

}

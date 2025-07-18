namespace GoogleMapAPI_Practise.Models.DTOs;

public class GooglePlaceResult
{
    public string PlaceId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Rating { get; set; }
    public string[] Types { get; set; } = Array.Empty<string>();
    public bool IsOpen { get; set; }
}

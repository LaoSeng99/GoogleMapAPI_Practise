namespace GoogleMapAPI_Practise.Models.DTOs;

public class GooglePlaceDetailResult
{
    public string PlaceId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string FormattedAddress { get; set; } = default!;
    public GoogleGeometry Geometry { get; set; } = default!;
    public string[] Types { get; set; } = [];
}

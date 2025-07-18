namespace GoogleMapAPI_Practise.Models.DTOs;

public class GoogleLocationRequest
{
    public string Input { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int? Radius { get; set; }
    public string[]? Types { get; set; }
}

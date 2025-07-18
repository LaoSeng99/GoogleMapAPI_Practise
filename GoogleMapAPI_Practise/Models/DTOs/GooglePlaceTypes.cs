namespace GoogleMapAPI_Practise.Models.DTOs;

public static class GooglePlaceTypes
{
    public const string Restaurant = "restaurant";
    public const string Cafe = "cafe";
    public const string Bar = "bar";
    public const string Store = "store";
    public const string Hospital = "hospital";
    public const string GasStation = "gas_station";
    public const string Pharmacy = "pharmacy";

    public static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
{
    Restaurant,
    Cafe,
    Bar,
    Store,
    Hospital,
    GasStation,
    Pharmacy
};
}

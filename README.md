# Google Maps API 

A simple ASP.NET Core 8+ wrapper for Google Maps API services including geocoding, reverse geocoding, autocomplete, nearby places search, and place details.

## Features

- 🔍 **Address Autocomplete** - Get address suggestions as users type
- 🌍 **Geocoding** - Convert addresses to coordinates
- 📍 **Reverse Geocoding** - Convert coordinates to addresses
- 🏢 **Nearby Places Search** - Find places around a location
- 📋 **Place Details** - Get detailed information about specific places
- 💾 **Memory Caching** - Cached autocomplete results for better performance
- 🛡️ **Input Validation** - Proper parameter validation and error handling

## Quick Start

### 1. Configuration

Add your Google Maps API configuration to `appsettings.json`:

```json
{
  "GoogleMaps": {
    "ApiKey": "your-google-maps-api-key",
    "AutocompleteCacheMinutes": 30,
    "AutocompleteRadiusMeters": 10000,
    "NearbySearchRadiusMeters": 1000,
    "MaxCacheEntries": 1000
  }
}
```

### 2. Service Registration

Register the services in your `Program.cs`:

```csharp
// Google Maps Configuration
builder.Services.Configure<GoogleMapConfig>(builder.Configuration.GetSection("GoogleMaps"));

// HTTP Client for Google Maps API
builder.Services.AddHttpClient<IGoogleMapsClient, GoogleMapsClient>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Memory Cache
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = builder.Configuration.GetValue<int>("GoogleMaps:MaxCacheEntries", 1000);
});

// CORS (if needed)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Location Service
builder.Services.AddScoped<IGoogleLocationService, GoogleLocationService>();
```

### 3. Enable CORS (if needed)

```csharp
app.UseCors("AllowAll");
```

## API Endpoints

### Get Address Autocomplete
```http
GET /api/location/autocomplete?input={search_text}&lat={latitude}&lng={longitude}
```

**Example:**
```http
GET /api/location/autocomplete?input=starbucks&lat=40.7128&lng=-74.0060
```

### Get Nearby Places
```http
POST /api/location/nearby
Content-Type: application/json

{
  "latitude": 40.7128,
  "longitude": -74.0060,
  "radius": 1000,
  "types": ["restaurant", "cafe"]
}
```

### Geocode Address
```http
GET /api/location/geocode?address={address}
```

**Example:**
```http
GET /api/location/geocode?address=Times Square, New York
```

### Reverse Geocode
```http
GET /api/location/reverse-geocode?latitude={lat}&longitude={lng}
```

**Example:**
```http
GET /api/location/reverse-geocode?latitude=40.7128&longitude=-74.0060
```

### Get Place Details
```http
GET /api/location/place-details?placeId={place_id}
```

## Architecture

The project follows a clean layered architecture:

- **Controller Layer** (`LocationController`) - Handles HTTP requests and responses
- **Service Layer** (`GoogleLocationService`) - Business logic and caching
- **Client Layer** (`GoogleMapsClient`) - Direct Google Maps API communication
- **Models/DTOs** - Data transfer objects for API responses

## Key Components

### GoogleLocationService
- Implements caching for autocomplete results
- Handles input validation and filtering
- Provides clean interface for controllers

### GoogleMapsClient
- Direct HTTP client for Google Maps API
- Handles API request/response mapping
- Manages API key authentication

### Caching Strategy
- Autocomplete results are cached for 30 minutes (configurable)
- Cache size limit: 1000 entries (configurable)
- Uses sliding expiration for better performance

## Error Handling

The API includes comprehensive error handling:

- **400 Bad Request** - Invalid input parameters
- **404 Not Found** - No results found for the query
- **500 Internal Server Error** - API or server errors

## Requirements

- .NET 8.0 or later
- Google Maps API Key with the following APIs enabled:
  - Geocoding API
  - Places API
  - Maps JavaScript API

## Configuration Options

| Setting | Description | Default |
|---------|-------------|---------|
| `ApiKey` | Your Google Maps API key | Required |
| `AutocompleteCacheMinutes` | Cache duration for autocomplete results | 30 |
| `AutocompleteRadiusMeters` | Search radius for autocomplete | 10000 |
| `NearbySearchRadiusMeters` | Default radius for nearby search | 1000 |
| `MaxCacheEntries` | Maximum number of cached entries | 1000 |

## Usage Example

```csharp
public class MyService
{
    private readonly IGoogleLocationService _locationService;
    
    public MyService(IGoogleLocationService locationService)
    {
        _locationService = locationService;
    }
    
    public async Task<List<GoogleAutocompleteResult>> SearchPlaces(string input)
    {
        return await _locationService.GetAutocompleteAsync(input);
    }
}
```
## Support

For issues and questions, please create an issue in the repository.
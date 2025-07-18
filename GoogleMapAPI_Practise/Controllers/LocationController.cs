using GoogleMapAPI_Practise.Models.DTOs;
using GoogleMapAPI_Practise.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoogleMapAPI_Practise.Controllers;
/// <summary>
/// 位置服务控制器，提供地理位置相关的 API 功能
/// </summary>
[Route("api/location")]
[ApiController]
public class LocationController(
    IGoogleLocationService service
    ) : ControllerBase
{
    /// <summary>
    /// 根据输入文本获取地址自动完成建议
    /// </summary>
    /// <param name="input">搜索输入文本</param>
    /// <param name="lat">可选的纬度参数，用于优化搜索结果</param>
    /// <param name="lng">可选的经度参数，用于优化搜索结果</param>
    /// <returns>返回自动完成建议列表</returns>
    /// <response code="200">成功返回自动完成建议列表</response>
    /// <response code="400">输入参数为空或无效</response>
    [HttpGet("autocomplete")]
    public async Task<ActionResult<List<GoogleAutocompleteResult>>> GetAutocomplete([FromQuery] string input, [FromQuery] string? lat, [FromQuery] string? lng)
    {
        if (string.IsNullOrWhiteSpace(input))
            return BadRequest("Input cannot be empty");

        var results = await service.GetAutocompleteAsync(input);
        return Ok(results);
    }

    /// <summary>
    /// 获取指定坐标附近的地点信息
    /// </summary>
    /// <param name="request">包含纬度、经度、搜索半径和地点类型的请求对象</param>
    /// <returns>返回附近地点信息列表</returns>
    /// <response code="200">成功返回附近地点列表</response>
    /// <response code="400">坐标参数缺失或无效</response>
    [HttpPost("nearby")]
    public async Task<ActionResult<List<GoogleNearbyResult>>> GetNearbyPlaces([FromBody] GoogleLocationRequest request)
    {
        if (request.Latitude == null || request.Longitude == null)
            return BadRequest("Latitude and longitude are required");

        if (request.Latitude < -90 || request.Latitude > 90 ||
            request.Longitude < -180 || request.Longitude > 180)
            return BadRequest("Invalid coordinates");

        var results = await service.GetNearbyPlacesAsync(
            request.Latitude.Value,
            request.Longitude.Value,
            request.Radius,
            request.Types);

        return Ok(results);
    }

    /// <summary>
    /// 将地址转换为坐标信息（地理编码）
    /// </summary>
    /// <param name="address">需要转换的地址字符串</param>
    /// <returns>返回地址对应的坐标和详细信息</returns>
    /// <response code="200">成功返回地理编码结果</response>
    /// <response code="400">地址参数为空</response>
    /// <response code="404">未找到对应地址</response>
    [HttpGet("geocode")]
    public async Task<ActionResult<List<GoogleAutocompleteResult>>> Geocode([FromQuery] string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return BadRequest("Address cannot be empty");

        var result = await service.GeocodeAsync(address);
        if (result == null)
            return NotFound("Address not found");

        return Ok(result);
    }

    /// <summary>
    /// 将坐标转换为地址信息（反向地理编码）
    /// </summary>
    /// <param name="latitude">纬度，范围 -90 到 90</param>
    /// <param name="longitude">经度，范围 -180 到 180</param>
    /// <returns>返回坐标对应的地址信息</returns>
    /// <response code="200">成功返回反向地理编码结果</response>
    /// <response code="400">坐标参数无效</response>
    /// <response code="404">未找到对应位置</response>
    [HttpGet("reverse-geocode")]
    public async Task<ActionResult<List<GoogleAutocompleteResult>>> ReverseGeocode([FromQuery] double latitude, [FromQuery] double longitude)
    {
        if (latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180)
            return BadRequest("Invalid coordinates");

        var result = await service.ReverseGeocodeAsync(latitude, longitude);
        if (result == null)
            return NotFound("Location not found");

        return Ok(result);
    }

    /// <summary>
    /// 根据 Place ID 获取地点的详细信息
    /// </summary>
    /// <param name="placeId">Google Places API 的地点 ID</param>
    /// <returns>返回地点的详细信息</returns>
    /// <response code="200">成功返回地点详细信息</response>
    /// <response code="400">Place ID 参数为空</response>
    /// <response code="404">未找到对应地点</response>
    [HttpGet("place-details")]
    public async Task<ActionResult<List<GoogleAutocompleteResult>>> GetPlaceDetail([FromQuery] string placeId)
    {
        if (string.IsNullOrWhiteSpace(placeId))
            return BadRequest("Place Id cannot be empty");

        var result = await service.GetPlaceDetailsAsync(placeId);
        if (result == null)
            return NotFound("Place Id not found");

        return Ok(result);
    }


}

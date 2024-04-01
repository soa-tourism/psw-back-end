using Explorer.Tours.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Explorer.API.Controllers.Author.Administration
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/tours")]
    public class EquipmentController : BaseApiController
    {
        private readonly HttpClient _httpClient;

        public EquipmentController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://host.docker.internal:8081/v1/tours");
        }

        [HttpGet("{id:long}/equipment/available")]
        public async Task<ActionResult<List<EquipmentDto>>> GetAvailableEquipment(long id, [FromQuery] List<long>? equipmentIds)
        {
            var requestUri = ConstructUrl($"{id}/equipment/available", equipmentIds);

            using var response = await _httpClient.GetAsync(requestUri);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest($"Failed to get equipment: {result}");
            }

            var equipment = JsonSerializer.Deserialize<List<EquipmentDto>>(result);
            return Ok(equipment);
        }

        private string ConstructUrl(string relativePath, List<long>? equipmentIds)
        {
            var requestUri = $"{_httpClient.BaseAddress}/{relativePath}";

            if (equipmentIds is not { Count: > 0 }) return requestUri;

            var queryParams = string.Join('&', equipmentIds.Select(id => $"equipmentIds={id}"));
            requestUri += $"?{queryParams}";

            return requestUri;
        }
    }
}

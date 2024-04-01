using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace Explorer.API.Controllers.Administrator.Administration
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administration/equipment")]
    public class EquipmentController : BaseApiController
    {
        private readonly HttpClient _httpClient;

        public EquipmentController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://host.docker.internal:8081/v1/tours/equipment");
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<EquipmentDto>>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            using var response = await _httpClient.GetAsync("");
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = Result.Fail($"Failed to get equipment: {result}");
                return CreateResponse(error);
            }

            var equipment = JsonSerializer.Deserialize<List<EquipmentDto>>(result);
            if (equipment is  null)
            {
                var noEquipment = Result.Ok("No equipment found.");
                return CreateResponse(noEquipment);

            }

            var pagedResult = PaginateResult(page, pageSize, equipment);
            return Ok(pagedResult);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<EquipmentDto>> GetById(long id)
        {
            using var response = await _httpClient.GetAsync(ConstructUrl(id.ToString()));
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = Result.Fail($"Failed to get equipment: {result}");
                return CreateResponse(errorResult);
            }

            var equipment = JsonSerializer.Deserialize<EquipmentDto>(result);
            return Ok(equipment);
        }

        [HttpPost]
        public async Task<ActionResult<EquipmentDto>> Create([FromBody] EquipmentDto equipment)
        {
            using var jsonContent = new StringContent(JsonSerializer.Serialize(equipment), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync("", jsonContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<EquipmentDto>(responseContent);

            return CreateResponse(result.ToResult());
        }
        
        [HttpPut("{id:long}")]
        public async Task<ActionResult<EquipmentDto>> Update([FromBody] EquipmentDto equipment, long id)
        {
            using var jsonContent = new StringContent(JsonSerializer.Serialize(equipment), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PutAsync(ConstructUrl(id.ToString()), jsonContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<EquipmentDto>(responseContent);

            return CreateResponse(result.ToResult());
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> Delete(long id)
        {
            using var response = await _httpClient.DeleteAsync(ConstructUrl(id.ToString()));

            if (response.IsSuccessStatusCode) return NoContent();

            var errorResult = Result.Fail("Failed to delete equipment.");
            return CreateResponse(errorResult);
        }

        private string ConstructUrl(string relativePath)
        {
            return $"{_httpClient.BaseAddress}/{relativePath}";
        }

        private static PagedResult<EquipmentDto> PaginateResult(int page, int pageSize, List<EquipmentDto> equipment)
        {
            if (page == 0 && pageSize == 0)
            {
                return new PagedResult<EquipmentDto>(equipment, equipment.Count);
            }

            var totalCount = equipment.Count;
            var startIndex = (page - 1) * pageSize;
            var paginatedList = equipment.Skip(startIndex).Take(pageSize).ToList();
            return new PagedResult<EquipmentDto>(paginatedList, totalCount);
        }
    }
}

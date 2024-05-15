using Explorer.API.Dtos.Tours;
using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.Text;
using System.Text.Json;

namespace Explorer.API.Controllers.Tourist.Tourism
{

    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourism/position")]
    public class TouristPositionController : BaseApiController
    {
        private readonly HttpClient _httpClient;

        public TouristPositionController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://host.docker.internal:8083/v1/position");
        }

        //[HttpGet]
        //public async Task<ActionResult<PagedResult<TouristPositionDto>>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        //{
        //    using var response = await _httpClient.GetAsync("");
        //    var result = await response.Content.ReadAsStringAsync();

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        var error = Result.Fail($"Failed to get tourist positions: {result}");
        //        return CreateResponse(error);
        //    }

        //    var tours = JsonSerializer.Deserialize<List<TouristPositionDto>>(result);
        //    if (tours is null)
        //    {
        //        var noTours = Result.Ok("No tourist positions found.");
        //        return CreateResponse(noTours);

        //    }

        //    var pagedResult = PaginateResult(page, pageSize, tours);
        //    return Ok(pagedResult);
        //}

        //[HttpGet("/{id}")]
        //public async Task<ActionResult<List<TouristPositionDto>>> GetPositionByCreator([FromQuery] int page, [FromQuery] int pageSize, string id)
        //{
        //    using var response = await _httpClient.GetAsync(ConstructUrl(id));
        //    var result = await response.Content.ReadAsStringAsync();

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        var errorResult = Result.Fail($"Failed to get tourist position: {result}");
        //        return CreateResponse(errorResult);
        //    }

        //    var tour = JsonSerializer.Deserialize<TouristPositionDto>(result);
        //    return Ok(tour);
        //}

        //[HttpPost]
        //public async Task<ActionResult<TouristPositionDto>> Create([FromBody] TouristPositionDto position)
        //{
        //    using var jsonContent = new StringContent(JsonSerializer.Serialize(position), Encoding.UTF8, "application/json");
        //    using var response = await _httpClient.PostAsync("", jsonContent);

        //    var responseContent = await response.Content.ReadAsStringAsync();
        //    var result = JsonSerializer.Deserialize<TouristPositionDto>(responseContent);

        //    return CreateResponse(result.ToResult());
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult<TouristPositionDto>> Update([FromBody] TouristPositionDto position, string id)
        //{
        //    using var jsonContent = new StringContent(JsonSerializer.Serialize(position), Encoding.UTF8, "application/json");
        //    using var response = await _httpClient.PutAsync(ConstructUrl(id), jsonContent);

        //    var responseContent = await response.Content.ReadAsStringAsync();
        //    var result = JsonSerializer.Deserialize<TouristPositionDto>(responseContent);

        //    return CreateResponse(result.ToResult());
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(string id)
        //{
        //    using var response = await _httpClient.DeleteAsync(ConstructUrl(id));

        //    if (response.IsSuccessStatusCode) return NoContent();

        //    var errorResult = Result.Fail("Failed to delete tourist position.");
        //    return CreateResponse(errorResult);
        //}

        private string ConstructUrl(string relativePath)
        {
            return $"{_httpClient.BaseAddress}/{relativePath}";
        }

        private static PagedResult<TouristPositionDto> PaginateResult(int page, int pageSize, List<TouristPositionDto> positions)
        {
            if (page == 0 && pageSize == 0)
            {
                return new PagedResult<TouristPositionDto>(positions, positions.Count);
            }

            var totalCount = positions.Count;
            var startIndex = (page - 1) * pageSize;
            var paginatedList = positions.Skip(startIndex).Take(pageSize).ToList();
            return new PagedResult<TouristPositionDto>(paginatedList, totalCount);
        }
    }

}

using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Explorer.API.Dtos.Tours;

namespace Explorer.API.Controllers.Author.Administration
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/administration/tours")]
    public class TourController : BaseApiController
    {
        private readonly HttpClient _httpClient;

        public TourController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://host.docker.internal:8083/v1/tours");
        }


        [HttpGet]
        public async Task<ActionResult<PagedResult<BasicTourDto>>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            using var response = await _httpClient.GetAsync("");
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = Result.Fail($"Failed to get tours: {result}");
                return CreateResponse(error);
            }

            var tours = JsonSerializer.Deserialize<List<BasicTourDto>>(result);
            if (tours is null)
            {
                var noTours = Result.Ok("No tours found.");
                return CreateResponse(noTours);

            }

            var pagedResult = PaginateResult(page, pageSize, tours);
            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BasicTourDto>> GetById(string id)
        {
            using var response = await _httpClient.GetAsync(ConstructUrl(id));
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = Result.Fail($"Failed to get tour: {result}");
                return CreateResponse(errorResult);
            }

            var tour = JsonSerializer.Deserialize<BasicTourDto>(result);
            return Ok(tour);
        }

        [HttpPost]
        public async Task<ActionResult<BasicTourDto>> Create([FromBody] BasicTourDto tour)
        {
            using var jsonContent = new StringContent(JsonSerializer.Serialize(tour), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync("", jsonContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<BasicTourDto>(responseContent);

            return CreateResponse(result.ToResult());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BasicTourDto>> Update([FromBody] BasicTourDto tour, string id)
        {
            using var jsonContent = new StringContent(JsonSerializer.Serialize(tour), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PutAsync(ConstructUrl(id), jsonContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<BasicTourDto>(responseContent);

            return CreateResponse(result.ToResult());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            using var response = await _httpClient.DeleteAsync(ConstructUrl(id.ToString()));

            if (response.IsSuccessStatusCode) return NoContent();

            var errorResult = Result.Fail("Failed to delete equipment.");
            return CreateResponse(errorResult);
        }

        [HttpGet("author/{id:long}")]
        public async Task<ActionResult<List<BasicTourDto>>> GetByAuthor([FromQuery] int page, [FromQuery] int pageSize, long id)
        {
            var requestUri = ConstructUrl($"author/{id}");

            using var response = await _httpClient.GetAsync(requestUri);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest($"Failed to get tour: {result}");
            }

            var tours = JsonSerializer.Deserialize<List<BasicTourDto>>(result);
            if (tours is null)
            {
                var noTours = Result.Ok("No tours found.");
                return CreateResponse(noTours);

            }

            var pagedResult = PaginateResult(page, pageSize, tours);
            return Ok(pagedResult);
        }

        [HttpPost("{id}/equipment/{equipmentId}")]
        public async Task<ActionResult> AddEquipment(string id, string equipmentId)
        {
            using var response = await _httpClient.PostAsync(ConstructUrl($"{id}/equipment/{equipmentId}"), null);

            if (response.IsSuccessStatusCode) return Ok();

            var errorResult = Result.Fail("Failed to add equipment.");
            return CreateResponse(errorResult);
        }

        [HttpDelete("{id}/equipment/{equipmentId}")]
        public async Task<ActionResult> RemoveEquipment(string id, string equipmentId)
        {
            using var response = await _httpClient.DeleteAsync(ConstructUrl($"{id}/equipment/{equipmentId}"));

            if (response.IsSuccessStatusCode) return NoContent();

            var errorResult = Result.Fail("Failed to delete equipment.");
            return CreateResponse(errorResult);
        }


        // [HttpPut("publishedTours/{id:int}")]
        // public ActionResult<BasicTourDto> Publish(int id)
        // {
        //     var result = _tourService.Publish(id, User.PersonId());
        //     return CreateResponse(result);
        // }
        //
        // [HttpPut("archivedTours/{id:int}")]
        // public ActionResult<BasicTourDto> Archive(int id)
        // {
        //     var result = _tourService.Archive(id, User.PersonId());
        //     return CreateResponse(result);
        // }
        //
        // [HttpPut("tourTime/{id:int}")]
        // public ActionResult<BasicTourDto> AddTime(TourTimesDto tourTimesDto, int id)
        // {
        //     var result = _tourService.AddTime(tourTimesDto, id, User.PersonId());
        //     return CreateResponse(result);
        // }

        private string ConstructUrl(string relativePath)
        {
            return $"{_httpClient.BaseAddress}/{relativePath}";
        }

        private static PagedResult<BasicTourDto> PaginateResult(int page, int pageSize, List<BasicTourDto> tours)
        {
            if (page == 0 && pageSize == 0)
            {
                return new PagedResult<BasicTourDto>(tours, tours.Count);
            }

            var totalCount = tours.Count;
            var startIndex = (page - 1) * pageSize;
            var paginatedList = tours.Skip(startIndex).Take(pageSize).ToList();
            return new PagedResult<BasicTourDto>(paginatedList, totalCount);
        }
    }
}

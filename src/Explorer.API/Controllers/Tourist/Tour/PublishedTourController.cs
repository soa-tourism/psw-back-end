using System.Text.Json;
using Explorer.Tours.API.Dtos;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Tour
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/published-tours")]
    public class PublishedTourController : BaseApiController
    {
        // TODO
        private readonly HttpClient _httpClient;

        public PublishedTourController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8081/v1/tours");
        }

        [HttpGet]
        public async Task<ActionResult<List<PublishedTourDto>>> GetAll()
        {
            using var response = await _httpClient.GetAsync(ConstructUrl("published"));
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = Result.Fail($"Failed to get tours: {result}");
                return CreateResponse(error);
            }

            var tours = JsonSerializer.Deserialize<List<PublishedTourDto>>(result);
            if (tours is not null) return Ok(tours);

            var noTours = Result.Ok("No tours found.");
            return CreateResponse(noTours);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<PublishedTourDto>> GetPublishedTour(long id)
        {
            using var response = await _httpClient.GetAsync(ConstructUrl($"published/{id}"));
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = Result.Fail($"Failed to get tours: {result}");
                return CreateResponse(error);
            }

            var tour = JsonSerializer.Deserialize<PublishedTourDto>(result);
            return Ok(tour);
        }

        [HttpGet("average/{id:long}")]
        public async Task<ActionResult<double>> GetAverageRating(long id)
        {
            using var response = await _httpClient.GetAsync(ConstructUrl($"reviews/average/{id}"));
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = Result.Fail($"Failed to get tour: {result}");
                return CreateResponse(error);
            }

            var rating = JsonSerializer.Deserialize<double>(result);
            return Ok(rating);
        }

        // TODO
        // [HttpPost("byChekpoints")]
        // public ActionResult<List<PublishedTourDto>> GetToursByPublicCheckpoints([FromBody] List<PublicCheckpointDto> checkpoints)
        // {
        //     var result = _tourService.GetToursByPublicCheckpoints(checkpoints);
        //     return CreateResponse(result);
        // }
        //
        //
        // [HttpGet("averageRating/{tourId:int}")]
        // public double GetAverageRating(long tourId)
        // {
        //     var result = _tourService.GetAverageRating(tourId);
        //     return result;
        // }

        private string ConstructUrl(string relativePath)
        {
            return $"{_httpClient.BaseAddress}/{relativePath}";
        }
    }
}

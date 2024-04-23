using Explorer.API.Dtos.Tours;
using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/tours/reviews")]
    public class TourRatingTouristController : BaseApiController
    {
        private readonly HttpClient _httpClient;


        public TourRatingTouristController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://host.docker.internal:8083/v1/tours/reviews");
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TourRatingDto>>> GetAllReviews([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string id, [FromQuery] string type)
        {
            var endpoint = type.ToLower() switch
            {
                "tourist" => $"tourist/{long.Parse(id)}",
                "tour" => $"tour/{id}",
                _ => throw new ArgumentException("Invalid type. Type must be 'tourist' or 'tour'.")
            };

            using var response = await _httpClient.GetAsync(ConstructUrl(endpoint));
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = Result.Fail($"Failed to get tour reviews: {result}");
                return CreateResponse(error);
            }

            var tourReviews = JsonSerializer.Deserialize<List<TourRatingDto>>(result);
            if (tourReviews is null)
            {
                var noReviews = Result.Ok("No tour reviews found.");
                return CreateResponse(noReviews);
            }

            var pagedResult = PaginateResult(page, pageSize, tourReviews);
            return Ok(pagedResult);
        }

        [HttpPost]
        public async Task<ActionResult<TourRatingDto>> Create([FromForm] TourRatingDto tourRating, [FromForm] List<IFormFile>? images = null)
        {
            tourRating.Images = await ConvertToByteList(images); 

            using var jsonContent = new StringContent(JsonSerializer.Serialize(tourRating), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync("", jsonContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TourRatingDto>(responseContent);

            return CreateResponse(result.ToResult());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TourRatingDto>> Update(string id, [FromForm] TourRatingDto tourRating, [FromForm] List<IFormFile>? images = null)
        {
            tourRating.Images = await ConvertToByteList(images);

            using var jsonContent = new StringContent(JsonSerializer.Serialize(tourRating), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PutAsync(ConstructUrl($"{id}"), jsonContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TourRatingDto>(responseContent);

            return CreateResponse(result.ToResult());

        }

        private async Task<List<byte[]>> ConvertToByteList(List<IFormFile>? images)
        {
            var imageBytesList = new List<byte[]>();

            if (images == null) return imageBytesList;

            foreach (var image in images)
            {
                using var ms = new MemoryStream();
                await image.CopyToAsync(ms);
                imageBytesList.Add(ms.ToArray());
            }

            return imageBytesList;
        }

        private string ConstructUrl(string relativePath)
        {
            return $"{_httpClient.BaseAddress}/{relativePath}";
        }

        private static PagedResult<TourRatingDto> PaginateResult(int page, int pageSize, List<TourRatingDto> reviews)
        {
            if (page == 0 && pageSize == 0)
            {
                return new PagedResult<TourRatingDto>(reviews, reviews.Count);
            }

            var totalCount = reviews.Count;
            var startIndex = (page - 1) * pageSize;
            var paginatedList = reviews.Skip(startIndex).Take(pageSize).ToList();
            return new PagedResult<TourRatingDto>(paginatedList, totalCount);
        }
    }
}

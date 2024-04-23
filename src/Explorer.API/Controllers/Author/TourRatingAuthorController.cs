using Explorer.API.Dtos.Tours;
using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/tours/reviews")]
    public class TourRatingAuthorController : BaseApiController
    {
        private readonly HttpClient _httpClient;

        public TourRatingAuthorController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://host.docker.internal:8083/v1/tours/reviews");
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TourRatingDto>>> GetAllByAuthor([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] long authorId)
        {
            using var response = await _httpClient.GetAsync(ConstructUrl($"author/{authorId}"));
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

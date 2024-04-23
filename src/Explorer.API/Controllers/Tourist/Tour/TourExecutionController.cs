using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Explorer.API.Dtos.Tours;


namespace Explorer.API.Controllers.Tourist.Tour
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tour-execution")]
    public class TourExecutionController : BaseApiController
    {
        private readonly HttpClient _httpClient;

        public TourExecutionController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://host.docker.internal:8083/v1/execution");
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TourExecutionDto>>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            using var response = await _httpClient.GetAsync("");
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = Result.Fail($"Failed to get executions: {result}");
                return CreateResponse(error);
            }

            var executions = JsonSerializer.Deserialize<List<TourExecutionDto>>(result);
            if (executions is null)
            {
                var noTours = Result.Ok("No executions found.");
                return CreateResponse(noTours);

            }

            var pagedResult = PaginateResult(page, pageSize, executions);
            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TourExecutionDto>> GetById(string id)
        {
            using var response = await _httpClient.GetAsync(ConstructUrl(id));
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = Result.Fail($"Failed to get execution: {result}");
                return CreateResponse(errorResult);
            }

            var execution = JsonSerializer.Deserialize<TourExecutionDto>(result);
            return Ok(execution);
        }

        [HttpPost("{tourId}")]
        public async Task<ActionResult<TourExecutionDto>> Create(string tourId)
        {
            var dto = new TourExecutionDto
            {
                TourId = tourId,
                TouristId = User.PersonId(),
                Start = DateTime.Now,
                LastActivity = DateTime.Now,
                ExecutionStatus = "InProgress",
                CompletedCheckpoints = new List<CheckpointCompletitionDto>()
            };

            using var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync("", jsonContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TourExecutionDto>(responseContent);

            return CreateResponse(result.ToResult());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TourExecutionDto>> Update([FromBody] TourExecutionDto execution, string id)
        {
            using var jsonContent = new StringContent(JsonSerializer.Serialize(execution), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PutAsync(ConstructUrl(id), jsonContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TourExecutionDto>(responseContent);

            return CreateResponse(result.ToResult());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            using var response = await _httpClient.DeleteAsync(ConstructUrl(id));

            if (response.IsSuccessStatusCode) return NoContent();

            var errorResult = Result.Fail("Failed to delete tour execution.");
            return CreateResponse(errorResult);
        }

        [HttpGet("all/{tourId}/{touristId:long}")]
        public async Task<ActionResult<List<TourExecutionDto>>> GetByTouristAndTour(string tourId, long touristId)
        {
            var requestUri = ConstructUrl($"all/{tourId}/{touristId}");

            using var response = await _httpClient.GetAsync(requestUri);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest($"Failed to get tour executions: {result}");
            }

            var tours = JsonSerializer.Deserialize<List<TourExecutionDto>>(result);
            if (tours is null)
            {
                var noTours = Result.Ok("No tours found.");
                return CreateResponse(noTours);

            }

            var pagedResult = PaginateResult(0, 0, tours);
            return Ok(pagedResult);
        }

        [HttpGet("{tourId}/{touristId:long}")]
        public async Task<ActionResult<List<TourExecutionDto>>> GetActiveByTouristAndTour(string tourId, long touristId)
        {
            var requestUri = ConstructUrl($"{tourId}/{touristId}");

            using var response = await _httpClient.GetAsync(requestUri);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest($"Failed to get tour executions: {result}");
            }

            var execution = JsonSerializer.Deserialize<TourExecutionDto>(result);
            return Ok(execution);
        }
        private string ConstructUrl(string relativePath)
        {
            return $"{_httpClient.BaseAddress}/{relativePath}";
        }

        private static PagedResult<TourExecutionDto> PaginateResult(int page, int pageSize, List<TourExecutionDto> checkpoints)
        {
            if (page == 0 && pageSize == 0)
            {
                return new PagedResult<TourExecutionDto>(checkpoints, checkpoints.Count);
            }

            var totalCount = checkpoints.Count;
            var startIndex = (page - 1) * pageSize;
            var paginatedList = checkpoints.Skip(startIndex).Take(pageSize).ToList();
            return new PagedResult<TourExecutionDto>(paginatedList, totalCount);
        }



        //      private readonly ITourExecutionService _tourExecutionService;
        //      private readonly ITourRecommendationService _tourRecommendationService;
        //      private readonly IEmailService _emailService;

        //      public TourExecutionController(ITourExecutionService tourExecutionService, ITourRecommendationService tourRecommendationService,
        //          IEmailService emailService)
        //      {
        //          _tourExecutionService = tourExecutionService;
        //          _tourRecommendationService = tourRecommendationService;
        //          _emailService = emailService;
        //      }
        //      [HttpPut("{id:int}")]
        //      public ActionResult<TourExecutionDto> CheckPosition([FromBody] TouristPositionDto touristPosition, long id)
        //      {
        //          var result = _tourExecutionService.CheckPosition(touristPosition, id);
        //          return CreateResponse(result);
        //      }
        //      [HttpPut("abandoned")]
        //      public ActionResult<TourExecutionDto> Abandon([FromBody] long id)
        //      {
        //          var result = _tourExecutionService.Abandon(id, User.PersonId());
        //          return CreateResponse(result);
        //      }
        //      [HttpGet("get-suggested-tours/{id:int}")]
        //      public ActionResult<TourExecutionDto> GetSuggestedTours(long id)
        //      {
        //          var result = _tourExecutionService.GetSuggestedTours(id, User.PersonId(), _tourRecommendationService.GetAppropriateTours(User.PersonId()));
        //          return CreateResponse(result);
        //      }
        //      [HttpGet("send-tours-to-mail/{id:int}")]
        //      public ActionResult<TourPreviewDto> SendRecommendedToursToMail(long id)
        //      {
        //	var result = _tourExecutionService.GetSuggestedTours(id, User.PersonId(), _tourRecommendationService.GetAppropriateTours(User.PersonId()));
        //          string email = _tourExecutionService.GetEmailByUserId(User.PersonId()).Value;
        //          string name = _tourExecutionService.GetNameByUserId(User.PersonId()).Value;
        //	List<long> recommendedToursIds = new List<long>();
        //          List<string> tourNames = new List<string>();
        //	foreach (var rt in result.Value)
        //          {
        //              recommendedToursIds.Add(rt.Id);
        //              tourNames.Add(rt.Name);
        //          }
        //	_emailService.SendRecommendedToursEmail(email, name, recommendedToursIds, tourNames);
        //	return CreateResponse(result);
        //}
    }
}

using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;


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
            _httpClient.BaseAddress = new Uri("http://host.docker.internal:8081/v1/execution");
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

        [HttpGet("{id:long}")]
        public async Task<ActionResult<TourExecutionDto>> GetById(long id)
        {
            using var response = await _httpClient.GetAsync(ConstructUrl(id.ToString()));
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = Result.Fail($"Failed to get execution: {result}");
                return CreateResponse(errorResult);
            }

            var execution = JsonSerializer.Deserialize<TourExecutionDto>(result);
            return Ok(execution);
        }

        [HttpPost("{tourId:long}")]
        public async Task<ActionResult<TourExecutionDto>> Create(long tourId)
        {
            TourExecutionDto dto = new TourExecutionDto();
            dto.TourId = tourId;
            dto.TouristId = User.PersonId();
            dto.Start = DateTime.Now;
            dto.LastActivity = DateTime.Now;
            dto.ExecutionStatus = "InProgress";
            dto.CompletedCheckpoints = new List<CheckpointCompletitionDto>();

            using var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync("", jsonContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TourExecutionDto>(responseContent);

            return CreateResponse(result.ToResult());
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<TourExecutionDto>> Update([FromBody] TourExecutionDto execution, long id)
        {
            using var jsonContent = new StringContent(JsonSerializer.Serialize(execution), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PutAsync(ConstructUrl(id.ToString()), jsonContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TourExecutionDto>(responseContent);

            return CreateResponse(result.ToResult());
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> Delete(long id)
        {
            using var response = await _httpClient.DeleteAsync(ConstructUrl(id.ToString()));

            if (response.IsSuccessStatusCode) return NoContent();

            var errorResult = Result.Fail("Failed to delete tour execution.");
            return CreateResponse(errorResult);
        }

        [HttpGet("all/{tourId:long}/{touristId:long}")]
        public async Task<ActionResult<List<TourExecutionDto>>> GetByTouristAndTour(long tourId, long touristId)
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

        [HttpGet("{tourId:long}/{touristId:long}")]
        public async Task<ActionResult<List<TourExecutionDto>>> GetActiveByTouristAndTour(long tourId, long touristId)
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

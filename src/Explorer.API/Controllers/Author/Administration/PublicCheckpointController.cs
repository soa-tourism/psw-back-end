using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Explorer.API.Controllers.Author.Administration
{
    //[Authorize(Policy = "administratorAndAuthorPolicy")] //administratorAndAuthorPolicy authorPolicy
    [Route("api/administration/publicCheckpoint")]
    public class PublicCheckpointController : BaseApiController
    {
        private readonly HttpClient _httpClient;
        public PublicCheckpointController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8081/v1/tours/publicCheckpoint");
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<PublicCheckpointDto>>> GetAll()
        {
            using var response = await _httpClient.GetAsync("");
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = Result.Fail($"Failed to get public checkpoints: {result}");
                return CreateResponse(error);
            }

            var checkpoints = JsonSerializer.Deserialize<List<PublicCheckpointDto>>(result);
            if (checkpoints is null)
            {
                var noPublicCh = Result.Ok("No public checkpoints found.");
                return CreateResponse(noPublicCh);

            }

            var pagedResult = PaginateResult(0, 0, checkpoints);
            return Ok(pagedResult);
        }

        [HttpGet("details/{id:long}")]
        public async Task<ActionResult<PublicCheckpointDto>> GetById(long id)
        {
            using var response = await _httpClient.GetAsync(ConstructUrl(id.ToString()));
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = Result.Fail($"Failed to get public checkpoint: {result}");
                return CreateResponse(errorResult);
            }

            var tour = JsonSerializer.Deserialize<PublicCheckpointDto>(result);
            return Ok(tour);
        }

        // TODO - cant create because of notification 
        //[HttpPost("create/{checkpointRequestId:long}/{notificationComment}")]
        //public async Task<ActionResult<PublicCheckpointDto>> Create(int checkpointRequestId, string notificationComment)
        //{
        //    var result = _publicCheckpointService.Create(checkpointRequestId, notificationComment);
        //    return CreateResponse(result);
        //}

        // TODO - fix frontend url
        [HttpPut("{id:long}")]
        public async Task<ActionResult<PublicCheckpointDto>> Update(PublicCheckpointDto publicCheckpointDto)
        {
            using var jsonContent = new StringContent(JsonSerializer.Serialize(publicCheckpointDto), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PutAsync(ConstructUrl(publicCheckpointDto.Id.ToString()), jsonContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<BasicTourDto>(responseContent);

            return CreateResponse(result.ToResult());
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> Delete(long id)
        {
            using var response = await _httpClient.DeleteAsync(ConstructUrl(id.ToString()));

            if (response.IsSuccessStatusCode) return NoContent();

            var errorResult = Result.Fail("Failed to delete public checkpoint.");
            return CreateResponse(errorResult);
        }

        ///////
        private string ConstructUrl(string relativePath)
        {
            return $"{_httpClient.BaseAddress}/{relativePath}";
        }

        private static PagedResult<PublicCheckpointDto> PaginateResult(int page, int pageSize, List<PublicCheckpointDto> checkpoints)
        {
            if (page == 0 && pageSize == 0)
            {
                return new PagedResult<PublicCheckpointDto>(checkpoints, checkpoints.Count);
            }

            var totalCount = checkpoints.Count;
            var startIndex = (page - 1) * pageSize;
            var paginatedList = checkpoints.Skip(startIndex).Take(pageSize).ToList();
            return new PagedResult<PublicCheckpointDto>(paginatedList, totalCount);
        }


        //// TODO
        //[HttpGet("atPlace/{longitude:double}/{latitude:double}")]
        //public ActionResult<PublicCheckpointDto> GetAllAtPlace(double longitude, double latitude)
        //{
        //    var result = _publicCheckpointService.GetAllAtPlace(longitude, latitude);
        //    return CreateResponse(result);
        //}
    }
}

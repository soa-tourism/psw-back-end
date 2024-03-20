using Explorer.API.Services;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentResults;
using System.Text;
using System.Text.Json;

namespace Explorer.API.Controllers.Author.Administration
{
    [Route("api/administration/checkpoint")]
    public class CheckpointController : BaseApiController
    {
        private readonly HttpClient _httpClient;
        private readonly ImageService _imageService;

        public CheckpointController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8081/v1/checkpoint");
            _imageService = new ImageService();
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<CheckpointDto>>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            using var response = await _httpClient.GetAsync("");
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = Result.Fail($"Failed to get checkpoints: {result}");
                return CreateResponse(error);
            }

            var checkpoints = JsonSerializer.Deserialize<List<CheckpointDto>>(result);
            if (checkpoints is null)
            {
                var noTours = Result.Ok("No checkpoints found.");
                return CreateResponse(noTours);

            }

            var pagedResult = PaginateResult(page, pageSize, checkpoints);
            return Ok(pagedResult);
        }

        [HttpGet("details/{id:long}")]
        [Authorize(Policy = "authorPolicy")]
        public async Task<ActionResult<CheckpointDto>> GetById(long id)
        {
            using var response = await _httpClient.GetAsync(ConstructUrl(id.ToString()));
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = Result.Fail($"Failed to get checkpoint: {result}");
                return CreateResponse(errorResult);
            }

            var tour = JsonSerializer.Deserialize<CheckpointDto>(result);
            return Ok(tour);
        }

        [HttpPost("create/{status}")]
        [Authorize(Policy = "authorPolicy")]
        public async Task<ActionResult<CheckpointDto>> Create([FromForm] CheckpointDto checkpoint, [FromRoute] string status, [FromForm] List<IFormFile>? pictures = null)
        {
            if (pictures != null && pictures.Any())
            {
                var imageNames = _imageService.UploadImages(pictures);
                checkpoint.Pictures = imageNames;
            }

            using var jsonContent = new StringContent(JsonSerializer.Serialize(checkpoint), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync("", jsonContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CheckpointDto>(responseContent);

            return CreateResponse(result.ToResult());
        }

        [HttpPut("{id:long}")]
        [Authorize(Policy = "authorPolicy")]
        public async Task<ActionResult<CheckpointDto>> Update([FromForm] CheckpointDto checkpoint, long id, [FromForm] List<IFormFile>? pictures = null)
        {
            if (pictures != null && pictures.Any())
            {
                var imageNames = _imageService.UploadImages(pictures);
                checkpoint.Pictures = imageNames;
            }

            using var jsonContent = new StringContent(JsonSerializer.Serialize(checkpoint), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PutAsync(ConstructUrl(id.ToString()), jsonContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CheckpointDto>(responseContent);

            return CreateResponse(result.ToResult());
        }

        [HttpDelete("{id:long}")]
        [Authorize(Policy = "authorPolicy")]
        public async Task<ActionResult> Delete(long id)
        {
            using var response = await _httpClient.DeleteAsync(ConstructUrl(id.ToString()));

            if (response.IsSuccessStatusCode) return NoContent();

            var errorResult = Result.Fail("Failed to delete checkpoint.");
            return CreateResponse(errorResult);
        }

        [HttpGet("{id:long}")]
        [Authorize(Policy = "authorPolicy")]
        public async Task<ActionResult<List<CheckpointDto>>> GetAllByTour([FromQuery] int page, [FromQuery] int pageSize, long id)
        {
            var requestUri = ConstructUrl(id.ToString());

            using var response = await _httpClient.GetAsync(requestUri);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest($"Failed to get checkpoints for tour: {result}");
            }

            var checkpoints = JsonSerializer.Deserialize<List<CheckpointDto>>(result);
            if (checkpoints is null)
            {
                var noToursCheckpoints = Result.Ok("No checkpoints for tour found.");
                return CreateResponse(noToursCheckpoints);

            }

            var pagedResult = PaginateResult(page, pageSize, checkpoints);
            return Ok(pagedResult);
        }


        private string ConstructUrl(string relativePath)
        {
            return $"{_httpClient.BaseAddress}/{relativePath}";
        }

        private static PagedResult<CheckpointDto> PaginateResult(int page, int pageSize, List<CheckpointDto> checkpoints)
        {
            if (page == 0 && pageSize == 0)
            {
                return new PagedResult<CheckpointDto>(checkpoints, checkpoints.Count);
            }

            var totalCount = checkpoints.Count;
            var startIndex = (page - 1) * pageSize;
            var paginatedList = checkpoints.Skip(startIndex).Take(pageSize).ToList();
            return new PagedResult<CheckpointDto>(paginatedList, totalCount);
        }

        //// TODO - checkpoint secret
        //[HttpPut("createSecret/{id:long}")]
        //[Authorize(Policy = "authorPolicy")]
        //public ActionResult<CheckpointDto> CreateCheckpointSecret([FromForm] CheckpointSecretDto secretDto, long id, [FromForm] List<IFormFile>? pictures = null)
        //{
        //    if (pictures != null && pictures.Any())
        //    {
        //        var imageNames = _imageService.UploadImages(pictures);
        //        secretDto.Pictures = imageNames;
        //    }

        //    var result = _checkpointService.CreateChechpointSecreat(secretDto, id, User.PersonId());
        //    return CreateResponse(result);
        //}

        //[HttpPut("updateSecret/{id:long}")]
        //[Authorize(Policy = "authorPolicy")]
        //public ActionResult<CheckpointDto> UpdateCheckpointSecret([FromForm] CheckpointSecretDto secretDto, long id, [FromForm] List<IFormFile>? pictures = null)
        //{
        //    if (pictures != null && pictures.Any())
        //    {
        //        var imageNames = _imageService.UploadImages(pictures);
        //        secretDto.Pictures = imageNames;
        //    }

        //    var result = _checkpointService.UpdateChechpointSecreat(secretDto, id, User.PersonId());
        //    return CreateResponse(result);
        //}
    }
}

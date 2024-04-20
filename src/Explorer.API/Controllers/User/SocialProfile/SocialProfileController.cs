using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Explorer.API.Controllers.User.SocialProfile
{
    [Authorize(Policy = "userPolicy")]
    [Route("api/social-profile")]
    public class SocialProfileController : BaseApiController
    {
        private readonly HttpClient _httpClient;

        public SocialProfileController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            //_httpClient.BaseAddress = new Uri("http://host.docker.internal:8082");
            _httpClient.BaseAddress = new Uri("http://localhost:8082");
        }

        [HttpGet("get/{userId:long}")]
        public async Task<ActionResult<SocialProfileDto>> GetSocialProfile(long userId)
        {
            if (!(User.PersonId().ToString()).Equals(userId.ToString())) {
                return Forbid();
            }

            using var response = await _httpClient.GetAsync(ConstructUrl("profile/" + userId.ToString()));
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = Result.Fail($"Failed to get social profile: {result}");
                return CreateResponse(errorResult);
            }
        
            try
            {
                var profiles = JsonSerializer.Deserialize<List<SocialProfileDto>>(result); // Deserialization
                if (profiles.Count > 0)
                {   // Assuming you only expect one profile for a given user ID, return the first profile
                    return Ok(profiles[0]);
                }
                else
                {   // Handle the case where no profile is found for the given user ID
                    return NotFound();
                }
            }
            catch (Exception ex)
            {   
                return StatusCode(StatusCodes.Status500InternalServerError); // Log or handle the exception
            }
        }

        [HttpGet("get-followers/{userId:long}")]
        public async Task<ActionResult<SocialProfileDto>> GetFollowers(long userId)
        {
            if (!(User.PersonId().ToString()).Equals(userId.ToString()))
            {
                return Forbid();
            }

            using var response = await _httpClient.GetAsync(ConstructUrl("profiles/followers/" + userId.ToString()));
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = Result.Fail($"Failed to get social profile followers: {result}");
                return CreateResponse(errorResult);
            }

            try
            {
                var profiles = JsonSerializer.Deserialize<List<SocialProfileDto>>(result); // Deserialization
                return Ok(profiles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError); // Log or handle the exception
            }
        }

        [HttpGet("get-following/{userId:long}")]
        public async Task<ActionResult<SocialProfileDto>> GetFollowing(long userId)
        {
            if (!(User.PersonId().ToString()).Equals(userId.ToString()))
            {
                return Forbid();
            }

            using var response = await _httpClient.GetAsync(ConstructUrl("profiles/following/" + userId.ToString()));
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = Result.Fail($"Failed to get social profile following: {result}");
                return CreateResponse(errorResult);
            }

            try
            {
                var profiles = JsonSerializer.Deserialize<List<SocialProfileDto>>(result); // Deserialization
                return Ok(profiles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError); // Log or handle the exception
            }
        }

        // TODO - fix follow
        [HttpPost("follow/{userId:long}/{followedId:long}")]
        public async Task<ActionResult<SocialProfileDto>> Follow(long userId, long followedId)
        {
            if (!(User.PersonId().ToString()).Equals(userId.ToString()))
            {
                return Forbid();
            }

            using var response = await _httpClient.PostAsync(ConstructUrl("profiles/follow/" + userId.ToString() + "/" + followedId.ToString()), new StringContent(""));
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = Result.Fail($"Failed to follow social profile: {result}");
                return CreateResponse(errorResult);
            }

            try
            {
                var profiles = JsonSerializer.Deserialize<List<SocialProfileDto>>(result); // Deserialization
                return Ok(profiles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError); // Log or handle the exception
            }
        }

        // TODO - fix unfollow
        [HttpDelete("unfollow/{userId:long}/{followedId:long}")]
        public async Task<ActionResult<SocialProfileDto>> Unfollow(long userId, long followedId)
        {
            if (!(User.PersonId().ToString()).Equals(userId.ToString()))
            {
                return Forbid();
            }

            using var response = await _httpClient.DeleteAsync(ConstructUrl("profiles/unfollow/" + userId.ToString() + "/" + followedId.ToString()));
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = Result.Fail($"Failed to unfollow social profile: {result}");
                return CreateResponse(errorResult);
            }

            try
            {
                var profiles = JsonSerializer.Deserialize<List<SocialProfileDto>>(result); // Deserialization
                return Ok(profiles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError); // Log or handle the exception
            }
        }

        private string ConstructUrl(string relativePath)
        {
            return $"{_httpClient.BaseAddress}/{relativePath}";
        }
    }
}

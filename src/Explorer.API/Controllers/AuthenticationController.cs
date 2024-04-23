using Explorer.API.Services;
using Explorer.Blog.Core.Domain.BlogPosts;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using static System.Net.Mime.MediaTypeNames;

namespace Explorer.API.Controllers;

[Route("api/users")]
public class AuthenticationController : BaseApiController
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ImageService _imageService;
    private readonly IVerificationService _verificationService;
    private readonly HttpClient _httpClient;
    private readonly IUserService _userService;

    public AuthenticationController(IAuthenticationService authenticationService, IVerificationService verificationService, IHttpClientFactory httpClientFactory, IUserService userService)
    {
        _authenticationService = authenticationService;
        _imageService = new ImageService();
        _verificationService = verificationService;
        _httpClient = httpClientFactory.CreateClient();
        //_httpClient.BaseAddress = new Uri("http://host.docker.internal:8082");
        _httpClient.BaseAddress = new Uri("http://localhost:8082");
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<AccountRegistrationDto>> RegisterTourist([FromForm] AccountRegistrationDto account, IFormFile profilePicture = null)
    {
        if (profilePicture != null)
        {
            var pictureUrl = _imageService.UploadImages(new List<IFormFile> { profilePicture });
            account.ProfilePictureUrl = pictureUrl[0];
        }
        var result = _authenticationService.RegisterTourist(account);
        var username = result.Value.Username;
        var userId = _userService.GetUserByUsername(username).Value.Id;

        using var response = await _httpClient.PutAsync(ConstructUrl("profiles/add/" + userId.ToString() + "/" + username), new StringContent(""));
        var result2 = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var errorResult = Result.Fail($"Failed to follow social profile: {result}");
            return CreateResponse(errorResult);
        }
        return Ok();
    }


    [HttpPost("login")]
    public ActionResult<AuthenticationTokensDto> Login([FromBody] CredentialsDto credentials)
    {
        var result = _authenticationService.Login(credentials);
        return CreateResponse(result);
    }

    [HttpGet("verify/{verificationTokenData}")]
    public ActionResult VerifyUser(string verificationTokenData)
    {
        var result = _verificationService.Verify(verificationTokenData);
        return Redirect("http://localhost:4200/verification-success");
    }

    [HttpGet("verificationStatus/{username}")]
    public ActionResult<bool> IsUserVerified(string username)
    {
        var result = _verificationService.IsUserVerified(username);
        return CreateResponse(result);
    }

    [HttpGet("send-password-reset-email/{username}")]
    public ActionResult<bool> SendPasswordResetEmail(string username)
    {
        var result = _authenticationService.SendPasswordResetEmail(username);
        return CreateResponse(result);
    }
    private string ConstructUrl(string relativePath)
    {
        return $"{_httpClient.BaseAddress}/{relativePath}";
    }
}
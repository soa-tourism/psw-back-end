using Explorer.API.Services;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain.BlogPosts;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace Explorer.API.Controllers.User.Blogging;

[Route("api/blogging/blog-posts")]
public class BlogPostController : BaseApiController
{
    private readonly IBlogPostService _blogPostService;
    private readonly ImageService _imageService;
    private readonly HttpClient _httpClient;

    public BlogPostController(IBlogPostService blogPostService, HttpClient httpClient)
    {
        _blogPostService = blogPostService;
        _imageService = new ImageService();
        _httpClient = httpClient;
    }

    [HttpGet]
    public ActionResult<PagedResult<BlogPostDto>> GetAllNonDraft([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? status = null)
    { 
        var result = status is null
            ? _blogPostService.GetAllNonDraft(page, pageSize)
            : _blogPostService.GetFilteredByStatus(page, pageSize, status);
        return CreateResponse(result);
    }

    [HttpGet("{id:int}")]
    public ActionResult<BlogPostDto> GetById(int id)
    {
        var result = _blogPostService.Get(id);
        return CreateResponse(result);
    }

    [HttpPost]
    [Authorize(Policy = "userPolicy")]
    public ActionResult<BlogPostDto> Create([FromForm] BlogPostDto blogPost, [FromForm] List<IFormFile>? images = null)
    {
        if (User.PersonId() != blogPost.UserId) return CreateResponse(Result.Fail(FailureCode.Forbidden));

        if (images != null && images.Any())
        {
            var imageNames = _imageService.UploadImages(images);
            blogPost.ImageNames = imageNames;
        }

        var result = _blogPostService.Create(blogPost);
        return CreateResponse(result);
    }

    [HttpGet("user/{userId:int}")]
    [Authorize(Policy = "userPolicy")]
    public ActionResult<PagedResult<BlogPostDto>> GetAllByUser([FromQuery] int page, [FromQuery] int pageSize, int userId)
    {
        if (User.PersonId() != userId) return CreateResponse(Result.Fail(FailureCode.Forbidden));

        var result = _blogPostService.GetAllByUser(page, pageSize, userId);
        return CreateResponse(result);
    }

    [HttpGet("followers/{userId:long}")]
    [Authorize(Policy = "userPolicy")]
    public async Task<ActionResult<PagedResult<BlogPostDto>>> GetAllByFollowing(long userId)
    {
        if (User.PersonId() != userId) return CreateResponse(Result.Fail(FailureCode.Forbidden));

        var followersResponse = await _httpClient.GetAsync($"http://host.docker.internal:8082/profiles/following/{userId}");
        if (!followersResponse.IsSuccessStatusCode)
        {
            return StatusCode((int)followersResponse.StatusCode);
        }

        var followersJson = await followersResponse.Content.ReadAsStringAsync();
        var followers = JsonSerializer.Deserialize<List<SocialProfileDto>>(followersJson);

        List<BlogPostDto> ret = new();
        var result = _blogPostService.GetAllNonDraft(0, 100);
        foreach(var blog in result.Value.Results)
        {
            if(followers.Any(f=>f.userId==blog.UserId))
                ret.Add(blog);
        }
        return CreateResponse(ret.ToResult());
    }


    [HttpPut("{id:int}")]
    [Authorize(Policy = "userPolicy")]
    public ActionResult<BlogPostDto> Update(int id, [FromForm] BlogPostDto blogPost, [FromForm] List<IFormFile>? images = null)
    {
        if (images != null && images.Any())
        {
            var imageNames = _imageService.UploadImages(images);
            blogPost.ImageNames = imageNames;
        }

        blogPost.Id = id;
        var result = _blogPostService.Update(blogPost, User.PersonId());
        return CreateResponse(result);
    }

    [HttpPatch("{id:int}/close")]
    [Authorize(Policy = "userPolicy")]
    public ActionResult<BlogPostDto> Close(int id)
    {
        var result = _blogPostService.Close(id, User.PersonId());
        return CreateResponse(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "userPolicy")]
    public ActionResult Delete(int id)
    {
        var result = _blogPostService.Delete(id, User.PersonId());
        return CreateResponse(result);
    }

    [HttpPut("{id:int}/ratings")]
    [Authorize(Policy = "userPolicy")]
    public ActionResult<BlogPostDto> Rate(int id, [FromBody] BlogRatingDto blogRating)
    {
        if (User.PersonId() != blogRating.UserId) return CreateResponse(Result.Fail(FailureCode.Forbidden));

        var result = _blogPostService.Rate(id, blogRating);
        return CreateResponse(result);
    }
}
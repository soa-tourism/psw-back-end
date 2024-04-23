using Explorer.API.Services;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.User.Blogging;

[Route("api/blogging/blog-posts")]
public class BlogPostController : BaseApiController
{
    private readonly IBlogPostService _blogPostService;
    private readonly ImageService _imageService;

    public BlogPostController(IBlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
        _imageService = new ImageService();
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
    public ActionResult<PagedResult<BlogPostDto>> GetAllByFollowing([FromQuery] int page, [FromQuery] int pageSize, int userId)
    {
        if (User.PersonId() != userId) return CreateResponse(Result.Fail(FailureCode.Forbidden));

        var result = _blogPostService.GetAllByFollowing(page, pageSize, userId);
        return CreateResponse(result);
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
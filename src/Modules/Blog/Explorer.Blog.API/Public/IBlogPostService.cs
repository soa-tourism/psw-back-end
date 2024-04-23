using Explorer.Blog.API.Dtos;
using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;

namespace Explorer.Blog.API.Public;

public interface IBlogPostService
{
    Result<PagedResult<BlogPostDto>> GetAllNonDraft(int page, int pageSize);
    Result<PagedResult<BlogPostDto>> GetAllByUser(int page, int pageSize, int id);
    Result<PagedResult<BlogPostDto>> GetFilteredByStatus(int page, int pageSize, string blogPostStatus);
    Result<BlogPostDto> Get(int id);
    Result<BlogPostDto> Create(BlogPostDto blogPost);
    Result<BlogPostDto> Update(BlogPostDto blogPost, int userId);
    Result<BlogPostDto> Close(int id, int userId);
    Result Delete(int id, int userId);
    Result<BlogPostDto> Rate(int id, BlogRatingDto blogRating);
    Result<List<BlogPostDto>> GetTopRatedBlogPosts(int cout);
}
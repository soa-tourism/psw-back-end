using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain.BlogPosts;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Internal;
using FluentResults;

namespace Explorer.Blog.Core.UseCases;

public class BlogPostService : CrudService<BlogPostDto, BlogPost>, IBlogPostService
{
    private readonly IBlogPostRepository _blogPostsRepository;
    private readonly IMapper _mapper;
    private readonly IInternalUserService _userService;

    public BlogPostService(IBlogPostRepository repository, IMapper mapper, IInternalUserService userService) : base(
        repository, mapper)
    {
        _mapper = mapper;
        _blogPostsRepository = repository;
        _userService = userService;
    }

    public Result<PagedResult<BlogPostDto>> GetAllNonDraft(int page, int pageSize)
    {
        try
        {
            var blogPosts = _blogPostsRepository.GetAllNonDraft(page, pageSize);
            var blogPostDtos = MapToDto(blogPosts);
            foreach (var blogPostDto in blogPostDtos.Value.Results)
                blogPostDto.Username = GetUsername(blogPostDto.UserId);

            return blogPostDtos;
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }

    public Result<PagedResult<BlogPostDto>> GetAllByUser(int page, int pageSize, int id)
    {
        try
        {
            var blogPosts = _blogPostsRepository.GetAllByUser(page, pageSize, id);
            var blogPostDtos = MapToDto(blogPosts);
            foreach (var blogPostDto in blogPostDtos.Value.Results)
                blogPostDto.Username = GetUsername(blogPostDto.UserId);

            return blogPostDtos;
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }

    public Result<PagedResult<BlogPostDto>> GetFilteredByStatus(int page, int pageSize, string blogPostStatus)
    {
        try
        {
            if (!Enum.TryParse(blogPostStatus, true, out BlogPostStatus status))
                throw new ArgumentException("Invalid blog post status value.");

            var blogPosts = _blogPostsRepository.GetFilteredByStatus(page, pageSize, status);
            var blogPostDtos = MapToDto(blogPosts);
            foreach (var blogPostDto in blogPostDtos.Value.Results)
                blogPostDto.Username = GetUsername(blogPostDto.UserId);

            return blogPostDtos;
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }

    public override Result<BlogPostDto> Get(int id)
    {
        try
        {
            var blogPost = CrudRepository.Get(id);
            var blogPostDto = MapToDto(blogPost);

            blogPostDto.Username = GetUsername(blogPostDto.UserId);
            blogPostDto.Comments?.ForEach(comment => comment.Username = GetUsername(comment.UserId));

            return blogPostDto;
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }

    public Result<BlogPostDto> Update(BlogPostDto blogPostDto, int userId)
    {
        try
        {
            var blogPost = CrudRepository.Get(blogPostDto.Id);

            if(!blogPost.IsCreatedByUser(userId))
                throw new InvalidOperationException("Only the creator of the blog can update it.");

            if (blogPost.Status != BlogPostStatus.Draft)
                throw new ArgumentException("Only Blog Posts with Draft Status can be updated.");

            blogPost.Update(MapToDomain(blogPostDto));
            var result = CrudRepository.Update(blogPost);

            return MapToDto(result);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return Result.Fail(FailureCode.Forbidden).WithError(e.Message);
        }
    }

    public Result<BlogPostDto> Close(int id, int userId)
    {
        try
        {
            var blogPost = _blogPostsRepository.Get(id);

            if (!blogPost.IsCreatedByUser(userId))
                throw new InvalidOperationException("Only the creator of the blog can close it.");

            blogPost.Close();
            var result = _blogPostsRepository.Update(blogPost);

            return MapToDto(result);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return Result.Fail(FailureCode.Forbidden).WithError(e.Message);
        }
    }

    public Result Delete(int id, int userId)
    {
        try
        {
            var blogPost = _blogPostsRepository.Get(id);

            if (!blogPost.IsCreatedByUser(userId))
                throw new InvalidOperationException("Only the creator of the blog can delete it.");

            CrudRepository.Delete(id);
            return Result.Ok();
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return Result.Fail(FailureCode.Forbidden).WithError(e.Message);
        }
    }

    public Result<BlogPostDto> Rate(int id, BlogRatingDto blogRatingDto)
    {
        try
        {
            var blogPost = _blogPostsRepository.Get(id);
            Enum.TryParse<Rating>(blogRatingDto.Rating, out var rating);

            var blogRating = _mapper.Map<BlogRatingDto, BlogRating>(blogRatingDto);

            blogPost.AddRating(blogRating);
            var result = _blogPostsRepository.Update(blogPost);

            return MapToDto(result);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }

    private string GetUsername(int userId)
    {
        var user = _userService.Get(userId);
        return user.Value.Username;
    }

    public Result<List<BlogPostDto>> GetTopRatedBlogPosts(int count)
    {
        var publishedBlogPosts = _blogPostsRepository.GetAllPublished();
        var topRatedPosts = publishedBlogPosts
            .OrderByDescending(post => post.Ratings?.Sum(rating => rating.Rating == Rating.Upvote ? 1 : -1)) 
            .Take(count)
            .ToList();

        return MapToDto(topRatedPosts); ;
    }

}
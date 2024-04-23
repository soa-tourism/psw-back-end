using Explorer.Blog.Core.Domain.BlogPosts;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.API.Dtos;
using System.Net.Http;
using System.Text.Json;

namespace Explorer.Blog.Infrastructure.Database.Repositories;

public class BlogPostDatabaseRepository : CrudDatabaseRepository<BlogPost, BlogContext>, IBlogPostRepository
{
    private readonly HttpClient _httpClient;
    public BlogPostDatabaseRepository(BlogContext blogContext, HttpClient httpClient) : base(blogContext)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public PagedResult<BlogPost> GetAllNonDraft(int page, int pageSize)
    {
        var query = DbContext.BlogPosts.Where(bp => bp.Status != BlogPostStatus.Draft);
        var count = query.Count();
        var items = PageResults(page, pageSize, query);

        return new PagedResult<BlogPost>(items, count);
    }

    public PagedResult<BlogPost> GetAllByUser(int page, int pageSize, long userId)
    {
        var query = DbContext.BlogPosts.Where(bp => bp.UserId == userId);
        var count = query.Count();
        var items = PageResults(page, pageSize, query);

        return new PagedResult<BlogPost>(items, count);
    }

    public PagedResult<BlogPost> GetFilteredByStatus(int page, int pageSize, BlogPostStatus status)
    {
        var query = DbContext.BlogPosts.Where(bp => bp.Status == status);
        var count = query.Count();
        var items = PageResults(page, pageSize, query);

        return new PagedResult<BlogPost>(items, count);
    }

    private List<BlogPost> PageResults(int page, int pageSize, IQueryable<BlogPost> query)
    {
        if (pageSize != 0 && page != 0)
            return query.OrderByDescending(bp => bp.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

        return query.ToList();
    }
    
    public List<BlogPost> GetAllPublished()
    {
        return DbContext.BlogPosts
            .Where(bp => bp.Status == BlogPostStatus.Published)
            .ToList();
    }

    public PagedResult<BlogPost> GetAllByFollowing(int page, int pageSize, long userId)
    {
        var followingUserIds = GetFollowerIds(userId).Result;
        var publishedBlogPosts = GetAllPublished();

        var blogsOfFollowers = publishedBlogPosts
            .Where(post => followingUserIds.Contains((int)post.UserId))
            .OrderByDescending(post => post.Ratings?.Sum(rating => rating.Rating == Rating.Upvote ? 1 : -1))
            .ToList();

        return new PagedResult<BlogPost>(blogsOfFollowers, blogsOfFollowers.Count);
    }



    private async Task<List<int>> GetFollowerIds(long userId)
    {
        var response = await _httpClient.GetAsync($"http://localhost:8082/profiles/following/{userId}");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var socialProfile = JsonSerializer.Deserialize<SocialProfileDto>(json);

            if (socialProfile != null && socialProfile.Followed != null)
            {
                return socialProfile.Followed.Select(user => user.Id).ToList();
            }
        }

        return new List<int>();
    }
}


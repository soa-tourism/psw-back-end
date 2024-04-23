using Explorer.Blog.Core.Domain.BlogPosts;
using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Blog.Core.Domain.RepositoryInterfaces;

public interface IBlogPostRepository : ICrudRepository<BlogPost>
{
    PagedResult<BlogPost> GetAllNonDraft(int page, int pageSize);
    PagedResult<BlogPost> GetAllByUser(int page, int pageSize, long userId);
    PagedResult<BlogPost> GetAllByFollowing(int page, int pageSize, long userId);
    PagedResult<BlogPost> GetFilteredByStatus(int page, int pageSize, BlogPostStatus status);
    List<BlogPost> GetAllPublished();
}
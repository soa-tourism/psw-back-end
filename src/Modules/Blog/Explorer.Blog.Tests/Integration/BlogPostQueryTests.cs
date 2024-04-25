using Explorer.API.Controllers.User.Blogging;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Blog.Tests.Integration;

[Collection("Sequential")]
public class BlogPostQueryTests : BaseBlogIntegrationTest
{
    public BlogPostQueryTests(BlogTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Retrieves_all_non_draft()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = ((ObjectResult)controller.GetAllNonDraft(0, 0).Result)?.Value as PagedResult<BlogPostDto>;

        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(3);
        result.TotalCount.ShouldBe(3);
    }

    [Fact]
    public void Retrieves_by_id()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = ((ObjectResult)controller.GetById(-1).Result)?.Value as BlogPostDto;

        result.ShouldNotBeNull();
        result.UserId.ShouldBe(-1);
        result.Title.ShouldBe("Title 1");
        result.Description.ShouldBe("Description 1");
    }

    [Fact]
    public void Retrieves_all_by_user()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = ((ObjectResult)controller.GetAllByUser(0, 0, -1).Result)?.Value as PagedResult<BlogPostDto>;

        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(3);
        result.TotalCount.ShouldBe(3);
    }

    private static BlogPostController CreateController(IServiceScope scope)
    {
        return null;
        // return new BlogPostController(scope.ServiceProvider.GetRequiredService<IBlogPostService>())
        // {
        //     ControllerContext = BuildContext("-1")
        // };
    }
}
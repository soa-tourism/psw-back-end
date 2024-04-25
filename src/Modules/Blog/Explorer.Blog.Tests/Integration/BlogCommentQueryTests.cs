using Explorer.API.Controllers.User.Blogging;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Blog.Tests.Integration;

[Collection("Sequential")]
public class BlogCommentQueryTests : BaseBlogIntegrationTest
{
    public BlogCommentQueryTests(BlogTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Retrieves_all()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetById(-11).Result)?.Value as BlogPostDto;

        // Assert
        result.ShouldNotBeNull();
        result.Comments.Count.ShouldBe(2);
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
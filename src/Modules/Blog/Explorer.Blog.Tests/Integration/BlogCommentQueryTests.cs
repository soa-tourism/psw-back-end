﻿/*using Explorer.API.Controllers.User.Blogging;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Blog.Tests.Integration;

[Collection("Sequential")]
public class BlogCommentQueryTests : BaseBlogIntegrationTest
{
    public BlogCommentQueryTests(BlogTestFactory factory) : base(factory) { }

    [Fact]
    public void Retrieves_all()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<BlogCommentDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(3);
        result.TotalCount.ShouldBe(3);
    }

    private static BlogCommentController CreateController(IServiceScope scope)
    {
        return new BlogCommentController(scope.ServiceProvider.GetRequiredService<IBlogCommentService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
*/
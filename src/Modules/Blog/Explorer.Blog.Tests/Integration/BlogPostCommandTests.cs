using Explorer.API.Controllers.User.Blogging;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Blog.Tests.Integration;

[Collection("Sequential")]
public class BlogPostCommandTests : BaseBlogIntegrationTest
{
    public BlogPostCommandTests(BlogTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Creates()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
        var newEntity = new BlogPostDto
        {
            UserId = -1,
            Title = "Sample Title",
            Description = "Sample Description",
            CreationDate = DateTime.Now.ToUniversalTime(),
            ImageNames = new List<string> { "image1.jpg", "image2.jpg" },
            Status = "DRAFT"
        };

        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as BlogPostDto;

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.UserId.ShouldBe(newEntity.UserId);
        result.Title.ShouldBe(newEntity.Title);

        var storedEntity = dbContext.BlogPosts.OrderBy(i => i.Id).LastOrDefault(i => i.UserId == newEntity.UserId);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Create_fails_invalid_data()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
        var newEntity = new BlogPostDto
        {
            UserId = -1,
            Description = "I don't have a title...",
            ImageNames = new List<string> { "image1.jpg", "image2.jpg" }
        };

        var result = (ObjectResult)controller.Create(newEntity).Result;

        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(400);
    }

    [Fact]
    public void Updates()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
        var updatedEntity = new BlogPostDto
        {
            Id = -1,
            UserId = -1,
            Title = "Sample Title 1",
            Description = "Sample Description 1",
            CreationDate = DateTime.Now.ToUniversalTime(),
            ImageNames = new List<string> { "sample_image1.jpg", "sample_image2.jpg" }
        };

        var result = ((ObjectResult)controller.Update(-1, updatedEntity).Result)?.Value as BlogPostDto;

        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.UserId.ShouldBe(updatedEntity.UserId);
        result.Title.ShouldBe(updatedEntity.Title);
        result.Description.ShouldBe(updatedEntity.Description);
        result.CreationDate.ShouldBe(updatedEntity.CreationDate);
        result.ImageNames.ShouldBe(updatedEntity.ImageNames);

        var storedEntity = dbContext.BlogPosts.FirstOrDefault(i => i.Title == updatedEntity.Title);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
        var oldEntity = dbContext.BlogPosts.FirstOrDefault(i => i.Title == "Title 1");
        oldEntity.ShouldBeNull();
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
        var updatedEntity = new BlogPostDto
        {
            Id = -42,
            UserId = -1,
            Title = "Sample Title 1",
            Description = "Sample Description 1",
            CreationDate = DateTime.Now.ToUniversalTime(),
            ImageNames = new List<string> { "sample_image1.jpg", "sample_image2.jpg" }
        };

        var result = (ObjectResult)controller.Update(-42, updatedEntity).Result;

        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    [Fact]
    public void Deletes()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

        var result = (OkResult)controller.Delete(-2);

        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        var storedCourse = dbContext.BlogPosts.FirstOrDefault(i => i.Id == -3);
        storedCourse.ShouldBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = (ObjectResult)controller.Delete(-42);

        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
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
using Explorer.API.Controllers.Administrator.Administration;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Author
{
    [Collection("Sequential")]
    public class ObjectRequestCommandTests : BaseStakeholdersIntegrationTest
    {
        public ObjectRequestCommandTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates_request()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var newRequest = new ObjectRequestDto
            {
                MapObjectId = 3,
                AuthorId = 1,
                Status = "OnHold"
            };

            // Act
            var result = ((ObjectResult)controller.Create(newRequest).Result)?.Value as ObjectRequestDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.AuthorId.ShouldBe(1);
            result.MapObjectId.ShouldBe(3);
            result.Status.ShouldBe("OnHold");
        }

        [Fact]
        public void Accept_Request()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            // Act
            var result = ((ObjectResult)controller.AcceptRequest(-1, "kom").Result)?.Value as ObjectRequestDto; ;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Status.ShouldBe("Accepted");

            // Assert - Database
            var storedRequest = dbContext.ObjectRequests.FirstOrDefault(i => i.Id == -1);
            storedRequest.Status.ShouldBe(RequestStatus.Accepted);
        }

        [Fact]
        public void Reject_Request()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            // Act
            var result = ((ObjectResult)controller.RejectRequest(-1, "kom").Result)?.Value as ObjectRequestDto; ;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Status.ShouldBe("Rejected");

            // Assert - Database
            var storedRequest = dbContext.ObjectRequests.FirstOrDefault(i => i.Id == -1);
            storedRequest.Status.ShouldBe(RequestStatus.Rejected);
        }

        private static ObjectRequestController CreateController(IServiceScope scope)
        {
            return new ObjectRequestController(scope.ServiceProvider.GetRequiredService<IObjectRequestService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
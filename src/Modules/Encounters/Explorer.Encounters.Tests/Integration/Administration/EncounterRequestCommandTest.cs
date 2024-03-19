//using Explorer.API.Controllers.Administrator.Administration;
//using Explorer.Encounters.API.Dtos;
//using Explorer.Encounters.API.Public;
//using Explorer.Encounters.Core.Domain.Encounters;
//using Explorer.Encounters.Infrastructure.Database;
//using Explorer.Stakeholders.API.Dtos;
//using Explorer.Stakeholders.API.Public;
//using Explorer.Stakeholders.Infrastructure.Database;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.DependencyInjection;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Explorer.Encounters.Tests.Integration.Administration
//{
//    [Collection("Sequential")]
//    public class EncounterRequestCommandTest : BaseEncountersIntegrationTest
//    {
//        public EncounterRequestCommandTest(EncountersTestFactory factory) : base(factory) { }

//        [Fact]
//        public void Creates_request()
//        {
//            // Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//            var newRequest = new EncounterRequestDto
//            {
//                TouristId = 1,
//                EncounterId = 1,
//                Status = "OnHold"
//            };

//            // Act
//            var result = ((ObjectResult)controller.Create(newRequest).Result)?.Value as EncounterRequestDto;

//            // Assert - Response
//            result.ShouldNotBeNull();
//            result.TouristId.ShouldBe(1);
//            result.EncounterId.ShouldBe(1);
//            result.Status.ShouldBe("OnHold");
//        }

//        [Fact]
//        public void Accept_Request()
//        {
//            // Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

//            // Act
//            var result = ((ObjectResult)controller.AcceptRequest(-1).Result)?.Value as EncounterRequestDto; ;

//            // Assert - Response
//            result.ShouldNotBeNull();
//            result.Status.ShouldBe("Accepted");

//            // Assert - Database
//            var storedRequest = dbContext.EncounterRequests.FirstOrDefault(i => i.Id == -1);
//            storedRequest.Status.ShouldBe(RequestStatus.Accepted);
//        }

//        [Fact]
//        public void Reject_Request()
//        {
//            // Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

//            // Act
//            var result = ((ObjectResult)controller.RejectRequest(-1).Result)?.Value as EncounterRequestDto; ;

//            // Assert - Response
//            result.ShouldNotBeNull();
//            result.Status.ShouldBe("Rejected");

//            // Assert - Database
//            var storedRequest = dbContext.EncounterRequests.FirstOrDefault(i => i.Id == -1);
//            storedRequest.Status.ShouldBe(RequestStatus.Rejected);
//        }

//        private static EncounterRequestController CreateController(IServiceScope scope)
//        {
//            return new EncounterRequestController(scope.ServiceProvider.GetRequiredService<IEncounterRequestService>())
//            {
//                ControllerContext = BuildContext("-1")
//            };
//        }
//    }
//}

//using Explorer.API.Controllers.Tourist.Encounters;
//using Explorer.Encounters.API.Dtos;
//using Explorer.Encounters.API.Public;
//using Explorer.Encounters.Core.Domain.Encounters;
//using Explorer.Encounters.Infrastructure.Database;
//using Explorer.Stakeholders.Core.Domain;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.DependencyInjection;
//using Shouldly;

//namespace Explorer.Encounters.Tests.Integration.Administration
//{
//    [Collection("Sequential")]
//    public class EncounterExecutionCommandTests : BaseEncountersIntegrationTest
//    {
//        public EncounterExecutionCommandTests(EncountersTestFactory factory) : base(factory) { }
//        [Fact]
//        public void CompleteEncounter_fail_tourist_out_of_range()
//        {
//            // Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//            var expectedResponseCode = 400;
//            var encounterExecutionId = -3;
//            var longitude = 46;
//            var latitude = 46;

//            // Act
//            var result = (ObjectResult)controller.CompleteExecution(encounterExecutionId, longitude, latitude).Result;

//            //Assert
//            result.ShouldNotBeNull();
//            result.StatusCode.ShouldBe(expectedResponseCode);
//        }

//        [Fact]
//        public void CompleteEncounter_success()
//        {
//            // Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//            var expectedResponseCode = 200;
//            var encounterExecutionId = -3;
//            var longitude = 45;
//            var latitude = 45;

//            // Act
//            var result = (ObjectResult)controller.CompleteExecution(encounterExecutionId, longitude, latitude).Result;

//            //Assert
//            result.ShouldNotBeNull();
//            (result.Value as EncounterExecutionDto).Status.ShouldBe("Completed");
//            result.StatusCode.ShouldBe(expectedResponseCode);
//        }

//        [Fact]
//        public void ActivateEncounter_fail_tourist_out_of_range()
//        {
//            // Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//            var expectedResponseCode = 400;
//            var encounterId = -5;
//            var longitude = 46;
//            var latitude = 46;

//            // Act
//            var result = (ObjectResult)controller.Activate(encounterId, longitude, latitude).Result;

//            //Assert
//            result.ShouldNotBeNull();
//            result.StatusCode.ShouldBe(expectedResponseCode);
//        }

//        [Fact]
//        public void ActivateEncounter_success()
//        {
//            // Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//            var expectedResponseCode = 200;
//            var encounterId = -5;
//            var longitude = 45;
//            var latitude = 45;

//            // Act
//            var result = (ObjectResult)controller.Activate(encounterId, longitude, latitude).Result;

//            //Assert
//            result.ShouldNotBeNull();
//            (result.Value as EncounterExecutionDto).Status.ShouldBe("Active");
//            result.StatusCode.ShouldBe(expectedResponseCode);
//        }

//        [Fact]
//        public void CheckPosition_success()
//        {
//            // Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//            var expectedResponseCode = 200;
//            var id = -1;
//            var longitude = 45.001;
//            var tourId = -7;
//            var latitude = 45.002;

//            // Act
//            var result = (ObjectResult)controller.CheckPosition(tourId, id, longitude, latitude).Result;

//            //Assert
//            result.ShouldNotBeNull();
//            result.StatusCode.ShouldBe(expectedResponseCode);
//        }

//        [Fact]
//        public void CheckPosition_not_in_range()
//        {
//            // Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//            var expectedResponseCode = 400;
//            var tourId = -7;
//            var encounterExecutionId = -1;
//            var longitude = 20;
//            var latitude = 45.3;

//            // Act
//            var result = (ObjectResult)controller.CheckPosition(tourId, encounterExecutionId, longitude, latitude).Result;

//            //Assert
//            result.ShouldNotBeNull();
//            result.StatusCode.ShouldBe(expectedResponseCode);
//        }

//        private static EncounterExecutionController CreateController(IServiceScope scope)
//        {
//            return new EncounterExecutionController(scope.ServiceProvider.GetRequiredService<IEncounterExecutionService>(), scope.ServiceProvider.GetRequiredService<IEncounterService>())
//            {
//                ControllerContext = BuildContext("-23")
//            };
//        }
//    }
//}

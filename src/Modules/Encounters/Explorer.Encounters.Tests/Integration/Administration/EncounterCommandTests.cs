//using Explorer.API.Controllers.Author.Administration;
//using Explorer.Encounters.API.Dtos;
//using Explorer.Encounters.API.Public;
//using Explorer.Encounters.Infrastructure.Database;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.DependencyInjection;
//using Shouldly;

//namespace Explorer.Encounters.Tests.Integration.Administration;
//[Collection("Sequential")]
//public class EncounterCommandTests : BaseEncountersIntegrationTest
//{
//    public EncounterCommandTests(EncountersTestFactory factory) : base(factory) { }
    
//    [Fact]
//    public void CreateMiscEncounterSuccessfully()
//    {
//        // Arrange
//        using var scope = Factory.Services.CreateScope();
//        var controller = CreateController(scope);
//        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//        var expectedResponseCode = 200;
//        var  checkpointId = -3;
//        bool isSecretPrerequisite = true;

//        var newEntity = new EncounterDto
//        {
//            AuthorId=-12,
//            Name = "Izazov misc",
//            Description = "Potrebno je uraditi 20 sklekova.",
//            XP = 2,
//            Type ="Misc",
//            Longitude=45,
//            Latitude=45
//        };

//        // Act
//        var result = (ObjectResult)controller.Create(newEntity,checkpointId, isSecretPrerequisite).Result;
//        var updatedObject = result?.Value as EncounterDto;


//        //Assert
//        result.ShouldNotBeNull();
//        result.StatusCode.ShouldBe(expectedResponseCode);

//        updatedObject?.AuthorId.ShouldBe(-12);
//        updatedObject?.Name.ShouldBe(newEntity.Name);
//        updatedObject?.Description.ShouldBe(newEntity.Description);
//        updatedObject?.XP.ShouldBe(2);
//        updatedObject?.Longitude.ShouldBe(45);
//        updatedObject?.Latitude.ShouldBe(45);
//    }

//    [Fact]
//    public void CreateHiddenLocationEncounterSuccessfully()
//    {
//        // Arrange
//        using var scope = Factory.Services.CreateScope();
//        var controller = CreateController(scope);
//        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//        var expectedResponseCode = 200;
//        var checkpointId = -3;
//        bool isSecretPrerequisite = true;


//        var newEntity = new EncounterDto
//        {
//            AuthorId = -12,
//            Name = "Izazov hidden location",
//            Description = "Pronadji gde je nastala slika.",
//            XP = 2,
//            Type = "Location",
//            Longitude = 45,
//            Latitude = 45,
//            LocationLongitude=45,
//            LocationLatitude=45,
//            Image="ttt",
//            Range=5

//};

//        // Act
//        var result = (ObjectResult)controller.Create(newEntity, checkpointId,isSecretPrerequisite).Result;
//        var updatedObject = result?.Value as EncounterDto;


//        //Assert
//        result.ShouldNotBeNull();
//        result.StatusCode.ShouldBe(expectedResponseCode);
//        updatedObject?.AuthorId.ShouldBe(-12);
//        updatedObject?.Name.ShouldBe(newEntity.Name);
//        updatedObject?.Description.ShouldBe(newEntity.Description);
//        updatedObject?.XP.ShouldBe(newEntity.XP);
//        updatedObject?.Longitude.ShouldBe(newEntity.Longitude);
//        updatedObject?.Latitude.ShouldBe(newEntity.Latitude);
//        updatedObject?.Image.ShouldBe(newEntity.Image);
//        updatedObject?.Range.ShouldBe(newEntity.Range);
//        updatedObject?.LocationLongitude.ShouldBe(newEntity.LocationLongitude);
//        updatedObject?.LocationLatitude.ShouldBe(newEntity.LocationLatitude);


//    }

//    [Fact]
//    public void CreateSocialEncounterSuccessfully()
//    {
//        // Arrange
//        using var scope = Factory.Services.CreateScope();
//        var controller = CreateController(scope);
//        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//        var expectedResponseCode = 200;
//        var checkpointId = -3;
//        bool isSecretPrerequisite = false;



//        var newEntity = new EncounterDto
//        {
//            AuthorId = -12,
//            Name = "Social izazov",
//            Description = "Potrebno je zajedno sa ostalim clanovima uraditi izazov.",
//            XP = 2,
//            Type = "Social",
//            Longitude = 45,
//            Latitude = 45,
//           Range =5,
//            RequiredPeople=5
//        };

//        // Act
//        var result = (ObjectResult)controller.Create(newEntity, checkpointId,isSecretPrerequisite).Result;
//        var updatedObject = result?.Value as EncounterDto;


//        //Assert
//        result.ShouldNotBeNull();
//        result.StatusCode.ShouldBe(expectedResponseCode);
//        updatedObject?.AuthorId.ShouldBe(-12);
//        updatedObject?.Name.ShouldBe(newEntity.Name);
//        updatedObject?.Description.ShouldBe(newEntity.Description);
//        updatedObject?.XP.ShouldBe(newEntity.XP);
//        updatedObject?.Longitude.ShouldBe(newEntity.Longitude);
//        updatedObject?.Latitude.ShouldBe(newEntity.Latitude);
//    }

//    [Fact]
//    public void InvalidAuthor()
//    {
//        // Arrange
//        using var scope = Factory.Services.CreateScope();
//        var controller = CreateController(scope);
//        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//        var expectedResponseCode = 403;
//        var checkpointId = -6;
//        bool isSecretPrerequisite = true;

//        var newEntity = new EncounterDto
//        {
//            AuthorId = -22,
//            Name = "Izazov misc",
//            Description = "Potrebno je uraditi 20 sklekova.",
//            XP = 2,
//            Type = "Misc",
//            Longitude = 45,
//            Latitude = 45
//        };

//        // Act
//        var result = (ObjectResult)controller.Create(newEntity, checkpointId, isSecretPrerequisite).Result;

//        //Assert
//        result.ShouldNotBeNull();
//        result.StatusCode.ShouldBe(expectedResponseCode);
//    }

//    [Fact]
//    public void InvalidCheckpoint()
//    {
//        // Arrange
//        using var scope = Factory.Services.CreateScope();
//        var controller = CreateController(scope);
//        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//        var expectedResponseCode = 404;
//        var checkpointId = -16;
//        bool isSecretPrerequisite = true;

//        var newEntity = new EncounterDto
//        {
//            AuthorId = -12,
//            Name = "Izazov misc",
//            Description = "Potrebno je uraditi 20 sklekova.",
//            XP = 2,
//            Type = "Misc",
//            Longitude = 45,
//            Latitude = 45
//        };

//        // Act
//        var result = (ObjectResult)controller.Create(newEntity, checkpointId, isSecretPrerequisite).Result;

//        //Assert

//        result.StatusCode.ShouldBe(expectedResponseCode);
//    }


//    [Fact]
//    public void UpdateMiscSuccessfully()
//    {
//        // Arrange
//        using var scope = Factory.Services.CreateScope();
//        var controller = CreateController(scope);
//        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//        var expectedResponseCode = 200;

//        var newEntity = new EncounterDto
//        {
//            Id = -2,
//            AuthorId = -12,
//            Name = "Misc Encounter",
//            Description = "Potrebno je uraditi 80 sklekova.",
//            XP = 20,
//            Type = "Misc",
//            Longitude = 45,
//            Latitude = 45,
//        };

//        // Act
//        var result = (ObjectResult)controller.Update(newEntity).Result;
//        var updatedObject = result?.Value as EncounterDto;


//        //Assert
//        result.ShouldNotBeNull();
//        result.StatusCode.ShouldBe(expectedResponseCode);
//        updatedObject.Id.ShouldBe(-2);
//        updatedObject.AuthorId.ShouldBe(-12);
//        updatedObject.Name.ShouldBe(updatedObject.Name);
//        updatedObject.Description.ShouldBe(updatedObject.Description);
//        updatedObject.XP.ShouldBe(updatedObject.XP);
//        updatedObject.Type.ShouldBe(updatedObject.Type);
//        updatedObject.Longitude.ShouldBe(updatedObject.Longitude);
//        updatedObject.Latitude.ShouldBe(updatedObject.Latitude);
//    }

//    [Fact]
//    public void UpdateSocialSuccessfully()
//    {
//        // Arrange
//        using var scope = Factory.Services.CreateScope();
//        var controller = CreateController(scope);
//        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//        var expectedResponseCode = 200;

//        var newEntity = new EncounterDto
//        {
//            Id = -3,
//            AuthorId = -12,
//            Name = "Misc Encounter",
//            Description = "Potrebno je uraditi 80 sklekova.",
//            XP = 20,
//            Type = "Misc",
//            Longitude = 45,
//            Latitude = 45,
//            RequiredPeople=111,
//            Range=200
//        };

//        // Act
//        var result = (ObjectResult)controller.Update(newEntity).Result;
//        var updatedObject = result?.Value as EncounterDto;


//        //Assert
//        result.ShouldNotBeNull();
//        result.StatusCode.ShouldBe(expectedResponseCode);
//        updatedObject?.Id.ShouldBe(-3);
//        updatedObject?.AuthorId.ShouldBe(-12);
//        updatedObject?.Name.ShouldBe(newEntity.Name);
//        updatedObject?.Description.ShouldBe(newEntity.Description);
//        updatedObject?.XP.ShouldBe(newEntity.XP);
//        updatedObject?.Longitude.ShouldBe(newEntity.Longitude);
//        updatedObject?.Latitude.ShouldBe(newEntity.Latitude);
//    }

//    [Fact]
//      public void InvalidAuthorOnUpdate()
//      {
//          // Arrange
//          using var scope = Factory.Services.CreateScope();
//          var controller = CreateController(scope);
//          var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//          var expectedResponseCode = 403;

//          var newEntity = new EncounterDto
//          {
//              Id=-1,
//              AuthorId = -16,
//              Name = "Misc Encounter",
//              Description = "Potrebno je uraditi 80 sklekova.",
//              XP = 20,
//              Type = "Misc",
//              Longitude = 45,
//              Latitude = 45
//          };

//          // Act
//          var result = (ObjectResult)controller.Update(newEntity).Result;

//          //Assert
//          result.ShouldNotBeNull();
//          result.StatusCode.ShouldBe(expectedResponseCode);

//      }

//      [Fact]
//      public void InvalidArgumentOnUpdate()
//      {
//          // Arrange
//          using var scope = Factory.Services.CreateScope();
//          var controller = CreateController(scope);
//          var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//          var expectedResponseCode = 400;

//          var newEntity = new EncounterDto
//          {
//              Id=-1,
//              AuthorId = -12,
//              Name = "",
//              Description = "Potrebno je uraditi 80 sklekova.",
//              XP = -20,
//              Type = "Misc",
//              Longitude = 45,
//              Latitude = 45
//          };

//          // Act
//          var result = (ObjectResult)controller.Update(newEntity).Result;
//          var updatedObject = result?.Value as EncounterDto;

//          //Assert
//          result.ShouldNotBeNull();
//          result.StatusCode.ShouldBe(expectedResponseCode);
//      }

//    [Fact]
//    public void Deletes()
//    {
//        // Arrange
//        using var scope = Factory.Services.CreateScope();
//        var controller = CreateController(scope);
//        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//        var checkpointId = -3;

//        // Act
//        var result = (OkResult)controller.Delete(checkpointId);

//        // Assert - Response
//        result.ShouldNotBeNull();
//        result.StatusCode.ShouldBe(200);

//    }

//    [Fact]
//    public void Delete_fails_invalid_id()
//    {
//        // Arrange
//        using var scope = Factory.Services.CreateScope();
//        var controller = CreateController(scope);
//        var checkpointId = -1000;

//        // Act
//        var result = (ObjectResult)controller.Delete(checkpointId);

//        // Assert
//        result.ShouldNotBeNull();
//        result.StatusCode.ShouldBe(404);
//    }
//    /*
//    [Fact]
//    public void FinishEncounterSuccessfully()
//    {
//        // Arrange
//        using var scope = Factory.Services.CreateScope();
//        var controller = CreateController(scope);
//        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
//        var expectedResponseCode = 200;
//        var encounterId = -1;
//        var touristId = -21;

//        // Act
//        var result = (ObjectResult)controller.FinishEncounter(encounterId, touristId).Result;

//        //Assert
//        result.ShouldNotBeNull();
//        result.StatusCode.ShouldBe(expectedResponseCode);
//    }

//    */


//    private static EncounterController CreateController(IServiceScope scope)
//    {
//        return new EncounterController(scope.ServiceProvider.GetRequiredService<IEncounterService>())
//        {
//            ControllerContext = BuildContext("-12")
//        };
//    }
//}


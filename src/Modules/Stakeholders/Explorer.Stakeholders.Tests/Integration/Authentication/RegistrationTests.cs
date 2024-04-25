using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.API.Controllers;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;

namespace Explorer.Stakeholders.Tests.Integration.Authentication;

[Collection("Sequential")]
public class RegistrationTests : BaseStakeholdersIntegrationTest
{
    public RegistrationTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Successfully_registers_tourist()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var controller = CreateController(scope);
        var account = new AccountRegistrationDto
        {
            Username = "turistaA@gmail.com",
            Email = "turistaA@gmail.com",
            Password = "turistaA",
            Name = "Žika",
            Surname = "Žikić",
            Biography = "nova bio",
            ProfilePictureUrl = "novaslikaaa.jpg",
            Motto = "neki moto moto",
            Role = "Author"
        };

        // Act
        // var authenticationResponse = ((ObjectResult)controller.RegisterTourist(account).Result).Value as AccountRegistrationDto;
        //
        // // Assert - Response
        // authenticationResponse.ShouldNotBeNull();
        // authenticationResponse.Username.ShouldBe("turistaA@gmail.com");
        // authenticationResponse.Name.ShouldBe("Žika");
        // authenticationResponse.Role.ShouldNotBe("turistaA@gmail.com");

        // Assert - Database
        dbContext.ChangeTracker.Clear();
        var storedAccount = dbContext.Users.FirstOrDefault(u => u.Username == account.Email);
        storedAccount.ShouldNotBeNull();
        var storedPerson = dbContext.People.FirstOrDefault(i => i.Email == account.Email);
        storedPerson.ShouldNotBeNull();
        storedPerson.UserId.ShouldBe(storedAccount.Id);
        storedPerson.Name.ShouldBe(account.Name);
    }

    private static AuthenticationController CreateController(IServiceScope scope)
    {
        return null;
        // return new AuthenticationController(scope.ServiceProvider.GetRequiredService<IAuthenticationService>(), scope.ServiceProvider.GetRequiredService<IVerificationService>());
    }
}

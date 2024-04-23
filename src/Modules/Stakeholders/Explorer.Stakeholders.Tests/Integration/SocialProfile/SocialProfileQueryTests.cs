//using Explorer.API.Controllers.User.SocialProfile;
//using Explorer.Stakeholders.API.Dtos;
//using Explorer.Stakeholders.API.Public;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.DependencyInjection;
//using Shouldly;

//namespace Explorer.Stakeholders.Tests.Integration.SocialProfile
//{
//    [Collection("Sequential")]
//    public class SocialProfileQueryTests : BaseStakeholdersIntegrationTest
//    {
//        public SocialProfileQueryTests(StakeholdersTestFactory factory) : base(factory) { }

//        [Fact]
//        public void Retrieves()
//        {
//            //Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            int id = -13;

//            // Act
//            var result = ((ObjectResult)controller.GetSocialProfile(id).Result)?.Value as SocialProfileDto;

//            // Assert
//            result.ShouldNotBeNull();
//            result.Id.ShouldBe(id);
//            result.Followable.Count.ShouldBe(3);
//            result.Followed.Count.ShouldBe(3);
//            result.Followers.Count.ShouldBe(1);
//        }


//        private static SocialProfileController CreateController(IServiceScope scope)
//        {
//            return new SocialProfileController(scope.ServiceProvider.GetRequiredService<ISocialProfileService>())
//            {
//                ControllerContext = BuildContext("-1")
//            };
//        }
//    }
//}

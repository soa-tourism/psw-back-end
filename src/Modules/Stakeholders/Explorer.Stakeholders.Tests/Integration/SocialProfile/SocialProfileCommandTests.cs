//using Explorer.API.Controllers.User.SocialProfile;
//using Explorer.Stakeholders.API.Dtos;
//using Explorer.Stakeholders.API.Public;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Shouldly;

//namespace Explorer.Stakeholders.Tests.Integration.SocialProfile
//{
//    [Collection("Sequential")]
//    public class SocialProfileCommandTests : BaseStakeholdersIntegrationTest
//    {
//        public SocialProfileCommandTests(StakeholdersTestFactory factory) : base(factory) { }


//        [Fact]
//        public void Follow_success()
//        {
//            //Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            int followerId = -23;
//            int followedId = -21;

//            // Act
//            var result = ((ObjectResult)controller.Follow(followerId, followedId).Result)?.Value as SocialProfileDto;

//            // Assert
//            result.ShouldNotBeNull();
//            result.Id.ShouldBe(followerId);
//            result.Followable.Count.ShouldBe(5);
//            result.Followed.Count.ShouldBe(1);
//            result.Followers.Count.ShouldBe(0);
//        }

//        [Fact]
//        public void Unfollow_success()
//        {
//            //Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            int followerId = -23;
//            int unfollowedId = -21;

//            // Act
//            var result = ((ObjectResult)controller.UnFollow(followerId, unfollowedId).Result)?.Value as SocialProfileDto;

//            // Assert
//            result.ShouldNotBeNull();
//            result.Id.ShouldBe(followerId);
//            result.Followable.Count.ShouldBe(6);
//            result.Followed.Count.ShouldBe(0);
//            result.Followers.Count.ShouldBe(0);
//        }

//        [Fact]
//        public void Follow_yourself()
//        {
//            //Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            int followerId = -22;
//            int followedId = -22;

//            // Act
//            var result = (ObjectResult)controller.Follow(followerId, followedId).Result;

//            // Assert
//            result.ShouldNotBeNull();
//            result.StatusCode.ShouldBe(400);
//        }

//        [Fact]
//        public void Unfollow_yourself()
//        {
//            //Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            int followerId = -22;
//            int followedId = -22;

//            // Act
//            var result = (ObjectResult)controller.UnFollow(followerId, followedId).Result;

//            // Assert
//            result.ShouldNotBeNull();
//            result.StatusCode.ShouldBe(400);
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

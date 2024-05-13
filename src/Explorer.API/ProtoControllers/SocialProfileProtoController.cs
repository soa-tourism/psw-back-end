using Explorer.Stakeholders.API.Dtos;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceTranscoding;
using Microsoft.AspNetCore.Authentication;

namespace Explorer.API.ProtoControllers
{
    public class SocialProfileProtoController : SocialProfile.SocialProfileBase
    {
        private readonly ILogger<SocialProfileProtoController> _logger;

        public SocialProfileProtoController(ILogger<SocialProfileProtoController> logger)
        {
            _logger = logger;
        }

        public override async Task<SocialProfileResponse> GetSocialProfile(UserId userId,
            ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8082", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new SocialProfile.SocialProfileClient(channel);
            var response = client.GetSocialProfile(userId);

            return await Task.FromResult(response);
        }
        public override async Task<SocialProfilesResponse> GetFollowers(UserId userId,
            ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8082", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new SocialProfile.SocialProfileClient(channel);
            var response = client.GetFollowers(userId);

            return await Task.FromResult(response);
        }
        public override async Task<SocialProfilesResponse> GetFollowing(UserId userId,
            ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8082", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new SocialProfile.SocialProfileClient(channel);
            var response = client.GetFollowing(userId);

            return await Task.FromResult(response);
        }
        public override async Task<SocialProfilesResponse> GetRecommended(UserId userId,
            ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8082", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new SocialProfile.SocialProfileClient(channel);
            var response = client.GetRecommended(userId);

            return await Task.FromResult(response);
        }
        public override async Task<SocialProfilesResponse> Follow(FollowRequest request,
            ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8082", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new SocialProfile.SocialProfileClient(channel);
            var response = client.Follow(request);

            return await Task.FromResult(response);
        }
        public override async Task<SocialProfilesResponse> Unfollow(UnfollowRequest request,
            ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8082", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new SocialProfile.SocialProfileClient(channel);
            var response = client.Unfollow(request);

            return await Task.FromResult(response);
        }

        public override async Task<SocialProfilesResponse> Search(Username username,
            ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8082", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new SocialProfile.SocialProfileClient(channel);
            var response = client.Search(username);

            return await Task.FromResult(response);
        }
    }
}

using FluentResults;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceTranscoding;

namespace Explorer.API.ProtoControllers
{
    public class AuthenticationProtoController : Authorize.AuthorizeBase
    {
        private readonly ILogger<AuthenticationProtoController> _logger;

        public AuthenticationProtoController(ILogger<AuthenticationProtoController> logger)
        {
            _logger = logger;
        }

        private static Authorize.AuthorizeClient GetClient()
        {
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8089", new GrpcChannelOptions { HttpHandler = httpHandler });

            return new Authorize.AuthorizeClient(channel);
        }

        private static SocialProfile.SocialProfileClient GetSocialProfileClient()
        {
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8082", new GrpcChannelOptions { HttpHandler = httpHandler });

            return new SocialProfile.SocialProfileClient(channel);
        }


        public override async Task<AuthenticationTokens> Login(Credentials request, ServerCallContext context)
        {
            var client = GetClient();
            var response = await client.LoginAsync(request);
            _logger.Log(LogLevel.Warning, message: request.ToString());

            return await Task.FromResult(response);
        }

        public override async Task<AccountRegistrationResponse> Register(AccountRegistrationRequest request, ServerCallContext context)
        {
        
            var authClient = GetClient();
            var authResponse = await authClient.RegisterAsync(request);
            _logger.Log(LogLevel.Warning, message: request.ToString());

            
            if (await Task.FromResult(authResponse) == null)
            {
                return await Task.FromResult(authResponse);
            }

            var socialProfileClient = GetSocialProfileClient();
            var socialProfileRequest = new SocialProfileRequest
            {
                UserId = authResponse.Id,
                Username = request.Username,
            };
            socialProfileClient.CreateSocialProfile(socialProfileRequest);

            return await Task.FromResult(authResponse);

        }
    }
}

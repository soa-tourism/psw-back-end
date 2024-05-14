using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceTranscoding;
using Microsoft.AspNetCore.Authentication;

namespace Explorer.API.ProtoControllers
{
    public class TourProtoController : Tour.TourBase
    {
        private readonly ILogger<TourProtoController> _logger;

        public TourProtoController(ILogger<TourProtoController> logger)
        {
            _logger = logger;
        }

        public override async Task<EquipmentResponse> GetEquipment(EquipmentId id,
            ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8083/v1/tours/equipment", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new Tour.TourClient(channel);
            var response = client.GetEquipment(id);

            return await Task.FromResult(response);
        }
    }
}

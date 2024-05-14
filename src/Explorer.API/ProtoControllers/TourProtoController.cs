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

        // Equipment
        public override async Task<EquipmentsResponse> GetAvailableEquipment(EquipmentIds ids,
            ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8083/v1/tours", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new Tour.TourClient(channel);
            var response = client.GetAvailableEquipment(ids);

            return await Task.FromResult(response);
        }
        public override async Task<PagedEquipmentsResponse> GetAllEquipment(Page page,
            ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8083/v1/tours/equipment", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new Tour.TourClient(channel);
            var response = client.GetAllEquipment(page);

            return await Task.FromResult(response);
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
        public override async Task<EquipmentResponse> CreateEquipment(EquipmentResponse id,
            ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8083/v1/tours/equipment", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new Tour.TourClient(channel);
            var response = client.CreateEquipment(id);

            return await Task.FromResult(response);
        }
        public override async Task<EquipmentResponse> UpdateEquipment(UpdateEquipmentId id,
            ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8083/v1/tours/equipment", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new Tour.TourClient(channel);
            var response = client.UpdateEquipment(id);

            return await Task.FromResult(response);
        }
        public override async Task<EquipmentResponse> DeleteEquipment(EquipmentId id,
            ServerCallContext context)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8083/v1/tours/equipment", new GrpcChannelOptions { HttpHandler = httpHandler });

            var client = new Tour.TourClient(channel);
            var response = client.DeleteEquipment(id);

            return await Task.FromResult(response);
        }
    }
}

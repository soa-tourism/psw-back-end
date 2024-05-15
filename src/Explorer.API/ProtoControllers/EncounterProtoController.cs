using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceTranscoding;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.ProtoControllers
{
    public class EncounterProtoController : Encounter.EncounterBase
    {
        private readonly ILogger<EncounterProtoController> _logger;

        public EncounterProtoController(ILogger<EncounterProtoController> logger)
        {
            _logger = logger;
        }
        private Encounter.EncounterClient getClient()
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("http://host.docker.internal:8087", new GrpcChannelOptions { HttpHandler = httpHandler });

            return new Encounter.EncounterClient(channel);
        }

        public override async Task<EncounterDto> Create(CreateRequest request,
            ServerCallContext context)
        {
            var client = getClient();
            _logger.Log(LogLevel.Warning, request.ToString());
            var response = client.Create(request);

            return await Task.FromResult(response);
        }
        public override async Task<EncounterDto> Update(UpdateRequest request,
            ServerCallContext context)
        {
            var client = getClient();
            _logger.Log(LogLevel.Warning, request.ToString());
            var response = client.Update(request);

            return await Task.FromResult(response);
        }
        public override async Task<Empty> Delete(EncounterId id,
            ServerCallContext context)
        {
            var client = getClient();
            _logger.Log(LogLevel.Warning, "Deleting encounter with id:" + id.Id.ToString());
            var response = client.Delete(id);

            return await Task.FromResult(response);
        }
        public override async Task<EncounterDto> GetById(EncounterId id,
            ServerCallContext context)
        {
            var client = getClient();
            _logger.Log(LogLevel.Warning, "Getting encounter with id:" + id.Id.ToString());
            var response = client.GetById(id);

            return await Task.FromResult(response);
        }
        public override async Task<EncounterExecutionDto> CreateEncounterExecution(EncounterExecutionDto executionDto,
            ServerCallContext context)
        {
            var client = getClient();
            _logger.Log(LogLevel.Warning, "Creating execution with startTime:" + executionDto.StartTime.ToString());
            var response = client.CreateEncounterExecution(executionDto);

            return await Task.FromResult(response);
        }
        public override async Task<EncounterDto> GetEncounterById(EncounterId id,
            ServerCallContext context)
        {
            var client = getClient();
            _logger.Log(LogLevel.Warning, "Getting encounter with id:" + id.Id.ToString());
            var response = client.GetEncounterById(id);

            return await Task.FromResult(response);
        }
        public override async Task<EncounterExecutionDto> UpdateEncounterExecution(EncounterExecutionDto executionDto,
            ServerCallContext context)
        {
            var client = getClient();
            _logger.Log(LogLevel.Warning, "Updating encounter execution with startTime:" + executionDto.StartTime.ToString());
            var response = client.UpdateEncounterExecution(executionDto);

            return await Task.FromResult(response);
        }
        public override async Task<EncounterExecutionDto> ActivateEncounterExecution(ActivateRequest request,
            ServerCallContext context)
        {
            var client = getClient();
            _logger.Log(LogLevel.Warning, "Activating encounter execution with touristLatitude:" + request.TouristLatitude.ToString());
            var response = client.ActivateEncounterExecution(request);

            return await Task.FromResult(response);
        }
        public override async Task<EncounterExecutionDto> CompleteExecution(ActivateRequest request,
            ServerCallContext context)
        {
            var client = getClient();
            _logger.Log(LogLevel.Warning, "Completing encounter execution with touristLatitude:" + request.TouristLatitude.ToString());
            var response = client.CompleteExecution(request);

            return await Task.FromResult(response);
        }
        public override async Task<Empty> DeleteExecution(EncounterId id,
            ServerCallContext context)
        {
            var client = getClient();
            _logger.Log(LogLevel.Warning, "Deleting encounter execution with id:" + id.Id.ToString());
            var response = client.DeleteExecution(id);

            return await Task.FromResult(response);
        }
        public override async Task<PagedExecutions> GetAllExecutionsByTourist(EncounterId id,
            ServerCallContext context)
        {
            var client = getClient();
            _logger.Log(LogLevel.Warning, "Getting all encounter executions bz the tourist with id:" + id.Id.ToString());
            var response = client.GetAllExecutionsByTourist(id);

            return await Task.FromResult(response);
        }
        public override async Task<PagedExecutions> GetAllCompletedExecutionsByTourist(PagedRequestWithId id,
            ServerCallContext context)
        {
            var client = getClient();
            _logger.Log(LogLevel.Warning, "Getting all completed encounter executions bz the tourist with id:" + id.Id.ToString());
            var response = client.GetAllCompletedExecutionsByTourist(id);

            return await Task.FromResult(response);
        }
    }
}

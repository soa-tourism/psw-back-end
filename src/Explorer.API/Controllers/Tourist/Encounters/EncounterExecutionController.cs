using Explorer.API.Services;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain.Encounters;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using System.Text;
using Explorer.Stakeholders.Core.Domain;
using Azure;

namespace Explorer.API.Controllers.Tourist.Encounters
{
    [Route("api/tourist/encounter-execution")]
    // [Authorize(Policy = "touristPolicy")]
    public class EncounterExecutionController : BaseApiController
    {
        static readonly HttpClient client = new HttpClient();
        private string baseUrl = $"http://localhost:8090/execution";


        public EncounterExecutionController() { }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EncounterDto>> GetById([FromRoute] int id)
        {
            using HttpResponseMessage response = await client.GetAsync(baseUrl + "/getAll")
        }

        [HttpPut]
        public ActionResult<EncounterExecutionDto> Update([FromForm] EncounterExecutionDto encounterExecution)
        {
            var result = _encounterExecutionService.Update(encounterExecution, User.PersonId());
            return CreateResponse(result);
        }

        [HttpPut("activate/{id:int}")]
        public ActionResult<EncounterExecutionDto> Activate([FromRoute] int id, [FromForm] double touristLatitude, [FromForm] double touristLongitude)
        {
            var result = _encounterExecutionService.Activate(User.PersonId(), touristLatitude, touristLongitude, id);
            return CreateResponse(result);
        }

        [HttpPut("completed/{id:int}")]
        public ActionResult<EncounterExecutionDto> CompleteExecution([FromRoute] int id, [FromForm] double touristLatitude, [FromForm] double touristLongitude)
        {
            var result = _encounterExecutionService.CompleteExecution(id, User.PersonId(), touristLatitude, touristLongitude);
            if (result.IsSuccess)
                result = _encounterService.AddEncounter(result.Value);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _encounterExecutionService.Delete(id, User.PersonId());
            return CreateResponse(result);
        }

        [HttpGet("get-all/{id:int}")]
        public ActionResult<PagedResult<EncounterExecutionDto>> GetAllByTourist(int id, [FromQuery] int page, [FromQuery] int pageSize)
        {
            if (id != User.PersonId())
            {
                return Unauthorized();
            }
            var result = _encounterExecutionService.GetAllByTourist(id, page, pageSize);
            return CreateResponse(result);
        }

        [HttpGet("get-all-completed")]
        public async Task<ActionResult<PagedResult<EncounterExecutionDto>>> GetAllCompletedByTourist([FromQuery] int page, [FromQuery] int pageSize)
        {
            HttpResponseMessage response = await client.GetAsync($"{baseUrl}/getAllCompletedByTourist/{User.PersonId()}");
            Response.ContentType = "application/json";
            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();

                // Deserialize JSON string to EncounterDto object
                List<EncounterExecutionDto> responseEncounter = JsonSerializer.Deserialize<List<EncounterExecutionDto>>(responseJson);

                var result = new Result<List<EncounterExecutionDto>>().WithValue(responseEncounter);
                // Use the responseEncounter object as needed
                return CreateResponse(result);
            }
            return CreateResponse(new Result<EncounterDto>().WithError("Error"));
        }
        
        [HttpGet("get-by-tour/{id:int}")]
        public async Task<ActionResult<EncounterExecutionDto>> GetByTour([FromRoute] int id, [FromQuery] double touristLatitude, [FromQuery] double touristLongitude)
        {
            List<int> encounterIds = new();  //get encounters on tour by ID: id
            var json = JsonSerializer.Serialize(encounterIds);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"{baseUrl}/getByTour/{touristLatitude}/{touristLongitude}/{User.PersonId()}", content);
            Response.ContentType = "application/json";
            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();

                // Deserialize JSON string to EncounterDto object
                EncounterExecutionDto responseEncounter = JsonSerializer.Deserialize<EncounterExecutionDto>(responseJson);

                var result = new Result<EncounterExecutionDto>().WithValue(responseEncounter);
                // Use the responseEncounter object as needed
                return CreateResponse(result);
            }
            return CreateResponse(new Result<EncounterDto>().WithError("Error"));
        }

        [HttpGet("social/checkRange/{id:int}/{tourId:int}")]
        public async Task<ActionResult<EncounterExecutionDto>> CheckPosition([FromRoute] int tourId, [FromRoute] int id, [FromQuery] double touristLatitude, [FromQuery] double touristLongitude)
        {
            List<int> encounterIds = new();  //get encounters on tour by ID: id
            var json = JsonSerializer.Serialize(encounterIds);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"{baseUrl}/checkPosition/{id}/{touristLatitude}/{touristLongitude}/{User.PersonId()}", content);
            Response.ContentType = "application/json";
            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();

                // Deserialize JSON string to EncounterDto object
                EncounterExecutionDto responseEncounter = JsonSerializer.Deserialize<EncounterExecutionDto>(responseJson);

                var result = new Result<EncounterExecutionDto>().WithValue(responseEncounter);
                // Use the responseEncounter object as needed
                return CreateResponse(result);
            }
            return CreateResponse(new Result<EncounterDto>().WithError("Error"));
        }

        [HttpGet("location/checkRange/{id:int}/{tourId:int}")]
        public async Task<ActionResult<EncounterExecutionDto>> CheckPositionLocationEncounter([FromRoute] int tourId, [FromRoute] int id, [FromQuery] double touristLatitude, [FromQuery] double touristLongitude)
        {
            List<int> encounterIds = new();  //get encounters on tour by ID: id
            var json = JsonSerializer.Serialize(encounterIds);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"{baseUrl}/checkPositionLocationEncounter/{id}/{touristLatitude}/{touristLongitude}/{User.PersonId()}", content);
            Response.ContentType = "application/json";
            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();

                // Deserialize JSON string to EncounterDto object
                EncounterExecutionDto responseEncounter = JsonSerializer.Deserialize<EncounterExecutionDto>(responseJson);

                var result = new Result<EncounterExecutionDto>().WithValue(responseEncounter);
                // Use the responseEncounter object as needed
                return CreateResponse(result);
            }
            return CreateResponse(new Result<EncounterDto>().WithError("Error"));
        }

        [HttpGet("active/by-tour/{id:int}")]
        public async Task<ActionResult<List<EncounterExecutionDto>>> GetActiveByTour([FromRoute] int id)
        {
            List<int> encounterIds = new();  //get encounters on tour by ID: id
            var json = JsonSerializer.Serialize(encounterIds);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"{baseUrl}/getActiveByTour/{User.PersonId()}", content);
            Response.ContentType = "application/json";
            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();

                // Deserialize JSON string to EncounterDto object
                List<EncounterExecutionDto> responseEncounter = JsonSerializer.Deserialize<List<EncounterExecutionDto>>(responseJson);

                var result = new Result<List<EncounterExecutionDto>>().WithValue(responseEncounter);
                // Use the responseEncounter object as needed
                return CreateResponse(result);
            }
            return CreateResponse(new Result<EncounterDto>().WithError("Error"));

        }
    }
}

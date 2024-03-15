using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain.Encounters;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using Explorer.Stakeholders.Core.Domain;

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
            using HttpResponseMessage response = await client.GetAsync(baseUrl + $"/get/{id}");

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return CreateResponse(jsonResponse.ToResult());
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        [HttpPut]
        public async Task<ActionResult<EncounterExecutionDto>> Update([FromForm] EncounterExecutionDto encounterExecution)
        {
            var json = JsonSerializer.Serialize(encounterExecution);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"{baseUrl}/update", content);
            Response.ContentType = "application/json";
            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();

                EncounterExecutionDto responseExecution = JsonSerializer.Deserialize<EncounterExecutionDto>(responseJson);

                var result = new Result<EncounterExecutionDto>().WithValue(responseExecution);
                return CreateResponse(result);
            }


            return CreateResponse(new Result<EncounterExecutionDto>().WithError("Error"));
        }

        [HttpPut("activate/{id:int}")]
        public async Task<ActionResult<EncounterExecutionDto>> Activate([FromRoute] int id, [FromForm] double touristLatitude, [FromForm] double touristLongitude)
        {
            HttpResponseMessage response = await client.PutAsync($"{baseUrl}/activate/{id}/{touristLatitude}/{touristLongitude}", null);
            Response.ContentType = "application/json";
            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();

                EncounterExecutionDto responseExecution = JsonSerializer.Deserialize<EncounterExecutionDto>(responseJson);

                var result = new Result<EncounterExecutionDto>().WithValue(responseExecution);
                return CreateResponse(result);
            }


            return CreateResponse(new Result<EncounterExecutionDto>().WithError("Error"));
        }

        [HttpPut("completed/{id:int}")]
        public async Task<ActionResult<EncounterExecutionDto>> CompleteExecution([FromRoute] int id, [FromForm] double touristLatitude, [FromForm] double touristLongitude)
        {
            HttpResponseMessage response = await client.PutAsync($"{baseUrl}/completed/{id}/{touristLatitude}/{touristLongitude}", null);
            Response.ContentType = "application/json";
            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();

                EncounterExecutionDto responseExecution = JsonSerializer.Deserialize<EncounterExecutionDto>(responseJson);

                var result = new Result<EncounterExecutionDto>().WithValue(responseExecution);
                return CreateResponse(result);
            }


            return CreateResponse(new Result<EncounterExecutionDto>().WithError("Error"));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"{baseUrl}/delete/{id}");
            return StatusCode((int)response.StatusCode);
        }

        [HttpGet("getAllByTourist/{id:int}")]
        public async Task<ActionResult<PagedResult<EncounterExecutionDto>>> GetAllByTourist(int id)
        {
            using HttpResponseMessage response = await client.GetAsync(baseUrl + $"/getAllByTourist/{id}");

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return CreateResponse(jsonResponse.ToResult());
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        [HttpGet("get-all-completed")]
        public ActionResult<PagedResult<EncounterExecutionDto>> GetAllCompletedByTourist([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _encounterExecutionService.GetAllCompletedByTourist(User.PersonId(), page, pageSize);
            return CreateResponse(result);
        }
        
        [HttpGet("get-by-tour/{id:int}")]
        public ActionResult<EncounterExecutionDto> GetByTour([FromRoute] int id, [FromQuery] double touristLatitude, [FromQuery] double touristLongitude)
        {
            var result = _encounterExecutionService.GetVisibleByTour(id, touristLongitude, touristLatitude, User.PersonId());
            if(result.IsSuccess)
                result = _encounterService.AddEncounter(result.Value);
            return CreateResponse(result);
        }

        [HttpGet("social/checkRange/{id:int}/{tourId:int}")]
        public ActionResult<EncounterExecutionDto> CheckPosition([FromRoute] int tourId, [FromRoute] int id, [FromQuery] double touristLatitude, [FromQuery] double touristLongitude)
        {
            var result = _encounterExecutionService.GetWithUpdatedLocation(tourId, id, touristLongitude, touristLatitude, User.PersonId());
            if (result.IsSuccess)
                result = _encounterService.AddEncounter(result.Value);
            return CreateResponse(result);
        }

        [HttpGet("location/checkRange/{id:int}/{tourId:int}")]
        public ActionResult<EncounterExecutionDto> CheckPositionLocationEncounter([FromRoute] int tourId, [FromRoute] int id, [FromQuery] double touristLatitude, [FromQuery] double touristLongitude)
        {
            var result = _encounterExecutionService.GetHiddenLocationEncounterWithUpdatedLocation(tourId, id, touristLongitude, touristLatitude, User.PersonId());
            if (result.IsSuccess)
                result = _encounterService.AddEncounter(result.Value);
            return CreateResponse(result);
        }

        [HttpGet("active/by-tour/{id:int}")]
        public ActionResult<List<EncounterExecutionDto>> GetActiveByTour([FromRoute] int id)
        {
            var result = _encounterExecutionService.GetActiveByTour(User.PersonId(), id);
            if (result.IsSuccess)
                result = _encounterService.AddEncounters(result.Value);
            return CreateResponse(result);
        }
    }
}

using Explorer.API.Services;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain.Encounters;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Explorer.API.Controllers.Author.Administration
{
    [Route("api/administration/encounter")]
    [Authorize(Policy = "authorPolicy")]


    public class EncounterController : BaseApiController
    {
        private readonly ImageService _imageService;

        private static readonly HttpClient client = new HttpClient();
        private string baseUrl = $"http://localhost:8090/encounter";

        public EncounterController(IEncounterService encounterService)
        {
            _imageService = new ImageService();

        }


        [HttpPost]
        public async Task<ActionResult<EncounterDto>> Create([FromForm] EncounterDto encounter,[FromQuery] long checkpointId, [FromQuery] bool isSecretPrerequisite, [FromForm] List<IFormFile>? imageF = null)
        {
            
            if (imageF != null && imageF.Any())
            {
                var imageNames = _imageService.UploadImages(imageF);
                if (encounter.Type =="Location")
                    encounter.Image = imageNames[0];
            }
            var json = JsonSerializer.Serialize(encounter);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{baseUrl}/create", content);
            Response.ContentType = "application/json";
            if (response.IsSuccessStatusCode)
            {
                // Read response content as JSON string
                string responseJson = await response.Content.ReadAsStringAsync();

                // Deserialize JSON string to EncounterDto object
                EncounterDto responseEncounter = JsonSerializer.Deserialize<EncounterDto>(responseJson);

                var result = new Result<EncounterDto>().WithValue(responseEncounter);
                // Use the responseEncounter object as needed
                return CreateResponse(result);
            }
            

            return CreateResponse(new Result<EncounterDto>().WithError("Error"));
        }

        [HttpPut]
        public async Task<ActionResult<EncounterDto>> Update([FromForm] EncounterDto encounter, [FromForm] List<IFormFile>? imageF = null)
        {

            if (imageF != null && imageF.Any())
            {
                var imageNames = _imageService.UploadImages(imageF);
                if (encounter.Type == "Location")
                    encounter.Image = imageNames[0];
            }
            var json = JsonSerializer.Serialize(encounter);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"{baseUrl}/update", content);
            Response.ContentType = "application/json";
            if (response.IsSuccessStatusCode)
            {
                // Read response content as JSON string
                string responseJson = await response.Content.ReadAsStringAsync();

                // Deserialize JSON string to EncounterDto object
                EncounterDto responseEncounter = JsonSerializer.Deserialize<EncounterDto>(responseJson);

                var result = new Result<EncounterDto>().WithValue(responseEncounter);
                // Use the responseEncounter object as needed
                return CreateResponse(result);
            }


            return CreateResponse(new Result<EncounterDto>().WithError("Error"));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"{baseUrl}/delete/{id}");
            return StatusCode((int)response.StatusCode);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EncounterDto>> GetById(int id)
        {

            HttpResponseMessage response = await client.GetAsync($"{baseUrl}/get/{id}");
            Response.ContentType = "application/json";
            if (response.IsSuccessStatusCode)
            {
                // Read response content as JSON string
                string responseJson = await response.Content.ReadAsStringAsync();

                // Deserialize JSON string to EncounterDto object
                EncounterDto responseEncounter = JsonSerializer.Deserialize<EncounterDto>(responseJson);

                var result = new Result<EncounterDto>().WithValue(responseEncounter);
                // Use the responseEncounter object as needed
                return CreateResponse(result);
            }


            return CreateResponse(new Result<EncounterDto>().WithError("Error"));
        }

    }
}

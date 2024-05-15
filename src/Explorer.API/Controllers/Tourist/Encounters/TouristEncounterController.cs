using Explorer.API.Services;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using System.Text;
using Explorer.Stakeholders.API.Internal;

namespace Explorer.API.Controllers.Tourist.Encounters
{
    [Route("api/administration/touristEncounter")]
    public class TouristEncounterController : BaseApiController
    {
        private readonly ImageService _imageService;
        private readonly IInternalTouristService _personService;

        private static readonly HttpClient client = new HttpClient();
        private string baseUrl = $"http://host.docker.internal:8090/touristEncounter";


        public TouristEncounterController(IInternalTouristService s)
        {
            _imageService = new ImageService();
            _personService = s;
        }

        //[HttpPost]
        //[Authorize(Policy = "touristPolicy")]
        //public async Task<ActionResult<EncounterDto>> Create([FromForm] EncounterDto encounter, [FromQuery] long checkpointId, [FromQuery] bool isSecretPrerequisite, [FromForm] List<IFormFile>? imageF = null)
        //{
        //    if(_personService.Get(User.PersonId()).Value.Level < 10)
        //        return CreateResponse(new Result<EncounterDto>().WithError("Error"));
        //    // Transformacija koordinata za longitude
        //    encounter.Longitude = TransformisiKoordinatu(encounter.Longitude);

        //    // Transformacija koordinata za latitude
        //    encounter.Latitude = TransformisiKoordinatu(encounter.Latitude);

        //    if (imageF != null && imageF.Any())
        //    {
        //        var imageNames = _imageService.UploadImages(imageF);
        //        if (encounter.Type == "Location")
        //            encounter.Image = imageNames[0];
        //    }
        //    var json = JsonSerializer.Serialize(encounter);

        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = await client.PostAsync($"{baseUrl}/create/{checkpointId}/{isSecretPrerequisite}", content);
        //    Response.ContentType = "application/json";
        //    if (response.IsSuccessStatusCode)
        //    {
        //        // Read response content as JSON string
        //        string responseJson = await response.Content.ReadAsStringAsync();

        //        // Deserialize JSON string to EncounterDto object
        //        EncounterDto responseEncounter = JsonSerializer.Deserialize<EncounterDto>(responseJson);

        //        var result = new Result<EncounterDto>().WithValue(responseEncounter);
        //        // Use the responseEncounter object as needed
        //        return CreateResponse(result);
        //    }
        //    return CreateResponse(new Result<EncounterDto>().WithError("Error"));
        //}

        [HttpPut]
        [Authorize(Policy = "touristPolicy")]
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
        [Authorize(Policy = "touristPolicy")]
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"{baseUrl}/delete/{id}");
            return StatusCode((int)response.StatusCode);
        }

        [HttpGet]
        [Authorize(Policy = "administratorPolicy")]
        public async Task<ActionResult<PagedResult<EncounterRequestDto>>> GetAll()
        {
            using HttpResponseMessage response = await client.GetAsync(baseUrl + "/getAll");

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

        [HttpGet("{id:int}")]
        [Authorize(Policy = "touristPolicy")]
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

        // Funkcija za transformaciju koordinata
        private double TransformisiKoordinatu(double koordinata)
        {
            // Pretvori broj u string kako bi se mogao indeksirati
            string koordinataString = koordinata.ToString();

            // Ako je koordinata dovoljno dugačka
            if (koordinataString.Length > 2)
            {
                // Uzmi prva dva znaka
                string prviDeo = koordinataString.Substring(0, 2);

                // Uzmi ostatak broja posle prva dva znaka
                string drugiDeo = koordinataString.Substring(2);

                // Sastavi transformisanu vrednost
                string transformisanaKoordinataString = prviDeo + '.' + drugiDeo;

                // Parsiraj rezultat nazad kao double
                if (double.TryParse(transformisanaKoordinataString, NumberStyles.Any, CultureInfo.InvariantCulture, out double transformisanaKoordinata))
                {
                    return transformisanaKoordinata;
                }
            }

            // Ako je koordinata prekratka ili neuspešno parsiranje, vrati nepromenjenu vrednost
            return koordinata;
        }
    }
}

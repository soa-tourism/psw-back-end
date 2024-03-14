﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Explorer.API.Controllers.Administrator.Administration
{
    [Authorize(Policy = "touristAndAdministratorPolicy")]
    [Route("api/administration/encounterRequests")]
    public class EncounterRequestController : BaseApiController
    {

        static readonly HttpClient client = new HttpClient();
        private string baseUrl = $"http://localhost:8090/encounterRequests";

        public EncounterRequestController() { }


        [HttpGet]
        public async Task<ActionResult<PagedResult<EncounterRequestDto>>> GetAll(){
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


        [HttpPut("accept/{id:int}")]
        public async Task<ActionResult<EncounterRequestDto>> AcceptRequest(int id)
        {
            HttpResponseMessage response = await client.PutAsync($"{baseUrl}/accept/{id}", null);

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


        [HttpPut("reject/{id:int}")]
        public async Task<ActionResult<EncounterRequestDto>> RejectRequest(int id)
        {
            HttpResponseMessage response = await client.PutAsync($"{baseUrl}/reject/{id}", null);

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
    }
}

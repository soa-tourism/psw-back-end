using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Explorer.API.Controllers.Tourist.Tour
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/map-object")]
    public class TouristMapObjectController : BaseApiController
    {
        private readonly IMapObjectService _mapObjectService;
        public TouristMapObjectController(IMapObjectService service) 
        {
            _mapObjectService = service;
        }

        [HttpGet]
        public ActionResult<PagedResult<MapObjectDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _mapObjectService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }
    }
}

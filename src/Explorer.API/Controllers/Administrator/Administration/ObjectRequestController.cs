using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.Administration
{
    [Authorize(Policy = "administratorAndAuthorPolicy")]
    [Route("api/administration/objectRequests")]
    public class ObjectRequestController : BaseApiController
    {

        private readonly IObjectRequestService _objectRequestService;

        public ObjectRequestController(IObjectRequestService objectRequestService)
        {
            _objectRequestService = objectRequestService;
        }

        [HttpPost]
        public ActionResult<ObjectRequestDto> Create([FromBody] ObjectRequestDto request)
        {
            var result = _objectRequestService.Create(request);
            return CreateResponse(result);
        }

        [HttpGet]
        public ActionResult<List<ObjectRequestDto>> GetAll()
        {
            var result = _objectRequestService.GetAll();
            return CreateResponse(result);
        }

        [HttpPut("accept/{id:int}/{notificationComment}")]
        public ActionResult<ObjectRequestDto> AcceptRequest(int id, string notificationComment)
        {
            var result = _objectRequestService.AcceptRequest(id, notificationComment);
            return CreateResponse(result);
        }

        [HttpPut("reject/{id:int}/{notificationComment}")]
        public ActionResult<ObjectRequestDto> RejectRequest(int id, string notificationComment)
        {
            var result = _objectRequestService.RejectRequest(id, notificationComment);
            return CreateResponse(result);
        }
    }
}

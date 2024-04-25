using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Tour
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tour/applicationGrade")]
    public class ApplicationGradeController : BaseApiController
    {
        private readonly IApplicationGradeService _applicationGradeService;

        public ApplicationGradeController(IApplicationGradeService applicationGradeService)
        {
            _applicationGradeService = applicationGradeService;
        }

        [HttpPost]
        public ActionResult<ApplicationGradeDto> EvaluateApplication([FromBody] ApplicationGradeDto applicationGrade)
        {
            var result = _applicationGradeService.EvaluateApplication(applicationGrade);
            return CreateResponse(result);
        }
    }
}

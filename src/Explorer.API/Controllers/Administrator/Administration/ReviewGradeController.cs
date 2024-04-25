using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.Administration
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administration/applicationGrade")]
    public class ReviewGradeController : BaseApiController
    {
        private readonly IApplicationGradeService _applicationGradeService;

        public ReviewGradeController(IApplicationGradeService applicationGradeService)
        {
            _applicationGradeService = applicationGradeService;
        }

        [HttpGet]
        public ActionResult<List<ApplicationGradeDto>> ReviewGrades(int page, int pageSize)
        {
            var result = _applicationGradeService.ReviewGrades(page, pageSize);
            return CreateResponse(result);
        }

    }
}

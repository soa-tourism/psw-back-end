using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.Administration
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administration/accountsManagement")]
    public class AccountsManagementController : BaseApiController
    {
        private readonly IAccountsManagementService _accountsManagementService;

        public AccountsManagementController(IAccountsManagementService accountsManagementService)
        {
            _accountsManagementService = accountsManagementService;
        }

        [HttpGet]
        public ActionResult<PagedResult<UserDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _accountsManagementService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPut("block/{id:int}")]
        public ActionResult<UserDto> Block(int id)
        {
            var result = _accountsManagementService.Block(id);
            return CreateResponse(result);
        }

    }
}

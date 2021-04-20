using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetBaseData.API.Entities;
using PetBaseData.API.Models;
using PetBaseData.API.Filters;
using PetBaseData.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace PetBaseData.API.Controllers
{

    [ApiKeyAuth]
    [ApiController]
    [Route("v1/[controller]")]
    public class AccountController: ControllerBase
    {
        private readonly IUserManagementRepository _repository;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IUserManagementRepository repository, ILogger<AccountController> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        [Route("password-change", Name = "ChangePassword")]
        [HttpPut]
        [ProducesResponseType(typeof(AccountResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AccountResponse>> ChangePassword([FromBody] AccountRequest accountRequest)
        {
            return Ok(await _repository.ChangePassword(accountRequest));
        }

        [Route("game-reset", Name = "ResetGame")]
        [HttpPut]
        [ProducesResponseType(typeof(AccountResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AccountResponse>> ResetGame([FromBody] AccountRequest accountRequest)
        {
            return Ok(await _repository.ResetGame(accountRequest));
        }

        [Route("username-change", Name = "ChangeUsername")]
        [HttpPut]
        [ProducesResponseType(typeof(AccountResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AccountResponse>> ChangeUsername([FromBody] AccountRequest accountRequest)
        {
            return Ok(await _repository.ChangeUsername(accountRequest));
        }

        [Route("email-change", Name = "ChangeEmail")]
        [HttpPut]
        [ProducesResponseType(typeof(AccountResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AccountResponse>> ChangeEmail([FromBody] AccountRequest accountRequest)
        {
            return Ok(await _repository.ChangeEmailAdress(accountRequest));
        }

        [Route("delete-account", Name = "DeleteAccount")]
        [HttpDelete]
        [ProducesResponseType(typeof(AccountResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AccountResponse>> DeleteAccount([FromBody] AccountRequest accountRequest)
        {
            return Ok(await _repository.DeleteUserData(accountRequest));
        }

        [Route("password-reset", Name = "ResetPassword")]
        [HttpPut]
        [ProducesResponseType(typeof(AccountResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AccountResponse>> ResetPassword([FromBody] AccountRequest accountRequest)
        {
            return Ok(await _repository.ForgotPassword(accountRequest));
        }

        [Route("game-save", Name = "SaveGame")]
        [HttpPut]
        [ProducesResponseType(typeof(AccountResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AccountResponse>> SaveGame([FromBody] AccountRequest accountRequest)
        {
            return Ok(await _repository.UpdateSavedData(accountRequest));
        }

    }
}

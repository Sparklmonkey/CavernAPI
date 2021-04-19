using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetBaseData.API.Entities;
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
    public class UserController: ControllerBase
    {
        private readonly IUserLoginRepository _repository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserLoginRepository repository, ILogger<UserController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [Route("register", Name = "Register")]
        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<LoginResponse>> RegisterUser([FromBody] LoginRequest loginRequest)
        {
            return Ok(await _repository.RegisterUser(loginRequest.Username, loginRequest.Password));
        }

        [Route("login", Name = "Login")]
        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<LoginResponse>> LoginUser([FromBody] LoginRequest loginRequest)
        {
            return Ok(await _repository.LoginUser(loginRequest.Username, loginRequest.Password));
        }

        [Route("save-data", Name = "Update")]
        [HttpPut]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<LoginResponse>> UpdateSavedData([FromBody] UpdateRequest updateRequest)
        {
            return Ok(await _repository.UpdateSavedData(updateRequest.PlayerId, updateRequest.SavedData));
        }

        [Route("delete-user", Name = "Delete")]
        [HttpDelete]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<LoginResponse>> DeleteUser([FromBody] DeleteRequest deleteRequest)
        {
            return Ok(await _repository.DeleteUserData(deleteRequest.Username, deleteRequest.Password, deleteRequest.PlayerId));
        }
    }
}

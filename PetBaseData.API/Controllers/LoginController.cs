using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            return Ok(await _repository.RegisterUser(loginRequest));
        }

        [Route("login", Name = "Login")]
        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<LoginResponse>> LoginUser([FromBody] LoginRequest loginRequest)
        {
            return Ok(await _repository.LoginUser(loginRequest));
        }

    }
}

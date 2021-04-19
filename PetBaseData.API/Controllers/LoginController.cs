using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetBaseData.API.Entities;
using PetBaseData.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PetBaseData.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LoginController: ControllerBase
    {
        private readonly IUserLoginRepository _repository;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IUserLoginRepository repository, ILogger<LoginController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [Route("register", Name = "Register")]
        [HttpPost]
        [ProducesResponseType(typeof(SavedData), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<SavedData>> RegisterUser(string username, string password)
        {
            return Ok(await _repository.RegisterUser(username, password));
        }

        [Route("login", Name = "Login")]
        [HttpPost]
        [ProducesResponseType(typeof(SavedData), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<SavedData>> LoginUser(string username, string password)
        {
            return Ok(await _repository.LoginUser(username, password));
        }
    }
}

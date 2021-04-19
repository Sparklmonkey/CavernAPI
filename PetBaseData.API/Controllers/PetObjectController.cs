using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetBaseData.API.Entities;
using PetBaseData.API.Repositories;
using Realms.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PetBaseData.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PetObjectController : ControllerBase
    {
        private readonly IPetObjectRepository _repository;
        private readonly ILogger<PetObjectController> _logger;

        public PetObjectController(IPetObjectRepository repository, ILogger<PetObjectController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PetObject>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PetObject>>> GetPetObjects()
        {
            var petObjects = await _repository.GetPetObjects();
            return Ok(petObjects);
        }

        [HttpGet("{id:length(24)}", Name = "GetPet")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PetObject), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PetObject>> GetPetObjectById(string id)
        {
            var petObject = await _repository.GetPetObjectById(id);
            if (petObject == null)
            {
                _logger.LogError($"Pet with id: {id} not found");
                return NotFound();
            }
            return Ok(petObject);
        }

        [Route("[action]/{petGrade}", Name = "GetPetByGrade")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PetObject>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PetObject>>> GetPetsByGrade(string petGrade)
        {
            var petObjects = await _repository.GetPetByGrade(petGrade);
            return Ok(petObjects);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PetObject), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PetObject>> CreatePet([FromBody] PetObject petObject)
        {
            await _repository.CreatePetObject(petObject);
            return CreatedAtRoute("Get PetObject", new { id = petObject.Id}, petObject);
        }

        [HttpPut]
        [ProducesResponseType(typeof(PetObject), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdatePet([FromBody] PetObject petObject)
        {
            return Ok(await _repository.UpdatePetObject(petObject));
        }

        [HttpDelete("{id:length(24)}", Name = "DeletePetObject")]
        [ProducesResponseType(typeof(PetObject), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeletePetObject(string id)
        {
            return Ok(await _repository.DeletePetObject(id));
        }


    }
}

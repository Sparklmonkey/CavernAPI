using PetBaseData.API.Entities;
using Realms.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBaseData.API.Repositories
{
    public interface IPetObjectRepository
    {
        Task<string> DoesUserExist(string username, string password);
        Task<IEnumerable<PetObject>> GetPetObjects();
        Task<PetObject> GetPetObjectById(string id);
        Task<IEnumerable<PetObject>> GetPetByName(string name);
        Task<IEnumerable<PetObject>> GetPetByGrade(string petGrade);

        Task CreatePetObject(PetObject petObject);
        Task<bool> UpdatePetObject(PetObject petObject);
        Task<bool> DeletePetObject(string id);
    }
}

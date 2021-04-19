using System;
using PetBaseData.API.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetBaseData.API.Entities;
using MongoDB.Driver;
using Realms.Sync;

namespace PetBaseData.API.Repositories
{
    public class PetObjectRepository : IPetObjectRepository
    {
        private readonly IPetContext _context;

        public PetObjectRepository(IPetContext context)
        {
            _context = context;
        }


        public async Task<string> DoesUserExist(string username, string password)
        {
            var app = App.Create("petcavern-apaxs");
            await app.EmailPasswordAuth.RegisterUserAsync(username, password);
            var user = await app.LogInAsync(Credentials.EmailPassword(username, password));
            return user.AccessToken;
        }
        public async Task<IEnumerable<PetObject>> GetPetObjects()
        {
            return await _context
                            .PetObjects
                            .Find(p => true)
                            .ToListAsync();
        }
        public async Task<PetObject> GetPetObjectById(string id)
        {
            return await _context
                            .PetObjects
                            .Find(p => p.Id == id)
                            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PetObject>> GetPetByName(string name)
        {
            FilterDefinition<PetObject> filter = Builders<PetObject>.Filter.Eq(p => p.PetName, name);
            return await _context
                            .PetObjects
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task<IEnumerable<PetObject>> GetPetByGrade(string petGrade)
        {
            FilterDefinition<PetObject> filter = Builders<PetObject>.Filter.Eq(p => p.PetGrade, petGrade);
            return await _context
                            .PetObjects
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task CreatePetObject(PetObject petObject)
        {
            await _context.PetObjects.InsertOneAsync(petObject);
        }

        public async Task<bool> UpdatePetObject(PetObject petObject)
        {
            var updateResult = await _context.PetObjects.ReplaceOneAsync(filter: g => g.Id == petObject.Id, replacement: petObject);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeletePetObject(string id)
        {
            FilterDefinition<PetObject> filter = Builders<PetObject>.Filter.Eq(p => p.Id, id);
            DeleteResult deleteResult = await _context
                                                    .PetObjects
                                                    .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

    }
}

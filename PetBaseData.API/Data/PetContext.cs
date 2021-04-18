using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PetBaseData.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBaseData.API.Data
{
    public class PetContext : IPetContext
    {
        public PetContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            petObjects = database.GetCollection<PetObject>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
            PetObjectSeed.SeedData(petObjects);
        }
        public IMongoCollection<PetObject> petObjects { get; }
    }
}

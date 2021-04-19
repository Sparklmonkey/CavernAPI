using System;
using MongoDB.Driver;
using PetBaseData.API.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBaseData.API.Data
{
    public interface IPetContext
    {
        IMongoCollection<PetObject> PetObjects { get; }
    }
}

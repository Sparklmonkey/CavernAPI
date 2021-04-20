using MongoDB.Driver;
using PetBaseData.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBaseData.API.Data
{
    public interface IUserDataContext
    {
        IMongoCollection<UserData> UserDataCollection { get; }
        IMongoCollection<SavedData> SavedDataCollection { get; }
        IMongoCollection<PetObject> PetObjectCollection { get; }
    }
}

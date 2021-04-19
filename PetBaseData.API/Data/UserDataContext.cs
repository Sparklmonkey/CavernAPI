using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PetBaseData.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBaseData.API.Data
{
    public class UserDataContext: IUserDataContext
    {
        public UserDataContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            UserDataCollection = database.GetCollection<UserData>(configuration.GetValue<string>("DatabaseSettings:UserCollectionName"));
            SavedDataCollection = database.GetCollection<SavedData>(configuration.GetValue<string>("DatabaseSettings:SavedDataCollectionName"));

        }
        public IMongoCollection<UserData> UserDataCollection { get; }
        public IMongoCollection<SavedData> SavedDataCollection { get; }
    }
}

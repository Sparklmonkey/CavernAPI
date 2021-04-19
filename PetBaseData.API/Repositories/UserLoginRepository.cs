using MongoDB.Driver;
using PetBaseData.API.Data;
using PetBaseData.API.Entities;
using Realms.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBaseData.API.Repositories
{
    public class UserLoginRepository : IUserLoginRepository
    {
        private readonly IUserDataContext _context;

        public UserLoginRepository(IUserDataContext context)
        {
            _context = context;
        }
        public async Task<SavedData> LoginUser(string username, string password)
        {
            var app = App.Create("petcavern-apaxs");
            var user = await app.LogInAsync(Credentials.EmailPassword(username, password));

            IAsyncCursor<UserData> asyncCursor = await _context.UserDataCollection.FindAsync(p => p.Id == user.Id);
            UserData userData = asyncCursor.Single();

            IAsyncCursor<SavedData> savedDataCursor = await _context.SavedDataCollection.FindAsync(g => g.Id == userData.SavedDataId);

            return savedDataCursor.Single();
        }

        public async Task<SavedData> RegisterUser(string username, string password)
        {
            var app = App.Create("petcavern-apaxs");
            await app.EmailPasswordAuth.RegisterUserAsync(username, password);
            var user = await app.LogInAsync(Credentials.EmailPassword(username, password));

            SavedData newSavedData = UserDataSeed.GetDefaultSavedData();
            await _context.SavedDataCollection.InsertOneAsync(newSavedData);
            UserData newUser = new()
            {
                SavedDataId = newSavedData.Id,
                SaltValue = Guid.NewGuid().ToString(),
                Username = username,
                Password = Sha256("{password}{SaltValue}"),
                Id = user.Id
            };

            await _context.UserDataCollection.InsertOneAsync(newUser);

            return newSavedData;
        }
        static string Sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}

using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PetBaseData.API.Data;
using PetBaseData.API.Entities;
using PetBaseData.API.Helpers;
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
        private readonly IConfiguration _configuration;

        public UserLoginRepository(IUserDataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ErrorCases> DeleteUserData(string username, string password, string playerId)
        {
            var hashPass = $"{password}{_configuration.GetValue<string>("Salt")}".Sha256();
            IAsyncCursor<UserData> userAsyncCursor = await _context.UserDataCollection.FindAsync(p => p.Id == playerId);
            UserData userData = userAsyncCursor.FirstOrDefault();

            if (userData == null)
            {
                return ErrorCases.UserDoesNotExist;
            }

            if (userData.Password != hashPass)
            {
                return ErrorCases.IncorrectPassword;
            }

            IAsyncCursor<SavedData> savedDataCursor = await _context.SavedDataCollection.FindAsync(g => g.Id == userData.SavedDataId);
            SavedData savedData = savedDataCursor.FirstOrDefault();

            if(userData.Username != username)
            {
                return ErrorCases.UserMismatch;
            }

            await _context.SavedDataCollection.DeleteOneAsync(q => q.Id == savedData.Id);
            await _context.UserDataCollection.DeleteOneAsync(q => q.Id == userData.Id);

            return ErrorCases.AllGood;
        }

        public async Task<LoginResponse> LoginUser(string username, string password)
        {
            var hashPass = $"{password}{_configuration.GetValue<string>("Salt")}".Sha256();
            IAsyncCursor<UserData> userAsyncCursor = await _context.UserDataCollection.FindAsync(p => p.Username == username);
            UserData userData = userAsyncCursor.FirstOrDefault();

            if(userData == null)
            {
                return new LoginResponse()
                {
                    ErrorMessage = ErrorCases.UserDoesNotExist
                };
            }

            if(userData.Password != hashPass)
            {
                return new LoginResponse()
                {
                    ErrorMessage = ErrorCases.IncorrectPassword
                };
            }

            IAsyncCursor<SavedData> savedDataCursor = await _context.SavedDataCollection.FindAsync(g => g.Id == userData.SavedDataId);

            LoginResponse returnValue = new()
            {
                PlayerData = savedDataCursor.SingleOrDefault(),
                PlayerId = userData.Id,
                ErrorMessage = ErrorCases.AllGood
            };

            return returnValue;
        }

        public async Task<LoginResponse> RegisterUser(string username, string password)
        {

            var hashPass = $"{password}{_configuration.GetValue<string>("Salt")}".Sha256();
            IAsyncCursor<UserData> userAsyncCursor = await _context.UserDataCollection.FindAsync(p => p.Username == username);

            UserData userData = userAsyncCursor.FirstOrDefault();

            if(userData != null)
            {
                return new LoginResponse()
                {
                    ErrorMessage = ErrorCases.UserNameInUse
                };
            }

            SavedData newSavedData = UserDataSeed.GetDefaultSavedData();
            await _context.SavedDataCollection.InsertOneAsync(newSavedData);
            UserData newUser = new()
            {
                SavedDataId = newSavedData.Id,
                Username = username,
                Password = hashPass
            };

            await _context.UserDataCollection.InsertOneAsync(newUser);

            LoginResponse returnValue = new()
            {
                PlayerData = newSavedData,
                PlayerId = newUser.Id,
                ErrorMessage = ErrorCases.AllGood
            };
            return returnValue;
        }

        public async Task<ErrorCases> UpdateSavedData(string playerId, SavedData savedData)
        {
            IAsyncCursor<UserData> userAsyncCursor = await _context.UserDataCollection.FindAsync(p => p.Id == playerId);
            UserData userData = userAsyncCursor.FirstOrDefault();

            if (userData == null)
            {
                return ErrorCases.UserDoesNotExist;
            }

            var updateResult = await _context.SavedDataCollection.ReplaceOneAsync(filter: g => g.Id == userData.SavedDataId, replacement: savedData);
            return ErrorCases.AllGood;
        }
    }
}

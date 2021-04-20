using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PetBaseData.API.Data;
using PetBaseData.API.Entities;
using PetBaseData.API.Models;
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

        public async Task<LoginResponse> LoginUser(LoginRequest loginRequest)
        {
            var hashPass = $"{loginRequest.Password}{_configuration.GetValue<string>("Salt")}".Sha256();
            IAsyncCursor<UserData> userAsyncCursor = await _context.UserDataCollection.FindAsync(p => p.Username == loginRequest.Username);
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

        public async Task<LoginResponse> RegisterUser(LoginRequest loginRequest)
        {

            var hashPass = $"{loginRequest.Password}{_configuration.GetValue<string>("Salt")}".Sha256();
            IAsyncCursor<UserData> userAsyncCursor = await _context.UserDataCollection.FindAsync(p => p.Username == loginRequest.Username);

            UserData userData = userAsyncCursor.FirstOrDefault();

            if(userData != null)
            {
                return new LoginResponse()
                {
                    ErrorMessage = ErrorCases.UserNameInUse
                };
            }

            SavedData newSavedData = UserDataSeed.GetDefaultSavedData();
            newSavedData.ListOfPets = _context.PetObjectCollection
                                                                .Find(p => true)
                                                                .ToEnumerable();

            await _context.SavedDataCollection.InsertOneAsync(newSavedData);
            UserData newUser = new()
            {
                SavedDataId = newSavedData.Id,
                Username = loginRequest.Username,
                Password = hashPass,
                EmailAddress = loginRequest.EmailAddress
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

        
    }
}

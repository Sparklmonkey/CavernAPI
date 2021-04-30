using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PetBaseData.API.Data;
using PetBaseData.API.Entities;
using PetBaseData.API.Helpers;
using PetBaseData.API.Models;

namespace PetBaseData.API.Repositories
{
    public class UserManagementRepository: IUserManagementRepository
    {
        private readonly IUserDataContext _context;
        private readonly IConfiguration _configuration;

        public UserManagementRepository(IUserDataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<AccountResponse> ChangePassword(AccountRequest accountRequest)
        {
            AccountResponse response = new AccountResponse();
            var hashPass = $"{accountRequest.Password}{_configuration.GetValue<string>("Salt")}".Sha256();
            IAsyncCursor<UserData> userAsyncCursor = await _context.UserDataCollection.FindAsync(p => p.Username == accountRequest.Username);
            UserData userData = userAsyncCursor.FirstOrDefault();

            if (userData == null)
            {
                response.ErrorMessage = ErrorCases.UserDoesNotExist;
                return response;
            }

            if (userData.Password != hashPass)
            {
                response.ErrorMessage = ErrorCases.IncorrectPassword;
                return response;
            }

            userData.Password = $"{accountRequest.NewPassword}{_configuration.GetValue<string>("Salt")}".Sha256();

            var result = await _context.UserDataCollection.ReplaceOneAsync(p => p.Id == userData.Id, userData);

            if(result.IsAcknowledged && result.ModifiedCount > 0)
            {
                response.ErrorMessage = ErrorCases.AllGood;
                return response;
            }

            response.ErrorMessage = ErrorCases.UnknownError;
            return response;

        }

        public async Task<AccountResponse> ChangeUsername(AccountRequest accountRequest)
        {
            AccountResponse response = new AccountResponse();
            var hashPass = $"{accountRequest.Password}{_configuration.GetValue<string>("Salt")}".Sha256();
            IAsyncCursor<UserData> userAsyncCursor = await _context.UserDataCollection.FindAsync(p => p.Username == accountRequest.Username);
            UserData userData = userAsyncCursor.FirstOrDefault();

            if (userData == null)
            {
                response.ErrorMessage = ErrorCases.UserDoesNotExist;
                return response;
            }

            if (userData.Password != hashPass)
            {
                response.ErrorMessage = ErrorCases.IncorrectPassword;
                return response;
            }

            IAsyncCursor<UserData> checkIfUsernameIsUsedCursor = await _context.UserDataCollection.FindAsync(p => p.Username == accountRequest.NewUsername);
            UserData checkIfUsernameIsUsed = userAsyncCursor.FirstOrDefault();

            if(checkIfUsernameIsUsed != null)
            {
                response.ErrorMessage = ErrorCases.UserNameInUse;
                return response;
            }

            userData.Username = accountRequest.NewUsername;

            var result = await _context.UserDataCollection.ReplaceOneAsync(p => p.Id == userData.Id, userData);

            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                response.ErrorMessage = ErrorCases.AllGood;
                return response;
            }

            response.ErrorMessage = ErrorCases.UnknownError;
            return response;
        }

        public async Task<AccountResponse> ForgotPassword(AccountRequest accountRequest)
        {
            AccountResponse response = new AccountResponse();

            IAsyncCursor<UserData> userAsyncCursor = await _context.UserDataCollection.FindAsync(p => p.Username == accountRequest.Username);
            UserData userData = userAsyncCursor.FirstOrDefault();

            if(userData.EmailAddress != accountRequest.EmailAddress)
            {
                response.ErrorMessage = ErrorCases.IncorrectEmail;
                return response;
            }

            var hashPass = $"{accountRequest.NewPassword}{_configuration.GetValue<string>("Salt")}".Sha256();
            userData.Password = hashPass;

            var replaceResponse = await _context.UserDataCollection.ReplaceOneAsync(p => p.Username == accountRequest.Username, userData);

            if(replaceResponse.IsAcknowledged && replaceResponse.ModifiedCount > 0)
            {
                response.ErrorMessage = ErrorCases.AllGood;
                return response;
            }


            response.ErrorMessage = ErrorCases.UnknownError;
            return response;
        }

        public async Task<AccountResponse> ResetGame(AccountRequest accountRequest)
        {
            AccountResponse response = new AccountResponse();
            var hashPass = $"{accountRequest.Password}{_configuration.GetValue<string>("Salt")}".Sha256();
            IAsyncCursor<UserData> userAsyncCursor = await _context.UserDataCollection.FindAsync(p => p.Username == accountRequest.Username);

            UserData userData = userAsyncCursor.FirstOrDefault();

            if(userData.Password != hashPass)
            {
                response.ErrorMessage = ErrorCases.IncorrectPassword;
                return response;
            }

            var deleteResult = await _context.SavedDataCollection.DeleteOneAsync(p => p.Id == userData.SavedDataId);

            if(!deleteResult.IsAcknowledged)
            {
                response.ErrorMessage = ErrorCases.UnknownError;
                return response;
            }

            SavedData newSavedData = UserDataSeed.GetDefaultSavedData();
            newSavedData.ListOfPets = _context.PetObjectCollection
                                                                .Find(p => true)
                                                                .ToEnumerable();

            await _context.SavedDataCollection.InsertOneAsync(newSavedData);
            userData.SavedDataId = newSavedData.Id;

            var replaceResult = await _context.UserDataCollection.ReplaceOneAsync(p => p.Id == userData.Id, userData);
            if(replaceResult.IsAcknowledged && replaceResult.ModifiedCount > 0)
            {
                response.SavedData = newSavedData;
                response.ErrorMessage = ErrorCases.AllGood;
                return response;
            }

            response.ErrorMessage = ErrorCases.UnknownError;
            return response;
        }



        public async Task<AccountResponse> DeleteUserData(AccountRequest accountRequest)
        {
            AccountResponse response = new AccountResponse();
            var hashPass = $"{accountRequest.Password}{_configuration.GetValue<string>("Salt")}".Sha256();
            IAsyncCursor<UserData> userAsyncCursor = await _context.UserDataCollection.FindAsync(p => p.Username == accountRequest.Username);
            UserData userData = userAsyncCursor.FirstOrDefault();

            if (userData == null)
            {
                response.ErrorMessage = ErrorCases.UserDoesNotExist;
                return response;
            }

            if (userData.Password != hashPass)
            {
                response.ErrorMessage = ErrorCases.IncorrectPassword;
                return response;
            }

            IAsyncCursor<SavedData> savedDataCursor = await _context.SavedDataCollection.FindAsync(g => g.Id == userData.SavedDataId);
            SavedData savedData = savedDataCursor.FirstOrDefault();

            await _context.SavedDataCollection.DeleteOneAsync(q => q.Id == savedData.Id);
            await _context.UserDataCollection.DeleteOneAsync(q => q.Id == userData.Id);

            response.ErrorMessage = ErrorCases.AllGood;
            return response;
        }

        public async Task<AccountResponse> UpdateSavedData(AccountRequest accountRequest)
        {
            AccountResponse response = new AccountResponse();
            IAsyncCursor<UserData> userAsyncCursor = await _context.UserDataCollection.FindAsync(p => p.Id == accountRequest.playerId);
            UserData userData = userAsyncCursor.FirstOrDefault();

            if (userData == null)
            {
                response.ErrorMessage = ErrorCases.UserDoesNotExist;
                return response;
            }


            var updateResult = await _context.SavedDataCollection.ReplaceOneAsync(filter: g => g.Id == userData.SavedDataId, replacement: accountRequest.SavedData);

            if(updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
            {
                response.ErrorMessage = ErrorCases.AllGood;
                return response;
            }


            response.ErrorMessage = ErrorCases.UnknownError;
            return response;
        }

        public async Task<AccountResponse> ChangeEmailAdress(AccountRequest accountRequest)
        {
            AccountResponse response = new AccountResponse();
            var hashPass = $"{accountRequest.Password}{_configuration.GetValue<string>("Salt")}".Sha256();
            IAsyncCursor<UserData> userAsyncCursor = await _context.UserDataCollection.FindAsync(p => p.Username == accountRequest.Username);
            UserData userData = userAsyncCursor.FirstOrDefault();

            if (userData == null)
            {
                response.ErrorMessage = ErrorCases.UserDoesNotExist;
                return response;
            }

            if (userData.Password != hashPass)
            {
                response.ErrorMessage = ErrorCases.IncorrectPassword;
                return response;
            }

            userData.EmailAddress = accountRequest.EmailAddress;
            userData.IsVerified = false;

            var result = await _context.UserDataCollection.ReplaceOneAsync(p => p.Id == userData.Id, userData);

            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                response.ErrorMessage = ErrorCases.AllGood;
                return response;
            }

            response.ErrorMessage = ErrorCases.UnknownError;
            return response;
        }
    }
}

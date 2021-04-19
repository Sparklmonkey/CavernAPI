using PetBaseData.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBaseData.API.Repositories
{
    public interface IUserLoginRepository
    {
        Task<LoginResponse> RegisterUser(string username, string password);
        Task<LoginResponse> LoginUser(string username, string password);
        Task<ErrorCases> UpdateSavedData(string playerId, SavedData savedData);
        Task<ErrorCases> DeleteUserData(string username, string password, string playerId);
    }
}

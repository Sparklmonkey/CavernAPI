using PetBaseData.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBaseData.API.Repositories
{
    public interface IUserLoginRepository
    {
        Task<SavedData> RegisterUser(string username, string password);
        Task<SavedData> LoginUser(string username, string password);
    }
}

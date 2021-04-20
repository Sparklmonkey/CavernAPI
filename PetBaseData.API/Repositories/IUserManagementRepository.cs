using System;
using System.Threading.Tasks;
using PetBaseData.API.Entities;
using PetBaseData.API.Models;

namespace PetBaseData.API.Repositories
{
    public interface IUserManagementRepository
    {
        Task<AccountResponse> ChangePassword(AccountRequest accountRequest);
        Task<AccountResponse> ForgotPassword(AccountRequest accountRequest);
        Task<AccountResponse> ChangeUsername(AccountRequest accountRequest);
        Task<AccountResponse> ChangeEmailAdress(AccountRequest accountRequest);
        Task<AccountResponse> ResetGame(AccountRequest accountRequest);
        Task<AccountResponse> UpdateSavedData(AccountRequest accountRequest);
        Task<AccountResponse> DeleteUserData(AccountRequest accountRequest);
    }
}

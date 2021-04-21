using PetBaseData.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBaseData.API.Repositories
{
    public interface IUserLoginRepository
    {
        Task<LoginResponse> RegisterUser(LoginRequest loginRequest);
        Task<LoginResponse> LoginUser(LoginRequest loginRequest);
        Task<LoginResponse> ValidateUser(LoginRequest loginRequest);
        Task<LoginResponse> ResendOtp(LoginRequest loginRequest);
    }
}

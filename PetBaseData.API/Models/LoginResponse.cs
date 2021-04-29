using System;
using PetBaseData.API.Entities;

namespace PetBaseData.API.Models
{
    public class LoginResponse
    {
        public string PlayerId { get; set; }
        public SavedData PlayerData { get; set; }
        public ErrorCases ErrorMessage { get; set; }
    }
}

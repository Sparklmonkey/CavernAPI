using System;
namespace PetBaseData.API.Entities
{
    public class LoginResponse
    {
        public string PlayerId { get; set; }
        public SavedData PlayerData { get; set; }
        public ErrorCases ErrorMessage { get; set; }
    }
}

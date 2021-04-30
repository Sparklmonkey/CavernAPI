using System;
using PetBaseData.API.Entities;

namespace PetBaseData.API.Models
{
    public class AccountRequest
    {
        public string playerId { get; set; }

        public string Username { get; set; }
        public string NewUsername { get; set; }

        public string Password { get; set; }
        public string NewPassword { get; set; }

        public string EmailAddress { get; set; }
        public string NewEmailAddress { get; set; }

        public SavedData SavedData { get; set; }
    }
}

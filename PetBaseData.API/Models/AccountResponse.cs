using System;
using PetBaseData.API.Entities;

namespace PetBaseData.API.Models
{
    public class AccountResponse
    {
        public ErrorCases ErrorMessage { get; set; }
        public SavedData SavedData { get; set; }
    }
}

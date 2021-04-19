using System;
namespace PetBaseData.API.Entities
{
    public class UpdateRequest
    {
        public string PlayerId { get; set; }
        public SavedData SavedData { get; set; }
    }
}

using System;
namespace PetBaseData.API.Entities
{
    public class DeleteRequest
    {
        public string Password { get; set; }
        public string Username { get; set; }
        public string PlayerId { get; set; }
    }
}

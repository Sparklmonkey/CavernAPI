using System;
namespace PetBaseData.API.Models
{
    public class LoginRequest
    {
        public string PlayerId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string OtpCode { get; set; }
    }
}

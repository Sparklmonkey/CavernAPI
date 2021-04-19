using System;
using System.Text;

namespace PetBaseData.API.Helpers
{
    public static class ExtendMethods
    {

        public static string Sha256(this string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        public static bool isValidKey(this string appKey)
        {
            return false;
        }
    }
}

using System.Security.Cryptography;
using System.Text;

namespace HeroAPI.Helpers.Auth
{
    public static class PasswordHelper
    {
        public static void CreatePasswordHash(string password, out byte[] salt, out byte[] passwordHash)
        {
            using(var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool PasswordIsValid(string password, byte[] salt, byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                return hmac.ComputeHash(Encoding.UTF8.GetBytes(password)).SequenceEqual(passwordHash);
            }
        }
    }
}

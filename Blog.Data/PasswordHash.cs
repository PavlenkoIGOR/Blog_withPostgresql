using System.Security.Cryptography;
using System.Text;

namespace Blog_withPostgresql
{
    public class PasswordHash
    {
        public static string HashPassword(string enteredPassword)
        {
            using (var HashP = SHA256.Create())
            {
                var hashedBytes = HashP.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
}

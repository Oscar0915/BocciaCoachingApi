using System.Security.Cryptography;

namespace BocciaCoaching.Utils
{
    public class SecurityCodeGenerator
    {
        public static string GenerateCode(int length = 6)
        {
            var bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);
            return string.Join("", bytes.Select(b => (b % 10).ToString()));
        }
    }
}

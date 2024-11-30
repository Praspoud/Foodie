using System.Security.Cryptography;
namespace Foodie.Common.Services
{
    public static class PasswordHash
    {
        public static string ToHashString(this string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

            // Uses SHA256 to create the hash
            using var sha = new SHA256Managed();
            // Convert the string to a byte array first, to be processed
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text);
            byte[] hashBytes = sha.ComputeHash(textBytes);

            // Convert back to a string, removing the '-' that BitConverter adds
            string hash = BitConverter
                .ToString(hashBytes)
                .Replace("-", String.Empty);

            return hash;
        }
    }
}

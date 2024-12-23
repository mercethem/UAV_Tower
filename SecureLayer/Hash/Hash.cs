using System.Security.Cryptography;
using System.Text;

namespace Hash
{
    class Hash
    {
        // Static method to get the SHA-128 hash (using first 16 bytes of SHA-1) 
        public static string GetSHA128Hash(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] data = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                byte[] truncatedData = new byte[16];
                Array.Copy(data, truncatedData, 16);
                return BitConverter.ToString(truncatedData).Replace("-", "").ToLower();
            }
        }

        // Static method to get the SHA-256 hash
        public static string GetSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(data).Replace("-", "").ToLower();
            }
        }
    }
}

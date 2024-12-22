using System.Security.Cryptography;
using System.Text;

namespace Hash
{
    class Hash
    {
        // Static method to get the SHA-128 hash (using first 16 bytes of SHA-1) // SHA-128 hash almak için statik metod (SHA-1'in ilk 16 byte'ını kullanarak)
        public static string GetSHA128Hash(string input)
        {
            using (SHA1 sha1 = SHA1.Create()) // Create an instance of SHA1 to compute the hash // SHA1 örneği oluşturuluyor ve hash hesaplanıyor
            {
                byte[] data = sha1.ComputeHash(Encoding.UTF8.GetBytes(input)); // Compute the hash of the input string // Girdi dizisinin hash'ini hesapla
                byte[] truncatedData = new byte[16];  // Create an array of 16 bytes to hold the truncated hash // Truncate SHA-1 hash to 16 bytes for SHA-128
                Array.Copy(data, truncatedData, 16); // Copy the first 16 bytes of the SHA-1 hash // SHA-1 hash'inin ilk 16 byte'ını kopyala
                return BitConverter.ToString(truncatedData).Replace("-", "").ToLower(); // Convert the hash to a hex string and return it // Hash'i hex string'e dönüştür ve döndür
            }
        }

        // Static method to get the SHA-256 hash // SHA-256 hash almak için statik metod
        public static string GetSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create()) // Create an instance of SHA256 to compute the hash // SHA256 örneği oluşturuluyor ve hash hesaplanıyor
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input)); // Compute the hash of the input string // Girdi dizisinin hash'ini hesapla
                return BitConverter.ToString(data).Replace("-", "").ToLower(); // Convert the hash to a hex string and return it // Hash'i hex string'e dönüştür ve döndür
            }
        }
    }
}

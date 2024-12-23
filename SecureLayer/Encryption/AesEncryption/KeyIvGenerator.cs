using System;
using System.Security.Cryptography;
using System.Text;

namespace Encryption
{
    // Anahtar ve IV Oluşturma Sınıfı
    public class KeyIvGenerator
    {
        public static (byte[] Key, byte[] IV) GenerateRandomKeyAndIV()
        {
            byte[] key = new byte[16]; // 128-bit key 
            byte[] iv = new byte[16];  // 128-bit IV 
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key); // Fills key with random bytes 
                rng.GetBytes(iv); // Fills IV with random bytes
            }
            return (key, iv); // Returns the generated key and IV 
        }

        public static byte[] AdjustKeyLength(string keyInput)
        {
            byte[] key = Encoding.UTF8.GetBytes(keyInput); // Converts input string to bytes 
            Array.Resize(ref key, 16); // Resizes key to 16 bytes 
            return key; // Returns adjusted key
        }

        public static byte[] AdjustIVLength(string ivInput)
        {
            byte[] iv = Encoding.UTF8.GetBytes(ivInput); // Converts input string to bytes 
            Array.Resize(ref iv, 16); // Resizes IV to 16 bytes 
            return iv; // Returns adjusted IV 
        }
    }
}
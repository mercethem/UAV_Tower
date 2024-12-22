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
            byte[] key = new byte[16]; // 128-bit key // 128-bit anahtar
            byte[] iv = new byte[16];  // 128-bit IV // 128-bit IV
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key); // Fills key with random bytes // Anahtarı rastgele baytlarla doldurur
                rng.GetBytes(iv); // Fills IV with random bytes // IV'yi rastgele baytlarla doldurur
            }
            return (key, iv); // Returns the generated key and IV // Üretilen anahtar ve IV'yi döndürür
        }

        public static byte[] AdjustKeyLength(string keyInput)
        {
            byte[] key = Encoding.UTF8.GetBytes(keyInput); // Converts input string to bytes // Girdi dizesini baytlara dönüştürür
            Array.Resize(ref key, 16); // Resizes key to 16 bytes // Anahtarı 16 bayta yeniden boyutlandırır
            return key; // Returns adjusted key // Ayarlanmış anahtarı döndürür
        }

        public static byte[] AdjustIVLength(string ivInput)
        {
            byte[] iv = Encoding.UTF8.GetBytes(ivInput); // Converts input string to bytes // Girdi dizesini baytlara dönüştürür
            Array.Resize(ref iv, 16); // Resizes IV to 16 bytes // IV'yi 16 bayta yeniden boyutlandırır
            return iv; // Returns adjusted IV // Ayarlanmış IV'yi döndürür
        }
    }
}
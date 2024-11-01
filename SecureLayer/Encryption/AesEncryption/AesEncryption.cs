using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Text;

namespace Encryption
{
    // AesEncryption Sınıfı
    public class AesEncryption : BaseEncryption, IEncryption
    {
        public byte[] key { get; } // Implementation of key property // Anahtar property'sinin implementasyonu
        public byte[] iv { get; } // Property to store IV (Initialization Vector) // IV (Başlatma Vektörü) için property

        public AesEncryption(string keyInput, string ivInput)
        {
            key = KeyIvGenerator.AdjustKeyLength(keyInput); // Adjusts and sets the encryption key // Şifreleme anahtarını ayarlar
            iv = KeyIvGenerator.AdjustIVLength(ivInput); // Adjusts and sets the IV // IV'yi ayarlar
        }

        public AesEncryption()
        {
            (key, iv) = KeyIvGenerator.GenerateRandomKeyAndIV(); // Generates random key and IV // Rastgele anahtar ve IV oluşturur
        }

        public override byte[] Encrypt(string plainText)
        {
            var cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine())); // Sets up AES cipher in CBC mode // AES şifrelemesini CBC modunda ayarlar
            var keyParam = new KeyParameter(key); // Prepares key parameter // Anahtar parametresini hazırlar
            var parameters = new ParametersWithIV(keyParam, iv); // Combines key and IV // Anahtar ve IV'yi birleştirir
            cipher.Init(true, parameters); // Initializes cipher for encryption // Şifrelemeye yönelik şifreleme için başlatır

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText); // Converts plaintext to bytes // Düz metni baytlara dönüştürür
            byte[] cipheredData = new byte[cipher.GetOutputSize(plainBytes.Length)]; // Prepares buffer for ciphertext // Şifreli veri için tamponu hazırlar

            int len = cipher.ProcessBytes(plainBytes, 0, plainBytes.Length, cipheredData, 0); // Encrypts the data // Veriyi şifreler
            len += cipher.DoFinal(cipheredData, len); // Finalizes encryption // Şifrelemeyi tamamlar

            Array.Resize(ref cipheredData, len); // Resizes to the actual size // Gerçek boyuta göre yeniden boyutlandırır
            return cipheredData; // Returns encrypted data // Şifreli veriyi döndürür
        }

        public override string Decrypt(byte[] cipherText)
        {
            var cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine())); // Sets up AES cipher in CBC mode // AES şifrelemesini CBC modunda ayarlar
            var keyParam = new KeyParameter(key); // Prepares key parameter // Anahtar parametresini hazırlar
            var parameters = new ParametersWithIV(keyParam, iv); // Combines key and IV // Anahtar ve IV'yi birleştirir
            cipher.Init(false, parameters); // Initializes cipher for decryption // Şifre çözme için başlatır

            byte[] decryptedData = new byte[cipher.GetOutputSize(cipherText.Length)]; // Prepares buffer for decrypted data // Çözülmüş veri için tamponu hazırlar
            int len = cipher.ProcessBytes(cipherText, 0, cipherText.Length, decryptedData, 0); // Decrypts the data // Veriyi çözer
            len += cipher.DoFinal(decryptedData, len); // Finalizes decryption // Şifre çözmeyi tamamlar

            Array.Resize(ref decryptedData, len); // Resizes to the actual size // Gerçek boyuta göre yeniden boyutlandırır
            return Encoding.UTF8.GetString(decryptedData); // Converts decrypted bytes back to string // Çözülmüş baytları tekrar string'e çevirir
        }
    }
}

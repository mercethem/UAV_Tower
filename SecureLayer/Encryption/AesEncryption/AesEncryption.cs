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
        public byte[] key { get; } // Implementation of key property
        public byte[] iv { get; } // Property to store IV (Initialization Vector) 

        public AesEncryption(string keyInput, string ivInput)
        {
            key = KeyIvGenerator.AdjustKeyLength(keyInput); // Adjusts and sets the encryption key 
            iv = KeyIvGenerator.AdjustIVLength(ivInput); // Adjusts and sets the IV
        }

        public AesEncryption()
        {
            (key, iv) = KeyIvGenerator.GenerateRandomKeyAndIV(); // Generates random key and IV 
        }

        public override byte[] Encrypt(string plainText)
        {
            var cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine())); // Sets up AES cipher in CBC mode 
            var keyParam = new KeyParameter(key); // Prepares key parameter 
            var parameters = new ParametersWithIV(keyParam, iv); // Combines key and IV 
            cipher.Init(true, parameters); // Initializes cipher for encryption 

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText); // Converts plaintext to bytes 
            byte[] cipheredData = new byte[cipher.GetOutputSize(plainBytes.Length)]; // Prepares buffer for ciphertext ar

            int len = cipher.ProcessBytes(plainBytes, 0, plainBytes.Length, cipheredData, 0); // Encrypts the data 
            len += cipher.DoFinal(cipheredData, len); // Finalizes encryption 

            Array.Resize(ref cipheredData, len); // Resizes to the actual size 
            return cipheredData; // Returns encrypted data 
        }

        public override string Decrypt(byte[] cipherText)
        {
            var cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine())); // Sets up AES cipher in CBC mode
            var keyParam = new KeyParameter(key); // Prepares key parameter
            var parameters = new ParametersWithIV(keyParam, iv); // Combines key and IV 
            cipher.Init(false, parameters); // Initializes cipher for decryption 

            byte[] decryptedData = new byte[cipher.GetOutputSize(cipherText.Length)]; // Prepares buffer for decrypted data 
            int len = cipher.ProcessBytes(cipherText, 0, cipherText.Length, decryptedData, 0); // Decrypts the data
            len += cipher.DoFinal(decryptedData, len); // Finalizes decryptioN

            Array.Resize(ref decryptedData, len); // Resizes to the actual size 
            return Encoding.UTF8.GetString(decryptedData); // Converts decrypted bytes back to string 
        }
    }
}

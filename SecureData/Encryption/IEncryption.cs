namespace Encryption
{
    // Şifreleme Arayüzü
    public interface IEncryption
    {
        public byte[] key { get; } // Property to get the encryption key // Şifreleme anahtarını almak için property
        byte[] Encrypt(string plainText); // Method to encrypt plaintext // Düz metni şifrelemek için metot
        string Decrypt(byte[] cipherText); // Method to decrypt ciphertext // Şifreli metni çözmek için metot
    }
}
namespace Encryption
{
    // Şifreleme Arayüzü
    public interface IEncryption
    {
        byte[] Encrypt(string plainText); // Method to encrypt plaintext 
        string Decrypt(byte[] cipherText); // Method to decrypt ciphertext
    }
}

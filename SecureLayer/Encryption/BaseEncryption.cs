namespace Encryption
{
    // Soyut Şifreleme Sınıfı
    public abstract class BaseEncryption : IEncryption
    {
        public abstract byte[] Encrypt(string plainText); // Abstract method for encryption // Şifreleme için soyut metot
        public abstract string Decrypt(byte[] cipherText); // Abstract method for decryption // Şifre çözme için soyut metot
    }
}

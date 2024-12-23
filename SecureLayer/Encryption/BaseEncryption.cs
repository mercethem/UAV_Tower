namespace Encryption
{
    // Soyut Şifreleme Sınıfı
    public abstract class BaseEncryption : IEncryption
    {
        public abstract byte[] Encrypt(string plainText); 
        public abstract string Decrypt(byte[] cipherText);
    }
}

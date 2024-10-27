﻿// Ana Program Sınıfı
class TEST
{
    static void Main()
    {
        string plainText = "Hello, World! This is a longer message to test the AES encryption and decryption functionality.";

        // Anahtar ve IV belirleme
        string keyInput = "benimsifrembenimsifrem"; // Suitable key for 16 bytes
        string ivInput = "benimivbenimivbe"; // Suitable IV for 16 bytes

        var aes = new Encryption.AesEncryption(keyInput, ivInput); // Initialize with specified key and IV
        // var aes = new AesEncryption(); // Initialize with random key and IV

        // 1. Şifreleme
        Console.WriteLine("1. Şifrelenecek Metin: " + plainText);
        Console.WriteLine("Anahtar (Hex): " + BitConverter.ToString(aes.key).Replace("-", ""));
        Console.WriteLine("IV (Hex): " + BitConverter.ToString(aes.iv).Replace("-", ""));

        byte[] cipherText = aes.Encrypt(plainText); // Encrypt the plaintext
        Console.WriteLine("2. Şifrelenmiş Metin (Base64): " + Convert.ToBase64String(cipherText));
        Console.WriteLine("   Şifrelenmiş Metin (Hex): " + BitConverter.ToString(cipherText).Replace("-", ""));

        string decryptedText = aes.Decrypt(cipherText); // Decrypt the ciphertext
        Console.WriteLine("3. Çözümlenmiş Metin: " + decryptedText);
    }
}
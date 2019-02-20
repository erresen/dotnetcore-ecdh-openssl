using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace dotnetcore_ecdh_openssl
{
    class Alice
    {
        public static ECDiffieHellmanPublicKey AlicePublicKey;

        public static void Main(string[] args)
        {
            using (ECDiffieHellmanOpenSsl alice = new ECDiffieHellmanOpenSsl())
            {
                AlicePublicKey = alice.PublicKey;

                Bob bob = new Bob();

                byte[] aliceKey = alice.DeriveKeyMaterial(bob.BobPublicKey);
                byte[] encryptedMessage = null;
                byte[] iv = null;

                Send(aliceKey, "Secret message", out encryptedMessage, out iv);
                bob.Receive(encryptedMessage, iv);
            }
        }

        private static void Send(byte[] key, string secretMessage, out byte[] encryptedMessage, out byte[] iv)
        {
            using (Aes aes = new AesCryptoServiceProvider())
            {
                aes.Key = key;
                iv = aes.IV;

                // Encrypt the message
                using (MemoryStream ciphertext = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ciphertext, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] plaintextMessage = Encoding.UTF8.GetBytes(secretMessage);
                    cs.Write(plaintextMessage, 0, plaintextMessage.Length);
                    cs.Close();
                    encryptedMessage = ciphertext.ToArray();
                }
            }
        }
    }

    public class Bob
    {
        public ECDiffieHellmanPublicKey BobPublicKey;
        private byte[] _bobPrivateKey;
        public Bob()
        {
            using (ECDiffieHellmanOpenSsl bob = new ECDiffieHellmanOpenSsl())
            {
                BobPublicKey = bob.PublicKey;
                _bobPrivateKey = bob.DeriveKeyMaterial(Alice.AlicePublicKey);
            }
        }

        public void Receive(byte[] encryptedMessage, byte[] iv)
        {
            using (Aes aes = new AesCryptoServiceProvider())
            {
                aes.Key = _bobPrivateKey;
                aes.IV = iv;

                // Decrypt the message
                using (MemoryStream plaintext = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(plaintext, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedMessage, 0, encryptedMessage.Length);
                        cs.Close();
                        string message = Encoding.UTF8.GetString(plaintext.ToArray());
                        Console.WriteLine(message);
                    }
                }
            }
        }
    }
}
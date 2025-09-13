using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KMS.Common.Constants
{
    public static class EntityExtensions
    {
        public static string EncryptString(string? plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return "";
            try
            {
                var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
                var passwordBytes = Encoding.UTF8.GetBytes("pass.phudev.com");
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
                var bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);
                return Convert.ToBase64String(bytesEncrypted);
            }
            catch
            {
                return "";
            }
        }

        public static string DecryptString(this string? encryptedText)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedText)) return "";
                var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
                var passwordBytes = Encoding.UTF8.GetBytes("pass.phudev.com");
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
                var bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);
                return Encoding.UTF8.GetString(bytesDecrypted);
            }
            catch
            {
                return "";
            }
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using MemoryStream ms = new MemoryStream();
            using RijndaelManaged aes = new RijndaelManaged();
            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                cs.Close();
            }

            var encryptedBytes = ms.ToArray();
            return encryptedBytes;
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            try
            {
                var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                using MemoryStream ms = new MemoryStream();
                using RijndaelManaged aes = new RijndaelManaged();
                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);
                aes.Mode = CipherMode.CBC;

                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                    cs.Close();
                }

                var decryptedBytes = ms.ToArray();

                return decryptedBytes;
            }
            catch
            {
                throw;
            }
        }
    }
}

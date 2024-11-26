using System.Security.Cryptography;

namespace AESUtil
{
    internal static class Cipher
    {
        public static byte[] Encrypt(byte[] data, byte[] aesKey, byte[] aesIV)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = aesKey;
            aesAlg.IV = aesIV;
            aesAlg.Padding = PaddingMode.PKCS7;
            aesAlg.Mode = CipherMode.CBC;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            return encryptor.TransformFinalBlock(data, 0, data.Length);
        }

        public static byte[] Decrypt(byte[] data, byte[] aesKey, byte[] aesIV)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = aesKey;
            aesAlg.IV = aesIV;
            aesAlg.Padding = PaddingMode.PKCS7;
            aesAlg.Mode = CipherMode.CBC;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            return decryptor.TransformFinalBlock(data, 0, data.Length);
        }
    }
}
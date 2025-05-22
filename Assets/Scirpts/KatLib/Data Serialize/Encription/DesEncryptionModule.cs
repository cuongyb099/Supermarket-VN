using System;
using System.IO;
using System.Security.Cryptography;

namespace KatLib.Data_Serialize
{
    public class DesEncryptionModule : IEncryptionModule
    {
        public byte[] Encrypt(byte[] data, string keyString)
        {
            var (key, iv) = SerializeUtil.GenerateKeyAndIVFromPassword(keyString);
            
            using (var des = DES.Create())
            {
                var keyBytes = System.Text.Encoding.UTF8.GetBytes(keyString);
                Array.Copy(keyBytes, key, Math.Min(keyBytes.Length, key.Length));

                des.Key = key;
                des.IV = iv;

                using (var encryptor = des.CreateEncryptor())
                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        public byte[] Decrypt(byte[] cipherText, string keyString)
        {
            if (cipherText.Length % 8 != 0)
                throw new ArgumentException("Error");

            var (key, iv) = SerializeUtil.GenerateKeyAndIVFromPassword(keyString);
            
            using (var des = DES.Create())
            {
                var keyBytes = System.Text.Encoding.UTF8.GetBytes(keyString);
                Array.Copy(keyBytes, key, Math.Min(keyBytes.Length, key.Length));

                des.Key = key;
                des.IV = iv;

                using (var decryptor = des.CreateDecryptor())
                using (var ms = new MemoryStream(cipherText))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var resultStream = new MemoryStream())
                {
                    cs.CopyTo(resultStream);
                    return resultStream.ToArray();
                }
            }
        }
    }
}

using System.IO;
using System.Security.Cryptography;

namespace KatLib.Data_Serialize
{
    public class AesEncryption : IEncryptionModule
    {
        public byte[] Encrypt(byte[] data, string keyString)
        {
            var (key, iv) = SerializeUtil.GenerateKeyAndIVFromPassword(keyString);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.Close();
                    return ms.ToArray();
                }
            }
        }

        public byte[] Decrypt(byte[] cipherText, string keyString)
        {
            var (key, iv) = SerializeUtil.GenerateKeyAndIVFromPassword(keyString);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor())
                using (var ms = new MemoryStream(cipherText))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new MemoryStream())
                {
                    cs.CopyTo(sr);
                    cs.Close();
                    return sr.ToArray();
                }
            }
        }
    }
}

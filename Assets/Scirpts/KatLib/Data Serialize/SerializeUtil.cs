using System.Security.Cryptography;

namespace KatLib.Data_Serialize
{
    internal static class SerializeUtil
    {
        public static (byte[] Key, byte[] IV) GenerateKeyAndIVFromPassword(string password)
        {
            const int KEY_SIZE = 32; // 256 bits
            const int IV_SIZE = 16;  // 128 bits
            const int ITERATIONS = 10000;
    
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt: new byte[8], ITERATIONS))
            {
                byte[] key = deriveBytes.GetBytes(KEY_SIZE);
                byte[] iv = deriveBytes.GetBytes(IV_SIZE);
                return (key, iv);
            }
        }
    }
}

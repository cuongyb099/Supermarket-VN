namespace KatLib.Data_Serialize
{
    public interface IEncryptionModule
    {
        public byte[] Encrypt(byte[] data, string keyString);
        public byte[] Decrypt(byte[] cipherText, string keyString);
    }
}

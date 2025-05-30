namespace KatLib.Data_Serialize
{
    public interface ISerializeModule
    {
        public void SaveFile(string filePath);
        public void LoadFile(string filePath);
        public T GetData<T>(string key);
        public void SetData<T>(string key, T value);
    }
}

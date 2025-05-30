using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace KatLib.Data_Serialize
{
    public class JsonSerialize : ISerializeModule
    {
        private Dictionary<string, object> _datas;
        private IEncryptionModule _encryptionModule;
        private SaveConfig _saveConfig;
        
        public static JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,                         
        };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            JsonConvert.DefaultSettings = () => DefaultSettings;
        }
        
        public JsonSerialize(IEncryptionModule encryptionModule, SaveConfig saveConfig)
        {
            _encryptionModule = encryptionModule;
            _saveConfig = saveConfig;
        }
        
        public void SaveFile(string filePath)
        {
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                if (directoryPath != null) Directory.CreateDirectory(directoryPath);
            }

            string json = JsonConvert.SerializeObject(_datas);
            
            if (_encryptionModule != null)
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(json);
                byte[] encryptedBytes = _encryptionModule.Encrypt(dataBytes, _saveConfig.EncryptionKey);
                json = Convert.ToBase64String(encryptedBytes);
            }
            
            File.WriteAllText(filePath, json);
        }

        public void LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                _datas = new Dictionary<string, object>();
                SaveFile(filePath);
                return;
            }
            
            string json = File.ReadAllText(filePath);
            
            if (_encryptionModule != null)
            {
                byte[] dataBytes = Convert.FromBase64String(json);
                byte[] encryptedBytes = _encryptionModule.Decrypt(dataBytes, _saveConfig.EncryptionKey);
                json = Encoding.UTF8.GetString(encryptedBytes);;
            }
            
            _datas = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }

        public T GetData<T>(string key)
        {
            if (!_datas.TryGetValue(key, out object value))
            {
                return default;
            }

            if(value is T realData) return realData;
            
            return default;
        }

        public void SetData<T>(string key, T value)
        {
            _datas[key] = value;
        }
    }
}

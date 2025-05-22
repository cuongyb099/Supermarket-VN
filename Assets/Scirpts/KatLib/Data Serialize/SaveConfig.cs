using UnityEngine;

namespace KatLib.Data_Serialize
{
    [CreateAssetMenu(fileName = "SaveConfig")]
    public class SaveConfig : ScriptableObject
    {
        [Header("File Name Don't Include File Name Extensions")]
        public string FileName;
        public string EncryptionKey;
        public EncryptionType EncryptionType;
        public SerializeType SerializeType;
    }
}

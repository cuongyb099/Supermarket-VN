using Newtonsoft.Json;
using UnityEngine;

namespace KatJsonInventory.Item
{
    public abstract class ItemData
    {
        [JsonProperty("ID")] public string ID { get; protected set; }
        //[JsonProperty("Icon"), JsonConverter(typeof(SpriteConverter))] 
        public Sprite Icon { get; protected set; }
        [JsonProperty("Description")] public string Description { get; protected set; }

        public abstract IInstanceItem CreateItem(int quantity = 1);

        public bool Equals(ItemData other)
        {
            if (other == null) return false;
            return ID == other.ID;
        }
    }
}

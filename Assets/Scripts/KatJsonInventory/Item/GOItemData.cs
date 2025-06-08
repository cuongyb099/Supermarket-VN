using Newtonsoft.Json;
using KatLib.Logger;
using UnityEngine;

namespace KatJsonInventory.Item
{
    public abstract class GOItemData : ItemData
    {
        [JsonConverter(typeof(PrefabConverter)), JsonProperty("Prefab Address")]
        protected GameObject Prefab;
        
        public override IInstanceItem CreateItem(int quantity = 1)
        {
            if (Prefab == null)
            {
                LogCommon.LogError("Prefab Load Failed");
                return null;
            }

            if (!Object.Instantiate(Prefab).TryGetComponent(out ItemBase item))
            {
                LogCommon.LogError("ItemBase Component Not Found");
                return null;
            }
            
            item.gameObject.SetActive(false);
            item.Init(this);
            return item;
        }
    }
}

using UnityEngine;

namespace KatJsonInventory.Item
{
    public abstract class ItemBase : MonoBehaviour, IInstanceItem
    {
        public abstract void Init(ItemData itemData);
        
        public abstract ItemData GetItemData();
    }
}

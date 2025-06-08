using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KatJsonInventory.Item;
using UnityEngine;

namespace KatJsonInventory
{
    public class Inventory : MonoBehaviour
    {
        [field: SerializeField, Range(1, 1000)]
        public int Capacity { get; private set;} = 99;
        public bool IsFull => _inventory.Count >= Capacity;
        private List<IInstanceItem> _inventory;
        
#if UNITY_EDITOR
        public List<IInstanceItem> InventoryEditor => _inventory;
#endif
        
        [SerializeField] private List<StartedItem> _startedItems;
        public Action OnInventoryChange;
        public Action OnAddItem;
        
        public ItemDatabase Database { get; private set; }

        private void Awake()
        {
            StartCoroutine(LoadStatedItem());
        }

        private IEnumerator LoadStatedItem()
        {
            yield return null;
            _inventory = new List<IInstanceItem>();

            foreach (var item in _startedItems)
            {
                Add(item.ID, item.Quantity);
            }
		}

        private IItemStack FindFirstItemNotFullStack(string id)
        {
            foreach (IInstanceItem item in _inventory)
            {
                if(item is not IItemStack stack || item.GetItemData().ID.Equals(id) || stack.Quantity == stack.MaxStack) continue;
                    
                return stack;
            }

            return null;
        }
        
        public IInstanceItem Add(string id, int quantity = 1, bool notifyChange = true)
        {
            if (id.Equals(string.Empty) || quantity < 1) return null;
            
            var itemData = Database.SearchItem(id);
            if(itemData == null) return null;
            
            bool canStack = itemData is IItemStack;
            if (!canStack) return IsFull ? null : NewItem(itemData, quantity);

            var existItemStack = FindFirstItemNotFullStack(itemData.ID);
            if (existItemStack == null) return IsFull ? null : NewItem(itemData, quantity);

            float resultQuantity = quantity + existItemStack.Quantity;
            if (resultQuantity > existItemStack.MaxStack)
            {
                existItemStack.Quantity = existItemStack.MaxStack;
                NotifyItemChange(notifyChange);
                return existItemStack as IInstanceItem;
            }
                    
            existItemStack.Quantity += quantity;
            NotifyItemChange(notifyChange);
            return existItemStack as IInstanceItem;

        }

        private void NotifyItemChange(bool value)
        {
            if(!value) return;
            
            OnInventoryChange?.Invoke();
        }
        
        private IInstanceItem NewItem(Item.ItemData itemBase, int quantity = 1, bool notifyChange = true)
        {
            var newItem = itemBase.CreateItem(quantity);
            
            if (newItem is MonoBehaviour newItemGO)
            {
                newItemGO.transform.SetParent(transform, false);
            }
            
            _inventory.Add(newItem);
            NotifyItemChange(notifyChange);
            return newItem;
        }
        
        public IInstanceItem RemoveItem(string id, int quantity = 1, bool notifyChange = true)
        {
            if(id.Equals(string.Empty) || quantity < 1) return null;
            
            for (int i = _inventory.Count - 1; i >= 0; i--)
            {
                var item = _inventory[i];
                
                if (item.GetItemData().ID != id) continue;

                if (item is not IItemStack stack)
                {
                    _inventory.RemoveAt(i);
                    NotifyItemChange(notifyChange);
                    return item;
                }
                
                if (stack.Quantity - quantity <= 0)
                {
                    _inventory.RemoveAt(i);
                    NotifyItemChange(notifyChange);
                    return item;
                }
                 
                stack.Quantity -= quantity;
                NotifyItemChange(notifyChange);
                return item;
            }

            return null;
        }

        public List<IInstanceItem> RemoveItems(string id, int quantity = 1, bool notifyChange = true)
        {
            if(id.Equals(string.Empty) || quantity < 1) return null;
            
            var removedItems = new List<IInstanceItem>();
            
            for (int i = _inventory.Count - 1; i >= 0; i--)
            {
                var item = _inventory[i];
                
                if (item.GetItemData().ID != id) continue;

                if (item is not IItemStack stack)
                {
                    _inventory.RemoveAt(i);
                    removedItems.Add(item);
                    continue;
                }
                
                if (stack.Quantity - quantity <= 0)
                {
                    _inventory.RemoveAt(i);
                    removedItems.Add(item);
                    continue;
                }
                 
                stack.Quantity -= quantity;
            }
            
            NotifyItemChange(notifyChange);
            return removedItems;
        }
        
        public void RemoveItemsOfType<T>(bool notifyChange = true) where T : IInstanceItem
        {
            _inventory.RemoveAll(x => x is T);
            NotifyItemChange(notifyChange);
        }

        public bool Has(string id)
        {
            foreach (IInstanceItem item in _inventory)
            {
                if (item.GetItemData().ID == id)
                    return true;
            }

            return false;
        }

        public List<T> GetItemsOfType<T>() where T : IInstanceItem
        {
            return _inventory.OfType<T>().ToList();
        }

        public void Clear()
        {
            _inventory.Clear();
            OnInventoryChange?.Invoke();
        }
    }
}
[Serializable]
public class StartedItem
{
    public string ID;
    public int Quantity;
}
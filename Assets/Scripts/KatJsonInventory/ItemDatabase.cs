using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KatJsonInventory.Item;
using KatLib.Singleton;
using Newtonsoft.Json;
using UnityEngine;

namespace KatJsonInventory
{
    public class ItemDatabase : Singleton<ItemDatabase>
    {
        private Dictionary<string, ItemData> _itemDictionary;
        public ReadOnlyDictionary<string, ItemData> ItemDictionary;

        protected override void Awake()
        {
            base.Awake();
            _ = Init();
        }

        public async UniTask Init(CancellationToken token = default)
        {
            const string AddressableLabel = "Items";
            var textAsset = await AddressablesManager.Instance.LoadAssetAsync<TextAsset>(AddressableLabel, token: token);
            var datas = JsonConvert.DeserializeObject<List<ItemData>>(textAsset.text);
            _itemDictionary = datas.ToDictionary(key => key.ID, value => value);
            ItemDictionary = new ReadOnlyDictionary<string, ItemData>(_itemDictionary);
            AddressablesManager.Instance.ReleaseInstance(AddressableLabel);
        }
        
        public ItemData SearchItem(string id)
        {
            return _itemDictionary.GetValueOrDefault(id);
        }
    }
}

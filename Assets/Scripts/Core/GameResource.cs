using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class GameResource<T> where T : UnityEngine.Object
{
    private static Dictionary<string, T> _resource = new Dictionary<string, T>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        _resource.Clear();
    }
    
    public static async UniTask LoadAsset(string key)
    {
        var loadedAssets = await AddressablesManager.Instance.LoadAssetsAsync<T>(key);
        
        foreach (var asset in loadedAssets)
        {
            _resource.Add(asset.name, asset);
        }
        
        AddressablesManager.Instance.Release(loadedAssets);
    }

    public static void RemoveAsset(string key)
    {
        _resource.Remove(key);
    }
    
    public static T GetAsset(string key)
    {
        return _resource.GetValueOrDefault(key);
    }
    
    public static void Clear()
    {
        _resource.Clear();
    }
}
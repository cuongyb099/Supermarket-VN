using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KatLib.Pooling
{
    public class ReturnToPool : MonoBehaviour
    {
        [NonSerialized] public IObjectPool PoolObjects;
        [NonSerialized] public Object RootComponent;
        
        public void OnDisable()
        {
            PoolObjects.AddToPool(RootComponent);
        }
    }
}
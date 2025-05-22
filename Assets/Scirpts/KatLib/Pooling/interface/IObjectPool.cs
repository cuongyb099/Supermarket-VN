using UnityEngine;

namespace KatLib.Pooling
{
    public interface IObjectPool
    {
        public Object GetFromPool(Vector3 position, Quaternion rotation);
        public void AddToPool(Object obj);
    }
}

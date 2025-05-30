using System.Collections.Generic;
using UnityEngine;

namespace KatLib.Pooling
{
    public abstract class ObjectPoolBase<T>: IObjectPool where T : Object
    {
        protected Stack<T> inActive = new Stack<T>();
        protected T baseObject;

        public ObjectPoolBase(T baseObject)
        {
            this.baseObject = baseObject;
        }
        
        public virtual Object GetFromPool(Vector3 position, Quaternion rotation)
        {
            T obj;
            if (inActive.Count > 0)
            {
                obj = inActive.Pop();
                var gameObject = GetGameobjectOfObject(obj);
                gameObject.transform.position = position;
                gameObject.transform.rotation = rotation;
                gameObject.SetActive(true);
                return obj;
            }

            ReturnToPool returnToPool = GetReturnToPool(position, rotation, out obj);
            returnToPool.PoolObjects = this;
            returnToPool.RootComponent = obj;
            return obj;
        }

        public abstract GameObject GetGameobjectOfObject(T obj);
        public abstract ReturnToPool GetReturnToPool(Vector3 position, Quaternion rotation, out T obj);
        
        public virtual void AddToPool(Object obj)
        {
            inActive.Push(obj as T);
        }
    }
}

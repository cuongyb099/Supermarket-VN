using UnityEngine;

namespace KatLib.Pooling
{
    public class GameObjectObjectPool : ObjectPoolBase<GameObject>
    {
        public GameObjectObjectPool(GameObject baseObject) : base(baseObject)
        {
            
        }

        public override GameObject GetGameobjectOfObject(GameObject obj) => obj;

        public override ReturnToPool GetReturnToPool(Vector3 position, Quaternion rotation, out GameObject obj)
        {
            obj = Object.Instantiate(this.baseObject, position, rotation);
            return obj.gameObject.AddComponent<ReturnToPool>();
        }
    }
}

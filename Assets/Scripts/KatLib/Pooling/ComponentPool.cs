using UnityEngine;

namespace KatLib.Pooling
{
    public class ComponentObjectPool : ObjectPoolBase<Component>
    {
        public ComponentObjectPool(Component baseComponent) : base(baseComponent)
        {
        }

        public override GameObject GetGameobjectOfObject(Component obj) => obj.gameObject;
        
        public override ReturnToPool GetReturnToPool(Vector3 position, Quaternion rotation, out Component obj)
        {
            obj = Object.Instantiate(this.baseObject, position, rotation);
            return obj.gameObject.AddComponent<ReturnToPool>();
        }
    }
}

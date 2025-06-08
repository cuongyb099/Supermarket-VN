using System.Collections.Generic;
using KatLib.Pooling;
using UnityEngine;

namespace KatAudio
{
    public class AudioChildPool : ObjectPoolBase<AudioChild>
    {
        public AudioChildPool(AudioChild baseObject) : base(baseObject)
        {
        }

        public override GameObject GetGameobjectOfObject(AudioChild obj) => obj.gameObject;

        public override ReturnToPool GetReturnToPool(Vector3 position, Quaternion rotation, out AudioChild obj)
        {
            obj = Object.Instantiate(this.baseObject, position, rotation);
            return obj.gameObject.AddComponent<ReturnToPool>();
        }

    }
}

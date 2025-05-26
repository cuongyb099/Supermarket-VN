using UnityEngine;

namespace Core.Interact
{
    public abstract class OutlineBase : MonoBehaviour
    {
        public abstract void EnableOutline();
        public abstract void DisableOutline();
        public bool IsEnable { get; protected set; }
        public abstract void SetConfig(OutlineConfigSO config);
    }
}

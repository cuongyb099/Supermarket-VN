using UnityEngine;

namespace Core.Interact
{
    public abstract class OutlineBase : MonoBehaviour
    {
        protected Renderer[] renderers;
        public abstract void EnableOutline();
        public abstract void DisableOutline();
        public bool IsEnable { get; protected set; }
        public abstract void SetConfig(OutlineConfigSO config);
    }
}

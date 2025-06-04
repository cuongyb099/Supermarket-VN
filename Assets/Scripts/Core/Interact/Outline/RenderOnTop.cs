using UnityEngine;

namespace Core.Interact
{
    public abstract class RenderOnTop : MonoBehaviour
    {
        public abstract void SetOnTop();
        public abstract void ReturnDefault();
    }
}

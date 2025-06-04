using UnityEngine;

namespace Core.UI
{
    public abstract class LoadingBarBase : MonoBehaviour
    {
        public abstract void SetProgress(float progress);
        public abstract void SetDescription(string description);
        public abstract void ResetProgress();
        public abstract float CurrentProgress { get; }
    }
}

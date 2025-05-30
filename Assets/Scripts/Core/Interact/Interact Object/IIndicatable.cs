using UnityEngine;

namespace Core.Interact
{
    public interface IIndicatable
    {
        public void EnableIndicator(Color color);
        public void DisableIndicator();
    }
}

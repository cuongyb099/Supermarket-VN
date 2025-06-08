using UnityEngine;

namespace Core.Scene
{
    public class Gameplay : SceneCtrlBase<Gameplay>
    {
        protected override void OnAwake()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

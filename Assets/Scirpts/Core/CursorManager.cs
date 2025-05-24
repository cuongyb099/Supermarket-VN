using KatLib.Singleton;
using UnityEngine;

namespace Core
{
    public class CursorManager : SingletonPersistent<CursorManager>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            GetInstance();
        }
        
        private void OnEnable()
        {
            LockCursor();
        }

        public void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
            
        public void UnlockCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

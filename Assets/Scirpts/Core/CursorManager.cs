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
            DisableCursor();
        }

        public void DisableCursor()
        {
            Cursor.visible = false;   
            Cursor.lockState = CursorLockMode.Locked;
        }
            
        public void EnableCursor()
        {
            Cursor.visible = true;   
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

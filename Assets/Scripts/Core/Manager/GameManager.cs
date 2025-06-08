using KatLib.Singleton;
using UnityEngine;

namespace Core.Manager
{
    public class GameManager : SingletonPersistent<GameManager>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            GetInstance();
            //Test
            //Cursor.lockState = CursorLockMode.Locked;   
        }
    }
}

using UnityEngine.SceneManagement;

namespace Core.Utilities
{
    public static class Utilities
    {
        public static bool CheckSceneIsActive(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == sceneName) return true;
            }

            return false;
        }
    }
}

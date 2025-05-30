using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Entity.NPC
{
    public class RandomStartedSkin : MonoBehaviour
    {
        [SerializeField] protected SkinnedMeshRenderer[] skins;
        public int SelectedSkinIndex { get; protected set; }
        
        private void Reset()
        {
            skins = GetComponentsInChildren<SkinnedMeshRenderer>(true);

            for (var i = 1; i < skins.Length; i++)
            {
                skins[i].gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            RandomSkin();
        }

        public void RandomSkin()
        {
            skins[SelectedSkinIndex].gameObject.SetActive(false);
            SelectedSkinIndex = Random.Range(0, skins.Length);
            skins[SelectedSkinIndex].gameObject.SetActive(true);
        }
    }
}

using UnityEngine;

namespace Core.Interact
{
    [CreateAssetMenu(menuName = "Outline Config")]
    public class OutlineConfigSO : ScriptableObject
    {
        [field: SerializeField] public float OutlineWidth { get; private set; } = 0.001f;
    }
}

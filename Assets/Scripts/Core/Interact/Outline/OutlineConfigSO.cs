using UnityEngine;

namespace Core.Interact
{
    [CreateAssetMenu(menuName = "Outline Config")]
    public class OutlineConfigSO : ScriptableObject
    {
        [field: SerializeField] public float OutlineWidth { get; private set; } = 0.001f;
        [field: SerializeField] public Color OutlineColor { get; private set; } = Color.green;
        [field: SerializeField] public OutlineFillMask.OutlineMode OutlineMode{ get; private set; }
    }
}

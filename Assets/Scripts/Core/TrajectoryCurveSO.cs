using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Trajectory Curve")]
public class TrajectoryCurveSO : ScriptableObject
{
    [field: SerializeField] public float MaxHeight { get; private set; }
    [field: SerializeField] public float Duration { get; private set; }
    [field: SerializeField] public AnimationCurve Curve { get; private set; }
    [field: SerializeField] public Ease EaseMode { get; private set; }
}

using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SplineAnimate))]
public class WalkingNPC : MonoBehaviour
{
    protected SplineAnimate splineAnimate;
    [SerializeField] protected float timeToNextWalkCycle = 0.5f;
    
    private void Awake()
    {
        splineAnimate = GetComponent<SplineAnimate>();
        var pathManager = NPCPathManager.Instance;
        var splineContainer = pathManager.GetWalkingSpline(Random.Range(0, pathManager.WalkingSplineCount));
        splineAnimate.Container = splineContainer;
        splineAnimate.enabled = true;
        splineAnimate.Updated += OnAnimated;
    }

    private void OnAnimated(Vector3 pos, Quaternion rot)
    {
        if (splineAnimate.NormalizedTime < 0.999f) return;
        
        this.gameObject.SetActive(false);
        DOVirtual.DelayedCall(timeToNextWalkCycle, () =>
        {
            this.gameObject.SetActive(true);
            splineAnimate.Restart(true);
        });
    }
}
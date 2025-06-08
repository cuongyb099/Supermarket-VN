using System.Collections.Generic;
using KatLib.Singleton;
using UnityEngine;
using UnityEngine.Splines;

public class NPCPathManager : Singleton<NPCPathManager>
{
    [SerializeField] private List<SplineContainer> _walkingSpline;
    
    public int WalkingSplineCount => _walkingSpline.Count;
    public SplineContainer GetWalkingSpline(int index) => _walkingSpline[index];
}
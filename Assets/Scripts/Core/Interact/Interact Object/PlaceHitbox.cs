using UnityEngine;

namespace Core.Interact
{
    public class PlaceHitbox : MonoBehaviour
    {
        public Vector3 Size;

#if UNITY_EDITOR
        
        public bool DebugMode;
        
        private void OnDrawGizmos()
        {
            if(!DebugMode) return;
            
            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, this.Size);
        }
#endif
    }
}

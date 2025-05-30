using UnityEngine;

namespace Core.Interact
{
    public class PlaceHitbox : MonoBehaviour
    {
        public Vector3 Size;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, this.Size);
        }
    }
}

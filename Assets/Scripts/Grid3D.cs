using UnityEngine;

public class Grid3D : MonoBehaviour
{
    public Vector3Int Size;
    public Vector3 CellSize;
    
    public bool CellToWorldPosition(Vector3Int cellPosition, out Vector3 result)
    {
        result = Vector3.zero;
        
        if(!IsInvalidCell(cellPosition)) return false;
        
        result = transform.position 
                + transform.forward * cellPosition.z * CellSize.z
                + transform.up * cellPosition.y * CellSize.y 
                + transform.right * cellPosition.x * CellSize.x;
        
        return true;
    }
    
    public bool IsInvalidCell(Vector3Int cellPosition)
    {
        return !(cellPosition.x >= Size.x) && !(cellPosition.y >= Size.y) && !(cellPosition.z >= Size.z);
    }
    
#if UNITY_EDITOR
    public float CellDrawRadius = 0.1f;
#endif
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                for (int k = 0; k < Size.z; k++)
                {
                    Gizmos.DrawSphere(Vector3.zero + new Vector3(i * CellSize.x, j * CellSize.y, k * CellSize.z), CellDrawRadius);;
                }
            }
        }
    }
}
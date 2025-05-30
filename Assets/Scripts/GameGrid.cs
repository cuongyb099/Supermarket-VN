using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public Vector3 gridSize;
    public Vector3 cellSize;
    
    public Vector3 GetPosition(int cellX, int cellY, int cellZ)
    {
        return transform.position + new Vector3(cellX * cellSize.x, cellY * cellSize.y, cellZ * cellSize.z);
    }

#if UNITY_EDITOR
    public float CellDrawRadius = 0.1f;
#endif
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                for (int k = 0; k < gridSize.z; k++)
                {
                    Gizmos.DrawSphere(transform.position + new Vector3(i * cellSize.x, j * cellSize.y, k * cellSize.z), CellDrawRadius);;
                }
            }
        }
    }
}
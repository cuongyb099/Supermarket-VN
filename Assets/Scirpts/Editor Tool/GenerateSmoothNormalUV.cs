#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GenerateSmoothNormalUV : EditorWindow
{
    [MenuItem("Editor Tool/GenerateSmoothNormalUV")]
    private static void ShowWindow()
    {
        var window = GetWindow<GenerateSmoothNormalUV>();
        window.titleContent = new GUIContent("Generate Smooth Normal UV");
        window.Show();
    }
    
    private List<MeshFilter> _selectedMeshfilter = new List<MeshFilter>();
    private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();
    private List<Mesh> bakeKeys = new List<Mesh>();
    private List<List<Vector3>> bakeValues = new List<List<Vector3>>();
    private static int uv = 3;
    private string uvInput = "3";

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("UV Overide: ", GUILayout.ExpandWidth(false));
        uvInput = GUILayout.TextField(uvInput);
        GUILayout.EndHorizontal();

        if (int.TryParse(uvInput, out int parsedValue) && parsedValue > 0)
        {
          uv = parsedValue;
        }
        else
        {
          uv = 0;
          uvInput = "0";
        }

        
        foreach (var gameObject in Selection.gameObjects)
        {
            _selectedMeshfilter.AddRange(gameObject.GetComponentsInChildren<MeshFilter>());
        }

        GUILayout.Label("Count: " + _selectedMeshfilter.Count);;
        
        if (GUILayout.Button("Generate"))
        {
            Bake(_selectedMeshfilter);
        }
        
        if (GUILayout.Button("Clear"))
        {
            Clear();
        }
        
        _selectedMeshfilter.Clear();
    }

    public static void Bake(List<MeshFilter> meshFilters) {
        foreach (var meshFilter in meshFilters) {
            var smoothNormals = SmoothNormals(meshFilter.sharedMesh);
            meshFilter.sharedMesh.SetUVs(uv, smoothNormals);
            CombineSubmeshes(meshFilter.sharedMesh, meshFilter.GetComponent<MeshRenderer>().sharedMaterials);
        }
    }
    
    void Clear()
    {
        foreach (var meshFilter in _selectedMeshfilter)
        {
          meshFilter.sharedMesh.SetUVs(uv, new List<Vector2>());
        }
    }

  private static List<Vector3> SmoothNormals(Mesh mesh) {

    // Group vertices by location
    var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

    // Copy normals to a new list
    var smoothNormals = new List<Vector3>(mesh.normals);

    // Average normals for grouped vertices
    foreach (var group in groups) {

      // Skip single vertices
      if (group.Count() == 1) {
        continue;
      }

      // Calculate the average normal
      var smoothNormal = Vector3.zero;

      foreach (var pair in group) {
        smoothNormal += smoothNormals[pair.Value];
      }

      smoothNormal.Normalize();

      // Assign smooth normal to each vertex
      foreach (var pair in group) {
        smoothNormals[pair.Value] = smoothNormal;
      }
    }

    return smoothNormals;
  }

  private static void CombineSubmeshes(Mesh mesh, Material[] materials) {
    // Skip meshes with a single submesh
    if (mesh.subMeshCount == 1) {
      return;
    }

    // Skip if submesh count exceeds material count
    if (mesh.subMeshCount > materials.Length) {
      return;
    }

    // Append combined submesh
    mesh.subMeshCount++;
    mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
  }
}
#endif
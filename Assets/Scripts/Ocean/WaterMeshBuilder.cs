using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WaterMeshBuilder : MonoBehaviour
{
    [SerializeField] private float size = 160f;
    [SerializeField] private float cellSize = 0.8f;

    private void Awake() => Build();

    private void Build()
    {
        int cells = Mathf.CeilToInt(size / cellSize);
        int vertsPerSide = cells + 1;

        var vertices = new Vector3[vertsPerSide * vertsPerSide];
        float half = size * 0.5f;

        for (int z = 0; z < vertsPerSide; z++)
        for (int x = 0; x < vertsPerSide; x++)
            vertices[z * vertsPerSide + x] =
                new Vector3(x * cellSize - half, 0f, z * cellSize - half);

        var triangles = new int[cells * cells * 6];
        int t = 0;
        for (int z = 0; z < cells; z++)
        for (int x = 0; x < cells; x++)
        {
            int i = z * vertsPerSide + x;
            triangles[t++] = i;
            triangles[t++] = i + vertsPerSide;
            triangles[t++] = i + 1;

            triangles[t++] = i + 1;
            triangles[t++] = i + vertsPerSide;
            triangles[t++] = i + vertsPerSide + 1;
        }

        var mesh = new Mesh { indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
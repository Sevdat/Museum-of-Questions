using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfSphere : MonoBehaviour
{
    int segments = 32; // Number of segments to approximate the half-sphere
    float radius = 10f;

    void CreateHalfSphere()
    {
        // Create a new GameObject with the name "HalfSphere"
        GameObject halfSphere = new GameObject("HalfSphere");

        // Add MeshFilter and MeshRenderer components to the new GameObject
        MeshFilter meshFilter = halfSphere.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = halfSphere.AddComponent<MeshRenderer>();

        // Create the mesh
        Mesh mesh = new Mesh();

        // Calculate the number of vertices and triangles
        int numVertices = (segments + 1) * (segments + 1);
        int numTriangles = segments * segments * 6;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numTriangles];
        Vector2[] uv = new Vector2[numVertices];

        // Generate vertices and UVs
        int vertexIndex = 0;
        for (int y = 0; y <= segments; y++)
        {
            float v = (float)y / segments;
            float elevation = v * Mathf.PI; // Half-sphere (0 to 90 degrees)

            for (int x = 0; x <= segments; x++)
            {
                float u = (float)x / segments;
                float azimuth = u * Mathf.PI * 2; // Full circle (0 to 360 degrees)

                // Calculate vertex position
                vertices[vertexIndex] = new Vector3(
                    Mathf.Sin(elevation) * Mathf.Cos(azimuth) * radius,
                    Mathf.Cos(elevation) * radius,
                    Mathf.Sin(elevation) * Mathf.Sin(azimuth) * radius
                );
                // UV mapping
                uv[vertexIndex] = new Vector2(u, v);

                vertexIndex++;
            }
        }

        // Generate triangles
        int triangleIndex = 0;
        for (int y = 0; y < segments; y++)
        {
            for (int x = 0; x < segments; x++)
            {
                int current = y * (segments + 1) + x;
                int next = current + segments + 1;

                // First triangle
                triangles[triangleIndex++] = current;
                triangles[triangleIndex++] = next;
                triangles[triangleIndex++] = current + 1;

                // Second triangle
                triangles[triangleIndex++] = next;
                triangles[triangleIndex++] = next + 1;
                triangles[triangleIndex++] = current + 1;
            }
        }

        // Assign vertices, triangles, and UVs to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        // Assign the mesh to the MeshFilter
        meshFilter.mesh = mesh;

        // Optionally, assign a default material to the MeshRenderer
        meshRenderer.material = new Material(Resources.Load<Material>("globe"));
    }
    void Start() {
        CreateHalfSphere();
    }
}

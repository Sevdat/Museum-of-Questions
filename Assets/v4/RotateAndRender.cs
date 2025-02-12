using UnityEngine;

public class RenderFromGPUWithTriangles : MonoBehaviour
{
    public ComputeShader computeShader;
    public Material material; // Material using a standard shader

    private ComputeBuffer verticesBuffer;
    private Mesh mesh;
    private int vertexCount = 4; // Example: Quad mesh

    void Start()
    {
        // Step 1: Initialize vertex data
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-0.5f, -0.5f, 0), // Bottom-left
            new Vector3(0.5f, -0.5f, 0),  // Bottom-right
            new Vector3(-0.5f, 0.5f, 0),  // Top-left
            new Vector3(0.5f, 0.5f, 0)    // Top-right
        };

        // Step 2: Create a Compute Buffer for vertices
        verticesBuffer = new ComputeBuffer(vertexCount, sizeof(float) * 3);
        verticesBuffer.SetData(vertices);

        // Step 3: Create a mesh and define triangle connectivity
        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = new int[] { 0, 2, 1, 1, 2, 3 }; // Quad triangles
        mesh.RecalculateNormals();

        // Step 4: Assign the mesh to a MeshFilter
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        // Step 5: Assign the material to a MeshRenderer
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
    }

    void Update()
    {
        // Step 6: Dispatch the Compute Shader to rotate vertices
        int kernel = computeShader.FindKernel("RotateVertices");
        computeShader.SetBuffer(kernel, "Vertices", verticesBuffer);
        computeShader.SetFloat("Angle", Time.time * 0.5f); // Rotate over time
        computeShader.Dispatch(kernel, Mathf.CeilToInt(vertexCount / 64f), 1, 1);

        // Step 7: Update the mesh with the modified vertices
        Vector3[] updatedVertices = new Vector3[vertexCount];
        verticesBuffer.GetData(updatedVertices);
        mesh.vertices = updatedVertices;
        mesh.RecalculateNormals();
    }

    void OnDestroy()
    {
        // Step 8: Release the Compute Buffer
        verticesBuffer.Release();
    }
}
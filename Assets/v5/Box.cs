using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Box : MonoBehaviour
{
    void createCollider(GameObject gameObject,float width, float height, float length,Vector3 origin){
        BoxCollider boxColliderUp = gameObject.AddComponent<BoxCollider>();
        boxColliderUp.size = new Vector3(width, height, length);
        boxColliderUp.center = origin; // Offset the center
    }
    void createColliders(GameObject gameObject,float width, float height, float length,Vector3 origin){
        createCollider(gameObject,width*2,1,length*2,origin + new Vector3(0,height+0.5f,0));
        createCollider(gameObject,width*2,1,length*2,origin - new Vector3(0,height+0.5f,0));

        createCollider(gameObject,1,height*2,length*2,origin + new Vector3(width+0.5f,0,0));
        createCollider(gameObject,1,height*2,length*2,origin - new Vector3(width+0.5f,0,0));

        createCollider(gameObject,width*2,height*2,1,origin + new Vector3(0,0,length+0.5f));
        createCollider(gameObject,width*2,height*2,1,origin - new Vector3(0,0,length+0.5f));
    }
    void createBox(float width, float height, float length, Vector3 origin){
            // Create a new GameObject with the name "HalfSphere"
            GameObject box = new GameObject("Box");
            
            Mesh mesh = new Mesh();
            // Add MeshFilter and MeshRenderer components to the new GameObject
            MeshFilter meshFilter = box.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = box.AddComponent<MeshRenderer>();

            createColliders(box,width,height,length,origin);

            Vector3[] vertices = new Vector3[]
            {
                // Front face
                new Vector3(-width, -height, -length) + origin,
                new Vector3(width, -height, -length)+ origin,
                new Vector3(width, height, -length)+ origin,
                new Vector3(-width, height, -length)+ origin,

                // Back face
                new Vector3(-width, -height, length)+ origin,
                new Vector3(width, -height, length)+ origin,
                new Vector3(width, height, length)+ origin,
                new Vector3(-width, height, length)+ origin
            };
            int[] triangles = new int[]
            {
                // Front face
                2, 0, 1,
                3, 0, 2,

                // Back face
                6, 5, 4,
                7, 6, 4,

                // Left face
                3, 4, 0,
                7, 4, 3,

                // Right face
                2, 1, 5,
                6, 2, 5,

                // Top face
                3, 2, 6,
                7, 3, 6,

                // Bottom face
                1, 0, 4,
                5, 1, 4
            };

            // Assign vertices, triangles, and UVs to the mesh
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            // Assign the mesh to the MeshFilter
            meshFilter.mesh = mesh;

            // Optionally, assign a default material to the MeshRenderer
            meshRenderer.material = new Material(Resources.Load<Material>("globe"));
    }

    void Start()
    {
        createBox(30,10,30,new Vector3(0,10,0));
    }
}

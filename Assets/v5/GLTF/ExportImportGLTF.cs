using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityGLTF;

public class ExportImportGLTF : MonoBehaviour
{
    FolderPaths folderPaths;
    // Start is called before the first frame update
    void Start()
    {
        folderPaths = transform.GetComponent<FolderPaths>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)){
            ImportGLTF("ExportedModel/ExportedModel.gltf");
        }
        if (Input.GetKeyDown(KeyCode.K)){
            StartCoroutine(ExportGameObject(folderPaths.currentMap,"ExportedModel"));
        }
    }

    internal IEnumerator ExportGameObject(GameObject targetObject,string relativeFolderPath){
        // Load the GameObject from Resources
        if (targetObject == null){
            Debug.LogError("Target GameObject is not assigned.");
            yield break;
        }
        // Create a GLTFSceneExporter instance
        GLTFSceneExporter exporter = new GLTFSceneExporter(
            new[] { targetObject.transform }, // Array of root transforms to export
            new ExportContext()
        );
        // Export to GLB file
        string exportPath = $"{Application.persistentDataPath}/{relativeFolderPath}";
        exporter.SaveGLTFandBin(exportPath, Path.GetFileName(exportPath));
        Debug.Log($"GameObject exported to {exportPath}");
    }

    internal async void ImportGLTF(string relativeFilePathToGLTF){
        // Create an instance of GLTFSceneImporter
        GLTFSceneImporter sceneImporter = new GLTFSceneImporter(
            $"{Application.persistentDataPath}/{relativeFilePathToGLTF}", // Path to the .gltf file
            new ImportOptions()
        );
        // Load the GLTF scene asynchronously
        try {
            await sceneImporter.LoadNodeAsync(0,new System.Threading.CancellationToken());
            Debug.Log("GLTF file imported successfully!");

            // Get the root GameObject of the imported scene
            folderPaths.currentMap = sceneImporter.CreatedObject;
            AddMeshCollidersToHierarchy(folderPaths.currentMap);
        }
        catch (System.Exception ex){
            Debug.LogError($"Failed to import GLTF file: {ex.Message}");
        }
    }

    void AddMeshCollidersToHierarchy(GameObject root){
        // Check if the current GameObject has a MeshFilter
        MeshFilter meshFilter = root.GetComponent<MeshFilter>();
        if (meshFilter != null){
            // Add a MeshCollider if it doesn't already have one
            if (root.GetComponent<MeshCollider>() == null){
                MeshCollider meshCollider = root.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = meshFilter.sharedMesh;
            }
        }

        // Recursively process all children
        foreach (Transform child in root.transform){
            AddMeshCollidersToHierarchy(child.gameObject);
        }
    }
    
}

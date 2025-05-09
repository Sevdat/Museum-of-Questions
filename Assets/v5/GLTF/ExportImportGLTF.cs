using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityGLTF;

public class ExportImportGLTF : MonoBehaviour
{
    Main folderPaths;
    GameObject exp;
    string str;
    // Start is called before the first frame update
    void Start()
    {
        folderPaths = transform.GetComponent<Main>();
    }
 
    // Update is called once per frame
    void Update()
    {

    }

    internal IEnumerator ExportGameObject(GameObject targetObject,string exportPath, bool unloadLoadedGameObject = false){
        print("targetObject");
        // Load the GameObject from Resources
        if (targetObject == null){
            print("Target GameObject is not assigned.");
            yield break;
        }
        
        // Create a GLTFSceneExporter instance
        GLTFSceneExporter exporter = new GLTFSceneExporter(
            new[] { targetObject.transform }, // Array of root transforms to export
            new ExportContext()
        );
        // Export to GLB file
        exporter.SaveGLTFandBin(exportPath, Path.GetFileName(exportPath));
        print($"GameObject exported to {exportPath}");
        // if (unloadLoadedGameObject) Resources.UnloadAsset(targetObject);
    }

    internal async Task<GameObject> ImportGLTF(string importPath, int index){
        // Create an instance of GLTFSceneImporter
        GLTFSceneImporter sceneImporter = new GLTFSceneImporter(
            importPath, // Path to the .gltf file
            new ImportOptions()
        );
        // Load the GLTF scene asynchronously
        try {
            await sceneImporter.LoadNodeAsync(index,new System.Threading.CancellationToken());
            // folderPaths.currentMap.transform.position = folderPaths.editPrefab.placeInfrontOfPlayer(1,2,4);
            AddMeshCollidersToHierarchy(sceneImporter.CreatedObject);

            return sceneImporter.CreatedObject;
        }
        catch (System.Exception ex){
            print($"Failed to import GLTF file: {ex.Message}");
        }
        return null;
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

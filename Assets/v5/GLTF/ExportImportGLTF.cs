using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityGLTF;

public class ExportImportGLTF : MonoBehaviour
{
    Main folderPaths;
    GameObject exp;
    string str;
    // Start is called before the first frame update
    //BOKI  (hayq Art) Karboosx
    // terrain isn't supported, KHR_materials_pbrSpecularGlossiness has been deprecated  older specular-glossiness PBR workflow
    void Start()
    {
        folderPaths = transform.GetComponent<Main>();
        exp = Resources.Load<GameObject>("OutPostOnDesert");
        Resources.UnloadAsset(exp);
        
        // // StartCoroutine(ExportGameObject(exp, $"{Application.persistentDataPath}/Expertod"));

        // str = Application.dataPath+"/Resources/GeneratedAssets/BERCEST STUDIO/LOW POLY MEDIEVAL SHIP/Prefab/Content/Vegetation Pack 2_URP Scene/Vegetation Pack 2_URP Scene.gltf";
        // string str1 = Application.dataPath+"/Resources/GeneratedAssets/LineAcroos/Environment Forest/Prefab/Content/EnvironmentForestScene/EnvironmentForestScene.gltf";
        // // ImportGLTF(str);
        // // print(Application.persistentDataPath + "/Expertod");
        // print(str);
    }
 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)){
            ImportGLTF($"{Application.persistentDataPath}/ExportedModel/ExportedModel.gltf");
        }
        if (Input.GetKeyDown(KeyCode.K)){
            StartCoroutine(ExportGameObject(exp,$"{Application.persistentDataPath}/ExportedModel"));
        }
    }

    internal IEnumerator ExportGameObject(GameObject targetObject,string exportPath, bool unloadLoadedGameObject = false){
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

    internal async void ImportGLTF(string importPath){
        // Create an instance of GLTFSceneImporter
        GLTFSceneImporter sceneImporter = new GLTFSceneImporter(
            importPath, // Path to the .gltf file
            new ImportOptions()
        );
        // Load the GLTF scene asynchronously
        try {
            await sceneImporter.LoadNodeAsync(0,new System.Threading.CancellationToken());
            print("GLTF file imported successfully!");

            // Get the root GameObject of the imported scene
            folderPaths.currentMap = sceneImporter.CreatedObject;
            folderPaths.currentMap.transform.position = folderPaths.editPrefab.placeInfrontOfPlayer(1,2,4);
            AddMeshCollidersToHierarchy(folderPaths.currentMap);

        }
        catch (System.Exception ex){
            print($"Failed to import GLTF file: {ex.Message}");
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

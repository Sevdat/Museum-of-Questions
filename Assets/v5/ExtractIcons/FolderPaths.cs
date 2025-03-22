using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGLTF;


public class FolderPaths : MonoBehaviour
{

    public GameObject follow, player;

    internal string currentDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    internal LoadPrefabScript loadPrefabScript;
    internal EditPrefab editPrefab;

    internal AnimateCharacter animateCharacter;
    internal RotateCameraFollow rotateCameraFollow;
    internal TerminalScript terminalScript;
    
    internal GameObject entrencePortal;

    internal GameObject paths;
    internal GameObject rootPortalPrefab,rootIconPrefab;
    internal GameObject currentMap;
    GameObject targetObject;
    
    public void ExportGameObject(){
        // Load the GameObject from Resources
        GameObject targetObject = Instantiate(Resources.Load<GameObject>("ImportedScenes/_Barking_Dog/Scene/Test_Map 1"));
        
        if (targetObject == null)
        {
            Debug.LogError("Target GameObject is not assigned.");
            return;
        }

        // Create a GLTFSceneExporter instance
        var exporter = new GLTFSceneExporter(
            new[] { targetObject.transform }, // Array of root transforms to export
            new ExportContext()
        );

        // Export to GLB file
        string exportPath = Path.Combine(Application.persistentDataPath, "ExportedModel");
        exporter.SaveGLTFandBin(exportPath, Path.GetFileName(exportPath));
        Debug.Log($"GameObject exported to {exportPath}");
    }

    async void ImportGLTF(){
        // Create an instance of GLTFSceneImporter
        var sceneImporter = new GLTFSceneImporter(
            @$"{Application.persistentDataPath}/ExportedModel/ExportedModel.gltf", // Path to the .gltf file
            new ImportOptions()
        );
        // Load the GLTF scene asynchronously
        try
        {
            await sceneImporter.LoadSceneAsync();
            Debug.Log("GLTF file imported successfully!");

            // Get the root GameObject of the imported scene
            targetObject = sceneImporter.LastLoadedScene;
            AddMeshCollidersToHierarchy(targetObject);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to import GLTF file: {ex.Message}");
        }
    }

    public void AddMeshCollidersToHierarchy(GameObject root){
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
    
    void Awake(){
        init();
        ExportGameObject();
        ImportGLTF();
    }
    public void createPaths(){
        // StartCoroutine(loadPrefabScript.LoadAndInstantiatePrefab(Vector3.zero));
        paths = new GameObject("Paths");
        GameObject folder = new GameObject("Folder");
        GameObject file = new GameObject("File");

        folder.transform.SetParent(paths.transform);
        file.transform.SetParent(paths.transform);


    }
    // Start is called before the first frame update
    void Start(){
        createPaths();
        
        // LoadSceneAdditively("Assets/Resources/ImportedScenes/AltonGames/Demo 1.unity");
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.T)){
            currentDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
        if (Input.GetKeyDown(KeyCode.Space)){
            editPrefab.ray();
        }
    }

    public void init(){
        loadPrefabScript = transform.AddComponent<LoadPrefabScript>();
        editPrefab = transform.AddComponent<EditPrefab>();
        rotateCameraFollow = follow.AddComponent<RotateCameraFollow>();
        animateCharacter = player.AddComponent<AnimateCharacter>();
        terminalScript = transform.AddComponent<TerminalScript>();
        rotateCameraFollow.player = player;
        initiateRootPrefab(ref rootPortalPrefab, "Scenes/Portal blue Variant 1");
        initiateRootPrefab(ref rootIconPrefab, "Scenes/Icon");
        getFiles();
        getFolders();
    }
    public void initiateRootPrefab(ref GameObject prefab, string path){
        string iconPrefabPath = path;
        prefab = Resources.Load<GameObject>(iconPrefabPath);
    }
    public string[] getDrives(){
        string[] drives = Environment.GetLogicalDrives();
        return drives;
    }
    public string[] getFiles(){
        editPrefab.files = Directory.GetFiles(currentDirectoryPath);
        return editPrefab.files;
    }
    public string[] getFolders(){
        editPrefab.folders = Directory.GetDirectories(currentDirectoryPath);
        return editPrefab.folders;
    }
    public void specialFolderPaths(){
        Environment.SpecialFolder[] folderNames = 
            (Environment.SpecialFolder[])Enum.GetValues(typeof(Environment.SpecialFolder));
        foreach (Environment.SpecialFolder folderName in folderNames){
            string fullPath = Environment.GetFolderPath(folderName);
            if (fullPath.Length>0){
                string[] strArray = fullPath.Split("\\");
                string name = strArray[strArray.Length-1];
            }
        }
    }
}

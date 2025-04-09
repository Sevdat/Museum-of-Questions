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
    internal GeneratedAssets loadPrefabScript;
    internal EditPrefab editPrefab;

    internal AnimateCharacter animateCharacter;
    internal RotateCameraFollow rotateCameraFollow;
    internal ExportImportGLTF exportImportGLTF;
    internal OrginizePaths orginizePaths;
    
    internal GameObject entrencePortal;

    internal GameObject paths;
    internal GameObject rootPortalPrefab,rootIconPrefab;
    internal GameObject currentMap;
    
    public bool assetTerminalActive = false;
    public GameObject assetTerminalGameObject;
    internal AssetTerminal assetTerminal;

    public bool terminalActive = false;
    public GameObject terminal;
    internal TerminalScript terminalScript;
    
    void Awake(){
        init();
        // currentMap = Instantiate(Resources.Load<GameObject>("GeneratedAssets/_Barking_Dog/3D Free Modular Kit/Prefab/Door_Left_01"));
        // ImportGLTF();
        // ExportGameObject(currentMap);
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
        loadPrefabScript = transform.AddComponent<GeneratedAssets>();
        editPrefab = transform.AddComponent<EditPrefab>();
        rotateCameraFollow = follow.AddComponent<RotateCameraFollow>();
        animateCharacter = player.AddComponent<AnimateCharacter>();
        exportImportGLTF = transform.AddComponent<ExportImportGLTF>();
        orginizePaths = transform.AddComponent<OrginizePaths>();
        rotateCameraFollow.player = player;
        initiateRootPrefab(ref rootPortalPrefab, "Scenes/Portal blue Variant 1");
        initiateRootPrefab(ref rootIconPrefab, "Scenes/Icon");
        getFiles();
        getFolders();
        // assetTerminalGameObject = Instantiate(assetTerminalGameObject);
        // assetTerminal = assetTerminalGameObject.GetComponent<AssetTerminal>();
        // assetTerminal.folderPaths = this;
        terminal = Instantiate(terminal);
        terminalScript = terminal.GetComponent<TerminalScript>();
        terminalScript.folderPaths = this;
        
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
    internal IEnumerator LoadAndInstantiatePrefab(string path,GameObject destroyPortal,Vector3 vec){
        // Load the prefab from the Resources folder
        ResourceRequest request = Resources.LoadAsync<GameObject>(path);
        // Wait until the prefab is fully loaded
        yield return request;
        // Check if the prefab was loaded successfully
        if (request.asset == null){
            yield break;
        }
        // Instantiate the prefab into the scene
        GameObject root = request.asset as GameObject;
        if (root != null){
            currentMap = Instantiate(root, vec, Quaternion.identity);
            Destroy(destroyPortal);
        }
    }
    internal IEnumerator LoadAndInstantiatePrefab(string path,Vector3 vec){
        // Load the prefab from the Resources folder
        ResourceRequest request = Resources.LoadAsync<GameObject>(path);
        // Wait until the prefab is fully loaded
        yield return request;
        // Check if the prefab was loaded successfully
        if (request.asset == null){
            yield break;
        }
        // Instantiate the prefab into the scene
        GameObject root = request.asset as GameObject;
        if (root != null){
            currentMap = Instantiate(root, vec, Quaternion.identity);
        }
    }
    internal IEnumerator DeleteMapPrefab(GameObject prefab){
        // Destroy the last object
        Destroy(prefab);
        // Yield to the next frame to spread out the workload
        yield return null;
    }
}

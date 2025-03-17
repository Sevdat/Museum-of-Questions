using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

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

    void Awake(){
        init();
    }
    public void createPaths(){
        StartCoroutine(loadPrefabScript.LoadAndInstantiatePrefab(Vector3.zero));
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
        loadPrefabScript = transform.AddComponent<LoadPrefabScript>();
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

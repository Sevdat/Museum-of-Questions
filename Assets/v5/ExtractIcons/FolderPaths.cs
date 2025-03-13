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
    internal string previousDirectoryPath = "";
    internal string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    internal LoadPrefabScript loadPrefabScript;
    
    GameObject paths;
    GameObject rootPortalPrefab,rootIconPrefab;
    internal GameObject currentMap;

    void Awake(){
        loadPrefabScript = new LoadPrefabScript(this);
        initiatePrefab(ref rootPortalPrefab, "Scenes/Portal blue Variant 1");
        initiatePrefab(ref rootIconPrefab, "Scenes/Icon");
    }
    // Start is called before the first frame update
    void Start(){
        paths = new GameObject("Paths");
        GameObject folder = new GameObject("Folder");
        GameObject file = new GameObject("File");

        folder.transform.SetParent(paths.transform);
        file.transform.SetParent(paths.transform);
        
        createPortal(getFolders()[portalName], new Vector3(3,2,3));
        createIcon(getFiles()[iconName], new Vector3(0,2,3));
        
        // LoadSceneAdditively("Assets/Resources/ImportedScenes/AltonGames/Demo 1.unity");

    }
    int portalName = 0;
    int iconName = 0;
    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.E)){
            getPortal();
        }
        if (Input.GetKeyDown(KeyCode.Q)){
            getIcon();
        }
        if (Input.GetKeyDown(KeyCode.C)){
            getPortal();
            getIcon();
            paths.transform.GetChild(0).GetChild(0).transform.position = player().transform.position + player().transform.forward*4 + player().transform.right*2 +player().transform.up*2;
            paths.transform.GetChild(1).GetChild(0).transform.position = player().transform.position + player().transform.forward*4 + player().transform.right*-2+player().transform.up*2;
        }
        if (Input.GetKeyDown(KeyCode.T)){
            directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
    }

    public void getPortal(){
        string[] folders = getFolders();
        if (folders.Length>0){
            portalName++;
            if (portalName>=folders.Length) portalName = 0;
            paths.transform.GetChild(0).GetChild(0).GetComponent<PortalGameObject>().rename(folders[portalName]); 
        } else {
            paths.transform.GetChild(0).GetChild(0).GetComponent<PortalGameObject>().rename(""); 
        }
    }
    public void getIcon(){
        string[] files = getFiles();
        if (files.Length>0){
            iconName++;
            if (iconName>=files.Length) iconName = 0;
            paths.transform.GetChild(1).GetChild(0).GetComponent<IconGameObject>().rename(files[iconName]); 
        } else {
            paths.transform.GetChild(1).GetChild(0).GetComponent<IconGameObject>().rename("");
        }
    }

    public string[] getDrives(){
        string[] drives = Environment.GetLogicalDrives();
        return drives;
    }
    public string[] getFiles(){
        string[] files = Directory.GetFiles(directoryPath);
        return files;
    }
    public string[] getFolders(){
        string[] folders = Directory.GetDirectories(directoryPath);
        return folders;
    }
    public void specialFolderPaths(){
        Environment.SpecialFolder[] folderNames = 
            (Environment.SpecialFolder[])Enum.GetValues(typeof(Environment.SpecialFolder));
        foreach (Environment.SpecialFolder folderName in folderNames){
            string fullPath = Environment.GetFolderPath(folderName);
            if (fullPath.Length>0){
                string[] strArray = fullPath.Split("\\");
                string name = strArray[strArray.Length-1];
                print($"{fullPath} {name}");
            }
        }
    }

    public GameObject player(){
        return transform.GetChild(2).gameObject;
    }
    public void initiatePrefab(ref GameObject prefab, string path){
        string iconPrefabPath = path;
        prefab = Resources.Load<GameObject>(iconPrefabPath);
    }
    public void createPortal(string path, Vector3 vec){
        GameObject portalGameObject = Instantiate(rootPortalPrefab, vec, transform.rotation);
        portalGameObject.GetComponent<PortalGameObject>().init(paths.transform.GetChild(0).gameObject,portalGameObject,player(),path);
    }
    public void createIcon(string path, Vector3 vec){
        GameObject iconGameObject = Instantiate(rootIconPrefab, vec, transform.rotation);
        iconGameObject.GetComponent<IconGameObject>().init(paths.transform.GetChild(1).gameObject,iconGameObject,player(),path);
    }
}

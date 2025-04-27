using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGLTF;


public class Main : MonoBehaviour
{

    public GameObject follow, player;

    internal GeneratedAssets generatedAssets;
    internal EditPrefab editPrefab;

    internal AnimateCharacter animateCharacter;
    internal RotateCameraFollow rotateCameraFollow;
    internal ExportImportGLTF exportImportGLTF;
    internal OrginizePaths orginizePaths;

    internal GameObject rootPortalPrefab,rootIconPrefab;

    public GameObject defaultMapRoot;
    internal GameObject currentMap;
    
    public GameObject assetTerminalGameObject;
    internal AssetTerminal assetTerminal;

    public GameObject terminalGameObject;
    internal TerminalScript terminalScript;

    public GameObject menuGameObject;
    internal Menu menu;

    internal bool allMenuDisabled = false;
    
    void Awake(){
        init();
        // currentMap = Instantiate(Resources.Load<GameObject>("GeneratedAssets/_Barking_Dog/3D Free Modular Kit/Prefab/Door_Left_01"));
        // ImportGLTF();
        // ExportGameObject(currentMap);
    }

    // Start is called before the first frame update
    void Start(){
    }

    // Update is called once per frame
    void Update(){
        allMenuDisabled = !terminalGameObject.activeSelf && !assetTerminalGameObject.activeSelf && !menuGameObject.activeSelf;
        if (Input.GetKeyDown(KeyCode.Escape)){
            menuGameObject.SetActive(!menu.gameObject.activeSelf);
            terminalGameObject.SetActive(false);
            assetTerminalGameObject.SetActive(false);
        }
    }

    public void init(){
        currentMap = Instantiate(defaultMapRoot);
        generatedAssets = transform.AddComponent<GeneratedAssets>();
        editPrefab = transform.AddComponent<EditPrefab>();
        rotateCameraFollow = follow.AddComponent<RotateCameraFollow>();
        animateCharacter = player.AddComponent<AnimateCharacter>();
        exportImportGLTF = transform.AddComponent<ExportImportGLTF>();
        orginizePaths = transform.AddComponent<OrginizePaths>();
        rotateCameraFollow.player = player;
        initiateRootPrefab(ref rootPortalPrefab, "Scenes/Portal blue Variant 1");
        initiateRootPrefab(ref rootIconPrefab, "Scenes/Icon");
        rotateCameraFollow.folderPaths = this;
        animateCharacter.folderPaths = this;

        assetTerminalGameObject = Instantiate(assetTerminalGameObject);
        assetTerminal = assetTerminalGameObject.GetComponent<AssetTerminal>();
        assetTerminal.main = this;
        terminalGameObject.SetActive(false);

        terminalGameObject = Instantiate(terminalGameObject);
        terminalScript = terminalGameObject.GetComponent<TerminalScript>();
        terminalScript.folderPaths = this;
        assetTerminalGameObject.SetActive(false);

        menuGameObject = Instantiate(menuGameObject);
        menu = menuGameObject.GetComponent<Menu>();
        menu.main = this;
    }
    public void initiateRootPrefab(ref GameObject prefab, string path){
        string iconPrefabPath = path;
        prefab = Resources.Load<GameObject>(iconPrefabPath);
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

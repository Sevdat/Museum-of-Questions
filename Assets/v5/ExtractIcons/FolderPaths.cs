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
    
    public GameObject portalPrefab,iconPrefab;

    public GameObject defaultMapRoot;
    internal GameObject currentMap;

    public GameObject menuGameObject;
    internal Menu menu;
    
    public GameObject assetTerminalGameObject;
    internal AssetTerminal assetTerminal;

    public GameObject terminalGameObject;
    internal TerminalScript terminalScript;

    public GameObject textBoxTerminalGameObject;
    internal TextBoxTerminal textBoxTerminal;

    internal bool allMenuDisabled = false;
    
    void Awake(){
        init();
        print(KeyCode.A.ToString());
    }

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        allMenuDisabled = !menuGameObject.activeSelf && !terminalGameObject.activeSelf && !assetTerminalGameObject.activeSelf && !textBoxTerminalGameObject.activeSelf;
        if (Input.GetKeyDown(KeyCode.Escape)){
            menuGameObject.SetActive(!menu.gameObject.activeSelf);
            terminalGameObject.SetActive(false);
            assetTerminalGameObject.SetActive(false);
            textBoxTerminalGameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.H)){
            loadMap(Application.dataPath+"/Resources/GeneratedAssets/255 pixel studios/CITY package/Prefab/Content/POLYGON city pack Scene/POLYGON city pack Scene.gltf");
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
        rotateCameraFollow.folderPaths = this;
        animateCharacter.folderPaths = this;

        menuGameObject = Instantiate(menuGameObject);
        menu = menuGameObject.GetComponent<Menu>();
        menu.main = this;

        assetTerminalGameObject = Instantiate(assetTerminalGameObject);
        assetTerminal = assetTerminalGameObject.GetComponent<AssetTerminal>();
        assetTerminal.main = this;
        assetTerminalGameObject.SetActive(false);

        terminalGameObject = Instantiate(terminalGameObject);
        terminalScript = terminalGameObject.GetComponent<TerminalScript>();
        terminalScript.folderPaths = this;
        terminalGameObject.SetActive(false);

        textBoxTerminalGameObject = Instantiate(textBoxTerminalGameObject);
        textBoxTerminal = textBoxTerminalGameObject.GetComponent<TextBoxTerminal>();
        textBoxTerminal.main = this;
        textBoxTerminalGameObject.SetActive(false);
    }
    internal async void loadMap(string path){
        path = orginizePaths.getKey(path);
        if (path != null && !Directory.Exists(path) && path.EndsWith(".gltf", StringComparison.OrdinalIgnoreCase)){
            orginizePaths.currentDirectoryPath = transform.name;
            GameObject oldMap = currentMap;
            currentMap = await exportImportGLTF.ImportGLTF(path,0);
            teleportPlayer();
            Destroy(oldMap);
        } else {
            GameObject oldMap = currentMap;
            currentMap = Instantiate(defaultMapRoot);
            teleportPlayer();
            Destroy(oldMap);
        }
    }
    internal async void loadPrefab(string path){
        if (!Directory.Exists(path) && path.EndsWith(".gltf", StringComparison.OrdinalIgnoreCase)){
            GameObject prefab = await exportImportGLTF.ImportGLTF(path,0);
            prefab.transform.position = placeInfrontOfPlayer(1,2,4);
        }
    }
    internal void teleportPlayer(){
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = currentMap.transform.GetChild(0).transform.position;
        player.GetComponent<CharacterController>().enabled = true;
    }
    public Vector3 placeInfrontOfPlayer(float x,float y, float z){
        Transform playerTransform = player.transform;
        Vector3 right = playerTransform.right*x;
        Vector3 up = playerTransform.up*y;
        Vector3 front = playerTransform.forward*z;
        return playerTransform.position + right + up + front;
    }
    public void createPortal(string path){
        GameObject portalGameObject = Instantiate(portalPrefab, placeInfrontOfPlayer(1,2,4), transform.rotation);
        portalGameObject.GetComponent<PortalGameObject>().init(currentMap.transform.GetChild(3).GetChild(0).gameObject,player,path);
    }
    public void createIcon(string path){
        GameObject iconGameObject = Instantiate(iconPrefab, placeInfrontOfPlayer(1,2,4), transform.rotation);
        iconGameObject.GetComponent<IconGameObject>().init(currentMap.transform.GetChild(3).GetChild(1).gameObject,player,path);
    }
}

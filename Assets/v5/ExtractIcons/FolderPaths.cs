using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    internal FirstPersonFlythrough firstPersonFlythrough;
    
    public GameObject portalPrefab,iconPrefab;

    public GameObject defaultMapRoot;
    internal GameObject currentMap;

    public GameObject menuGameObject;
    internal Menu menu;
    
    public GameObject terminalGameObject;
    internal TerminalScript terminalScript;

    public GameObject assetTerminalGameObject;
    internal AssetTerminal assetTerminalScript;

    public GameObject textureTerminalGameObject;
    internal TextureTerminal textureTerminalScript;

    public GameObject saveMapGameObject;
    internal SaveMap saveMapScript;

    public Material selectTransparent;
    
    public Camera firstPerson, thirdPerson;
    internal bool allMenuDisabled = false;

    
    void Awake(){
        init();
        firstPerson.enabled = false;
    }

    // Start is called before the first frame update
    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update(){
        allMenuDisabled = !menuGameObject.activeSelf && !terminalGameObject.activeSelf && 
            !assetTerminalGameObject.activeSelf && !textureTerminalGameObject.activeSelf && !saveMapGameObject.activeSelf; 
        if (allMenuDisabled){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (editPrefab.dictionary.Count>0) editPrefab.release(); 
            else {
                menuGameObject.SetActive(!menu.gameObject.activeSelf);
                terminalGameObject.SetActive(false);
                assetTerminalGameObject.SetActive(false);
                textureTerminalGameObject.SetActive(false);
                saveMapGameObject.SetActive(false);
            }
        }
    }

    public void init(){
        generatedAssets = transform.AddComponent<GeneratedAssets>();
        editPrefab = transform.AddComponent<EditPrefab>();
        rotateCameraFollow = follow.AddComponent<RotateCameraFollow>();
        animateCharacter = player.AddComponent<AnimateCharacter>();
        exportImportGLTF = transform.AddComponent<ExportImportGLTF>();
        orginizePaths = transform.AddComponent<OrginizePaths>();
        firstPersonFlythrough = firstPerson.transform.AddComponent<FirstPersonFlythrough>();
        rotateCameraFollow.player = player;
        rotateCameraFollow.folderPaths = this;
        animateCharacter.folderPaths = this;
        orginizePaths.main = this;
        firstPersonFlythrough.main = this;
        editPrefab.main = this;

        menuGameObject = Instantiate(menuGameObject);
        menu = menuGameObject.GetComponent<Menu>();
        menu.main = this;

        assetTerminalGameObject = Instantiate(assetTerminalGameObject);
        assetTerminalScript = assetTerminalGameObject.GetComponent<AssetTerminal>();
        assetTerminalScript.main = this;
        assetTerminalGameObject.SetActive(false);

        terminalGameObject = Instantiate(terminalGameObject);
        terminalScript = terminalGameObject.GetComponent<TerminalScript>();
        terminalScript.folderPaths = this;
        terminalGameObject.SetActive(false);

        textureTerminalGameObject = Instantiate(textureTerminalGameObject);
        textureTerminalScript = textureTerminalGameObject.GetComponent<TextureTerminal>();
        textureTerminalScript.main = this;
        textureTerminalGameObject.SetActive(false);

        saveMapGameObject = Instantiate(saveMapGameObject);
        saveMapScript = saveMapGameObject.GetComponent<SaveMap>();
        saveMapScript.main = this;
        saveMapGameObject.SetActive(false);
    }
    internal async void loadMap(){
        string path = orginizePaths.fullPath(orginizePaths.getKey());
        if (path != null && !Directory.Exists(path) && path.EndsWith(".gltf", StringComparison.OrdinalIgnoreCase)){
            GameObject oldMap = currentMap;
            currentMap = await exportImportGLTF.ImportGLTF(path,0);
            teleportPlayer();
            respawnPath(currentMap);
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
            prefab.transform.SetParent(currentMap.transform.GetChild(2).transform);
        }
    }
    void respawnPath(GameObject map){
        Transform path = map.transform.GetChild(3);
        Transform folders = path.GetChild(0).transform;
        Transform files = path.GetChild(1).transform;
        GameObject[] folderChildren = folders.transform.Cast<Transform>().Select(t => t.gameObject).ToArray();
        GameObject[] filesChildren = files.transform.Cast<Transform>().Select(t => t.gameObject).ToArray();
        for (int i = 0; i<folderChildren.Length;i++){
            GameObject temp = folderChildren[i];
            createPortal(temp.transform.name).transform.position = temp.transform.position;
            Destroy(temp);
        }
        for (int i = 0; i<filesChildren.Length;i++){
            GameObject temp = filesChildren[i];
            createIcon(temp.transform.name).transform.position = temp.transform.position;
            Destroy(temp);
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
    public GameObject createPortal(string path){
        GameObject portalGameObject = Instantiate(portalPrefab, placeInfrontOfPlayer(1,2,4), transform.rotation);
        portalGameObject.GetComponent<PortalGameObject>().init(currentMap.transform.GetChild(3).GetChild(0).gameObject,player,path);
        return portalGameObject;
    }
    public GameObject createIcon(string path){
        GameObject iconGameObject = Instantiate(iconPrefab, placeInfrontOfPlayer(1,2,4), transform.rotation);
        iconGameObject.GetComponent<IconGameObject>().init(currentMap.transform.GetChild(3).GetChild(1).gameObject,player,path);
        return iconGameObject;
    }
}

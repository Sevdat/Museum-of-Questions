using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditPrefab : MonoBehaviour
{
    FolderPaths folderPaths;
    internal GameObject selectedGameObject;
    internal int portalName = 0,iconName = 0;
    internal string[] folders,files;

    // Start is called before the first frame update
    void Start(){
        folderPaths = transform.GetComponent<FolderPaths>();
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.C)){
            createPortal(folders[portalName]);
        }
        if (Input.GetKeyDown(KeyCode.V)){
            createIcon(files[iconName]);
        }
        if (Input.GetKeyDown(KeyCode.E)){
            if (selectedGameObject != null){
                if (selectedGameObject.tag == "Portal") getPortal();
                else if (selectedGameObject.tag == "Icon") getIcon();
            }
        } 
    }
    public Vector3 placeInfrontOfPlayer(float x,float y, float z){
        Transform playerTransform = folderPaths.player.transform;
        Vector3 right = playerTransform.right*x;
        Vector3 up = playerTransform.up*y;
        Vector3 front = playerTransform.forward*z;
        return playerTransform.position + right + up + front;
    }
    public void createPortal(string path){
        GameObject portalGameObject = Instantiate(folderPaths.rootPortalPrefab, placeInfrontOfPlayer(1,2,4), transform.rotation);
        portalGameObject.GetComponent<PortalGameObject>().init(folderPaths.paths.transform.GetChild(0).gameObject,folderPaths.player,path);
    }
    public void createIcon(string path){
        GameObject iconGameObject = Instantiate(folderPaths.rootIconPrefab, placeInfrontOfPlayer(1,2,4), transform.rotation);
        iconGameObject.GetComponent<IconGameObject>().init(folderPaths.paths.transform.GetChild(1).gameObject,folderPaths.player,path);
    }
    public void getPortal(){
        if (folders.Length>0){
            portalName++;
            if (portalName>=folders.Length) portalName = 0;
            selectedGameObject.GetComponent<PortalGameObject>().rename(folders[portalName]); 
        } else {
            selectedGameObject.GetComponent<PortalGameObject>().rename(""); 
        }
    }
    public void getIcon(){
        if (files.Length>0){
            iconName++;
            if (iconName>=files.Length) iconName = 0;
            selectedGameObject.GetComponent<IconGameObject>().rename(files[iconName]); 
        } else {
            selectedGameObject.GetComponent<IconGameObject>().rename("");
        }
    }
    public void ray(){
        // Get the center of the screen
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        // Create a ray from the camera through the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        // Perform the raycast
        if (Physics.Raycast(ray, out RaycastHit hit, 100)){
           selectedGameObject = hit.transform.gameObject;
           print(selectedGameObject.tag);
        }
    }
}

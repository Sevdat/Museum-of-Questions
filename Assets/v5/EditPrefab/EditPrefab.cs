using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditPrefab : MonoBehaviour
{
    Main folderPaths;
    internal GameObject selectedGameObject;

    // Start is called before the first frame update
    void Start(){
        folderPaths = transform.GetComponent<Main>();
    }

    // Update is called once per frame
    void Update(){
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
        portalGameObject.GetComponent<PortalGameObject>().init(folderPaths.currentMap.transform.GetChild(3).GetChild(0).gameObject,folderPaths.player,path);
    }
    public void createIcon(string path){
        GameObject iconGameObject = Instantiate(folderPaths.rootIconPrefab, placeInfrontOfPlayer(1,2,4), transform.rotation);
        iconGameObject.GetComponent<IconGameObject>().init(folderPaths.currentMap.transform.GetChild(3).GetChild(1).gameObject,folderPaths.player,path);
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

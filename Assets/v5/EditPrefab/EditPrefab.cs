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

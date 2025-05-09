using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeHandle;

public class EditPrefab : MonoBehaviour
{
    internal Main main;
    static GameObject selectedGameObjects;
    static Dictionary<GameObject,GameObjectData> dictionary = new Dictionary<GameObject, GameObjectData>();
    static RuntimeTransformHandle runtimeTransformHandle;

    
    struct GameObjectData {
        internal Material[] materials;
        internal Vector3 initialVec;
        internal Quaternion initialQuat;
        internal Transform oldParent;
        public GameObjectData(GameObject gameObject, bool editorMode = false){
            materials = gameObject.GetComponent<Renderer>().materials;
            initialVec = gameObject.transform.position;
            initialQuat = gameObject.transform.rotation;
            oldParent = gameObject.transform.parent;
            if (dictionary.Count == 0) {
                selectedGameObjects = new GameObject("selected");
                if (editorMode) runtimeTransformHandle = selectedGameObjects.AddComponent<RuntimeTransformHandle>();
                selectedGameObjects.transform.position = gameObject.transform.position;
                selectedGameObjects.transform.rotation = gameObject.transform.rotation;
            }
            gameObject.transform.SetParent(selectedGameObjects.transform);
            if (!editorMode) gameObject.GetComponent<Collider>().enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start(){
        main = transform.GetComponent<Main>();
    }
    
    // Update is called once per frame
    void Update(){

        if (Input.GetKeyDown(KeyCode.R)){
            ray();
        }
        if (Input.GetKeyDown(KeyCode.Y)){
            release();
        }
        if (Input.GetKeyDown(KeyCode.F)){
            if (main.thirdPerson.isActiveAndEnabled)
                selectedGameObjects.transform.SetParent(main.follow.transform);
            else if (main.firstPerson.isActiveAndEnabled) 
                selectedGameObjects.transform.SetParent(main.firstPerson.transform);
        }
        if (Input.GetKeyUp(KeyCode.F)){
           selectedGameObjects.transform.SetParent(null);
        }

        if (Input.GetKey(KeyCode.Return)){
           release(false);
        }
        
        if (runtimeTransformHandle != null && Input.GetKey(KeyCode.Alpha1)){
            runtimeTransformHandle.type = HandleType.POSITION;
        }
        if (runtimeTransformHandle != null && Input.GetKey(KeyCode.Alpha2)){
            runtimeTransformHandle.type = HandleType.ROTATION;
        }
        if (runtimeTransformHandle != null && Input.GetKey(KeyCode.Alpha3)){
            runtimeTransformHandle.type = HandleType.SCALE;
        }
        
    }
    public void ray(){
        // Get the center of the screen
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        // Create a ray from the camera through the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        // Perform the raycast
        if (Physics.Raycast(ray, out RaycastHit hit, 100)){
            select(hit.transform.gameObject);
        }
    }
    public void select(GameObject selected){
        if (!dictionary.ContainsKey(selected)) {
            Renderer renderer = selected.GetComponent<Renderer>();
            dictionary[selected] = new GameObjectData(selected,main.firstPerson.isActiveAndEnabled);
            renderer.material = main.selectTransparent;
        } else {
            GameObjectData gameObjectData = dictionary[selected];
            selected.GetComponent<Renderer>().materials = gameObjectData.materials;
            selected.transform.SetParent(gameObjectData.oldParent);
            dictionary.Remove(selected);
        }
    }

    public void release(bool resetPosition = true, bool resetMaterials = true){
        foreach (GameObject selected in dictionary.Keys){
            GameObjectData gameObjectData = dictionary[selected];
            if (resetMaterials) {
                selected.GetComponent<Renderer>().materials = gameObjectData.materials;
                selected.transform.SetParent(gameObjectData.oldParent);
                }
            if (resetPosition){
                selected.transform.position = gameObjectData.initialVec;
                selected.transform.rotation = gameObjectData.initialQuat;
            }
            selected.GetComponent<Collider>().enabled = true;
            
        }
        dictionary = new Dictionary<GameObject, GameObjectData>();
        if (resetMaterials) {
            Destroy(selectedGameObjects);
            runtimeTransformHandle = null;
        }
    }
}

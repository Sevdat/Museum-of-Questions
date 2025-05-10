using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeHandle;

public class EditPrefab : MonoBehaviour
{
    internal Main main;
    static GameObject selectedGameObjects;
    internal Dictionary<GameObject,GameObjectData> dictionary = new Dictionary<GameObject, GameObjectData>();
    static RuntimeTransformHandle runtimeTransformHandle;

    
    internal struct GameObjectData {
        internal Material[] materials;
        internal Vector3 initialVec;
        internal Quaternion initialQuat;
        internal Transform oldParent;
        public GameObjectData(GameObject gameObject, Dictionary<GameObject,GameObjectData> dictionary,Material selectTransparent,bool editorMode){
            initialVec = gameObject.transform.position;
            initialQuat = gameObject.transform.rotation;
            oldParent = gameObject.transform.parent;

            Renderer renderer = gameObject.GetComponent<Renderer>();
            if (renderer != null){
                materials = renderer.materials;
                Material[] newMaterials = renderer.materials; // Get a copy
                for (int i = 0; i<newMaterials.Length;i++){
                    newMaterials[i] = selectTransparent;
                }
                renderer.materials = newMaterials;
            } else materials = null;

            // if (renderer != null) renderer.material = selectTransparent;
            if (dictionary.Count == 0) {
                selectedGameObjects = new GameObject("selected");
                if (editorMode) runtimeTransformHandle = selectedGameObjects.AddComponent<RuntimeTransformHandle>();
                selectedGameObjects.transform.position = gameObject.transform.position;
                selectedGameObjects.transform.rotation = gameObject.transform.rotation;
            }
            gameObject.transform.SetParent(selectedGameObjects.transform);
            Collider collider = gameObject.GetComponent<Collider>();
            if (!editorMode && collider != null) collider.enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start(){
    }
    
    // Update is called once per frame
    void Update(){

        if (Input.GetKeyDown(KeyCode.R)){
            ray(true);
        }
        if (Input.GetKeyDown(KeyCode.E)){
            ray(false);
        }
        if (Input.GetKeyDown(KeyCode.Q)){
            delete();
        }
        if (runtimeTransformHandle != null && Input.GetKeyDown(KeyCode.F)){
            selectedGameObjects.transform.SetParent(main.firstPerson.transform);
        }
        if (runtimeTransformHandle != null && Input.GetKeyUp(KeyCode.F)){
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
    public void ray(bool selectAll){
        // Get the center of the screen
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        // Create a ray from the camera through the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        // Perform the raycast
        if (Physics.Raycast(ray, out RaycastHit hit, 100)){
            select(hit.transform.gameObject);
            if (selectAll) selectParent();
        }
    }
    public void selectParent(){
        HashSet<Transform> set = new HashSet<Transform>();
        foreach (GameObject selected in dictionary.Keys){
            Transform oldParent = dictionary[selected].oldParent;
            if (!set.Contains(oldParent))set.Add(oldParent);
        }
        foreach (Transform parent in set){
            RecursivelyProcessChildren(parent);
        }
    }
    void RecursivelyProcessChildren(Transform parent){
        // Loop through all children
        for (int i = 0; i < parent.childCount; i++){
            Transform child = parent.GetChild(i);
            RecursivelyProcessChildren(child);
            bool doesntContain = select(child.gameObject,false);
            if (doesntContain) i--;
        }
    }
    public bool select(GameObject selected, bool deselectDuplicate = true){
        bool doesntContain = !dictionary.ContainsKey(selected);
        if (doesntContain) {
            dictionary[selected] = new GameObjectData(selected,dictionary,main.selectTransparent,main.firstPerson.isActiveAndEnabled);
        } else if (deselectDuplicate) {
            GameObjectData gameObjectData = dictionary[selected];
            selected.GetComponent<Renderer>().materials = gameObjectData.materials;
            selected.transform.SetParent(gameObjectData.oldParent);
            dictionary.Remove(selected);
        }
        return doesntContain;
    }

    public void delete(){
        Destroy(selectedGameObjects);
        runtimeTransformHandle = null;
        dictionary = new Dictionary<GameObject, GameObjectData>();
    }

    public void release(bool resetPosition = true, bool resetMaterials = true){
        foreach (GameObject selected in dictionary.Keys){
            GameObjectData gameObjectData = dictionary[selected];
            if (resetMaterials) {
                Renderer renderer = selected.GetComponent<Renderer>();
                if (renderer!= null)renderer.materials = gameObjectData.materials;
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

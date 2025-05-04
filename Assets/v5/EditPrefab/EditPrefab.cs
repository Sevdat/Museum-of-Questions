using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditPrefab : MonoBehaviour
{
    Main folderPaths;
    internal static GameObject selectedGameObject;
    Dictionary<GameObject,GameObjectData> selected = new Dictionary<GameObject, GameObjectData>();

    float reverseRotation = 1f;
    float scale = 0.01f;

    struct GameObjectData {
        internal Material[] materials;
        internal Vector3 initialVec;
        internal Quaternion initialQuat;
        internal Transform oldParent;
        public GameObjectData(GameObject gameObject){
            materials = gameObject.GetComponent<Renderer>().materials;
            initialVec = gameObject.transform.position;
            initialQuat = gameObject.transform.rotation;
            oldParent = gameObject.transform.parent;
            if (selectedGameObject.transform.childCount == 0) {
                selectedGameObject.transform.position = gameObject.transform.position;
                selectedGameObject.transform.rotation = gameObject.transform.rotation;
                }
            gameObject.transform.SetParent(selectedGameObject.transform);
        }
    }
    // Start is called before the first frame update
    void Start(){
        folderPaths = transform.GetComponent<Main>();
        selectedGameObject = new GameObject("selected");
    }

    // Update is called once per frame
    void Update(){

        if (Input.GetKey(KeyCode.R)){
            ray();
        }
        if (Input.GetKeyDown(KeyCode.Y)){
            release();
        }
        if (Input.GetKeyDown(KeyCode.F)){
            selectedGameObject.transform.SetParent(folderPaths.follow.transform);
        }
        if (Input.GetKeyUp(KeyCode.F)){
           selectedGameObject.transform.SetParent(null);
        }

        reverseRotation = Input.GetKey(KeyCode.Tab)? -0.1f:0.1f;
        scale = Input.GetKey(KeyCode.Tab)? -0.001f:0.001f;

        if (Input.GetKey(KeyCode.Alpha1)){
           selectedGameObject.transform.Rotate(0, 0, reverseRotation);
        }
        if (Input.GetKey(KeyCode.Alpha2)){
           selectedGameObject.transform.Rotate(0, reverseRotation, 0);
        }
        if (Input.GetKey(KeyCode.Alpha3)){
           selectedGameObject.transform.Rotate(reverseRotation, 0, 0);
        }

        if (Input.GetKey(KeyCode.Alpha4)){
           selectedGameObject.transform.localScale += new Vector3(scale, scale, scale);
        }

        if (Input.GetKey(KeyCode.Return)){
           release(false);
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
            hit.collider.enabled = false;
        }
    }
    public void select(GameObject select){
        if (!selected.ContainsKey(select)) {
            Renderer renderer = select.GetComponent<Renderer>();

            selected[select] = new GameObjectData(select);
            renderer.material = folderPaths.selectTransparent;
        }
    }

    public void release(bool reset = true, bool resetMaterials = true){
        foreach (GameObject gameObject in selected.Keys){
            if (resetMaterials) gameObject.GetComponent<Renderer>().materials = selected[gameObject].materials;
            if (reset){
                gameObject.transform.position = selected[gameObject].initialVec;
                gameObject.transform.rotation = selected[gameObject].initialQuat;
            }
            gameObject.GetComponent<Collider>().enabled = true;
        }
        selected = new Dictionary<GameObject, GameObjectData>();
    }
}

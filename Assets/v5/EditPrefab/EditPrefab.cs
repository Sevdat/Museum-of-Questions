using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditPrefab : MonoBehaviour
{
    Main folderPaths;
    internal GameObject selectedGameObject;
    Dictionary<GameObject,GameObjectData> selected = new Dictionary<GameObject, GameObjectData>();
    
    struct GameObjectData {
        internal Material[] materials;
        internal Vector3 vec, initialVec;
        internal Quaternion quat, initialQuat;
        public GameObjectData(GameObject gameObject){
            materials = gameObject.GetComponent<Renderer>().materials;
            vec = Camera.main.transform.InverseTransformPoint(gameObject.transform.position);
            quat = Quaternion.Inverse(Camera.main.transform.rotation) * gameObject.transform.rotation;
            initialVec = gameObject.transform.position;
            initialQuat = gameObject.transform.rotation;
        }
    }
    // Start is called before the first frame update
    void Start(){
        folderPaths = transform.GetComponent<Main>();
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
            move();
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
    public void move(){
        foreach (GameObject obj in selected.Keys){
            Vector3 targetPosition = Camera.main.transform.TransformPoint(selected[obj].vec);
            obj.transform.position = targetPosition;
            obj.transform.rotation = Camera.main.transform.rotation * selected[obj].quat;
        }
    }
    public void release(){
        foreach (GameObject gameObject in selected.Keys){
            gameObject.GetComponent<Renderer>().materials = selected[gameObject].materials;
            gameObject.transform.position = selected[gameObject].initialVec;
            gameObject.transform.rotation = selected[gameObject].initialQuat;
            gameObject.GetComponent<Collider>().enabled = true;
        }
        selected = new Dictionary<GameObject, GameObjectData>();
    }
}

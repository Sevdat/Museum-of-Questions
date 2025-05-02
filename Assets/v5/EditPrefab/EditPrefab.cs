using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditPrefab : MonoBehaviour
{
    Main folderPaths;
    internal GameObject selectedGameObject;
    Dictionary<GameObject,Material[]> selected = new Dictionary<GameObject, Material[]>();

    // Start is called before the first frame update
    void Start(){
        folderPaths = transform.GetComponent<Main>();
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.R)){
            ray();
        }
        if (Input.GetKeyDown(KeyCode.Y)){
            release();
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
    public void select(GameObject select){
        if (!selected.ContainsKey(select)) {
            Renderer renderer = select.GetComponent<Renderer>();
            selected[select] = renderer.materials;
            renderer.material = folderPaths.selectTransparent;
        }
    }
    public void release(){
        foreach (GameObject gameObject in selected.Keys){
            gameObject.GetComponent<Renderer>().materials = selected[gameObject];
        }
        selected = new Dictionary<GameObject, Material[]>();
    }
}

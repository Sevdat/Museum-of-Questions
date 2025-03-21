using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;

public class LoadPrefabScript : MonoBehaviour
{
    // The name of the prefab to load (without the file extension)
    // public string prefabName = "ImportedScenes/Medieval Cute Series/Medieval Cute Build In 1";
    FolderPaths folderPaths;
    List<string> allScenePrefabs;
    

    public void Awake(){
        

    }
    public void Start(){
        folderPaths = transform.GetComponent<FolderPaths>();
        
    }

    public string relativePath(string path){
        return Path.Combine(Application.persistentDataPath, path);
    }
    public static string getRelativeResourcePath(string fullPath){
        string pattern = @"(?<=Resources\/).+(?=\.prefab)";
        return Regex.Match(fullPath, pattern).Value;
    }

    internal IEnumerator LoadAndInstantiatePrefab(GameObject destroyPortal,Vector3 vec){
        // Load the prefab from the Resources folder
        ResourceRequest request = Resources.LoadAsync<GameObject>(allScenePrefabs[Random.Range(0, allScenePrefabs.Count-1)]);
        // Wait until the prefab is fully loaded
        yield return request;
        // Check if the prefab was loaded successfully
        if (request.asset == null) yield break;
        
        // Instantiate the prefab into the scene
        GameObject root = request.asset as GameObject;
        if (root != null){
            folderPaths.currentMap = Instantiate(root, vec, Quaternion.identity);
            Destroy(destroyPortal);
        }
    }
    internal IEnumerator LoadAndInstantiatePrefab(Vector3 vec){
        // Load the prefab from the Resources folder
        ResourceRequest request = Resources.LoadAsync<GameObject>(allScenePrefabs[Random.Range(0, allScenePrefabs.Count-1)]);
        // Wait until the prefab is fully loaded
        yield return request;
        // Check if the prefab was loaded successfully
        if (request.asset == null) yield break;

        // Instantiate the prefab into the scene
        GameObject root = request.asset as GameObject;
        if (root != null) folderPaths.currentMap = Instantiate(root, vec, Quaternion.identity);
    }
    internal IEnumerator DeleteMapPrefab(GameObject prefab){
        // Destroy the last object
        Destroy(prefab);
        // Yield to the next frame to spread out the workload
        yield return null;
    }

}
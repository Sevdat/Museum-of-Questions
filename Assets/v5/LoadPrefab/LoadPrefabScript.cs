using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text.RegularExpressions;
using UnityEditor.Experimental.GraphView;
using System.IO;
using System.Collections.Generic;

public class LoadPrefabScript : MonoBehaviour
{
    // The name of the prefab to load (without the file extension)
    // public string prefabName = "ImportedScenes/Medieval Cute Series/Medieval Cute Build In 1";
    FolderPaths folderPaths;
    List<string> allScenePrefabs;

    public void Awake(){
        // allScenePrefabs = getAllPrefabs();
    }
    public void Start(){
        folderPaths = transform.GetComponent<FolderPaths>();
        string[] lol = getPrefabsInFolder("Assets/ImportedAssets");
        foreach (string i in lol) print(i);
    }

    public static string getRelativeResourcePath(string fullPath){
        string pattern = @"(?<=Resources\/).+(?=\.prefab)";
        return Regex.Match(fullPath, pattern).Value;
    }
    public string[] getPrefabsGUIDInFolder(string folderPath){
        return AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
    }
    public string[] getPrefabsInFolder(string folderPath){
        string[] prefabGUIDs = getPrefabsGUIDInFolder(folderPath);
        print(folderPath);
        for (int i = 0; i<prefabGUIDs.Length;i++){
            prefabGUIDs[i] = AssetDatabase.GUIDToAssetPath(prefabGUIDs[i]).Split(new [] {folderPath + "/"}, System.StringSplitOptions.None)[1];
        }
        return prefabGUIDs;
    }
    public List<string> getAllPrefabs(){
        string folderInResources = "ImportedScenes";
        string[] folders = Directory.GetDirectories(@$"C:\Users\sevda\Documents\GitHub\Museum-of-Questions\Assets\Resources\{folderInResources}");
        List<string> paths = new List<string>();
        for (int i = 0; i<folders.Length;i++){
            string[] str = folders[i].Split("\\");
            folders[i] = @$"Assets\Resources\{folderInResources}\" + folders[i].Split("\\")[str.Length-1];
        }
        foreach(string str in folders){
            paths.AddRange(getPrefabsInFolder(str));
        }
        return paths;
    }

    internal IEnumerator LoadAndInstantiatePrefab(GameObject destroyPortal,Vector3 vec){
        // Load the prefab from the Resources folder
        ResourceRequest request = Resources.LoadAsync<GameObject>(allScenePrefabs[Random.Range(0, allScenePrefabs.Count-1)]);
        // Wait until the prefab is fully loaded
        yield return request;
        // Check if the prefab was loaded successfully
        if (request.asset == null){
            yield break;
        }
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
        if (request.asset == null){
            yield break;
        }
        // Instantiate the prefab into the scene
        GameObject root = request.asset as GameObject;
        if (root != null){
            folderPaths.currentMap = Instantiate(root, vec, Quaternion.identity);
        }
    }
    internal IEnumerator DeleteMapPrefab(GameObject prefab){
        // Destroy the last object
        Destroy(prefab);
        // Yield to the next frame to spread out the workload
        yield return null;
    }
}
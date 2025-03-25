using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text.RegularExpressions;
using UnityEditor.Experimental.GraphView;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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
        // generatePrefabFolders("Prefab","Assets/ImportedAssets/ImportedAssets");
        // generatePrefabFolders("Scene","Assets/ImportedAssets/ImportedAssets");
    }

    public static string getRelativeResourcePath(string fullPath){
        string pattern = @"(?<=Resources\/).+(?=\.prefab)";
        return Regex.Match(fullPath, pattern).Value;
    }
    public string[] getPrefabsGUIDInFolder(string mode, string folderPath){
        return AssetDatabase.FindAssets($"t:{mode}", new string[] { folderPath });
    }
    public string[] getPrefabsInFolder(string mode,string folderPath, bool split = true){
        string[] prefabGUIDs = getPrefabsGUIDInFolder(mode,folderPath);
        if (split)
            for (int i = 0; i<prefabGUIDs.Length;i++){
                prefabGUIDs[i] = AssetDatabase.GUIDToAssetPath(prefabGUIDs[i]).Split(new string[] {folderPath + "/"}, System.StringSplitOptions.None)[1];
            }
        else
            for (int i = 0; i<prefabGUIDs.Length;i++){
                prefabGUIDs[i] = AssetDatabase.GUIDToAssetPath(prefabGUIDs[i]);
            }
        return prefabGUIDs;
    }
    public string createFolder(string path,bool persistentDataPath = false){
        // Define the path where you want to create the folder
        string folderPath = (!persistentDataPath)? $"{Application.dataPath}/{path}": $"{Application.persistentDataPath}/{path}";

        // Check if the folder already exists
        if (!Directory.Exists(folderPath)){
            // Create the folder
            Directory.CreateDirectory(folderPath);
        }
        return folderPath;
    }
    public void duplicateAsset(string assetPath, string newPath){
        AssetDatabase.CopyAsset(assetPath, newPath);
        AssetDatabase.Refresh();
    }
    // mode: Prefab,Scene,Texture2D,Shader,Material
    public void generatePrefabFolders(string mode, string path,bool persistentDataPath = false){
        string[] lol = getPrefabsInFolder(mode,path);
        foreach (string i in lol) {
            string[] split = i.Split("/");
            string folder = $"{split[0]}/{split[1]}/{mode}";
            string generatedPath = $"Resources/GeneratedAssets/{folder}";
            createFolder(generatedPath);
            duplicateAsset($"{path}/{i}",$"Assets/{generatedPath}/{split[split.Length-1]}");
        }
    }

    // public List<string> getAllPrefabs(){
    //     string folderInResources = "ImportedScenes";
    //     string[] folders = Directory.GetDirectories(@$"C:\Users\sevda\Documents\GitHub\Museum-of-Questions\Assets\Resources\{folderInResources}");
    //     List<string> paths = new List<string>();
    //     for (int i = 0; i<folders.Length;i++){
    //         string[] str = folders[i].Split("\\");
    //         folders[i] = @$"Assets\Resources\{folderInResources}\" + folders[i].Split("\\")[str.Length-1];
    //     }
    //     foreach(string str in folders){
    //         paths.AddRange(getPrefabsInFolder(str));
    //     }
    //     return paths;
    // }

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
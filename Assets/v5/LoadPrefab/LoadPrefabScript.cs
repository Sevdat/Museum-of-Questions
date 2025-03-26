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
        folderPaths = transform.GetComponent<FolderPaths>();
        // allScenePrefabs = getAllPrefabs();
    }
    public void Start(){
        // generatePrefabFolders("Prefab","Assets/ImportedAssets/ImportedAssets");
        // generatePrefabFolders("Scene","Assets/ImportedAssets/ImportedAssets");
        string[] str = getPrefabsInFolder("Prefab","Assets/ImportedAssets/ImportedAssets", false);
        int i = 0;
        foreach (string st in str) {
            SavePreviewFromPath(str[i],$"Assets/iconFolder/lol{i}.png");
            i++;
        }
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
    // mode: Prefab,Scene,Texture2D,Shader,Material
    public void generatePrefabFolders(string mode, string path,bool persistentDataPath = false){
        string[] lol = getPrefabsInFolder(mode,path);
        foreach (string i in lol) {
            string[] split = i.Split("/");
            string folder = $"{split[0]}/{split[1]}/{mode}";
            string generatedPath = $"Resources/GeneratedAssets/{folder}";
            createFolder(generatedPath);
            AssetDatabase.CopyAsset($"{path}/{i}",$"Assets/{generatedPath}/{split[split.Length-1]}");
        }
        AssetDatabase.Refresh();
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

private static Texture2D CreateReadableTexture(Texture2D source)
{
    // Create a temporary RenderTexture
    RenderTexture renderTex = RenderTexture.GetTemporary(
        source.width,
        source.height,
        0,
        RenderTextureFormat.Default,
        RenderTextureReadWrite.Linear);

    // Blit the source texture to RenderTexture
    Graphics.Blit(source, renderTex);
    
    // Create a new readable Texture2D
    Texture2D readableText = new Texture2D(
        source.width, 
        source.height, 
        TextureFormat.RGBA32, 
        false);
    
    // Read the RenderTexture into Texture2D
    RenderTexture.active = renderTex;
    readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
    readableText.Apply();
    
    // Clean up
    RenderTexture.active = null;
    RenderTexture.ReleaseTemporary(renderTex);
    
    return readableText;
}
private static void SaveTextureToPNG(Texture2D texture, string outputPath)
{
    try
    {
        // Create a readable copy if needed
        Texture2D readableTexture = texture.isReadable ? texture : CreateReadableTexture(texture);
        
        // Ensure path is correct
        if (!outputPath.StartsWith("Assets/"))
            outputPath = "Assets/" + outputPath;
        if (!outputPath.EndsWith(".png"))
            outputPath += ".png";

        // Ensure directory exists
        string directory = Path.GetDirectoryName(outputPath);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        // Save the file
        byte[] bytes = readableTexture.EncodeToPNG();
        string fullPath = Path.Combine(Application.dataPath.Replace("Assets", ""), outputPath);
        File.WriteAllBytes(fullPath, bytes);
        
        // Refresh asset database
        AssetDatabase.Refresh();
        Debug.Log($"Preview saved to: {outputPath}");
    }
    catch (System.Exception e)
    {
        Debug.LogError($"Failed to save preview: {e.Message}");
    }
}
public static void SavePreviewFromPath(string prefabPath, string outputPath)
{
    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
    if (prefab == null)
    {
        Debug.LogError($"Prefab not found at path: {prefabPath}");
        return;
    }

    int maxAttempts = 20000; // Increased to 60 attempts (~1 second at 60fps)
    int attempts = 0;
    
    EditorApplication.CallbackFunction callback = null;
    callback = () =>
    {
        attempts++;
        Texture2D previewTexture = AssetPreview.GetAssetPreview(prefab);
        
        if (previewTexture != null || attempts >= maxAttempts)
        {
            EditorApplication.update -= callback;
            
            if (previewTexture == null)
            {
                Debug.LogError($"Failed to generate preview after {maxAttempts} attempts");
                TryAlternativePreviewMethod(prefab, outputPath);
                return;
            }
            
            SaveTextureToPNG(previewTexture, outputPath);
        }
    };
    
    // Prime the preview generation
    AssetPreview.SetPreviewTextureCacheSize(256);
    AssetPreview.GetAssetPreview(prefab);
    
    EditorApplication.update += callback;
}
private static void TryAlternativePreviewMethod(GameObject prefab, string outputPath)
{
    // 1. First try mini thumbnail
    Texture2D miniPreview = AssetPreview.GetMiniThumbnail(prefab);
    if (miniPreview != null)
    {
        Debug.LogWarning("Using mini thumbnail as fallback");
        SaveTextureToPNG(miniPreview, outputPath);
        return;
    }

    // 2. If that fails, use the default prefab icon
    Texture2D defaultIcon = EditorGUIUtility.IconContent("Prefab Icon").image as Texture2D;
    if (defaultIcon != null)
    {
        Debug.LogWarning("Using default prefab icon as fallback");
        SaveTextureToPNG(defaultIcon, outputPath);
        return;
    }

    // 3. Final fallback - create a blank texture
    Debug.LogWarning("Creating blank texture as final fallback");
    Texture2D blankTexture = new Texture2D(128, 128);
    Color[] pixels = new Color[128 * 128];
    for (int i = 0; i < pixels.Length; i++)
        pixels[i] = Color.gray;
    blankTexture.SetPixels(pixels);
    blankTexture.Apply();
    SaveTextureToPNG(blankTexture, outputPath);
}
}
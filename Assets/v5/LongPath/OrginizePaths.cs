using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OrginizePaths : MonoBehaviour
{
    internal Dictionary<string,string> portalTravelToMap; 
    internal string currentDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    internal string traveledFrom;
    internal string mapName; 

    // portlePath-mapName
    // \AppData\LocalLow\DefaultCompany\Museum of Questions\Maps - mapFolder/mapName
    // Start is called before the first frame update

    void Start()
    {
        // print(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        // print(Application.persistentDataPath);
        // print(mapPath("lol"));
        // print(fullMapPath(mapPath("lol")));
        // allFolders();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void printAll(IEnumerable[] lol){
        foreach (IEnumerable str in lol) print(str);
    }
    string mapPath(string mapName){
        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return $"{Application.persistentDataPath}/Maps/{mapName}".Replace('/', '\\').Replace(userProfile, "");
    }
    string fullMapPath(string relativePath){
        return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + relativePath;
    }
    string[] allFolders(){
        string[] folders = Directory.GetDirectories(Application.dataPath+"/Resources/GeneratedAssets","*", SearchOption.AllDirectories);
        return folders;
    }
    public bool createFolder(string path){
        try {
            if (string.IsNullOrWhiteSpace(path)) return false;

            string fullPath = Path.Combine(Application.persistentDataPath, path);
            
            fullPath = Path.GetFullPath(fullPath);

            // Check for invalid characters manually (optional extra safety)
            if (fullPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0) return false;

            // Check if the folder already exists before creating it
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
            
            return true;
        }
        catch (Exception ex) when (
            ex is ArgumentException ||
            ex is NotSupportedException ||
            ex is PathTooLongException ||
            ex is IOException)
        {
            Debug.LogError($"Failed to create folder: {ex.Message}");
            return false;
        } 
    }
    void saveData(string filePath){                
        try {
            // Create and write to the file
            using (StreamWriter writer = new StreamWriter(filePath)){
                foreach (KeyValuePair<string, string> pair in portalTravelToMap){
                    writer.WriteLine(pair.Key);  // First line - path
                    writer.WriteLine(pair.Value);
                }
            }
            Console.WriteLine("File written successfully!");
        } catch (Exception ex){
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    void readData(string filePath){
        portalTravelToMap = new Dictionary<string, string>();
        try {
            using (StreamReader reader = new StreamReader(filePath)){
                while (!reader.EndOfStream){
                    string key = reader.ReadLine();
                    string value = reader.ReadLine();
                    if (key != null && value != null && !portalTravelToMap.ContainsKey(key)) {
                        portalTravelToMap[key] = value;
                    }
                }
            }
        }
        catch (Exception ex){
            Console.WriteLine($"Read error: {ex.Message}");
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class OrginizePaths : MonoBehaviour
{
    internal Main main;
    internal Dictionary<string,string> portalTravelToMap = new Dictionary<string, string>(); 
    internal string currentDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    internal string traveledFrom;
    internal string mapName;

    
    internal string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    internal string assetPath = @$"{Application.dataPath}/Resources/GeneratedAssets";
    internal string mapPath = Application.dataPath+ "/Resources/GeneratedMaps";
    internal string saveMapPath = Application.dataPath+ "/Resources/GeneratedMaps/MapData.txt";

    // portlePath-mapName
    // \AppData\LocalLow\DefaultCompany\Museum of Questions\Maps - mapFolder/mapName
    // Start is called before the first frame update

    void Awake()
    {
        
    }
    void Start()
    {
        readData();
        main.loadMap();
    }

    // Update is called once per frame
    void Update()
    {

    }
    string normilizePath(string mapName){
        return mapName.Replace('/', '\\').Replace(userPath, "");
    }
    internal string fullPath(string relativePath){
        return (userPath + relativePath).Replace('/', '\\');
    }
    
    public bool createFolder(string path){
        try {
            if (string.IsNullOrWhiteSpace(path)) return false;
            string fullPath = Path.Combine(Application.persistentDataPath, path);
            fullPath = Path.GetFullPath(fullPath);
            if (fullPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0) return false;
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
            return true;
        } catch (Exception ex) {
            Debug.LogError($"Failed to create folder: {ex.Message}");
            return false;
        } 
    }
    internal string getKey(){
        portalTravelToMap.TryGetValue(normilizePath(currentDirectoryPath),out string mapPath);
        return mapPath;
    }
    internal void setKey(string mapPath){
        string normilizedFolderPath = normilizePath(currentDirectoryPath);
        string normilizedMapPath = normilizePath(mapPath);
        if (!portalTravelToMap.ContainsKey(normilizedFolderPath)) 
            portalTravelToMap[normilizedFolderPath] = normilizedMapPath;
    }
    internal void deleteKey(string mapPath){
        string normilizedFolderPath = normilizePath(currentDirectoryPath);
        if (!portalTravelToMap.ContainsKey(normilizedFolderPath)) 
            portalTravelToMap.Remove(normilizedFolderPath);
        Directory.Delete(mapPath, recursive: true);
    }

    internal void saveData(){                
        try {
            // Create and write to the file
            using (StreamWriter writer = new StreamWriter(saveMapPath)){
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

    void readData(){
        if (!File.Exists(saveMapPath)) File.WriteAllText(saveMapPath, "");
        portalTravelToMap = new Dictionary<string, string>();
        try {
            using (StreamReader reader = new StreamReader(saveMapPath)){
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
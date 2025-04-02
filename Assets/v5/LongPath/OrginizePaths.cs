using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OrginizePaths : MonoBehaviour
{
    Dictionary<string,string> pathData; 
    // relativePath-mapName
    // \AppData\LocalLow\DefaultCompany\Museum of Questions\Maps - mapFolder/mapName
    // Start is called before the first frame update

    void Start()
    {
        // print(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        // print(Application.persistentDataPath);
        // print(mapPath("lol"));
        // print(fullMapPath(mapPath("lol")));
    }

    // Update is called once per frame
    void Update()
    {
        
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
        foreach (string str in folders) print(str);
        return folders;
    }
    public string createFolder(string path){
        // Define the path where you want to create the folder
        string folderPath = $"{Application.persistentDataPath}/{path}";

        // Check if the folder already exists
        if (!Directory.Exists(folderPath)){
            // Create the folder
            Directory.CreateDirectory(folderPath);
        }
        return folderPath;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconGameObject : ExtractIcon
{
    Main folderPaths;
    GameObject player;

    public void init(GameObject parent, GameObject player, string path){
        this.player = player;
        folderPaths = player.transform.parent.GetComponent<Main>();
        rename(path);
        transform.rotation = player.transform.rotation;
        transform.SetParent(parent.transform);
    }
    internal void createIcon(string path){
        print(folderPaths);
        transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = getGenericIcon(folderPaths.orginizePaths.fullPath(path));
    }
    internal void rename(string path){
        transform.name = path;
        createIcon(path);
        string[] strArray = path.Split("\\");
        transform.GetChild(1).GetComponent<TextMeshPro>().text = strArray[strArray.Length-1];
    }

    void OnTriggerEnter(Collider collision){
        if (collision.transform.name == player.name){
            Process.Start(folderPaths.orginizePaths.fullPath(transform.name));
        }   
    }

    // Process.Start(@"C:\Users\Public\Desktop\Firefox.lnk");
}

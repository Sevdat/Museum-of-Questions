using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconGameObject : ExtractIcon
{
    GameObject player;

    public void init(GameObject parent, GameObject player, string path){
        this.player = player;
        rename(path);
        transform.rotation = player.transform.rotation;
        transform.SetParent(parent.transform);
    }
    internal void createIcon(string path){
        transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = getGenericIcon(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)+path);
    }
    internal void rename(string path){
        transform.name = path;
        createIcon(path);
        string[] strArray = path.Split("\\");
        transform.GetChild(1).GetComponent<TextMeshPro>().text = strArray[strArray.Length-1];
    }

    void OnTriggerEnter(Collider collision){
        if (collision.transform.name == player.name){
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + transform.name);
        }   
    }

    // Process.Start(@"C:\Users\Public\Desktop\Firefox.lnk");
}

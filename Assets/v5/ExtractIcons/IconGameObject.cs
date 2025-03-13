using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconGameObject : ExtractIcon
{
    GameObject prefab,player;

    public void init(GameObject parent, GameObject prefab,GameObject player, string path){
        this.player = player;
        this.prefab = prefab;
        rename(path);
        this.prefab.transform.SetParent(parent.transform);
    }
    internal void createIcon(string path){
        prefab.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = getGenericIcon(path);
    }
    internal void rename(string path){
        prefab.transform.name = path;
        createIcon(path);
        string[] strArray = path.Split("\\");
        prefab.transform.GetChild(1).GetComponent<TextMeshPro>().text = strArray[strArray.Length-1];
    }

    void OnTriggerEnter(Collider collision){
        if (collision.transform.name == player.name){
            Process.Start(transform.name);
        }   
    }

    // Process.Start(@"C:\Users\Public\Desktop\Firefox.lnk");
}

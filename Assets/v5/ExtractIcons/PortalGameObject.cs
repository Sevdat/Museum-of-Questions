using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PortalGameObject : MonoBehaviour
{
    GameObject player;
    FolderPaths folderPaths;

    public void init(GameObject parent, GameObject player, string path){
        this.player = player;
        rename(path);
        transform.rotation = player.transform.rotation;
        transform.SetParent(parent.transform);
        folderPaths = player.transform.parent.GetComponent<FolderPaths>();
    }

    internal void rename(string path){
        transform.name = path;
        string[] strArray = path.Split("\\");
        transform.GetChild(2).GetComponent<TextMeshPro>().text = strArray[strArray.Length-1];
    }

    void OnTriggerEnter(Collider collision){
        if (collision.transform.name == player.transform.name){
            teleportPlayer();
        }
    }
    float height = 1;
    public void teleportPlayer(){
        if (transform.name != ""){
            if (folderPaths.currentMap != null) {
                folderPaths.currentDirectoryPath = transform.name;
                StartCoroutine(folderPaths.loadPrefabScript.DeleteMapPrefab(folderPaths.currentMap));
                folderPaths.getFiles();
                folderPaths.getFolders();
            }
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(100*height,100*height + 5,100*height);
            player.GetComponent<CharacterController>().enabled = true;
            StartCoroutine(folderPaths.loadPrefabScript.LoadAndInstantiateMapPrefab(new Vector3(100*height,100*height,100*height)));
            height++;
        }
    }
    // Process.Start(@"C:\Users\Public\Desktop\Firefox.lnk");
}

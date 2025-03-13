using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PortalGameObject : MonoBehaviour
{
    GameObject prefab,player;
    FolderPaths folderPaths;

    public void init(GameObject parent, GameObject prefab,GameObject player, string path){
        this.player = player;
        this.prefab = prefab;
        rename(path);
        this.prefab.transform.SetParent(parent.transform);
        folderPaths = player.transform.parent.GetComponent<FolderPaths>();
    }

    internal void rename(string path){
        prefab.transform.name = path;
        string[] strArray = path.Split("\\");
        prefab.transform.GetChild(2).GetComponent<TextMeshPro>().text = strArray[strArray.Length-1];
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
                folderPaths.previousDirectoryPath = folderPaths.directoryPath;
                folderPaths.directoryPath = transform.name;
                StartCoroutine(folderPaths.loadPrefabScript.DeleteMapPrefab(folderPaths.currentMap));
            }
            print(transform.name);
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(100*height,100*height + 5,100*height);
            player.GetComponent<CharacterController>().enabled = true;
            StartCoroutine(folderPaths.loadPrefabScript.LoadAndInstantiateMapPrefab(new Vector3(100*height,100*height,100*height)));
            height++;
        }
    }
    // Process.Start(@"C:\Users\Public\Desktop\Firefox.lnk");
}

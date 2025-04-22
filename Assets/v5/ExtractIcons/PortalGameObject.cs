using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PortalGameObject : MonoBehaviour
{
    GameObject player;
    Main folderPaths;

    public void init(GameObject parent, GameObject player, string path){
        this.player = player;
        rename(path);
        transform.rotation = player.transform.rotation;
        transform.SetParent(parent.transform);
        folderPaths = player.transform.parent.GetComponent<Main>();
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
    public void teleportPlayer(){
        if (transform.name != ""){
            transform.SetParent(null);
            if (folderPaths.currentMap != null) {
                folderPaths.currentDirectoryPath = transform.name;
                StartCoroutine(folderPaths.DeleteMapPrefab(folderPaths.currentMap));
            }
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = Vector3.zero;
            player.GetComponent<CharacterController>().enabled = true;
            StartCoroutine(folderPaths.LoadAndInstantiatePrefab(gameObject.name,gameObject,Vector3.zero));
        }
    }
    // Process.Start(@"C:\Users\Public\Desktop\Firefox.lnk");
}

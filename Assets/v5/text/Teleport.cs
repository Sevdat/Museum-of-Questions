using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject player;
    public GameObject parent;
    Vector3 teleportLocation = new Vector3(507f,10,0);
    void OnTriggerEnter(Collider other)
    {
        print(other.transform.name);
        if (other.transform.name == player.transform.name){
            parent.transform.position = teleportLocation;
        }
    }
}

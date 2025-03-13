using UnityEngine;
 
public class FaceCamera : MonoBehaviour
{
    Camera mainCamera;
    float rotationSpeed = 5f;
 
    void Start(){
        mainCamera = Camera.main;
    }
 
    void LateUpdate(){
        transform.rotation = Quaternion.Slerp(transform.rotation, mainCamera.transform.rotation, rotationSpeed * Time.deltaTime);
    }
}
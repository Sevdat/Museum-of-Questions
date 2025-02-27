using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class RotateCharacter : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachine; // Reference to the Cinemachine camera
    public GameObject player; // Reference to the player object
    public float sensitivityX = 5f; // Sensitivity for horizontal mouse movement
    public float sensitivityY = 5f; // Sensitivity for vertical mouse movement

    private float mouseX; // Change in mouse position on the X-axis
    private float mouseY; // Change in mouse position on the Y-axis
    Vector3 angles;
    float time = 0;
    Vector3 initialPosition;
    AnimateCharacter animateCharacter;

    void Start(){
        initialPosition = transform.position - player.transform.position;
        animateCharacter = player.GetComponent<AnimateCharacter>();
    }
    void Update(){
        // Get mouse input
        mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        // Rotate the camera based on mouse input
        RotateCamera(mouseX, mouseY);
        // Rotate the player to match the camera's horizontal rotation
        RotatePlayerWithCamera();

        transform.position = player.transform.position + initialPosition;
    }
    void FixedUpdate(){
        RotatePlayerLeft();
        RotatePlayerRight();
        RotatePlayerUp();
        RotatePlayerDown();
    }
    private void RotateCamera(float mouseX, float mouseY){
        // Rotate the camera horizontally (around the y-axis)
        transform.Rotate(-mouseY, mouseX, 0);

        // Remove any unintended z-axis rotation
        transform.Rotate(0, 0, -transform.eulerAngles.z);

        clampCameraY();
    }

    private void clampCameraY(){
        angles = transform.eulerAngles;
        //Clamp the Up/Down rotation
        if (angles.x > 180 && angles.x < 275){
            angles.x = 275;
        }
        else if(angles.x < 180 && angles.x > 85){
            angles.x = 85;
        }
        transform.eulerAngles = angles; 
    }

    private void RotatePlayerWithCamera(){
        // Set the player's rotation to match the camera's y-axis rotation
        player.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0, transform.eulerAngles.y, 0));
    }
    private void RotatePlayerLeft(){
        // Set the player's rotation to match the camera's y-axis rotation
        if (animateCharacter.leftPressed) player.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0, transform.eulerAngles.y -90, 0));
    }
    private void RotatePlayerRight(){
        // Set the player's rotation to match the camera's y-axis rotation
        if (animateCharacter.rightPressed) player.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0, transform.eulerAngles.y +90, 0));
    }
    private void RotatePlayerUp(){
        // Set the player's rotation to match the camera's y-axis rotation
        if (animateCharacter.forwardPressed) player.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0, transform.eulerAngles.y, 0));
    }
        private void RotatePlayerDown(){
        // Set the player's rotation to match the camera's y-axis rotation
        if (animateCharacter.backwardPressed) player.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0, transform.eulerAngles.y+180, 0));
    }
}
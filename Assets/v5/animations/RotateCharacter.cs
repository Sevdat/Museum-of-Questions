using UnityEngine;

public class RotateCameraFollow : MonoBehaviour
{
    internal Main folderPaths;
    internal GameObject player; // Reference to the player object
    CharacterController characterController;
    float sensitivityX = 5f; // Sensitivity for horizontal mouse movement
    float sensitivityY = 5f; // Sensitivity for vertical mouse movement

    private float mouseX; // Change in mouse position on the X-axis
    private float mouseY; // Change in mouse position on the Y-axis
    private Vector3 initialPosition;
    
    void Start(){
        initialPosition = transform.position - player.transform.position;
        characterController = player.GetComponent<CharacterController>();
    }

    void LateUpdate(){
        if (folderPaths.allMenuDisabled){
            rotateCharacter();
        }
    }
    private void rotateCharacter(){
        // Get mouse input
        mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        // Rotate the camera based on mouse input
        RotateCamera(mouseX, mouseY);
        transform.position = player.transform.position + initialPosition;
        RotatePlayerWithCamera();
    }
    private void RotateCamera(float mouseX, float mouseY){
        // Rotate the camera horizontally (around the y-axis)
        transform.Rotate(-mouseY, mouseX, 0);

        // Remove any unintended z-axis rotation
        transform.Rotate(0, 0, -transform.eulerAngles.z);

        clampCameraY();
    }

    private void clampCameraY(){
        Vector3 angles = transform.eulerAngles;
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
        float targetYRotation = transform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, targetYRotation, 0);
        characterController.transform.rotation = targetRotation;
    }
}
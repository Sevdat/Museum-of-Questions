using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonFlythrough : MonoBehaviour
{
    float sensitivityX = 5f; // Sensitivity for horizontal mouse movement
    float sensitivityY = 5f; // Sensitivity for vertical mouse movement

    internal float mouseX; // Change in mouse position on the X-axis
    internal float mouseY; // Change in mouse position on the Y-axis
    internal float speed = 0.05f;
    
    internal bool forwardPressed;
    internal bool backwardPressed;
    internal bool leftPressed;
    internal bool rightPressed;
    internal bool upPressed;
    internal bool downPressed;
    internal bool runPressed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        forwardPressed = Input.GetKey(KeyCode.W);
        backwardPressed = Input.GetKey(KeyCode.S);
        leftPressed = Input.GetKey(KeyCode.A);
        rightPressed = Input.GetKey(KeyCode.D);
        downPressed = Input.GetKey(KeyCode.LeftAlt);
        upPressed = Input.GetKey(KeyCode.Space);
        
        runPressed = Input.GetKey(KeyCode.LeftShift);

        speed = runPressed? 0.5f:0.05f;

        if (forwardPressed) transform.position += transform.forward*speed;
        if (backwardPressed) transform.position -= transform.forward*speed;
        if (leftPressed) transform.position -= transform.right*speed;
        if (rightPressed) transform.position += transform.right*speed;
        if (downPressed) transform.position -= transform.up*speed;
        if (upPressed) transform.position += transform.up*speed;

        mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        mouseY = Input.GetAxis("Mouse Y") * sensitivityY;
        rotateCamera(mouseX,mouseY);
    }
    
    private void rotateCamera(float mouseX, float mouseY){
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
}

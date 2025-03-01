using UnityEngine;

public class RotateCharacter : MonoBehaviour
{
    public GameObject fbxGameObject; // Reference to the player object
    Rigidbody fbxRigidBody;
    public float sensitivityX = 5f; // Sensitivity for horizontal mouse movement
    public float sensitivityY = 5f; // Sensitivity for vertical mouse movement

    private float mouseX; // Change in mouse position on the X-axis
    private float mouseY; // Change in mouse position on the Y-axis
    private Vector3 initialPosition;
    private AnimateCharacter animateCharacter;

    void Start(){
        initialPosition = transform.position - fbxGameObject.transform.position;
        animateCharacter = fbxGameObject.GetComponent<AnimateCharacter>();
        fbxRigidBody = fbxGameObject.GetComponent<Rigidbody>();
    }

    void LateUpdate(){
        // Get mouse input
        mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        // Rotate the camera based on mouse input
        RotateCamera(mouseX, mouseY);
        transform.position = Vector3.Lerp(transform.position, fbxGameObject.transform.position + initialPosition, Time.deltaTime * 100);
    }

    void FixedUpdate(){
        // Handle player rotation
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

        // if (animateCharacter.leftPressed) {
        //     targetYRotation -= 90;
        // }
        // if (animateCharacter.rightPressed) {
        //     targetYRotation += 90;
        // }
        // if (animateCharacter.backwardPressed) {
        //     targetYRotation += 180;
        // }

        Quaternion targetRotation = Quaternion.Euler(0, targetYRotation, 0);
        fbxRigidBody.MoveRotation(Quaternion.Slerp(fbxGameObject.transform.rotation, targetRotation, Time.fixedDeltaTime * 100));
    }
}
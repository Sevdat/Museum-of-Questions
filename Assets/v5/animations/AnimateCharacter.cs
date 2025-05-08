using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateCharacter : MonoBehaviour
{   
    internal Main folderPaths;
    CharacterController characterController;
    Animator animator;
    internal float velocityZ = 0.0f;
    internal float velocityX = 0.0f;
    float acceleration = 10.0f;
    float deacceleration = -10.0f;
    float maximumWalkVelocity = 5f;
    float maximumRunVelocity = 10f;
    int VelocityZHash;
    int VelocityXHash;
    internal bool forwardPressed;
    internal bool backwardPressed;
    internal bool leftPressed;
    internal bool rightPressed;
    internal bool runPressed;
    // Start is called before the first frame update
    void Start(){
        animator = GetComponent<Animator>();
        VelocityZHash = Animator.StringToHash("VelocityZ");
        VelocityXHash = Animator.StringToHash("VelocityX");
        characterController = transform.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update(){
        if (folderPaths.allMenuDisabled && folderPaths.thirdPerson.isActiveAndEnabled){
            animateCharacter();
        }
    }

    public void animateCharacter(){
        forwardPressed = Input.GetKey(KeyCode.W);
        backwardPressed = Input.GetKey(KeyCode.S);
        leftPressed = Input.GetKey(KeyCode.A);
        rightPressed = Input.GetKey(KeyCode.D);
        runPressed = Input.GetKey(KeyCode.LeftShift);

        float currentmaxVelocity = runPressed ? maximumRunVelocity: maximumWalkVelocity;

        if (forwardPressed  && velocityZ < currentmaxVelocity ){
            velocityZ += Time.deltaTime *acceleration;
        }

        if (backwardPressed && -velocityZ < currentmaxVelocity){
            velocityZ -= Time.deltaTime *acceleration;
        }

        if (!forwardPressed && !backwardPressed && velocityZ != 0.0f){
            velocityZ += Mathf.Sign(velocityZ)*Time.deltaTime *deacceleration;
            if (Mathf.Abs(velocityZ) < 0.05f) velocityZ = 0.0f;
        }

        if (rightPressed  && velocityX < currentmaxVelocity){
            velocityX += Time.deltaTime *acceleration;
        }
        if (leftPressed && -velocityX < currentmaxVelocity){
            velocityX -= Time.deltaTime *acceleration;
        }
        if (!leftPressed && !rightPressed && velocityX != 0.0f){
            velocityX += Mathf.Sign(velocityX)*Time.deltaTime *deacceleration;
            if (Mathf.Abs(velocityX) < 0.05f) velocityX = 0.0f;
        }

        animator.SetFloat(VelocityZHash, velocityZ);
        animator.SetFloat(VelocityXHash, velocityX);
        run();
    }

    public void run()
    {
        // Calculate the combined movement direction
        Vector3 movementDirection = Vector3.zero;
        float moveSpeed = 0;

        // Forward/Backward movement
        if (forwardPressed || backwardPressed || velocityZ != 0.0f){
            movementDirection += transform.forward * velocityZ;
            moveSpeed = Mathf.Abs(velocityZ);
        }

        // Left/Right movement
        if (leftPressed || rightPressed || velocityX != 0.0f){
            movementDirection += transform.right * velocityX;
            moveSpeed = (moveSpeed + Mathf.Abs(velocityX))/2;
        }

        // Normalize the direction to prevent faster diagonal movement
        if (movementDirection != Vector3.zero){
            movementDirection.Normalize();
        }

        // Apply the velocity to the Rigidbody
        characterController.SimpleMove(movementDirection * moveSpeed);
    }

}

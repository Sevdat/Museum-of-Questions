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
    internal float fly = 0.0f;
    float acceleration = 10.0f;
    float deacceleration = -10.0f;
    float maximumWalkVelocity = 5f;
    float maximumRunVelocity = 10f;
    int VelocityZHash;
    int VelocityXHash;
    int FlyHash;
    internal bool forwardPressed;
    internal bool backwardPressed;
    internal bool leftPressed;
    internal bool rightPressed;
    internal bool runPressed;
    internal bool jump;
    // Start is called before the first frame update
    void Start(){
        animator = GetComponent<Animator>();
        VelocityZHash = Animator.StringToHash("VelocityZ");
        VelocityXHash = Animator.StringToHash("VelocityX");
        FlyHash = Animator.StringToHash("fly");
        characterController = transform.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update(){
        forwardPressed = Input.GetKey(KeyCode.W);
        backwardPressed = Input.GetKey(KeyCode.S);
        leftPressed = Input.GetKey(KeyCode.A);
        rightPressed = Input.GetKey(KeyCode.D);
        runPressed = Input.GetKey(KeyCode.LeftShift);
        jump = Input.GetKey(KeyCode.Space);
        runCharacter();
        if (jump || (fly > 0 && fly <1)) fly += 0.0015f;
        if (!jump && fly > 0) fly -= 0.1f;
        if (fly <0) fly = 0;
        animator.SetFloat(FlyHash, fly);
    }

    public void runCharacter(){
        if (folderPaths.allMenuDisabled && folderPaths.thirdPerson.isActiveAndEnabled){
            animateCharacter();
            motion();
        } else {
            velocityZ = 0;
            velocityX = 0;
        }
        animator.SetFloat(VelocityZHash, velocityZ);
        animator.SetFloat(VelocityXHash, velocityX);
    }

    public void animateCharacter(){

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
    }

    public void motion()
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
        if (fly == 0) characterController.SimpleMove(movementDirection * moveSpeed);
        else characterController.Move(Camera.main.transform.forward * moveSpeed/100);
    }

}

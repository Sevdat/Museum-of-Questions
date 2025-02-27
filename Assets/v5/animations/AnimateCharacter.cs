using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateCharacter : MonoBehaviour
{
    Animator animator;
    internal Rigidbody rigidBody;
    internal float velocityZ = 0.0f;
    internal float velocityX = 0.0f;
    public float acceleration = 2.0f;
    public float deacceleration = -2.0f;
    public float maximumWalkVelocity = 0.5f;
    public float maximumRunVelocity = 2.0f;
    public int VelocityZHash;
    public int VelocityXHash;
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
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
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

        if ((leftPressed || rightPressed)  && velocityX < currentmaxVelocity ){
            velocityX += Time.deltaTime *acceleration;
            print(velocityX);
        }
        if (!leftPressed && !rightPressed && velocityX != 0.0f){
            velocityX += Mathf.Sign(velocityX)*Time.deltaTime *deacceleration;
            if (Mathf.Abs(velocityX) < 0.05f) velocityX = 0.0f;
        }

        animator.SetFloat(VelocityZHash, velocityZ);
        animator.SetFloat(VelocityXHash, velocityX);
    }

    void FixedUpdate(){
        run();
    }


    public void run(){
        if (forwardPressed || backwardPressed || velocityZ != 0.0f) {
            rigidBody.velocity = rigidBody.transform.forward *velocityZ;
        }
        if (leftPressed || rightPressed || velocityZ != 0.0f) {
            rigidBody.velocity = rigidBody.transform.forward *velocityX;
        }
    }

}

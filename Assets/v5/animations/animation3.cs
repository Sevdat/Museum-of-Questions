using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation3 : MonoBehaviour
{
    Animator animator;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    public float acceleration = 2.0f;
    public float deacceleration = 2.0f;
    public float maximumWalkVelocity = 0.5f;
    public float maximumRunVelocity = 2.0f;
    public int VelocityZHash;
    public int VelocityXHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        VelocityZHash = Animator.StringToHash("VelocityZ");
        VelocityXHash = Animator.StringToHash("VelocityX");
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        float curretmaxVelocity = runPressed ? maximumRunVelocity: maximumWalkVelocity;

        if (forwardPressed && velocityZ<curretmaxVelocity){
            velocityZ += Time.deltaTime *acceleration;
        }
        if (leftPressed && velocityX>-curretmaxVelocity){
            velocityX -= Time.deltaTime *acceleration;
        }
        if (rightPressed && velocityX<curretmaxVelocity){
            velocityX += Time.deltaTime *acceleration;
        }
        if (!forwardPressed && velocityZ >0.0f){
            velocityZ -= Time.deltaTime*acceleration;
        }
        if (!forwardPressed && velocityZ <0.0f){
            velocityZ = 0.0f;
        }
        if (!leftPressed && velocityX <0.0f){
            velocityX += Time.deltaTime *deacceleration;
        }
        if (!rightPressed && velocityX >0.0f){
            velocityX -= Time.deltaTime *deacceleration;
        }
        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX <0.05f)){
            velocityX = 0.0f;
        }
        if (forwardPressed && runPressed && velocityZ > curretmaxVelocity){
            velocityZ = curretmaxVelocity;
        } else if (forwardPressed && velocityZ> curretmaxVelocity){
            velocityZ -= Time.deltaTime * deacceleration;
        } else if (forwardPressed && velocityZ <curretmaxVelocity && velocityZ >curretmaxVelocity - 0.05f){
            velocityZ = curretmaxVelocity;
        }
        animator.SetFloat(VelocityZHash, velocityZ);
        animator.SetFloat(VelocityXHash, velocityX);
        
    }
}

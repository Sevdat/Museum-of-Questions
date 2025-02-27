using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation2 : MonoBehaviour
{
    Animator animator;
    float velocity = 0.0f;
    public float acceleration = 0.1f;
    int VelocityHash;
    void Start()
    {
        animator = GetComponent<Animator>();
        VelocityHash = Animator.StringToHash("Velocity");
    }

    // Update is called once per frame
    void Update()
    {
       bool forwardPressed = Input.GetKey("w");
       bool runPressed = Input.GetKey("left shift");

       if (forwardPressed && velocity <1.0f){
            velocity += Time.deltaTime * acceleration;
       }  
       if (!forwardPressed && velocity >0.0f){
            velocity -= Time.deltaTime * acceleration;
       }
        if (!forwardPressed && velocity <0.0f){
            velocity = 0.0f;
        }
       animator.SetFloat(VelocityHash,velocity);
    }
}

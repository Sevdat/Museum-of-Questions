using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        print(animator); 
    }

    // Update is called once per frame
    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");
        if (!isWalking && forwardPressed){
            animator.SetBool(isWalkingHash,true);
        }
        if (isWalking && !forwardPressed){
            animator.SetBool(isWalkingHash,false);
        }
        if (!isRunning && (forwardPressed && runPressed)){
            animator.SetBool(isRunningHash,true);
        }
        if (isRunning && (!forwardPressed || !runPressed)){
            animator.SetBool(isRunningHash,false);
        }
    }
}

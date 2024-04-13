using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class Body : MonoBehaviour
{
    // Start is called before the first frame update
    bodyStructure joints;
    public class bodyStructure : WorldBuilder{
        public Vector3 globalAngles;
        public Vector3[] tempBody;
        public Vector3[] globalBody = new Vector3[]{
         new Vector3(20f,18f,20f),
         new Vector3(20f,12f,20f),
         new Vector3(20f,4f,20f),
         new Vector3(20f,2f,20f),
         new Vector3(20f,2f,25f)
        };
        public Vector3 localHipAngle;
        public int[] hip = new int[]{0,1,2,3,4};
        public Vector3 localKneeAngle;
        public int[] knee = new int[]{1,2,3,4};
        public Vector3 localFootAngle;
        public int[] foot = new int[]{2,3,4};

        public Vector3[] moveHip(float alphaAngles, Vector3 ax){
            tempBody = BodyCreator.movePart(alphaAngles,hip,ax,globalBody,tempBody);
            localHipAngle = EularClass.getAngles(globalBody[hip[0]],globalBody[hip[1]]);
            return tempBody;
        }
        public void moveKnee(float alphaAngles){
            tempBody = BodyCreator.movePart(alphaAngles,knee,localKneeAngle,globalBody,tempBody);
            localKneeAngle = EularClass.getAngles(globalBody[knee[0]],globalBody[knee[1]]);
        }
        public void moveFoot(float alphaAngles){
            tempBody = BodyCreator.movePart(alphaAngles,foot,localFootAngle,globalBody,tempBody);
            localFootAngle = EularClass.getAngles(globalBody[foot[0]],globalBody[foot[1]]);
        }
        public void initBody(){
            tempBody = globalBody; //Dangerous because it links
            globalAngles = new Vector3(0,0,0);
            localHipAngle = EularClass.getAngles(globalBody[hip[0]],globalBody[hip[1]]);
            localKneeAngle = EularClass.getAngles(globalBody[knee[0]],globalBody[knee[1]]);
            localFootAngle = EularClass.getAngles(globalBody[foot[0]],globalBody[foot[1]]);
        }
        public void drawBody(){
            BitArrayManipulator.createOrDeleteObject(tempBody, true);
        }
    }  
    void Start(){
        joints = new bodyStructure(){ 
        };
        joints.initBody();
        joints.moveHip(-50f,new Vector3(1f,0f,0f));
        joints.globalBody = joints.tempBody;
        joints.drawBody();
    }
    Vector3 axi = new Vector3(0f,0f,1f);
    float angle = 0;
    float time = 0;
    void Update(){
        time += Time.deltaTime;
        if (time >0.01f){
            angle +=1f;
            joints.moveHip(angle,axi);
            joints.drawBody();
            time = 0f;
        }
    }
}
        // chest = WorldBuilder.moveObject(
        //     new Vector3(0f,0f,-1f),chest
        // );
        // chest = WorldBuilder.rotateObject(
        //     0,1,WorldBuilder.rotateZ,move,chest
        // );
        // WorldBuilder.createOrDeleteObject(joints.hip,true);

    //         IEnumerator Lol(){
    //     yield return joints.moveHipY();
    //     yield return joints.moveHipZ();
    // }

    //     WorldBuilder.createOrDeleteObject(joints.globalBody, false);
    // print(joints.localKneeAngle);
    // joints.moveKnee(xyMove(xAngle,zAngle));
    // joints.tempArray(joints.globalBody,0.1f);
    // joints.drawBody();
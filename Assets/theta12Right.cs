using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.Utility;
using System;

public class theta12Right : MonoBehaviour
{
    bool initialTrackersRecorded = false;
    RigidPose chest;
    RigidPose rawInitialTrackerChest;
    RigidPose rawTrackerChest;
    RigidPose initialTrackerElbowUp;
    RigidPose trackerElbowUp;
    RigidPose  elbowUp;
    Vector3 eulerElbowUp;
    Vector3 fEulerElbowUp;
    Matrix4x4 theta4RM;
    Int16 theta4;
    Int16 theta1;
    Int16 theta2;
    Vector3 refVecMinusYFixed = new Vector3(0,-1,0);
    Vector3 refVecMinusXFixed = new Vector3(1,0,0);
    Vector3 refVecMinusY;
    Vector3 refVecMinusX;
    Vector3 vecElbow;
    Vector3 vecElbowFw;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rawTrackerChest = VivePose.GetPoseEx(TrackerRole.Tracker1);
        trackerElbowUp = VivePose.GetPoseEx(TrackerRole.Tracker2);
        if(Input.GetKeyDown("b")){
            if(VivePose.IsValidEx(TrackerRole.Tracker1) && VivePose.IsValidEx(DeviceRole.Hmd) && VivePose.IsValidEx(TrackerRole.Tracker2))// && VivePose.IsValidEx(TrackerRole.Tracker3))
            {
                Debug.Log("Theta 1 & 2 calibrated with current pose.");
                rawInitialTrackerChest = rawTrackerChest;
                initialTrackerElbowUp = trackerElbowUp;
                initialTrackerElbowUp.rot = Quaternion.Inverse(initialTrackerElbowUp.rot) * rawInitialTrackerChest.rot;
                initialTrackersRecorded = true;
            }
        }

        if(true){
        // if(Input.GetKeyDown("c")){
            //Debug.Log("TestR");
            if(initialTrackersRecorded && VivePose.IsValidEx(TrackerRole.Tracker1) && VivePose.IsValidEx(DeviceRole.Hmd) && VivePose.IsValidEx(TrackerRole.Tracker2))// && VivePose.IsValidEx(TrackerRole.Tracker3))
            {
                chest.rot = Quaternion.Inverse(rawTrackerChest.rot) * rawInitialTrackerChest.rot;
                elbowUp.rot = trackerElbowUp.rot * initialTrackerElbowUp.rot;
                elbowUp.rot = Quaternion.Inverse(elbowUp.rot) * rawTrackerChest.rot;
                
                refVecMinusY = chest.rot * refVecMinusYFixed;

                vecElbow = elbowUp.rot * refVecMinusYFixed;
                refVecMinusXFixed.x = vecElbow.x;
                refVecMinusXFixed.y = vecElbow.y;
                refVecMinusX = chest.rot * refVecMinusXFixed;
                // vecElbowFw = elbowUp.rot * refVecMinusXFixed;

                theta1 = Convert.ToInt16(Vector3.Angle(refVecMinusY,vecElbow));
                theta2 = Convert.ToInt16(Vector3.Angle(refVecMinusX,vecElbow));
                print("T1: "+theta1+"         T2: "+theta2);
                
                fEulerElbowUp = elbowUp.rot.eulerAngles;
                eulerElbowUp.x = Convert.ToInt16(fEulerElbowUp.x);
                eulerElbowUp.y = Convert.ToInt16(fEulerElbowUp.y);
                eulerElbowUp.z = Convert.ToInt16(fEulerElbowUp.z);


                if(eulerElbowUp.x>180){
                    eulerElbowUp.x -= 360;
                }
                if(eulerElbowUp.y>180){
                    eulerElbowUp.y -= 360;
                }
                if(eulerElbowUp.z>180){
                    eulerElbowUp.z -= 360;
                }
                // Debug.Log(eulerElbowUp.x+","+eulerElbowUp.y+","+eulerElbowUp.z);

                // theta4RM =  Matrix4x4.Rotate(elbowDown.rot);
                // theta4 = Convert.ToInt16(Mathf.Asin(theta4RM[2,1])*Mathf.Rad2Deg);
                // if(theta4RM[1,1]<0.0){
                //     theta4 = Convert.ToInt16(180-theta4);
                // }
                // /*
                //     x   y   z
                // x   00  01  02
                // y   10  11  12
                // z   20  21  22
                // */
                //  Debug.Log(theta4);
            }
        }
    }
}
        
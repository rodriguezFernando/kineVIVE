using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.Utility;
using System;

public class theta4Right : MonoBehaviour
{
    bool initialTrackersRecorded = false;
    RigidPose rawInitialTrackerChest;
    RigidPose rawTrackerChest;
    RigidPose initialTrackerElbowUp;
    RigidPose trackerElbowUp;
    RigidPose  elbowUp;
    Vector3 eulerElbowUp;
    Vector3 fEulerElbowUp;
    RigidPose initialTrackerElbowDown;
    RigidPose trackerElbowDown;
    RigidPose  elbowDown;
    public Vector3 eulerElbowDown;
    Vector3 fEulerElbowDown;
    Matrix4x4 theta4RM;
    Int16 theta4;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rawTrackerChest = VivePose.GetPoseEx(TrackerRole.Tracker1);
        trackerElbowUp = VivePose.GetPoseEx(TrackerRole.Tracker5);
        trackerElbowDown = VivePose.GetPoseEx(TrackerRole.Tracker4);
        if(Input.GetKeyDown("b")){
            if(VivePose.IsValidEx(TrackerRole.Tracker1) && VivePose.IsValidEx(DeviceRole.Hmd) && VivePose.IsValidEx(TrackerRole.Tracker2))// && VivePose.IsValidEx(TrackerRole.Tracker3))
            {
                Debug.Log("Theta 4 calibrated with current pose.");
                rawInitialTrackerChest = rawTrackerChest;
                initialTrackerElbowUp = trackerElbowUp;
                initialTrackerElbowDown = trackerElbowDown;
                initialTrackerElbowDown.rot = Quaternion.Inverse(initialTrackerElbowDown.rot) * initialTrackerElbowUp.rot;
                initialTrackerElbowUp.rot = Quaternion.Inverse(initialTrackerElbowUp.rot) * rawInitialTrackerChest.rot;
                initialTrackersRecorded = true;
            }
        }

        if(Input.GetKeyDown("c")){
            //Debug.Log("TestR");
            if(initialTrackersRecorded && VivePose.IsValidEx(TrackerRole.Tracker1) && VivePose.IsValidEx(DeviceRole.Hmd) && VivePose.IsValidEx(TrackerRole.Tracker2))// && VivePose.IsValidEx(TrackerRole.Tracker3))
            {
                elbowUp.rot = trackerElbowUp.rot * initialTrackerElbowUp.rot;
                elbowUp.rot = Quaternion.Inverse(elbowUp.rot) * rawTrackerChest.rot;
                elbowDown.rot = trackerElbowDown.rot * initialTrackerElbowDown.rot;
                elbowDown.rot = Quaternion.Inverse(elbowDown.rot) * trackerElbowUp.rot;
                
                fEulerElbowUp = elbowUp.rot.eulerAngles;
                eulerElbowUp.x = Convert.ToInt16(fEulerElbowUp.x);
                eulerElbowUp.y = Convert.ToInt16(fEulerElbowUp.y);
                eulerElbowUp.z = Convert.ToInt16(fEulerElbowUp.z);

                fEulerElbowDown = elbowDown.rot.eulerAngles;
                eulerElbowDown.x = Convert.ToInt16(fEulerElbowDown.x);
                eulerElbowDown.y = Convert.ToInt16(fEulerElbowDown.y);
                eulerElbowDown.z = Convert.ToInt16(fEulerElbowDown.z);

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

                if(eulerElbowDown.x>180){
                    eulerElbowDown.x -= 360;
                }
                if(eulerElbowDown.y>180){
                    eulerElbowDown.y -= 360;
                }
                if(eulerElbowDown.z>180){
                    eulerElbowDown.z -= 360;
                }
                Debug.Log("Theta4: "+eulerElbowDown.x+","+eulerElbowDown.y+","+eulerElbowDown.z);

                theta4RM =  Matrix4x4.Rotate(elbowDown.rot);
                theta4 = Convert.ToInt16(Mathf.Asin(theta4RM[2,1])*Mathf.Rad2Deg);
                if(theta4RM[1,1]<0.0){
                    theta4 = Convert.ToInt16(180-theta4);
                }
                /*
                    x   y   z
                x   00  01  02
                y   10  11  12
                z   20  21  22
                */
                 Debug.Log(theta4);
            }
        }
    }
}
        
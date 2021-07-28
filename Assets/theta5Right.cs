using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.Utility;
using System;

public class theta5Right : MonoBehaviour
{
    bool initialTrackersRecorded = false;
    RigidPose rawInitialTrackerChest;
    RigidPose rawTrackerChest;
    RigidPose initialTrackerForearmUp;
    RigidPose trackerForearmUp;
    RigidPose  forearmUp;
    Vector3 eulerForearmUp;
    Vector3 fEulerForearmUp;
    RigidPose initialTrackerForearmDown;
    RigidPose trackerForearmDown;
    RigidPose  forearmDown;
    public Vector3 eulerForearmDown;
    Vector3 fEulerForearmDown;
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
        trackerForearmUp = VivePose.GetPoseEx(TrackerRole.Tracker4);
        trackerForearmDown = VivePose.GetPoseEx(TrackerRole.Tracker3);
        if(Input.GetKeyDown("b")){
            if(VivePose.IsValidEx(TrackerRole.Tracker1) && VivePose.IsValidEx(DeviceRole.Hmd) && VivePose.IsValidEx(TrackerRole.Tracker2))// && VivePose.IsValidEx(TrackerRole.Tracker3))
            {
                Debug.Log("Theta 5 calibrated with current pose.");
                rawInitialTrackerChest = rawTrackerChest;
                initialTrackerForearmUp = trackerForearmUp;
                initialTrackerForearmDown = trackerForearmDown;
                initialTrackerForearmDown.rot = Quaternion.Inverse(initialTrackerForearmDown.rot) * initialTrackerForearmUp.rot;
                initialTrackerForearmUp.rot = Quaternion.Inverse(initialTrackerForearmUp.rot) * rawInitialTrackerChest.rot;
                initialTrackersRecorded = true;
            }
        }

        if(Input.GetKeyDown("c")){
            //Debug.Log("TestR");
            if(initialTrackersRecorded && VivePose.IsValidEx(TrackerRole.Tracker1) && VivePose.IsValidEx(DeviceRole.Hmd) && VivePose.IsValidEx(TrackerRole.Tracker2))// && VivePose.IsValidEx(TrackerRole.Tracker3))
            {
                forearmUp.rot = trackerForearmUp.rot * initialTrackerForearmUp.rot;
                forearmUp.rot = Quaternion.Inverse(forearmUp.rot) * rawTrackerChest.rot;
                forearmDown.rot = trackerForearmDown.rot * initialTrackerForearmDown.rot;
                forearmDown.rot = Quaternion.Inverse(forearmDown.rot) * trackerForearmUp.rot;
                
                fEulerForearmUp = forearmUp.rot.eulerAngles;
                eulerForearmUp.x = Convert.ToInt16(fEulerForearmUp.x);
                eulerForearmUp.y = Convert.ToInt16(fEulerForearmUp.y);
                eulerForearmUp.z = Convert.ToInt16(fEulerForearmUp.z);

                fEulerForearmDown = forearmDown.rot.eulerAngles;
                eulerForearmDown.x = Convert.ToInt16(fEulerForearmDown.x);
                eulerForearmDown.y = Convert.ToInt16(fEulerForearmDown.y);
                eulerForearmDown.z = Convert.ToInt16(fEulerForearmDown.z);

                if(eulerForearmUp.x>180){
                    eulerForearmUp.x -= 360;
                }
                if(eulerForearmUp.y>180){
                    eulerForearmUp.y -= 360;
                }
                if(eulerForearmUp.z>180){
                    eulerForearmUp.z -= 360;
                }
                // Debug.Log(eulerForearmUp.x+","+eulerForearmUp.y+","+eulerForearmUp.z);

                if(eulerForearmDown.x>180){
                    eulerForearmDown.x -= 360;
                }
                if(eulerForearmDown.y>180){
                    eulerForearmDown.y -= 360;
                }
                if(eulerForearmDown.z>180){
                    eulerForearmDown.z -= 360;
                }
                Debug.Log("Theta5: "+ eulerForearmDown.x+","+eulerForearmDown.y+","+eulerForearmDown.z);

                // theta4RM =  Matrix4x4.Rotate(forearmDown.rot);
                // theta4 = (Int16)Mathf.Asin(theta4RM[2,1])*Mathf.Rad2Deg;
                // if(theta4RM[1,1]<0){
                //     theta4 = 180-theta4;
                // }
                /*
                    x   y   z
                x   00  01  02
                y   10  11  12
                z   20  21  22
                */
                // Debug.Log(theta4);
            }
        }
    }
}

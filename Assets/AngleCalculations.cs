using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.Utility;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
//using System.IO.Ports;
using System.Threading;

public class AngleCalculations : MonoBehaviour
{
    bool testArmR = true;
    bool testArmL = false;
    bool testHead = false;
    bool calibTrackersRecorded = false;
    RigidPose trackerChest;
    RigidPose calibTrackerChest;
    RigidPose chest;
    RigidPose trackerShoulderR;
    RigidPose calibTrackerShoulderR_chest;
    RigidPose shoulderR_chest;
    RigidPose theta2ShoulderR_chest;
    RigidPose trackerShoulderL;
    RigidPose calibTrackerShoulderL;
    RigidPose shoulderL;
    RigidPose trackerElbowR;
    RigidPose calibTrackerElbowR_chest;
    RigidPose elbowR_chest;
    RigidPose calibTrackerElbowR_shoulderR;
    RigidPose elbowR_shoulderR;
    RigidPose trackerElbowL;
    RigidPose calibTrackerElbowL;
    RigidPose elbowL;
    RigidPose trackerForearmR;
    RigidPose calibTrackerForearmR_chest;
    RigidPose forearmR_chest;
    RigidPose calibTrackerForearmR_elbowR;
    RigidPose forearmR_elbowR;
    RigidPose trackerForearmL;
    RigidPose calibTrackerForearmL;
    RigidPose forearmL;
    RigidPose trackerWristR;
    RigidPose calibTrackerWristR_forearmR;
    RigidPose wristR_forearmR;
    RigidPose trackerWristL;
    RigidPose calibTrackerWristL;
    RigidPose wristL;
    Vector3 refVecMinusYFixed = new Vector3(0,-1,0);
    Vector3 refVecPlusZFixed = new Vector3(0,0,1);
    Vector3 refVecMinusY;
    Vector3 refVecPlusZ;
    Vector3 refVecForTheta2Fixed;
    Vector3 refVecForTheta2;
    Vector3 vecElbowR;
    Vector3 vecElbowRForTheta2;
    Int16 theta1R;
    Int16 theta2R;
    Int16 theta2Rmod;
    Int16 theta3R;
    Int16 theta4R;
    Matrix4x4 theta4R_auxMat;
    Int16 theta5R;
    Int16 theta6R;
    string [] serialNums;


    // uint chestIndex = 2;
    // uint shoulderRIndex = 5;
    // uint elbowRIndex = 6;
    // uint forearmRIndex = 7;
    // uint wristRIndex = 8;

    Transform spChest;
    Transform spShoulderR;
    Transform spElbowR;
    Transform spForearmR;
    Transform spWristR;
    Transform spOrigin;
    Transform spRef;
    Transform spComp;
    float spScale;

    //UdpClient client = new UdpClient();

    void extract_poses()
    {
        //Debug.Log(myRoles.Chest);
        // for (uint i=1; i<=5; i++)
        // {
        //     if(String.Equals(myRoles.Chest,serialNums[i]))
        //     {
        //         trackerChest = VivePose.GetPose(i);
        //     }
        // }
        // if(testArmR)
        // {
        //     for (uint i=1; i<=5; i++)
        //     {
        //         if(String.Equals(myRoles.ShoulderR,serialNums[i]))
        //         {
        //             trackerShoulderR = VivePose.GetPose(i);
        //         }else{
        //             if(String.Equals(myRoles.ElbowR,serialNums[i]))
        //             {
        //                 trackerElbowR = VivePose.GetPose(i);
        //             }else{
        //                 if(String.Equals(myRoles.ForearmR,serialNums[i]))
        //                 {
        //                     trackerForearmR = VivePose.GetPose(i);
        //                 }
        //             }
        //         }
        //     }
        // }

        trackerChest = VivePose.GetPoseEx(TrackerRole.Tracker1);
        spChest.position = trackerChest.pos;
        // trackerChest = VivePose.GetPose(chestIndex);
        if(testArmR)
        {
            trackerShoulderR = VivePose.GetPoseEx(TrackerRole.Tracker3);
            trackerElbowR = VivePose.GetPoseEx(TrackerRole.Tracker2);
            spElbowR.position = trackerElbowR.pos;
            trackerForearmR = VivePose.GetPoseEx(TrackerRole.Tracker4);
            trackerWristR = VivePose.GetPoseEx(TrackerRole.Tracker5);
            // trackerShoulderR = VivePose.GetPose(shoulderRIndex);
            // trackerElbowR = VivePose.GetPose(elbowRIndex);
            // trackerForearmR = VivePose.GetPose(forearmRIndex);
            // trackerWristR = VivePose.GetPose(wristRIndex);
        }
    }
    void calibrate()
    {
        extract_poses();
        // calib_chest();
        Debug.Log("Chest calibrated.");
        calibTrackerChest = trackerChest;
        if(testArmR)
        {
            calibTrackerShoulderR_chest = trackerShoulderR;
            calibTrackerShoulderR_chest.rot = Quaternion.Inverse(calibTrackerShoulderR_chest.rot) * calibTrackerChest.rot;
            Debug.Log("Shoulder calibrated.");

            // calibTrackerElbowR_chest = trackerElbowR;
            // calibTrackerElbowR_chest.rot = Quaternion.Inverse(calibTrackerElbowR_chest.rot)*calibTrackerChest.rot;
            calibTrackerElbowR_chest = trackerElbowR;
            calibTrackerElbowR_chest.rot = Quaternion.Inverse(calibTrackerElbowR_chest.rot)*calibTrackerChest.rot;

            calibTrackerElbowR_shoulderR = trackerElbowR;
            calibTrackerElbowR_shoulderR.rot = Quaternion.Inverse(calibTrackerElbowR_shoulderR.rot)*trackerShoulderR.rot;
            Debug.Log("Elbow calibrated.");

            calibTrackerForearmR_chest = trackerForearmR;
            calibTrackerForearmR_chest.rot = Quaternion.Inverse(calibTrackerForearmR_chest.rot)*calibTrackerChest.rot;
            
            calibTrackerForearmR_elbowR = trackerForearmR;
            calibTrackerForearmR_elbowR.rot = Quaternion.Inverse(calibTrackerForearmR_elbowR.rot)*trackerElbowR.rot;
            Debug.Log("Forearm calibrated.");

            calibTrackerWristR_forearmR = trackerWristR;
            calibTrackerWristR_forearmR.rot = Quaternion.Inverse(calibTrackerWristR_forearmR.rot)*trackerForearmR.rot;
            Debug.Log("Wrist calibrated.");
        }
        calibTrackersRecorded = true;
    }
    void adjust_meas()
    {
        extract_poses();
        chest.rot = Quaternion.Inverse(trackerChest.rot)*calibTrackerChest.rot;
        chest.rot = Quaternion.Inverse(chest.rot);

        if(testArmR)
        {
            shoulderR_chest.rot = trackerShoulderR.rot*calibTrackerShoulderR_chest.rot;
            shoulderR_chest.rot = Quaternion.Inverse(shoulderR_chest.rot)*trackerChest.rot;

            elbowR_chest.rot = trackerElbowR.rot*calibTrackerElbowR_chest.rot;
            elbowR_chest.rot = Quaternion.Inverse(elbowR_chest.rot)*trackerChest.rot;
            elbowR_chest.rot = Quaternion.Inverse(elbowR_chest.rot);
            
            // elbowR_chest.rot = trackerElbowR.rot*calibTrackerElbowR_chest.rot;
            // elbowR_chest.rot = Quaternion.Inverse(elbowR_chest.rot)*Quaternion.Inverse(trackerChest.rot);

            elbowR_shoulderR.rot = trackerElbowR.rot*calibTrackerElbowR_shoulderR.rot;
            elbowR_shoulderR.rot = Quaternion.Inverse(elbowR_shoulderR.rot)*trackerShoulderR.rot;

            forearmR_elbowR.rot = trackerForearmR.rot*calibTrackerForearmR_elbowR.rot;
            forearmR_elbowR.rot = Quaternion.Inverse(forearmR_elbowR.rot)*trackerElbowR.rot;
            forearmR_elbowR.rot = Quaternion.Inverse(forearmR_elbowR.rot);

            wristR_forearmR.rot = trackerWristR.rot*calibTrackerWristR_forearmR.rot;
            wristR_forearmR.rot = Quaternion.Inverse(wristR_forearmR.rot)*trackerForearmR.rot;

            theta2ShoulderR_chest.rot = trackerShoulderR.rot*calibTrackerShoulderR_chest.rot;
            theta2ShoulderR_chest.rot = Quaternion.Inverse(theta2ShoulderR_chest.rot)*trackerChest.rot;
        }
    }
    void calculate_torso()
    {
        // euler
    }
    void calculate_theta_1_R()
    {
        refVecMinusY = chest.rot * refVecMinusYFixed;
        vecElbowR = elbowR_chest.rot * refVecMinusY;
        // spRef.position = refVecMinusY;
        // spComp.position = vecElbowR;
        theta1R = Convert.ToInt16(Vector3.Angle(refVecMinusY,vecElbowR));
        print("T1: "+theta1R);
    }
    void calculate_theta_2_R()
    {
        refVecPlusZ = chest.rot*(-refVecPlusZFixed);
        refVecPlusZ.y = 0;
        vecElbowRForTheta2 = vecElbowR;
        vecElbowRForTheta2 = chest.rot*vecElbowRForTheta2;
        vecElbowRForTheta2.y = 0;

        // spRef.position = refVecPlusZ;
        // spComp.position = vecElbowRForTheta2;
        
    //theta2R = Convert.ToInt16(Vector3.Angle(refVecPlusZ,vecElbowRForTheta2));
        theta2R = Convert.ToInt16(theta2ShoulderR_chest.rot.eulerAngles.y);
        // if (theta1R<20 && theta2R<20)
        // {
        //     theta2R = 10;
        // }
        // if (theta2R>150)
        //     {
        //         theta2R -= 180;
        //     }
        print("T2: "+theta2R);
    }
    void calculate_theta_3_R()
    {
        theta3R = Convert.ToInt16(elbowR_shoulderR.rot.eulerAngles.x);
        if(theta3R>180){
            theta3R -= 360;
        }
        print("T3: "+theta3R);
    }
    void calculate_theta_4_R()
    {
        theta4R = Convert.ToInt16(Quaternion.Angle(Quaternion.identity,forearmR_elbowR.rot));
        print("T4: "+theta4R);
    }
    void calculate_theta_5_R()
    {
        theta5R = Convert.ToInt16(wristR_forearmR.rot.eulerAngles.y);
        if(theta5R>180){
            theta5R -= 360;
        }
        print("T5: "+theta5R);
    }
    void calculate_theta_6_R()
    {
        // theta6R = Convert.ToInt16(Quaternion.Angle(Quaternion.identity,handR_wristR.rot));
        theta6R = Convert.ToInt16(wristR_forearmR.rot.eulerAngles.z);
        if(theta6R>180){
            theta6R -= 360;
        }
        print("T6: "+theta6R);
    }
    
    public Roles myRoles;

    void Awake()
    {
        string myLoadedRoles = JsonFileReader.LoadJsonAsResource("Roles/config.json");
        myRoles = JsonUtility.FromJson<Roles>(myLoadedRoles);
        //serialNums = serialNumbers.getSerialNumbers();
    }
    void Start()
    {
        //client.Connect(new IPEndPoint(IPAddress.Parse("192.168.0.177"),8080));
        //se llama a la funcion que reune los numeros seriales y se imprimen uno por uno
        // foreach (var item in serialNums)
        // {
        //     Debug.Log(item); //por testear, deberia funcionar aun si no estan todos los trackers conectados.
        // //     //Strat: un script para recoger todos los numeros seriales de los trackers 1 al 9 y  construir un array con ellos (HECHO)
        // //     //iterar entre los elementos del array y compararlo con los roles predefenidos por un archivo .json (IN WORK: ##TODO definir si eso va en otro script, se puso aqui porque este era el principal en versiones anteriores)
        // //     //llamar a funcion para asignar el dispositivo segun requiera el joint (NOT YET)

        // //     //TO DO: Archivo .json tendra un registro arbitrario de los numeros de serie, los roles que ocuparan y un ID para cada tracker
        // //     //La idea es facilitar el reconocimiento

        
        // }
        spScale = 0.1f;
        spChest = NewSphere(Vector3.one*spScale,Vector3.zero,Color.white);
        // spChest.x = spChest.y;
        spShoulderR = NewSphere(Vector3.one*spScale,Vector3.zero,Color.white);
        spElbowR = NewSphere(Vector3.one*spScale,Vector3.zero,Color.white);
        spForearmR = NewSphere(Vector3.one*spScale,Vector3.zero,Color.white);
        spWristR = NewSphere(Vector3.one*spScale,Vector3.zero,Color.white);
        spOrigin = NewSphere(Vector3.one*spScale,Vector3.zero,Color.magenta);
        spRef = NewSphere(Vector3.one*spScale,Vector3.zero,Color.cyan);
        spComp = NewSphere(Vector3.one*spScale,Vector3.zero,Color.yellow);
    }

    // Update is called once per frame
    void Update()
    {
        extract_poses();
        if(Input.GetKeyDown("l"))
        {
            calibrate();
        }
        if(calibTrackersRecorded){
            adjust_meas();
        }
        if(//Input.GetKeyDown("q") && 
        calibTrackersRecorded)
        {
            if(testArmR)
            {
                calculate_theta_1_R();
                calculate_theta_2_R();
                //calculate_theta_3_R();
                //calculate_theta_4_R();
                // calculate_theta_5_R();
                // calculate_theta_6_R();
            }
        }
        /* 
            if(calibTrackersRecorded// && Input.GetKeyDown("1")
                ){
                    calculate_theta_1_R();
                }
            if(calibTrackersRecorded// && Input.GetKeyDown("2")
                ){
                    calculate_theta_2_R();
                }
            if(calibTrackersRecorded && Input.GetKeyDown("3")
                ){
                    calculate_theta_3_R();
                }
            if(calibTrackersRecorded && Input.GetKeyDown("4")
                ){
                    calculate_theta_4_R();
                }
            if(calibTrackersRecorded// && Input.GetKeyDown("5")
                ){
                    calculate_theta_5_R();
                }
            if(calibTrackersRecorded// && Input.GetKeyDown("6")
                ){
                    calculate_theta_6_R();
                } 
        */
    }
    
    static Transform NewSphere(Vector3 scale, Vector3 position, Color color) {
        Transform shape = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        UnityEngine.Object.Destroy(shape.GetComponent<Collider>()); // no collider, please!
        shape.localScale = scale; // set the rectangular volume size
        shape.position = position;
        //shape.rotation = Quaternion.LookRotation(dir, p2b - p1b);
        shape.GetComponent<Renderer>().material.color = color;
        shape.GetComponent<Renderer>().enabled = true; // show it
        return shape;
    }
}

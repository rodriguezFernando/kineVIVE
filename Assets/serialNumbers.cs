using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.VRModuleManagement;

public class serialNumbers : MonoBehaviour
{
    private static uint index; 
    private static string sn = "LHR-DB6A8653"; 
    
    // Start is called before the first frame update
    public static string[] getSerialNumbers() //funcion para guardar todos los numeros seriales en un solo array de strings
    {
        string[] res = new string[10]; //aunque solo esten conectados 3 trackers por ejem. SteamVR guarda un registro de todos los trackers que han sido conectados, independientemente de su orden de conexion
        for (uint i=1; i<=5; i++) //puede que a un tracker con el ID 1 se le haya acabado la pila y los siguientes trackers por ejem. empiezan a contar del prox num (2,3,4 ...)
        {
            if (index != i) //con esta comparacion nos aseguramos de jalar todos los IDs aunque no esten conectados (HACE FALTA TESTEARLOOOOO)
                {
                    //teoricamente podemos ponerle de rango hasta los 9 trackers y no deberia haber problemas
                    index = i;
                    if (VRModule.IsValidDeviceIndex(index))
                    {
                        var deviceState = VRModule.GetDeviceState(index);
                        //Debug.Log(deviceState.serialNumber);
                        if (String.Equals(deviceState.serialNumber.ToString(),sn))
                        {
                            print("yes");
                        }
                        res[i] = deviceState.serialNumber.ToString();    
                }
            }
        }
        return res;
    }
}

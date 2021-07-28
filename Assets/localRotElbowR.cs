using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localRotElbowR : MonoBehaviour
{
    //float localX=0;
    Vector3 vectConv;
    Vector3 refVectNew = new Vector3(0,-1,0);
    Vector3 refVect2New = new Vector3(0,0,1);
    float angle, angle2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            angle = Vector3.Angle(refVectNew,transform.localRotation*refVectNew);
            vectConv = transform.localRotation*refVect2New;
            vectConv.y = 0;
            angle2 = Vector3.Angle(refVect2New, vectConv);
            // Debug.Log(transform.localRotation.x,);
            Debug.Log("Angles r: "+angle+" "+angle2);
            
        }

        
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceFromObject : MonoBehaviour
{
    public static float DistanceFromTarget;
    public float toTarget;

    // Update is called once per frame
    void Update()
    {
        RaycastHit Hit;
        if(Physics.Raycast(transform.position,
                           transform.TransformDirection(Vector3.forward), 
                           out Hit))
        {
            toTarget = Hit.distance;
            DistanceFromTarget = toTarget;
        }
    }
}

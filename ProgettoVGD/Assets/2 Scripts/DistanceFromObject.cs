using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceFromObject : MonoBehaviour
{
    public static float DistanceFromTarget; // Distanza dall'obiettivo
    public float toTarget; // obiettivo

    // Update is called once per frame
    void Update()
    {
        RaycastHit Hit;
        //Se il target e un collider, ne prendo le informazioni
        if(Physics.Raycast(transform.position,
                           transform.TransformDirection(Vector3.forward), 
                           out Hit))
        { 
            toTarget = Hit.distance; // Calcola la distanza tra il punto di origine e il collider
            DistanceFromTarget = toTarget; 
        }
    }
}

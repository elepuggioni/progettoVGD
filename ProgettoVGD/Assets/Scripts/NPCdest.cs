using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCdest : MonoBehaviour
{
    public int pivotPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "NPC")
        {
            if (pivotPoint == 4)
            {
                this.gameObject.transform.position = new Vector3(80, 22.2f, 58);
                pivotPoint = 0;
            }

            if (pivotPoint == 3)
            {
                this.gameObject.transform.position = new Vector3(63, 22, 47);
                pivotPoint = 4;
            }

            if (pivotPoint == 2)
            {
                this.gameObject.transform.position = new Vector3(72, 22, 30);
                pivotPoint = 3;
            }

            if (pivotPoint == 1)
            {
                this.gameObject.transform.position = new Vector3(90, 22, 33);
                pivotPoint = 2;
            }

            if (pivotPoint == 0)
            {
                this.gameObject.transform.position = new Vector3(101, 22, 27);
                pivotPoint = 1;
            }



        }
    }


}

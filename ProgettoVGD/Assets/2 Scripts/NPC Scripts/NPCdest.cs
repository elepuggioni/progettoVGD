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
                pivotPoint = 0;
            }

            if (pivotPoint == 3)
            {
                this.gameObject.transform.position = new Vector3(60.09f,31.08f,-38.20f);
                pivotPoint = 4;
            }

            if (pivotPoint == 2)
            {
                this.gameObject.transform.position = new Vector3(60.09f,31.08f,-61.09f);
                pivotPoint = 3;
            }

            if (pivotPoint == 1)
            {
                this.gameObject.transform.position = new Vector3(116f,24.44f,-54.04f);
                pivotPoint = 2;
            }

            if (pivotPoint == 0)
            {
                this.gameObject.transform.position = new Vector3(102.69f,24.44f,-20.02f);
                pivotPoint = 1;
            }



        }
    }


}

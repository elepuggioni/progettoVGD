using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCdestGuadia2 : MonoBehaviour
{
    public int pivotPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPC")
        {
            if (pivotPoint == 8)
            {
                pivotPoint = 0;
            }

            if (pivotPoint == 7)
            {
                this.gameObject.transform.position = new Vector3(42.37f, 22.524f, 37.38f);
                pivotPoint = 8;
            }

            if (pivotPoint == 6)
            {
                this.gameObject.transform.position = new Vector3(42.01f, 22.524f, 51.2f);
                pivotPoint = 7;
            }

            if (pivotPoint == 5)
            {
                this.gameObject.transform.position = new Vector3(48.94f, 22.98f, 62.68f);
                pivotPoint = 6;
            }

            if (pivotPoint == 4)
            {
                this.gameObject.transform.position = new Vector3(54.39f, 23.32f, 66.06f);
                pivotPoint = 5;
            }

            if (pivotPoint == 3)
            {
                this.gameObject.transform.position = new Vector3(67.11f, 22.59f, 77.32f);
                pivotPoint = 4;
            }

            if (pivotPoint == 2)
            {
                this.gameObject.transform.position = new Vector3(54.39f, 23.32f, 66.06f);
                pivotPoint = 3;
            }

            if (pivotPoint == 1)
            {
                this.gameObject.transform.position = new Vector3(48.94f, 22.98f, 62.68f);
                pivotPoint = 2;
            }

            if (pivotPoint == 0)
            {
                this.gameObject.transform.position = new Vector3(42.01f, 22.524f, 51.2f);
                pivotPoint = 1;
            }



        }
    }
}

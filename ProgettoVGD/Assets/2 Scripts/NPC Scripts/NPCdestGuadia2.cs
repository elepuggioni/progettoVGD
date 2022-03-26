using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCdestGuadia2 : MonoBehaviour
{
    // Indica la prossima posizione 
    public int pivotPoint;

    private void OnTriggerEnter(Collider other)
    {
        //  Se il tag e "NPC"
        if (other.tag == "NPC")
        {
            if (pivotPoint == 8) //Da capo
            {
                pivotPoint = 0; // torna alla prima posizione
            }

            if (pivotPoint == 7)
            {
                this.gameObject.transform.position = new Vector3(42.37f, 22.524f, 37.38f);// Sposta l'NPC
                pivotPoint = 8; // avanza 
            }

            if (pivotPoint == 6)
            {
                this.gameObject.transform.position = new Vector3(42.01f, 22.524f, 51.2f);// Sposta l'NPC
                pivotPoint = 7; // avanza
            }

            if (pivotPoint == 5)
            {
                this.gameObject.transform.position = new Vector3(48.94f, 22.98f, 62.68f);// Sposta l'NPC
                pivotPoint = 6;
            }

            if (pivotPoint == 4)
            {
                this.gameObject.transform.position = new Vector3(54.39f, 23.32f, 66.06f);// Sposta l'NPC
                pivotPoint = 5;
            }

            if (pivotPoint == 3)
            {
                this.gameObject.transform.position = new Vector3(67.11f, 22.59f, 77.32f);// Sposta l'NPC
                pivotPoint = 4;
            }

            if (pivotPoint == 2)
            {
                this.gameObject.transform.position = new Vector3(54.39f, 23.32f, 66.06f);// Sposta l'NPC
                pivotPoint = 3;
            }

            if (pivotPoint == 1)
            {
                this.gameObject.transform.position = new Vector3(48.94f, 22.98f, 62.68f);// Sposta l'NPC
                pivotPoint = 2;
            }

            if (pivotPoint == 0)
            {
                this.gameObject.transform.position = new Vector3(42.01f, 22.524f, 51.2f);// Sposta l'NPC
                pivotPoint = 1;
            }



        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCdestGuardia1 : MonoBehaviour
{
    // Indica la prossima posizione 
    public int pivotPoint;

    private void OnTriggerEnter(Collider other)
    {
        // Se il tag è "NPC"
        if (other.tag == "NPC")
        {
            if(pivotPoint == 10)//Da capo
            {
                pivotPoint = 0;// torna alla prima posizione
            }

            if (pivotPoint == 9)
            {
                this.gameObject.transform.position = new Vector3(100.87f, 22.06f, 35.78f); // Sposta l'NPC
                pivotPoint = 10; // avanza
            }

            if (pivotPoint == 8)
            {
                this.gameObject.transform.position = new Vector3(100.87f, 22.89f, 48.91f);// Sposta l'NPC
                pivotPoint = 9;// avanza
            }

            if (pivotPoint == 7)
            {
                this.gameObject.transform.position = new Vector3(110.1f, 22.89f, 55.28f);// Sposta l'NPC
                pivotPoint = 8;
            }

            if (pivotPoint == 6)
            {
                this.gameObject.transform.position = new Vector3(106.84f, 22.89f, 74.87f);// Sposta l'NPC
                pivotPoint = 7;
            }

            if (pivotPoint == 5)
            {
                this.gameObject.transform.position = new Vector3(95.93f, 22.89f, 74.87f);// Sposta l'NPC
                pivotPoint = 6;
            }

            if (pivotPoint == 4)
            {
                this.gameObject.transform.position = new Vector3(87.77f, 23.01f, 77.87f);// Sposta l'NPC
                pivotPoint = 5;
            }

            if (pivotPoint == 3)
            {
                this.gameObject.transform.position = new Vector3(95.93f, 22.89f, 74.87f);// Sposta l'NPC
                pivotPoint = 4;
            }
            
            if (pivotPoint == 2)
            {
               this.gameObject.transform.position = new Vector3(106.84f, 22.89f, 74.87f);// Sposta l'NPC
                pivotPoint = 3;
            }

            if (pivotPoint == 1)
            {
                this.gameObject.transform.position = new Vector3(110.1f, 22.89f, 55.28f);// Sposta l'NPC
                pivotPoint = 2;
            }

            if (pivotPoint == 0)
            {
                this.gameObject.transform.position = new Vector3(100.87f, 22.89f, 48.91f);// Sposta l'NPC
                pivotPoint = 1;
            }



        }
    }
}

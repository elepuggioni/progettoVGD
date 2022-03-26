using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCdest : MonoBehaviour
{
    public int pivotPoint; // Punti da attraversare 

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "NPC") // Se il tag è "NPC"
        {
            if (pivotPoint == 4) // A capo
            {
                pivotPoint = 0; //torna alla prima posizione
            }

            if (pivotPoint == 3)// Quarta destinazione
            {
                this.gameObject.transform.position = new Vector3(60.09f,31.08f,-38.20f); // Sposta l'NPC
                pivotPoint = 4; // vai avanti
            }

            if (pivotPoint == 2)// Terza destinazione
            {
                this.gameObject.transform.position = new Vector3(60.09f,31.08f,-61.09f); // Sposta l'NPC
                pivotPoint = 3; // vai avanti
            }

            if (pivotPoint == 1)// Seconda destinazione
            {
                this.gameObject.transform.position = new Vector3(116f,24.44f,-54.04f); // Sposta l'NPC
                pivotPoint = 2; // vai avanti
            }

            if (pivotPoint == 0) // Prima destinazione
            {
                this.gameObject.transform.position = new Vector3(102.69f,24.44f,-20.02f);  // Sposta l'NPC
                pivotPoint = 1; // vai avanti
            }



        }
    }


}

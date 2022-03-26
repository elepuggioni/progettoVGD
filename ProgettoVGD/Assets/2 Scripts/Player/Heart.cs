using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public int health; // vita
    public int numOfHearts; // numero di cuori

    public RawImage[] hearts; // Immagini dei cuori

    // Update is called once per frame
    void Update()
    {
        if (health > numOfHearts) // Se la vita è maggiore del numero di cuori
        {
            health = numOfHearts; 
        }

        // Per ogni immagine nel vettore
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < numOfHearts) // Se l'indice nel vettore di quell'immagine è minore dei miei cuori attuali
            {
                hearts[i].enabled = true; // attivo quel cuori
            }
            else
            {
                hearts[i].enabled = false; // diasattivo quel cuore
            }


        }



    }
}

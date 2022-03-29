using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool questMeleIniziata = false; //indica se hai iniziato la quest delle mele
    public bool questMeleTerminata = false; //indica se hai concluso la quest delle mele
    public bool gianniSconfitto = false;
    
    public int nMele = 0; //numero delle mele raccolte 

    public GameObject boss; // riferimento del boss
    public GameObject bossHealtBar; // riferimento della health bar del boss
    public GameObject muro; // riferimento del muro invisibile nell'arena della boss battle
    public PlayerController player; // riferimento al player

    public Light luce; // riferiemento alla luce principale
    public  TextMeshProUGUI pointMele; // riferimento alle vite

    // Update is called once per frame
    void Update()
    {
        nMele = player.meleRaccolte; // Aggiorna la mele 
        if (questMeleTerminata)
            pointMele.text = "";
    }
}

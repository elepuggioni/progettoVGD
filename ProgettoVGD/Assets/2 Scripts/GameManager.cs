using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool questMeleIniziata = false; //indica se hai iniziato la quest delle mele
    public bool questMeleTerminata = false; //indica se hai concluso la quest delle mele
    public int nMele = 0; //numero delle mele raccolte 

    public bool gianniSconfitto = false;

    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {
        nMele = player.meleRaccolte;
    }
}

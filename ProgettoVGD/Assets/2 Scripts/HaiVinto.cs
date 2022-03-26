using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HaiVinto : MonoBehaviour
{
    public GameManager gm; // riferimento al gameManager
    public GameObject testo; // Scritta 'Hai vinto'

    void Update()
    {
        //Se il boss viene sconfitto
         if(gm.gianniSconfitto){
            StartCoroutine(haiVinto());
        }  

    }

    public IEnumerator haiVinto(){
        testo.SetActive(true); // mostra il testo 
        yield return new WaitForSeconds(5f); //aspetta
        Cursor.lockState = CursorLockMode.None; // rendi libero e visibile il cursore
        Cursor.visible = true;
        SceneManager.LoadScene("Menu iniziale"); //Torna al menu iniziale
    }
}

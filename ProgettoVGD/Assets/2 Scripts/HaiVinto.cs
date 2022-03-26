using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HaiVinto : MonoBehaviour
{
    public GameManager gm;
    public GameObject testo;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
         if(gm.gianniSconfitto){
            StartCoroutine(haiVinto());
        }  

    }

    public IEnumerator haiVinto(){
        testo.SetActive(true);

        yield return new WaitForSeconds(5f);
        //riattiva cursore
        SceneManager.LoadScene("Menu iniziale");
    }
}

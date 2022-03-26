using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muro : MonoBehaviour
{
    public Collider muro;
    // Start is called before the first frame update
    void Start()
    {
        muro = GetComponent<Collider>();   
        muro.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit(Collider other){
        muro.isTrigger = false;

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCbehaviour : MonoBehaviour
{
    //Variabili
    private bool proximity;
    private bool canAdvanceText;

    // Riferimenti
    private DialogueManager dialogueManager;
    private DialogueTrigger dialogueTrigger;

    
    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueTrigger = FindObjectOfType<DialogueTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // Se sono vicino, il dialogo non e startato e premo F
        if(proximity && !(dialogueManager.isDialogueStarted) && Input.GetKeyDown(KeyCode.F)){ 
           dialogueTrigger.TriggerDialogue(); // Inzia il dialogo
           canAdvanceText = false; // Non posso avanzare
        }
        // Se premo F 
        if(Input.GetKeyUp(KeyCode.F)){
            canAdvanceText = true; // posso avanzare
        }
        // Se premo f, il dialogo e startato e il posso andare avanti
        if(Input.GetKeyDown(KeyCode.F) && dialogueManager.isDialogueStarted && canAdvanceText){
               dialogueManager.DisplayNextSentence(); // mostra la nuova frase
               canAdvanceText = false; // non posso avanzare
           }
        
    }

    void OnTriggerEnter(Collider collider){
        proximity = true; // Sono vicino
    }
    void OnTriggerExit(Collider collider){
        proximity = false; // Sono lontano
    }
}

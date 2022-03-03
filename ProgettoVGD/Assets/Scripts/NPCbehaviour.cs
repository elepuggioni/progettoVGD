using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCbehaviour : MonoBehaviour
{
    private bool proximity;

    private DialogueManager dialogueManager;

    private DialogueTrigger dialogueTrigger;

    private bool canAdvanceText;
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueTrigger = FindObjectOfType<DialogueTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(proximity && !(dialogueManager.dialogueStarted) && Input.GetKeyDown(KeyCode.F)){ 
           dialogueTrigger.TriggerDialogue();
           canAdvanceText = false;
        }
        if(Input.GetKeyUp(KeyCode.F)){
            canAdvanceText = true;
        }
        if(Input.GetKeyDown(KeyCode.F) && dialogueManager.dialogueStarted && canAdvanceText){
               dialogueManager.DisplayNextSentence();
               canAdvanceText = false;
           }
        
    }
    void OnTriggerEnter(Collider collider){
        proximity = true;
        //FindObjectOfType<DialogueTrigger>().TriggerDialogue();
    }
    void OnTriggerExit(Collider collider){
        proximity = false;
    }
}

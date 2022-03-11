using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject ActionDisplay;
    public GameObject ActionText;
    public Dialogue dialogue;
    
    public void TriggerDialogue(){
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, gameObject);
    }

    public void TurnOnGameObjects()
    {
        ActionDisplay.SetActive(true);
        ActionText.GetComponent<Text>().text = "Parla [E]";
        ActionText.SetActive(true);
    }

}

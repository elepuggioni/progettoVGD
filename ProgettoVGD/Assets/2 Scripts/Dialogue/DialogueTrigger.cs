using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/* Script che si inserisce nei Game Objects degli Npc che hanno un dialogo.
 * Permette di iniziare il dialogo quando richiesto */
public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] [Tooltip("Game Object \"Action Display\"")]
    private GameObject ActionDisplay;
    
    [SerializeField] [Tooltip("Game Object \"Action Text\"")]
    private GameObject ActionText;
    
    [SerializeField] [Tooltip("Box in cui inserire i dialoghi")]
    private Dialogue dialogue;

    public DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Fa iniziare il dialogo in game
    public void TriggerDialogue(){


        /* Cerca il Game Object "DialogueManager" a cui è attaccato
         * l'omonimo script e richiama un metodo al suo interno
         * Passa come parametri il campo dialogue e il Game Object
         * del Npc a cui è attaccato questo script */
        if (!dialogueManager.alreadyTalk)
                dialogueManager.StartDialogue(dialogue, gameObject);
        else
                TurnOffGameObjects();
    }

    // Metodo che attiva i Game Object che fanno apparire il comando "Parla" in game
    public void TurnOnGameObjects()
    {
        ActionDisplay.SetActive(true);
        ActionText.GetComponent<Text>().text = "Parla [E]";
        ActionText.SetActive(true);
    }
  // Metodo che disattiva i Game Object che fanno apparire il comando "Parla" in game
    public void TurnOffGameObjects()
    {
        ActionDisplay.SetActive(false);
        ActionText.GetComponent<Text>().text = "";
        ActionText.SetActive(false);
    }

}

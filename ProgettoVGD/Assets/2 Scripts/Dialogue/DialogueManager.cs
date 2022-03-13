using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Script che gestisce i dialoghi in game. E' inserito */
public class DialogueManager : MonoBehaviour
{
    [Tooltip("Indica se è in corso un dialogo")]
    public bool isDialogueStarted;
    
    [SerializeField] [Tooltip("Game Object \"Sub Text\"")]
    private GameObject subText;
    
    [SerializeField] [Tooltip("Game Object \"Sub Box\"")]
    private GameObject subBox;
    
    [SerializeField] [Tooltip("Game Object \"Action Display\"")]
    private GameObject ActionDisplay;
    
    [SerializeField] [Tooltip("Game Object \"Action Text\"")]
    private GameObject ActionText;

    [SerializeField] [Tooltip("Game Object del player")]
    private GameObject _player;

    /* Variabili locali */
    private GameObject _npc;                //Game Object del npc
    private Quaternion rotazioneNPC;        //rotazione del npc 
    private Animator _animator;             //animator del npc
    private Queue<string> sentences;        //Coda per le frasi del npc
    public GameObject buttonYes;
    public GameObject buttonNo;

    

    // Start is called before the first frame update
    void Start()
    {
        //Inizializza la coda che conterrà le frasi dette dal npc
        sentences = new Queue<string>();
        _player = GameObject.Find("Player");
    }
    
    /* Metodo che permette di iniziare la sequenza di dialogo.
     * Prende in input il box dialogue che contiene le frasi, e
     * il Game Object del npc con cui fare il dialogo*/
    public void StartDialogue(Dialogue dialogue, GameObject npc){
        //Setta il Boolean che indica che è in corso un dialogo a true
        isDialogueStarted = true;
        
        //Disattiva il movimento del player
        _player.GetComponent<PlayerController>().lockMovment = true;

        _npc = npc;
        
        //Salva la rotazione originaria del npc
        rotazioneNPC = npc.transform.rotation;

        //Fa girare l'npc verso il player 
        if(!(_npc.CompareTag("Prete")))
            _npc.transform.LookAt(_player.transform.position);

        //Riferimento all'animator del npc
        _animator = npc.GetComponent<Animator>();
        
        //Disattiva l'animazione del npc
        if (_npc.CompareTag("Cognata"))
            _animator.SetBool("Stendere", false);
        
        //Attiva i Game Objects per visualizzare le frasi dei dialoghi
        subBox.SetActive(true);
        subText.SetActive(true);
        
        //Svuota la coda
        sentences.Clear();
        
        //Riempie la coda con tutte le frasi presenti nel dialogue del npc
        foreach (string sentence in dialogue.sentences){
            sentences.Enqueue(sentence);
        }
        
        //Stampa a schermo la frase del dialogo
        DisplayNextSentence();
        
        //Disattiva i Game Objects per il comando di attivazione del dialogo
        ActionDisplay.SetActive(false);
        ActionText.SetActive(false); 
    }
    
    // Metodo che stampa a schermo le frasi dei dialoghi
    public void DisplayNextSentence(){
        //Se non ci sono più frasi nella coda, termina il dialogo
        if(sentences.Count == 0 )
        {
            if (_npc.CompareTag("SignoraDelleMele")){
                buttonNo.SetActive(true);
                buttonYes.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return;
            }
            else {
                EndDialogue();
                return;
            }
        }

        //Estrae una frase dalla coda
        string sentence = sentences.Dequeue();
        
        //Avvia la coroutine che digiti la frase
        StartCoroutine(TypeSentence(sentence));
    }

    // Coroutine che digita a schermo la frase 
    IEnumerator TypeSentence(string sentence){
        //Assegna una stringa vuota al box in cui digitare la frase
        subText.GetComponent<Text>().text = "";
        
        //Digita la frase aggiungiendo un carattere a ogni frame
        foreach(char letter in sentence){
            subText.GetComponent<Text>().text += letter;
            //aspetta 1 frame
            yield return null;
            /* Se viene premuto un qualsiasi tasto durante la digitazione,
             * stampa tutta la frase in un frame ed esce dal ciclo */
            if (Input.anyKeyDown)
            {
                subText.GetComponent<Text>().text = sentence;
                break;
            }
        }
        
        //Aspetta due decimi di secondo
        yield return new WaitForSeconds(0.2f);
        
        //Aspetta finché non viene premuto un qualsiasi tasto
        yield return new WaitUntil(() => Input.anyKeyDown);
        
        //Avvia la coroutine che digiti la frase
        DisplayNextSentence();
    }
    
    // Metodo che gestisce la conclusione del dialogo 
    public void EndDialogue(){
        //Disattiva i Game Objects per visualizzare le frasi dei dialoghi
        subBox.SetActive(false);
        subText.GetComponent<Text>().text = "";
        subText.SetActive(false);

        buttonNo.SetActive(false);
        buttonYes.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Setta il Boolean che indica che è in corso un dialogo a false
        isDialogueStarted = false;
        
        //Riattiva il movimento del player 
        _player.GetComponent<PlayerController>().lockMovment = false;
        
        //Resetta la rotazione del npc a quella iniziale
        _npc.transform.rotation = rotazioneNPC;
        
        //Riattiva l'animazione del npc
        if (_npc.CompareTag("Cognata"))
            _animator.SetBool("Stendere", true);
        
    }

    public void Yes()
    {
        EndDialogue();
    }
    public void No()
    {
        EndDialogue();
    }

}

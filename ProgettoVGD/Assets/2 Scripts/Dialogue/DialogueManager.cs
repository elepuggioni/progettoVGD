using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Script che gestisce i dialoghi in game. E' inserito */
public class DialogueManager : MonoBehaviour
{
    [Tooltip("Indica se è in corso un dialogo")] public bool isDialogueStarted;

    [SerializeField] [Tooltip("Game Object \"Sub Text\"")] private GameObject subText;
    [SerializeField] [Tooltip("Game Object \"Sub Box\"")] private GameObject subBox;
    [SerializeField] [Tooltip("Game Object \"Action Display\"")] private GameObject ActionDisplay;
    [SerializeField] [Tooltip("Game Object \"Action Text\"")] private GameObject ActionText;
    [SerializeField] [Tooltip("Game Object del player")] private GameObject _player;

    


    /* Variabili locali */
    private Quaternion rotazioneNPC;        //rotazione del npc 
    private Animator _animator;             //animator del npc
    private Queue<string> sentences;        //Coda per le frasi del npc
    private GameManager gameManager;
    private PlayerController playerController;
    private AudioHandler audioHandler;

    private GameObject _npc;                //Game Object del npc
    public GameObject viceCapo;
    
    #region GameObject dei vari pulsanti dei dialoghi
    public GameObject buttonYes;
    public GameObject buttonNo;
    public GameObject buttonInfoYes;
    public GameObject buttonInfoNo;
    public GameObject buttonBattle;
    public GameObject buttonGoAway;
    #endregion

    public Heart numeroCuori;
    private ViceCapoController viceCapoController;

    public Text meleDaRaccogliereText;  //mele da raccogliere
    public GameObject[] mele; // raccoglitore per le mele 

    #region Boolean di controllo
    //indica se il viceCapo ha iniziato ad attaccare
    private bool viceCapoIsAttacking = false;

    //indica se l'arciere sta parlando e quando deve uscire
    private bool infoAreDisplayed = false;

    // indica se un dialogo può essere ripetuto una volta averlo attivato
    public bool alreadyTalk = false;

    //controlla se bisogna mostrare i bottoni si/no quando parli con la signora delle mele
    public bool displayButtons;
    #endregion

    //frase che dice la signora delle mele se non hai abbastanza mele
    public string questMeleNonBastano; 

    //frasi che dice la signora delle mele se hai abbastanza mele
    public string[] questMeleBastano = new string[2]; 
    
    // Start is called before the first frame update
    void Start()
    {
        //Inizializza la coda che conterrà le frasi dette dal npc
        sentences = new Queue<string>();

        _player = GameObject.Find("Player");
        playerController = _player.GetComponent<PlayerController>();
        viceCapoController = viceCapo.GetComponent<ViceCapoController>();
        gameManager = FindObjectOfType<GameManager>();
        audioHandler = FindObjectOfType<AudioHandler>();
        
        // Inizializzazione frasi
        questMeleNonBastano = "Non hai abbastanza mele... me ne servono almeno dieci.";
        questMeleBastano = new string[]{"Grazie! Adesso mio figlio starà sicuramente meglio...", 
                                        "Come ti ho promesso, ecco la tua nuova armatura."}; 
    }
    
    /* Metodo che permette di iniziare la sequenza di dialogo.
     * Prende in input il box dialogue che contiene le frasi, e
     * il Game Object del npc con cui fare il dialogo */
    public void StartDialogue(Dialogue dialogue, GameObject npc){
        //Setta il Boolean che indica che è in corso un dialogo a true
        isDialogueStarted = true;
        displayButtons = true;

        //Disattiva il movimento del player
        playerController.lockMovment = true;

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

        //se parli con la signora delle mele dopo che hai iniziato la quest
        if(_npc.CompareTag("SignoraDelleMele") && gameManager.questMeleIniziata){
            //se hai abbastanza mele (controlla nel gameManager)
            if(gameManager.nMele >= 10){
                foreach (string sentence in questMeleBastano){
                    sentences.Enqueue(sentence);
                }
                gameManager.questMeleTerminata = true; // Termino la quest
                playerController.armaturaAcquisita = true; // ottengo l'armatura
                DisableQuestObject();
            }
            //se non hai abbastanza mele
            else{
                sentences.Enqueue(questMeleNonBastano);
            }
        }
        else{
            //Riempie la coda con tutte le frasi presenti nel dialogue del npc
            foreach (string sentence in dialogue.sentences){
                sentences.Enqueue(sentence);
            }
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
            // Se la signore delle mele mi ha detto tutto, sono ancora nel dialogo, e la quest non è ancora iniziata
            if (_npc.CompareTag("SignoraDelleMele") && displayButtons && !gameManager.questMeleIniziata){
             // Attiva i pulsanti e il cursore
                buttonNo.SetActive(true); 
                buttonYes.SetActive(true);
                ActiveCursor();
                return;
            }
            // Se sto parlando con l'arciere iniziale
            else if (_npc.CompareTag("ArciereInfo") )
            {
                if (infoAreDisplayed) // Se l'aricere ha finito di parlare
                {
                    infoAreDisplayed = false; // Posso abbandonare il dialogo
                    EndDialogue(); // Termino il dialogo
                    return;
                }

                // Attivo i pulsanti di risposta e il cursore
                buttonInfoNo.SetActive(true); 
                buttonInfoYes.SetActive(true);
                ActiveCursor();
                return;
            }
            // Se parlo con il viceCapo e ho ottenuto l'armatura dalla signora delle mele
            else if (_npc.CompareTag("ViceCapo") && playerController.armaturaAcquisita)
            {
                // Attivo i pulsanti di risposta e il cursore
                buttonBattle.SetActive(true);
                buttonGoAway.SetActive(true);
                ActiveCursor();
                return;
            }
            else {
                PriestHelth();
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
        playerController.lockMovment = false;
        
        //Resetta la rotazione del npc a quella iniziale
        _npc.transform.rotation = rotazioneNPC;
        
        //Riattiva l'animazione del npc
        if (_npc.CompareTag("Cognata"))
            _animator.SetBool("Stendere", true);
        
    }

    #region Funzioni per i pulsanti
    // Chiama questa funzione se accetti la quest della signora delle mele
    public void Yes()
    {
        sentences.Clear(); // Pulisce la coda
        sentences.Enqueue("Grazie! Allora aspetto il tuo ritorno."); // Mette in coda  una nuova frase
        displayButtons = false; // Indica che non mostra più i pulsanti 
        buttonNo.SetActive(false); // Disattiva i pulsanti
        buttonYes.SetActive(false);
        gameManager.questMeleIniziata = true; //segna nel gameManager se hai iniziato la quest delle mele
        DisplayNextSentence(); // Mostra la frase 

        meleDaRaccogliereText.gameObject.SetActive(true); // Attiva il counter delle mele
        for(int i = 0; i < mele.Length; i++)//attiva le mele nella foresta
        {
            mele[i].SetActive(true);
        } 
    }

    // Chiama questa funzione se non accetti la quest della signora delle mele
    public void No()
    {
        sentences.Clear(); // Pulisci la coda
        sentences.Enqueue("Oh no... E allora come faremo..."); // Mette in coda una nuova frase
        displayButtons = false; 
        buttonNo.SetActive(false); // Disattiva i pulsanti
        buttonYes.SetActive(false);
        DisplayNextSentence(); // Mostra la frase
    }

    // Mostra le info dall'aricere inziale se decido di andare avanti col dialogo 
    public void YesInfo()
    {
        sentences.Clear(); //pulisce la coda
        
        // Mette in coda 5 frasi
        sentences.Enqueue("Per prima cosa verso est c'è una signora che ha bisogno di aiuto per suo figlio, parlaci e vedi cosa ha da dirti. " +
                           "Sicuramente ti può ricompensare per bene");
        sentences.Enqueue("Dopo potresti andare a sfidare la spalla destra dello stregone, facendo così sicuramente lo indebolirai e " +
                          "portai rubare la sua affilatissima spada.");
        sentences.Enqueue("Il nostro capo malvagio è solito stare nella nicchia desertica tra le montagne, attualmente non c'è, " +
                          "ma ho sentito dire che tornerà molto presto");
        sentences.Enqueue("Ricorda che se sei ferito puoi sempre andare verso il prete per poterti curare. Attualmente si trova in cima " +
                          "alla montagna con la croce");
        sentences.Enqueue("Alla fine sarai sicurmente pronto per affrontare il nostro capo e liberarci. Grazie straniero");
        buttonInfoNo.SetActive(false); // Disattiva i pulsanti
        buttonInfoYes.SetActive(false);
        infoAreDisplayed = true; // Le info stanno venendo mostrate
        DisplayNextSentence(); // Mostra le frasi
    }

    // Chiudo il dialogo dall'arciere iniziale se decido di non leggere le info
    public void NoInfo()
    {
        sentences.Clear(); // Pulisco la coda
        sentences.Enqueue("In bocca al lupo straniero, ne avrai bisogno"); // Inserisco una frase
        buttonInfoNo.SetActive(false); // Disattiva i pulsanti
        buttonInfoYes.SetActive(false);
        infoAreDisplayed = true; // Le info sono mostrate
        DisplayNextSentence(); // Mostra la frase
    }

    // Chiama questa funzione se decido di combattere contro il vice capo
    public void YesBattle()
    {
        sentences.Clear(); // Pulisco la coda
        alreadyTalk = true; // Ho gia parlato
        audioHandler.StandardBackground.Pause();
        audioHandler.SecondaryBossBackground.PlayDelayed(1.3f);
        viceCapoController.enabled = true; // abilito il controller
        viceCapoIsAttacking = true; // inzia ad attaccare
        EndDialogue(); //Termino il dialogo
    }

    // Chiama questa funzione se decido di andare via e non combattere contro il vice capo
    public void NoGoAway()
    {
        sentences.Clear(); // Pulisce la coda
        EndDialogue(); //Termina il dialogo
    }
    #endregion

    // Attiva e mostra il cursore sullo schermo
    public void ActiveCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    } 

    // Vengo curato dal prete sulla montagna
    public void PriestHelth()
    {
        if (_npc.CompareTag("Prete")) // Se parli con il prete ti cura tutti i cuori
        {
            audioHandler.HealthRestored.Play();
            playerController.life = 10;
            numeroCuori.numOfHearts = 10;
            numeroCuori.health = 10;
        }
    }

    //Disabilita i GameObject a quest delle mele completata
    public void DisableQuestObject()
    {
        buttonNo.SetActive(false);
        buttonYes.SetActive(false);

        meleDaRaccogliereText.gameObject.SetActive(false);
    }

    // Getter per l'attributo viceCapoIsAttacking
    public bool getViceCapo()
    {
        return this.viceCapoIsAttacking;
    }
}

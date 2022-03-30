using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class ViceCapoController : MonoBehaviour
{
    #region Variabili
    private int life = 6;  // indica le vite
    private float speedAnimator = 0.0f; // velcocita del vice capo
    public bool isDead = false; // indica se e morto
    private bool followPlayer = true; // indica se deve seguire il player
    private bool isImmune = false; // indica se e immune
    public bool playerHitted;
    public bool AlreadyHitted;
    #endregion

    #region Riferimenti
    public LayerMask playerLayer;
    private NavMeshAgent agent;
    public TextMeshProUGUI lifeText;
    private Animator animator;
    private DialogueManager dialogueManager;
    private PlayerController playerController;
    private AudioHandler audioHandler;

    [Header("Sounds")] 
    [SerializeField] private AudioSource DeathVoice;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        lifeText.SetText(life.ToString());
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        playerController = FindObjectOfType<PlayerController>();
        audioHandler = FindObjectOfType<AudioHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowOrNot();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 position = transform.position;
        position.y += 1f;

        // Ritorna true se il raggio intereseca un collider
        if (Physics.Raycast(position, transform.forward, out hit, 1.5f))
        {
          
            if (hit.transform.CompareTag("Player")) // Entro se colpisco il Player
            {
                if (hit.distance <= 2f && !isDead && !playerController.isDead) // Se sono molto vicino al player
                {
                    followPlayer = false; // Smette di seguire il player  
                    animator.SetTrigger("Attack");
                }
            }
        }
    }

    // Funzione che permette al vice capo di seguire il player 
    // oppure di tornare alla sua posizione
    void FollowOrNot()
    {
        Mathf.Clamp01(speedAnimator); // Blocca il valore tra 0 e 1
        animator.SetFloat("Speed", speedAnimator); // Setta il valore di Speed dell'animator

        // Se il player si trova dentro il campo visivo del vice capo e non e morto
        if (!playerController.isDead && Physics.CheckSphere(transform.position, 20.0f, playerLayer) && !isDead) 
        {
            speedAnimator = 1f; 
            ChasePlayer();
        }
        else if(!isDead) // Se non è morto
        {
            Back();
        }
    }

    // Insegue il player
    void ChasePlayer()
    {
        //Calcola la distanza tra player e vice capo
        float distance = Vector3.Distance(GameObject.FindWithTag("Player").transform.position,
                                          transform.position);
        if (followPlayer)// Se devo seguire il player
            agent.SetDestination(GameObject.FindWithTag("Player").transform.position); // la posizione del player diventa la destinazione
        else if (distance > 2f) // Se il player si allontana
            followPlayer = true; // posso ritornare a seguire il player in seguito

    }

    // Torna al punto di partenza
    void Back()
    {
        followPlayer = true;

        //distanza tra viceCapo e il gameObject "Punto di ritorno" 
        float distance = Vector3.Distance(GameObject.FindWithTag("Finish").transform.position, this.transform.position); 

        if (distance > 2f)
        {
            speedAnimator = 0.5f; // Animazione di Walk
            agent.SetDestination(new Vector3(43.824f, 22.44319f, 74.70712f)); // Torna alla posizione iniziale del vice capo
        }
        else
        {
            speedAnimator = 0.0f; // Animazione di Idle
            agent.ResetPath(); // Resetto il path del vice capo
        }
    }

    // Chiamato quando il vice capo prende danno
    public void TakeDamage(int damage)
    {
        if (!isImmune) // Se non e immune
        {
            isImmune = true; // diventa immune
            life -= damage; // riduci le vite
            lifeText.SetText(life.ToString());
            if (life > 0)
            {
                audioHandler.EnemyHitted.Play();
            }
            else
            {
                DeathVoice.Play();
                GetComponent<ViceCapoController>().enabled = false;
                StopAllCoroutines(); // ferma tutte le coroutine
                StartCoroutine(Die()); // avvia la coroutine di Die
            }

            if(this.gameObject.activeSelf)// Se il vice capo risulta attivo
                StartCoroutine(Immunity());
        }    
    }

    // Dopo aver preso danno il vice capo e immune per 1.2 secondi
    public IEnumerator Immunity()
    {
        yield return new WaitForSeconds(1.2f);
        isImmune = false;
    }

    // Coroutine che gestisce l'animazione di morte 
    public IEnumerator Die()
    {
        audioHandler.EnemyKilled.Play();
        animator.SetFloat("Speed", 0); // Setto speed a zero
        isDead = true; // Indica la morte
        animator.SetLayerWeight(animator.GetLayerIndex("Die Layer"), 1); // Cambia layer dell'animator
        animator.SetTrigger("Die"); // Setta il trigger
        lifeText.SetText(""); // Elimina la scritta delle vite

        yield return new WaitForSeconds(2f);
        audioHandler.SecondaryBossBackground.Stop();
        audioHandler.StandardBackground.UnPause();
        
        yield return new WaitForSeconds(3f); // Aspetta 3 secondi
        playerController.spadaAcquisita = true; // Ottieni la spada
        GameObject.FindGameObjectWithTag("Sword").SetActive(false);
        this.gameObject.SetActive(false);

        // Disattiva gameObject e pulsanti 
        GetComponent<CapsuleCollider>().enabled = false;
        dialogueManager.alreadyTalk = false;
        dialogueManager.buttonBattle.SetActive(false);
        dialogueManager.buttonGoAway.SetActive(false);
        
    }
}

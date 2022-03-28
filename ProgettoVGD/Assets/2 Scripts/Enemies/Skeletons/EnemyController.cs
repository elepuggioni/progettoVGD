using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    #region Variabili
    private int life = 2;

    private float speedAnimator = 0.0f; // Velcocita nel parametro dell'animator

    public float wanderTime; // tempo di wander in una direzione
    public float movementSpeed; // velcocita dell'enemy

    private bool followPlayer = true; // indica se sta seguendo il player

    public bool AlreadyHitted; // indica se l'enemy viene hittato
    public bool PlayerHitted; // indica se il player viene hittato
    #endregion

    public LayerMask playerLayer; // riferiemento alla layer Mask del player
    private NavMeshAgent agent; // riferimento alla nav mesh agent
    public TextMeshProUGUI lifeText; // riferimento alle vite
    private Animator animator;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        lifeText.SetText(life.ToString());
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
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
        position.y += 1f; //Aumenta la y di position per alzare il punto di spawn del raycast
        
        if (Physics.Raycast(position, transform.forward, out hit, 1.5f))
        {
            if (hit.transform.CompareTag("Player") && !playerController.isDead) // Se hitto il player
            {
                animator.SetTrigger("Attack"); // Anima l'attacco
                followPlayer = false; //Smetti di seguire il player
            }
        }
    }

    // Funzione che indica se l'enemy deve seguire oppure no il player
    void FollowOrNot()
    {
        animator.SetFloat("Speed", speedAnimator); // Setta il parametro 
        if (!playerController.isDead && Physics.CheckSphere(transform.position, 10.0f, playerLayer)) // Se il player è dentro la mia sfera di visione
        {
            speedAnimator = 1f; // Setto speed ad 1
            ChasePlayer(); 
        }
        else
        {
            speedAnimator = 0.5f; // Setto speed a 0.5
            agent.ResetPath(); // Resetto il path dell'enemy
            Patrol();
        }
    }

    // Funzione che permette di seguire il player
    void ChasePlayer()
    {
        //Calcolo la distanza tra enemy e player
        float distance = Vector3.Distance(GameObject.FindWithTag("Player").transform.position,
                                          this.transform.position);
        
        if (followPlayer) // Se devo seguire il player
            agent.SetDestination(GameObject.FindWithTag("Player").transform.position); // la posizione del player diventa la destinazione
        else if (distance > 2f) // Se il player si allontana
            followPlayer = true; // posso ritornare a seguire il player in seguito

    }

    // Funzione che permette all'enemy di vagare per la mappa
    void Patrol()
    {
        followPlayer = true; // Seguo il player
        if (wanderTime > 0) // Finche il wanderTime > 0 
        {
            transform.Translate(Vector3.forward * movementSpeed); // Muovo l'enemy in avanti 
            wanderTime -= Time.deltaTime; // diminuisco il wanderTime
        }
        else
        {
            wanderTime = Random.Range(5.0f, 10.0f); // genero un valore random per il wander time 
            transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0); // Faccio una rotazione sull'enemy
        }

    }

    // Funzione che fa prendere danno allo scheletro
    public void TakeDamage(int damage)
    {
        life -= damage; // Diminusco le vite
        lifeText.SetText(life.ToString()); // mostro le vite sopra lo scheletro
        if (life <= 0) // Quando perdo tutte le vite
        {
           agent.gameObject.SetActive(false); // Disattivo lo scheletro
        }

    }
}

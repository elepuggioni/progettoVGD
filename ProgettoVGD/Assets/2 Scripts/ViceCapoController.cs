using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class ViceCapoController : MonoBehaviour
{
    private int life = 2;
    private float speedAnimator = 0.0f;
    private bool isDead = false;
    private bool followPlayer = true;
    private bool isImmune = false;


    public LayerMask playerLayer;
    
    private NavMeshAgent agent;
    public TextMeshProUGUI lifeText;
    private Animator animator;
    private DialogueManager dialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        lifeText.SetText(life.ToString());
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowOrNot();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        // Ritorna true se il raggio intereseca un collider
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            if (hit.transform.CompareTag("Player")) // Entro se colpisco il Player
            {
                if (hit.distance < 1f && !isDead) // Se sono molto vicino al player
                {
                    animator.SetTrigger("Attack");
                    followPlayer = false; // Smette di seguire il player  
                    hit.transform.GetComponent<PlayerController>().TakeDamage(1); // Il player prende danno

                }
            }
            //animator.SetBool("Attack", false);
        }
    }

    // Funzione che permette al vice capo di seguire il player se nel suo raggio
    // oppure di tornare alla sua posizione
    void FollowOrNot()
    {
        Mathf.Clamp01(speedAnimator);
        animator.SetFloat("Speed", speedAnimator);
        if (Physics.CheckSphere(transform.position, 20.0f, playerLayer))
        {
            speedAnimator = 1f;
            ChasePlayer();
        }
        else
        {
            agent.ResetPath();
            Back();
        }
    }

    // Insegue il player
    void ChasePlayer()
    {
        float distance = Vector3.Distance(GameObject.FindWithTag("Player").transform.position,
                                          this.transform.position);
        if (followPlayer)
            agent.SetDestination(GameObject.FindWithTag("Player").transform.position);
        else if (distance > 2f)
            followPlayer = true;

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
            agent.ResetPath();
        }
           

    }

    // Chiamato quando il vice capo prende danno
    public void TakeDamage(int damage)
    {
        if (!isImmune)
        {
            isImmune = true;
            life -= damage;
            lifeText.SetText(life.ToString());
            if (life <= 0)
            {
                StartCoroutine(Die());
                dialogueManager.alreadyTalk = false;
            }

            if(this.gameObject.activeSelf)
                StartCoroutine(Immunity());
        }    
    }

    // Dopo aver preso danno il vice capo è immune per 1.2 secondi
    public IEnumerator Immunity()
    {
        yield return new WaitForSeconds(1.2f);
        isImmune = false;
    }

    public IEnumerator Die()
    {
        speedAnimator = 0;
        isDead = true;
        animator.SetLayerWeight(animator.GetLayerIndex("Die Layer"), 1);
        animator.SetTrigger("Die");
        lifeText.SetText("");
        yield return new WaitForSeconds(4f);
        FindObjectOfType<PlayerController>().spadaAcquisita = true;
        agent.gameObject.SetActive(false);
        
    }
}

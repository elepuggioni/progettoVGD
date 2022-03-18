using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class ViceCapoController : MonoBehaviour
{
    private int life = 5;
    private float speedAnimator = 0.0f;
    private float velocity;
    private bool followPlayer = true;
    public bool isImmune = false;

    public LayerMask playerLayer;
    private NavMeshAgent agent;
    public TextMeshProUGUI lifeText;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        lifeText.SetText(life.ToString());
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowOrNot();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            if (hit.transform.CompareTag("Player"))
            {
                animator.SetTrigger("Attack");
                if (hit.distance < 1f)
                {
                    followPlayer = false;

                    if (hit.transform.GetComponent<PlayerController>().armaturaAcquisita)
                            hit.transform.GetComponent<PlayerController>().TakeDamage(1);
                    else
                            hit.transform.GetComponent<PlayerController>().TakeDamage(2);
                }
            }
        }
    }

    void FollowOrNot()
    {
        Mathf.Clamp01(speedAnimator);
        animator.SetFloat("Speed", speedAnimator);
        if (Physics.CheckSphere(transform.position, 12.0f, playerLayer))
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
            
            speedAnimator = 0.5f;
            agent.SetDestination(new Vector3(43.824f, 22.44319f, 74.70712f)); //posizione iniziale del vice capo
        }
        else
        {
            speedAnimator = 0.0f;
            agent.ResetPath();
        }
           

    }

    public void TakeDamage(int damage)
    {
        if (!isImmune)
        {
            Debug.Log("Sono immune");
            isImmune = true;
            life -= damage;
            lifeText.SetText(life.ToString());
            if (life <= 0)
            {
                agent.gameObject.SetActive(false);
            }
            if(this.gameObject.activeSelf)
                StartCoroutine(Immunity());
        }    
    }

    public IEnumerator Immunity()
    {
        yield return new WaitForSeconds(1.2f);
        isImmune = false;
        Debug.Log("Non sono immune");
    }
}

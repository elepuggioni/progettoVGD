using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private int life = 3;
    private bool followPlayer = false;
    private float speedAnimator = 0.0f;
    private float velocity;
    public float wanderTime;
    public float movementSpeed;

    public LayerMask playerLayer;
    private NavMeshAgent agent;
    public TextMeshProUGUI lifeText;
    private Animator animator;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
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
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.5f))
        {

            if (hit.transform.CompareTag("Player")) 
            {
                animator.SetTrigger("Attack");
                if (hit.distance < 0.5f)
                {
                    hit.transform.GetComponent<PlayerController>().TakeDamage(2);
                }
            }
        }
    }

    void FollowOrNot()
    {
        Mathf.Clamp01(speedAnimator);
        animator.SetFloat("Speed", speedAnimator);
        if (Physics.CheckSphere(transform.position, 10.0f, playerLayer))
        {
            speedAnimator = 0.5f;
            ChasePlayer();
        }
        else
        {
            agent.ResetPath();
            speedAnimator = 0f;
            Patrol();
        }
    }
    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    void Patrol()
    {
        if(wanderTime > 0)
        {
            
            transform.Translate(Vector3.forward * movementSpeed);
            wanderTime -= Time.deltaTime;
        }
        else
        {
            wanderTime = Random.Range(5.0f, 10.0f);
            transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        }

    }


}

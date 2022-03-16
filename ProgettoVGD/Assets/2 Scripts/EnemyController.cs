using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private int life = 2;
    private float speedAnimator = 0.0f;
    private float velocity;
    public float wanderTime;
    public float movementSpeed;
    private bool followPlayer = true;

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
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.5f))
        {

            if (hit.transform.CompareTag("Player")) 
            {
                animator.SetTrigger("Attack");
                if (hit.distance < 1f)
                {
                    followPlayer = false;
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
            speedAnimator = 1f;
            ChasePlayer();
        }
        else
        {
            speedAnimator = 0.5f;
            agent.ResetPath();
            Patrol();
        }
    }
    void ChasePlayer()
    {
        float distance = Vector3.Distance(GameObject.FindWithTag("Player").transform.position,
                                          this.transform.position);
        if (followPlayer)
            agent.SetDestination(GameObject.FindWithTag("Player").transform.position);
        else if (distance > 2f)
            followPlayer = true;

    }
    void Patrol()
    {
        followPlayer = true;
        if (wanderTime > 0)
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

    public void TakeDamage(int damage)
    {
        life -= damage;
        lifeText.SetText(life.ToString());
        if (life <= 0)
        {
           agent.gameObject.SetActive(false);
        }

    }
}

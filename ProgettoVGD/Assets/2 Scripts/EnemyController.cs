using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private int life = 3;
    private bool followPlayer = false;
    //private float velocity;
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
    }

    // Update is called once per frame
    void Update()
    {
        RunOrStop();
    }
    
    void RunOrStop()
    {
        if (Physics.CheckSphere(transform.position, 10.0f, playerLayer))
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void ChasePlayer()
    {
        followPlayer = true;
        agent.SetDestination(player.transform.position);
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

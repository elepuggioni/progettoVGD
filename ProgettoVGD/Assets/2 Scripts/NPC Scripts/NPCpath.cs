using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCpath : MonoBehaviour
{
    [SerializeField] private bool isGrounded; // indica se sta toccando il suolo
    [SerializeField] private float groundCheckDistance; // indica la distanza dal suolo
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity; // indica il valore di gravita
    
    private Vector3 speed;
    private NavMeshAgent agent;
    public GameObject dest; // destinazione

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.speed = 2; // Velocita dell nav mesh agent
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask); // Controllo se a terra

        // Se a terra e la y di speed minore di zero
        if (isGrounded && speed.y < 0)
        {
            speed.y = -2f; // diminuisco la speed.y
        }
        speed.y += gravity * Time.deltaTime; // calcolo la gravity
        agent.SetDestination(dest.transform.position); // imposta la destinazione
    }
}

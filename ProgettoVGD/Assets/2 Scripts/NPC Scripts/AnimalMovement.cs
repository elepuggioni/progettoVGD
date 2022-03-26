using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    // Variabili
    public float movementSpeed = 20f;
    public float rotationSpeed = 100f;

    // Variabili booleane di controllo
    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;

    // Riferimenti
    Rigidbody rb;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Se non sta camminando
        if (isWandering == false)
        {
            StartCoroutine(Wander());
        }

        // Se sta ruotando a destra
        if (isRotatingRight == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotationSpeed); // Ruota a destra
        }
        // Se sta ruotando a sinistra
        if (isRotatingLeft == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotationSpeed); // ruota a sinistra
        }

        // Se sta correndo
        if (isWalking == true)
        {
            rb.AddForce(transform.forward * movementSpeed); // Sposta in avanti
            animator.SetBool("isRunning", true); // Attiva il bool dell'animator
        }

        // Se smette di correre
        if(isWalking == false)
        {
            animator.SetBool("isRunning", false);// Disattiva il bool dell'animator

        }
    }

    // Coroutine che permette il controllo dell'animazione di wander
    IEnumerator Wander()
    {
        // Valori random per variabili
        int rotationTime = Random.Range(1, 3);
        int rotateWait = Random.Range(1, 3);
        int rotateDirection = Random.Range(1, 3);
        int walkWait = Random.Range(1, 3);
        int walkTime = Random.Range(1, 3);

        isWandering = true; // sta camminando

        yield return new WaitForSeconds(walkWait); // aspetta da 1 a 3 sec
        isWalking = true; // sta correndo

        yield return new WaitForSeconds(walkTime); // aspetta da 1 a 3 sec
        isWalking = false; // non sta correndo

        yield return new WaitForSeconds(rotateWait); // aspetta da 1 a 3 sec
        
        //Se uguale a 1
        if (rotateDirection == 1)
        {
            isRotatingLeft = true; // gira a sinitra
            yield return new WaitForSeconds(rotationTime); // aspetta 
            isRotatingLeft = false; // non gira a sinistra
        }
        // Se uguale a 2
        if (rotateDirection == 2)
        {
            isRotatingRight = true; // gira a destra
            yield return new WaitForSeconds(rotationTime); // aspetta
            isRotatingRight = false; // non gira a destra
        }

        isWandering = false; // smette di camminare
      
    }
    
}

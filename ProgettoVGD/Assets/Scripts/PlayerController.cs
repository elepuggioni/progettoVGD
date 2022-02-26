using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private float velocity = 5.0f;
    public float rotationSpeed;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    /*void Update()
    {

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        

        if (vertical > 0)
        {
            velocity += Time.deltaTime * 0.3f;
        }
        else
        {
            velocity -= Time.deltaTime * 2.0f;
        }
        //  Funzione che blocca il valore di velocity tra 0 e 1
        velocity = Mathf.Clamp01(velocity);

        // Setto i parameters dell'animator del Player
        animator.SetFloat("velocity", velocity);
        animator.SetFloat("turn", horizontal);


        controller.SimpleMove(transform.forward * velocity * 10.0f);
        transform.Rotate(0, horizontal * 90 * Time.deltaTime, 0);

    }*/

    void Update()
    {

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 movementDirection = new Vector3(horizontal, 0, vertical);
        movementDirection.Normalize();
        transform.Translate(movementDirection * velocity * Time.deltaTime, Space.World);

        if(movementDirection != Vector3.zero){
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
    
        // Setto i parameters dell'animator del Player
        animator.SetFloat("velocity", velocity);
    }
}

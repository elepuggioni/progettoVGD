using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    public CharacterController controller;
    private float velocity = 6.0f;
    public Animator animator;
    private PauseMenu pm;

    [SerializeField] AnimationCurve dodgeCurve;
    
    bool isDodging;
    float dodgeTimer;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        pm = GetComponent<PauseMenu>();

    }

    void Update()
     {

         StartPause();

         if (!isDodging) 
             PlayerMovement();

         if (Input.GetKeyDown(KeyCode.Space))
         {
             if (velocity > 0)
                 StartCoroutine(Dodge());
         } 

     }

    


    public IEnumerator Dodge()
    {
        animator.SetTrigger("Rolling");
        isDodging = true;
        float timer = 0;
        controller.center = new Vector3(0, 0.5f, 0);
        controller.height = 1;
        while (timer < dodgeTimer) {
            float speed = dodgeCurve.Evaluate(timer);
            Vector3 dir = (transform.forward * speed) +
                          (Vector3.up * velocity);
            controller.Move(dir * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        isDodging = false;
        controller.center = new Vector3(0, 0.99f, 0);
        controller.height = 2;
    }

    
    public void PlayerMovement()
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

        controller.SimpleMove(transform.forward * velocity * 5.0f);
        transform.Rotate(0, horizontal * 90 * Time.deltaTime, 0);
    } 

    public void StartPause() {
        if (Input.GetKeyDown(KeyCode.R))
            pm.Pause();
    }

} 
/*// Update is called once per frame
    void Update()
    {
        StartPause();

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, irection += Physics.gravity * Time.deltaTim0f, vertical).normalized;
        de;

        if (vertical != 0 || horizontal != 0)
        {
            velocity += Time.deltaTime * 4f;
        }
        else
        {
            velocity -= Time.deltaTime * 4f;
        }

        velocity = Mathf.Clamp01(velocity);
        
        // Setto i parameters dell'animator del Player
        animator.SetFloat("velocity", velocity);
        animator.SetFloat("turn", horizontal);

        if (direction.magnitude >= 0.1f)
        {
            controller.SimpleMove(direction * velocity * speed);
        }
    }*/
/*void Update()
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
    }*/


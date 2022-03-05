using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    private float velocity = 6.0f;

    [SerializeField] AnimationCurve dodgeCurve;
    bool isDodging;
    float dodgeTimer;


    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float rotationSpeed;

    private Vector3 moveDirection;
    private Vector3 speed;

    #endregion


    #region Refers
    private CharacterController controller;
    private Animator animator;
    private PauseMenu pm;
    private Transform cameraTransform;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        pm = GetComponent<PauseMenu>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
     {
         StartPause();

         if (!isDodging) 
             Move();

         if (Input.GetKeyDown(KeyCode.Space))
         {
                 StartCoroutine(Dodge());
         }
     }

    public void Move()
    {
        // passo la posizione del player, il raggio della sfera, layer da controllare
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if(isGrounded && speed.y < 0)
        {
            speed.y = -2f;
        }

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        moveDirection = new Vector3(horizontal, 0, vertical);
        moveDirection = moveDirection.x * cameraTransform.right.normalized + moveDirection.z * cameraTransform.forward.normalized;
        moveDirection.y = 0f;
        moveDirection = moveDirection.normalized;
        //moveDirection = transform.TransformDirection(moveDirection);

        if (isGrounded)
        {
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else if (moveDirection == Vector3.zero)
            {
                Idle();
            }

            moveDirection *= moveSpeed;
        }


        
        controller.Move(moveDirection * Time.deltaTime);

        speed.y += gravity; // * Time.deltaTime; // calcolo la gravità
        controller.Move(speed * Time.deltaTime); // applico la gravità
        
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Idle()
    {
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetFloat("Speed", -0.5f, 0.1f, Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        }
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.S)  || Input.GetKey(KeyCode.DownArrow))
        {
            moveSpeed = walkSpeed;
            animator.SetFloat("Speed", -0.5f, 0.1f, Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveSpeed = runSpeed;
            animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
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

    
    public void StartPause() {
        if (Input.GetKeyDown(KeyCode.R))
            pm.Pause();
    }

}
/*public void PlayerMovement()
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
   } */

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
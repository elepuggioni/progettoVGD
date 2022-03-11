using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Variables

    private float velocity = 6.0f;
    bool isDodging;
    float dodgeTimer;
    private int meleRaccolte = 0;
    private int life = 10;
    public Text meleText;

    [SerializeField] AnimationCurve dodgeCurve;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] 
    [Tooltip("Velocità di rotazione dell'object")]


    private float rotationSpeed;
    private bool isImmune = false;
    private FieldOfView _fieldOfView;

    private Vector3 moveDirection;
    private Vector3 speed;
    public GameObject ActionDisplay;
    public GameObject ActionText;

    #endregion


    #region Refers
    private CharacterController controller;
    private Animator animator;
    private PauseMenu pm;
    private Heart cuori;
    //riferimento al transform della main camera 
    private Transform cameraTransform;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        pm = GetComponent<PauseMenu>();
        cuori = GetComponent<Heart>();
        meleText.text = "Mele raccolte: " + meleRaccolte + "/10";
        
        //Accede al transform della main camera
        cameraTransform = Camera.main.transform;

        _fieldOfView = GetComponentInChildren<FieldOfView>();
    }

    void Update()
     {
         StartPause();

         if (!isDodging) 
             Move();

         if (Input.GetKeyDown(KeyCode.Space))
             StartCoroutine(Dodge());
        

         if (_fieldOfView.isVisible)
         {
             DialogueTrigger dialogueTrigger = _fieldOfView.targetTransform.GetComponent<DialogueTrigger>();
             if (dialogueTrigger != null)
             {
                 dialogueTrigger.TurnOnGameObjects();
             }
             
             if (Input.GetKeyDown(KeyCode.E) && dialogueTrigger != null)
             {
                 dialogueTrigger.TriggerDialogue();
             }
         }
         else
         {
             ActionDisplay.SetActive(false);
             ActionText.SetActive(false);
         }
     }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Collectable"))
        {
            other.gameObject.SetActive(false); //Attiva o disattiva l'oggetto
            meleRaccolte++;
            meleText.text = "Mele raccolte: " + meleRaccolte.ToString() + "/10";
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
        //Asse che permette gli spostamenti laterali del player, cioe sull'asse x
        float horizontal = Input.GetAxisRaw("Horizontal");
        moveDirection = new Vector3(horizontal, 0, vertical);
        
        /* Corregge la direzione del movimento del player in modo da seguire la rotazione
         * della camera, gestita dal mouse. Quindi quando l'utente muove la visuale, il
         * player segue anche questo movimento*/
        moveDirection = moveDirection.x * cameraTransform.right.normalized + moveDirection.z * cameraTransform.forward.normalized;
        
        //Evita movimenti indesiderati sull'asse y
        moveDirection.y = 0f; 
        
        //Normalizza il vettore per evitare che in movimento diagonale il player raddoppi la distanza percorsa
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

        speed.y += gravity; // calcolo la gravità
        controller.Move(speed * Time.deltaTime); // applico la gravità
        
        /* Istruzioni che gestiscono la rotazione del personaggio dovuta dal movimento della camera
         * e dal movimento congiunto sull'asse orizzontale (x) e verticale (z).
         * La rotazione del personaggio avviene solo quando il player si sta muovendo*/
        if (moveDirection != Vector3.zero)
        { 
            //Genera un quaternione che rappresenta la rotazione del player sull'asse y, dovuto al movimento della visuale
            Quaternion targetRotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
            
            /* Effettua la rotazione del player interpolando tra a = transform.rotation, che indica in che modo è
             * orientato il player attualmente, e b = targetRotation che indica l'orientamento che il player deve avere,
             * con velocità t = rotationSpeed * Time.deltaTime */
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            /* Se il player si deve muovere in avanti e in orizzontale, quindi diagonalmente
             * a destra o a sisnistra ma solo in avanti, il player viene ruotato in quella direzione*/
            if (horizontal != 0 && vertical > 0)
            {
                //Calcola l'angolo di cui il player deve essere ruotato
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                
                //Genera un quaternione che rappresenta la rotazione del player sull'asse y, dovuto al vector3 moveDirection
                Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
                
                /* Effettua la rotazione del player interpolando tra a = transform.rotation, che indica in che modo è
                 * orientato il player attualmente, e b = targetRotation che indica l'orientamento che il player deve avere,
                 * con velocità t = rotationSpeed * Time.deltaTime */
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            } 
        }
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

    
    public void StartPause() {
        if (Input.GetKeyDown(KeyCode.R))
            pm.Pause();
    }

    public void TakeDamage(int damage)
    {
        if (!isImmune)
        {
            isImmune = true;
            life -= damage;
            cuori.numOfHearts -= damage;
            if (life <= 0)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene(0);
            }
            StartCoroutine(Immunity());
        }
    }

    #region Coroutine
    public IEnumerator Dodge()
    {
        animator.SetTrigger("Rolling");
        isDodging = true;
        float timer = 0;
        controller.center = new Vector3(0, 0.5f, 0);
        controller.height = 1;
        while (timer < dodgeTimer)
        {
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

    public IEnumerator Immunity()
    {
        yield return new WaitForSeconds(1.5f);
        isImmune = false;
    }
    #endregion
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
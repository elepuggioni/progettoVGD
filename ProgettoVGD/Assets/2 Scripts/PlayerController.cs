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
    public bool lockMovment = false;
    float dodgeTimer;
    private int meleRaccolte = 0;
    private int life = 10;
    private bool isAttacking = false;
    public Text meleText;

    [SerializeField] AnimationCurve dodgeCurve;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float gravity;
    [SerializeField] [Tooltip("Velocità di rotazione dell'object")]
    private float rotationSpeed;
    
    [Header("Player Grounded")]
    [SerializeField] [Tooltip("indica se il player è a terra o no")]
    private bool isGrounded;
    [SerializeField] 
    private float groundCheckDistance = -0.12f;
    [SerializeField] [Tooltip("Raggio per il controllo")]
    private float groundedRadius = 0.24f;
    [SerializeField] [Tooltip("Quale layer viene usato come piano di appoggio")]
    private LayerMask groundMask;
    
    private bool isImmune = false;
    private FieldOfView _fieldOfView;

    private Vector3 moveDirection;
    private Vector3 speed;
    public GameObject ActionDisplay;
    public GameObject ActionText;
    public DialogueManager dialogueManager;
     private bool canAdvanceText;

    #endregion


    #region Refers
    private CharacterController controller;
    private Animator animator;
    private PauseMenu pm;
    private Heart cuori;
    //riferimento al transform della main camera 
    private Transform cameraTransform;
    private Animator mAnimator;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        dialogueManager = FindObjectOfType<DialogueManager>();
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

         if (!isDodging && !lockMovment)
                Move();

         if (lockMovment)
                Idle();

         if (Input.GetKeyDown(KeyCode.Space) && !lockMovment)
                StartCoroutine(Dodge());

         if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Q)))
         {
            StartCoroutine(AttackAnimation());  
         }
         
        

         if (_fieldOfView.isVisible)
         {
             DialogueTrigger dialogueTrigger = _fieldOfView.targetTransform.GetComponent<DialogueTrigger>();
             if (dialogueTrigger != null)
             {
                 dialogueTrigger.TurnOnGameObjects();
             }
             
             if (Input.GetKeyDown(KeyCode.E) && dialogueTrigger != null && !(dialogueManager.isDialogueStarted))
             {
                 dialogueTrigger.TriggerDialogue();
                 canAdvanceText = false;
             }
             if(Input.GetKeyUp(KeyCode.E)){
                canAdvanceText = true;
            }
            if(Input.GetKeyDown(KeyCode.E) && dialogueManager.isDialogueStarted && canAdvanceText){
                dialogueManager.DisplayNextSentence();
                canAdvanceText = false;
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

        if(other.CompareTag("Enemy") && isAttacking)
        {
            other.gameObject.GetComponent<EnemyController>().TakeDamage(1);
            isAttacking = false;
        }
    }



    #region Movement functions
    public void Move()
    {
        //Genera la posizione della sfera per controllare se il player è a terra
        Vector3 sphere = new Vector3(transform.position.x, transform.position.y - groundCheckDistance, transform.position.z);
        
        //Controlla se il player è a terra, ignorando la collisioni con i trigger
        isGrounded = Physics.CheckSphere(sphere, groundedRadius, groundMask, QueryTriggerInteraction.Ignore);
        
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
    public void Idle()
    {
        //animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
        animator.SetFloat("Speed", 0f);
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
    #endregion


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

    public void Attack()
    {
        //Ray ray = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.5f))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.gameObject.GetComponent<EnemyController>().TakeDamage(1);
            }
        }
    }

    #region Coroutine

    public IEnumerator AttackAnimation()
    {
        isAttacking = true;
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 1);
        animator.SetTrigger("Attack");
        this.GetComponent<BoxCollider>().isTrigger = true;

        yield return new WaitForSeconds(1.333f);
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 0);
        isAttacking = false;
        this.GetComponent<BoxCollider>().isTrigger = true;
    }

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
        yield return new WaitForSeconds(1.7f);
        isImmune = false;
    }
    #endregion
    
    /* Disegna sulla scena di unity la sfera con cui controlla se il player è a terra.
     * Sfera di colore verde in caso affermativo, rossa in caso non lo sia.
     * Metodo preso dal pacchetto Starter Assets presente nell'assets store di unity */
    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (isGrounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;
			
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundCheckDistance, transform.position.z), groundedRadius);
    }
}
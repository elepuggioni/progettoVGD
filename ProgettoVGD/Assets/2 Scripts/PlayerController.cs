using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Variables
    
    private bool isInteracting;
    public bool lockMovment;
    float dodgeTimer;
    public int meleRaccolte = 0;
    private int life = 10;
    private bool isAttacking;
    public Text meleText;
    private float vertical;
    private float horizontal;

    [SerializeField] AnimationCurve dodgeCurve;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float gravity;
    [SerializeField] [Tooltip("Velocità di rotazione dell'object")]
    private float rotationSpeed;
    
    [Header("Player Grounded")]
    [SerializeField] [Tooltip("indica se il player è a terra o no")] private bool isGrounded;
    [SerializeField] private float groundCheckDistance = -0.12f;
    [SerializeField] [Tooltip("Raggio per il controllo")] private float groundedRadius = 0.24f;
    [SerializeField] [Tooltip("Quale layer viene usato come piano di appoggio")] private LayerMask groundMask;
    
    [Header("Animator fields")]
    [SerializeField]
    [Tooltip("Rapidità di incremento o decremento dei parametri dell'animator")]
    private float animationSmoothTime = 0.1f;
    
    private bool isImmune = false;
    private FieldOfView _fieldOfView;

    private Vector3 moveDirection;
    public GameObject ActionDisplay;
    public GameObject ActionText;
    public DialogueManager dialogueManager;  
    
    //Riferimento ai parametri degll'animator
    private int _VerticalAnimatorID;
    private int _HorizontalAnimatorID;
    private int _isRunningAnimatorId;
    private int _RollTriggerAnimatorId;

    //Variabili per gestire i passaggi di parametri all'animator
    private Vector2 currentAnimationBlendVector;
    private Vector2 animationVelocity;
    private Vector2 movementVector2;
    #endregion

    

    #region Refers
    private CharacterController controller;
    private Animator animator;
    private PauseMenu pm;
    private Heart cuori;
    private Transform cameraTransform;     //riferimento al transform della main camera 

    #endregion

    private void Awake()
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
        
        //Assegna alle variabili l'id di riferimento dei parametri dell'animator
        _VerticalAnimatorID = Animator.StringToHash("Vertical");
        _HorizontalAnimatorID = Animator.StringToHash("Horizontal");
        _isRunningAnimatorId = Animator.StringToHash("isRunning");
        _RollTriggerAnimatorId = Animator.StringToHash("Roll");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Keyframe dodge_lastFrame = dodgeCurve[dodgeCurve.length - 1];
        dodgeTimer = dodge_lastFrame.time;
    }

    void Update()
     {
         StartPause();
         CheckVoidFall();
         Move();
         HandleInput();
         CheckDialog();
     }

    // Controlla se è disponibile un NPC con cui iniziare un dialogo
    private void CheckDialog()
    {
        if (_fieldOfView.isVisible && !dialogueManager.isDialogueStarted)
        {
            DialogueTrigger dialogueTrigger = _fieldOfView.targetTransform.GetComponent<DialogueTrigger>();
            if (dialogueTrigger != null)
            {
                dialogueTrigger.TurnOnGameObjects();
                if (Input.GetKeyDown(KeyCode.E))
                    dialogueTrigger.TriggerDialogue();
            }
        }
        else
        {
            ActionDisplay.SetActive(false);
            ActionText.SetActive(false);
        }
    }

    private void CheckVoidFall()
    {
        if (this.transform.position.y <= -10)
        {
            TakeDamage(20);
        }
    }

    private void SetAnimatorandSpeed()
    {
        //Permette di incrementare o decrementare i valori di input con un certo tasso stabilito da animationSmoothTime
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, movementVector2, ref animationVelocity, animationSmoothTime);

        if (isGrounded)
        {
            animator.SetFloat(_VerticalAnimatorID, currentAnimationBlendVector.y);
            animator.SetFloat(_HorizontalAnimatorID, currentAnimationBlendVector.x);
            
            //Cambia il valore del parametro che attiva l'animazione di corsa 
            if (vertical == 1f && Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = runSpeed;
                animator.SetBool(_isRunningAnimatorId, true);
            }
            else
            {
                moveSpeed = walkSpeed;
                animator.SetBool(_isRunningAnimatorId, false);
            }
            
            //Imposta la velocità con cui si sposta il player
            moveDirection *= moveSpeed;
        }
        else
        {
            Idle();
        }
    }

    private void HandleRotation()
    {
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

    private void HandleInput()
    {
        if (!lockMovment && !isInteracting)
        {
            if (Input.GetKeyDown(KeyCode.Space) && movementVector2 != Vector2.zero)
            {
                StartCoroutine(Dodge(movementVector2, cameraTransform));
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(AttackAnimation());
            }
        }


    }

    private void CheckIsGrounded()
    {
        //Genera la posizione della sfera per controllare se il player è a terra
        Vector3 sphere = new Vector3(transform.position.x, transform.position.y - groundCheckDistance, transform.position.z);
        
        //Controlla se il player è a terra, ignorando la collisioni con i trigger
        isGrounded = Physics.CheckSphere(sphere, groundedRadius, groundMask, QueryTriggerInteraction.Ignore);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            other.gameObject.SetActive(false); //Attiva o disattiva l'oggetto
            meleRaccolte++;
            meleText.text = "Mele raccolte: " + meleRaccolte.ToString() + "/10";
        }

        if( other.CompareTag("Enemy") && isAttacking && !Input.GetKeyDown(KeyCode.W))
        {
            other.gameObject.GetComponent<EnemyController>().TakeDamage(1);
        }
    }
    
    public void Move()
    {
        CheckIsGrounded();
        
        if (!lockMovment && !isInteracting) {
            vertical = Input.GetAxisRaw("Vertical");
            //Asse che permette gli spostamenti laterali del player, cioe sull'asse x
            horizontal = Input.GetAxisRaw("Horizontal");
        
            //Vettore con i valori di input dagli assi
            movementVector2 = new Vector2(horizontal, vertical);
            
            //Vettore con i valori di input per muovere il player
            moveDirection = new Vector3(movementVector2.x, 0, movementVector2.y);

             //Normalizza il vettore di movimento per non avere velocità incrementata in diagonale
            moveDirection = moveDirection.normalized;

            /* Corregge la direzione del movimento del player in modo da seguire la rotazione
             * della camera, gestita dal mouse. Quindi quando l'utente muove la visuale, il
             * player segue anche questo movimento*/
            moveDirection = moveDirection.x * cameraTransform.right.normalized +
                            moveDirection.z * cameraTransform.forward.normalized;

            //Evita movimenti indesiderati sull'asse y
            moveDirection.y = 0f;

            //Normalizza il vettore per evitare che in movimento diagonale il player raddoppi la distanza percorsa
            moveDirection = moveDirection.normalized;
            
            //Setta i paramentri dell'animator se il player è poggiato a terra, se non lo è resetta i parametri
            SetAnimatorandSpeed();
            HandleRotation();
        }
        else
        {
            if(lockMovment) 
                Idle();
            moveDirection = Vector3.zero;
        }
        
        moveDirection.y = gravity;
        controller.Move(moveDirection * Time.deltaTime);
        
    }
    
    public void Idle()
    {
        animator.SetFloat(_VerticalAnimatorID, 0f, animationSmoothTime, Time.deltaTime);
        animator.SetFloat(_HorizontalAnimatorID, 0f, animationSmoothTime, Time.deltaTime);
        animator.SetBool(_isRunningAnimatorId, false);
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
        isInteracting = true;
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 1);
        animator.SetTrigger("Attack");
        this.GetComponent<BoxCollider>().isTrigger = true;

        yield return new WaitForSeconds(1.333f);
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 0);
        isAttacking = false;
        isInteracting = false;
        this.GetComponent<BoxCollider>().isTrigger = false;
    }

    public IEnumerator Dodge(Vector2 input, Transform camera)
    {
        animator.SetTrigger(_RollTriggerAnimatorId);
        isInteracting = true;
        float timer = 0f; 
        Vector3 direction = new Vector3(input.x, 0f, input.y);
        direction = direction.x * camera.right + direction.z * camera.forward;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
        while (timer < dodgeTimer)
        {
            float speed = dodgeCurve.Evaluate(timer);
            Vector3 movement = direction;
            movement *= speed;
            controller.Move(movement * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        isInteracting = false;
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
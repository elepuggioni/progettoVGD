using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Variables
    
    [Header("Conditions")]
    public int life = 10;
    public bool isDead;
    public bool isInteracting;
    public bool isAttacking;
    public bool isImmune;
    public bool lockMovment;
    public bool isInTheMenu;
    public bool bossBattleIsStarted;

    
    [Header("Player Grounded")] 
    [SerializeField] [Tooltip("indica se il player è a terra o no")] 
    private bool isGrounded;
    [SerializeField] 
    private float groundCheckDistance = -0.12f;
    [SerializeField] [Tooltip("Raggio per il controllo")] 
    private float groundedRadius = 0.24f;
    [SerializeField] [Tooltip("Quale layer viene usato come piano di appoggio")] 
    private LayerMask groundMask;
    
    
    [Header("Movement")]
    [SerializeField] 
    private float vertical;
    [SerializeField]
    private float horizontal;
    [SerializeField]
    private bool sprint;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] [Tooltip("Velocità di rotazione dell'object")] 
    private float rotationSpeed;
    [SerializeField] private float gravity;
    
    
    [Header("Upgrades")]
    public bool armaturaAcquisita;
    public bool spadaAcquisita;
    [SerializeField] GameObject armorIcon;
    [SerializeField] GameObject swordIcon;
    

    [Header("Animator fields")] 
    [SerializeField] 
    private AnimationCurve dodgeCurve;
    [SerializeField] [Tooltip("Rapidità di incremento o decremento dei parametri dell'animator")] 
    private float animationSmoothTime = 0.1f;
    
    
    [Header("Quest")]
    public int meleRaccolte;
    
    
    [Header("Canvas")]
    public GameObject ActionDisplay;
    public GameObject ActionText;
    public Text meleText;

    [Header("Sounds")]
    [SerializeField] [Tooltip("Clip audio quando il player viene colpito")]
    private AudioSource PlayerHitted;
    [SerializeField] [Tooltip("Clip Audio per la morte del Player")]
    private AudioSource VoiceDeathSound;



    //Riferimento ai parametri degll'animator
    private int _VerticalAnimatorID;
    private int _HorizontalAnimatorID;
    private int _isRunningAnimatorId;
    private int _RollTriggerAnimatorId;
    private int _AttackTriggerAnimatorId;

    //Variabili per gestire i passaggi di parametri all'animator
    private Vector2 currentAnimationBlendVector;
    private Vector2 animationVelocity;
    private Vector2 movementVector2;
    
    private Vector3 moveDirection;
    private float dodgeTimer;

    #endregion
    
    #region Refers
    //Riferimenti a componenti del player
    private CharacterController controller;
    private Animator animator;
    private Transform cameraTransform;     //riferimento al transform della main camera 
    
    //Riferimenti ad altri script
    private PauseMenu pm;
    private Heart cuori;
    private FieldOfView _fieldOfView;
    private DialogueManager dialogueManager;
    private PlayerAnimationsEvents _playerAnimationsEvents;
    private GameObject gm;
    private AudioHandler audioHandler;
    
    
    #endregion

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        _playerAnimationsEvents = GetComponent<PlayerAnimationsEvents>();
        audioHandler = FindObjectOfType<AudioHandler>();
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
        _AttackTriggerAnimatorId = Animator.StringToHash("Attack");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Keyframe dodge_lastFrame = dodgeCurve[dodgeCurve.length - 1]; // Prendo l'ultimo keyframe
        dodgeTimer = dodge_lastFrame.time; // prendo il time del keyframe
        gm = GameObject.FindGameObjectWithTag("GameManager");
    }

    void Update()
     {
         ActiveIcon(); 

         StartPause();

         CheckVoidFall();
         
         Move();

         HandleInput();

         CheckDialog();
     }

    void OnTriggerEnter(Collider other)
    {
        // Mele da raccogliere
        if (other.CompareTag("Collectable"))
        {
            other.gameObject.SetActive(false); //Attiva o disattiva l'oggetto
            meleRaccolte++;
            meleText.text = "Mele raccolte: " + meleRaccolte.ToString() + "/10";
        }

        // Attacco contro il vice capo
        if (other.CompareTag("ViceCapo") && isAttacking && dialogueManager.getViceCapo())
        {
            other.gameObject.GetComponent<ViceCapoController>().TakeDamage(1);
        }

        // Attacco contro il boss
        if (other.CompareTag("Boss") && isAttacking)
        {
            other.gameObject.GetComponent<BossController>().TakeDamage(1);
        }

        // Quando si entra nell'arena dopo aver completato le missioni si attiva la boss battle
        if(other.CompareTag("Muro") && spadaAcquisita && armaturaAcquisita)
        {
            gm.GetComponent<GameManager>().boss.SetActive(true); // attiva il boss
            gm.GetComponent<GameManager>().bossHealtBar.SetActive(true); // attiva la health bar
            gm.GetComponent<GameManager>().luce.color = Color.black; // cambio della luce
        }

    }

    private void OnTriggerExit(Collider other)
    {
        // Una volta dentro la boss battle non si puo piu uscire
        if (other.CompareTag("Muro") && spadaAcquisita && armaturaAcquisita)
        {
            gm.GetComponent<GameManager>().muro.GetComponent<BoxCollider>().isTrigger = false;
            audioHandler.StandardBackground.Pause();
            audioHandler.BossBackground.PlayDelayed(1.0f);
            bossBattleIsStarted = true;
        }
    }


    // Controlla se è disponibile un NPC con cui iniziare un dialogo
    private void CheckDialog()
    {
        // Se il campo visivo risulta visibile e il dialogo non e inziato
        if (_fieldOfView.isVisible && !dialogueManager.isDialogueStarted)
        {
            DialogueTrigger dialogueTrigger = _fieldOfView.targetTransform.GetComponent<DialogueTrigger>();
            // Se ho trovato il component correttamente e non sto parlando
            if (dialogueTrigger != null && !dialogueTrigger.dialogueManager.alreadyTalk)
            {
                dialogueTrigger.TurnOnGameObjects(); // Attiva i gameObejcet
                if (Input.GetKeyDown(KeyCode.E)) // Se premo E
                    dialogueTrigger.TriggerDialogue(); // attivo il dialogo
            }
        }
        else
        {
            // Disattivo i gameObject
            ActionDisplay.SetActive(false); 
            ActionText.SetActive(false);
        }
    }

    //Controlla che il player non sia caduto nel vuoto
    private void CheckVoidFall()
    {
        if (this.transform.position.y <= -10)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(0);
        }
    }

    #region Metodi per la gestione del moviemento

    private void SetAnimatoraParameters()
    {
        //Permette di incrementare o decrementare i valori di input con un certo tasso stabilito da animationSmoothTime
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, movementVector2, ref animationVelocity, animationSmoothTime);

        if (isGrounded)
        {
            animator.SetFloat(_VerticalAnimatorID, currentAnimationBlendVector.y);
            animator.SetFloat(_HorizontalAnimatorID, currentAnimationBlendVector.x);
            
            //Cambia il valore del parametro che attiva l'animazione di corsa 
            if (sprint)
            {
                animator.SetBool(_isRunningAnimatorId, true);
            }
            else
            {
                animator.SetBool(_isRunningAnimatorId, false);
            }
        }
        else
        {
            Idle();
        }
    }

    private void SetSpeedMovement()
    {
        if (isAttacking) {
            moveSpeed = 2f;
            moveDirection = transform.forward;
        } else if (sprint)
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
        
        moveDirection *= moveSpeed;
    }

    private void HandleRotation()
    {
         /* Istruzioni che gestiscono la rotazione del personaggio dovuta dal movimento della camera
         * e dal movimento congiunto sull'asse orizzontale (x) e verticale (z).
         * La rotazione del personaggio avviene solo quando il player si sta muovendo*/
        if (moveDirection != Vector3.zero && !isAttacking)
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
                if (isAttacking)
                {
                    _playerAnimationsEvents.StopIsInteracting();
                    animator.Rebind();
                    isAttacking = false;
                }
                StartCoroutine(Dodge(movementVector2, cameraTransform));
            }

            if (!isAttacking && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Q)))
            {
                if (!isInTheMenu)
                {
                    animator.SetTrigger(_AttackTriggerAnimatorId);
                    isAttacking = true;
                }
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

    private void TakeInput()
    {
        vertical = Input.GetAxisRaw("Vertical");
        //Asse che permette gli spostamenti laterali del player, cioe sull'asse x
        horizontal = Input.GetAxisRaw("Horizontal");
        
        //Vettore con i valori di input dagli assi
        movementVector2 = new Vector2(horizontal, vertical);
        
        if (vertical == 1 && Input.GetKey(KeyCode.LeftShift))
        {
            sprint = true;
        }
        else
        {
            sprint = false;
        }
    }

    public void Move()
    {
        CheckIsGrounded();
        
        if (!lockMovment && !isInteracting) {
            TakeInput();
            
            //Vettore con i valori di input per muovere il player
            moveDirection = new Vector3(movementVector2.x, 0, movementVector2.y);

             //Normalizza il vettore di movimento per non avere velocità incrementata in diagonale
            //moveDirection = moveDirection.normalized;

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
            SetAnimatoraParameters();
            
            HandleRotation();
            
            SetSpeedMovement();
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

    #endregion

    // Controlla che il player abbia premuto il tasto per mettere in pausa
    public void StartPause() {
        if (Input.GetKeyDown(KeyCode.R))
            pm.Pause();
    }
    
    public void TakeDamage(int damage)
    {
        if (!isImmune && !isDead)
        {
            PlayerHitted.Play();
            isImmune = true;
            life -= damage;
            cuori.numOfHearts -= damage;
            if (life <= 0)
            {
                isDead = true;
                enabled = false;
                _playerAnimationsEvents.PlayTargetAnimation("Death");
                VoiceDeathSound.Play();
            }
            StartCoroutine(Immunity());
        }
    }

    #region Coroutine

    // Permette l'animazione di schivata
    public IEnumerator Dodge(Vector2 input, Transform camera)
    {
        animator.SetTrigger(_RollTriggerAnimatorId); // Attiva il trigger
        isInteracting = true; // sta schivando

        float timer = 0f; 
        Vector3 direction = new Vector3(input.x, 0f, input.y); // Prendo la direzione
        direction = direction.x * camera.right + direction.z * camera.forward;

        Quaternion rotation = Quaternion.LookRotation(direction); // Prendo la rotazione
        transform.rotation = rotation; // Applico la rotazione

        while (timer < dodgeTimer)
        {
            float speed = dodgeCurve.Evaluate(timer); // Valuta la curva nel momento del timer
            Vector3 movement = direction;
            movement *= speed;
            controller.Move(movement * Time.deltaTime); // Applico il movimento
            timer += Time.deltaTime;
            yield return null;
        }

        isInteracting = false; // smette di fare la schivata
    }

    // Permette al player di essere immune
    public IEnumerator Immunity()
    {
        yield return new WaitForSeconds(1.5f);
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

    private void ActiveIcon()
    {
        if (spadaAcquisita)
            swordIcon.SetActive(true);

        if (armaturaAcquisita)
            armorIcon.SetActive(true);
    }
}
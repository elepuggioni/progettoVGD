using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public CharacterController controller;
    private float velocity = 0.0f;
    public float speed = 7.0f;
    private float gravity = 20.0f;
    public float rotationSpeed;
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
    
    /*void Update()
    {

        StartPause();
        if (!isDodging) PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (velocity > 0)
                StartCoroutine(Dodge());
        } 

    }*/

    // Update is called once per frame
    void Update()
    {
        StartPause();

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        direction += Physics.gravity * Time.deltaTime;

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
        controller.center = new Vector3(0, 1.04f, 0);
        controller.height = 2;
    }

    public void PlayerMovement()
    {
       float vertical = Input.GetAxis("Vertical");
       float horizontal = Input.GetAxis("Horizontal");

       if (vertical > 0)
       {
           /* La variabile velocity in realtà rappresenta l'accellerazione del player,
            * più è alta e prima il player raggiunge la velocità massima. Evita quindi
            * quella lenta risposta del player alla pressione dei tasti, in cui parte
            * piano e lentamente raggiunge la massima velocità */
           velocity += Time.deltaTime * 4.0f;
            
            /* Facendo muovere il player dentro questo ciclo if, che controlla quando un
             * tasto che permette di muovere il player è premuto, si evita che il player
             * continui a muoversi per inerzia dopo aver rilasciato il tasto */
            //TODO: E comunque da risolvere un leggero delay quando viene rilasciato il tasto
            controller.SimpleMove(transform.forward * velocity * 5.0f);
            
            //TODO: Alla pressione del tasto 's' il player deve indietreggiare
            /* In un gioco in cui si combatte corpo a corpo è impensabile che non si possa
             * indietreggiare per schivare o allontanarsi dal nemico */
       }
       else
       {
           /* Chiamando la SimpleMove nel ciclo if, decrementare la velocità gradualmente
            * non serve per il movimento del personaggio. In questo caso però è utile in
            * quanto rende l'animazione da corsa e idle più fluida */
           velocity -= Time.deltaTime * 4.0f;
       }
       //  Funzione che blocca il valore di velocity tra 0 e 1
       velocity = Mathf.Clamp01(velocity);

       // Setto i parameters dell'animator del Player
       animator.SetFloat("velocity", velocity);
       animator.SetFloat("turn", horizontal);
        
       //TODO: Movimento laterale per i tasti 'a' e 'd'
       /* Premendo i tasti 'a' e 'd' il player non deve ruotare su se stesso, in quanto
        * inutile e molto fastidioso, ma deve spostarsi lateralmente. Se viene premuto
        * un tasto tra 'w' o 's' in contemporanea a un tasto tra 'a' o 'd', il player
        * dovrebbe muoversi in diagonale */
       transform.Rotate(0, horizontal * 90 * Time.deltaTime, 0);
    }

    public void StartPause() {
        if (Input.GetKeyDown(KeyCode.R))
            pm.Pause();
    }
}
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


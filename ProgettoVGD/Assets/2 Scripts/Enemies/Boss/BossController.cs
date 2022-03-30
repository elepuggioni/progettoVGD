using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    // Posizioni di teletrasporto
    private Vector3 CurrentPosition = Vector3.zero;
    private Vector3 tp1 = new Vector3(276, 20, -156);
    private Vector3 tp2 = new Vector3(327, 20, -99);
    private Vector3 tp3 = new Vector3(328, 20, -143);
    private Vector3 tp4 = new Vector3(275.5f, 20, -91.4f);


    #region Riferiementi 
    private GameObject player;
    public GameManager gm;
    private PlayerController playerController;
    private Animator animator;
    private AudioHandler audioHandler;
    [SerializeField] GameObject boss;
    [SerializeField] GameObject projectileDistance;
    [SerializeField] GameObject spawnPointProjectile;
    [SerializeField] GameObject bossHealthBar;
    #endregion

    public bool isShooting = false; // indica che il boss sta attaccando o si sta teletrasportando
    private bool isDead = false; //indica che il boss è morto
    private bool teleported;
    public bool AlreadyHitted;

    public Slider slider; // Lo slider che rappresenta la health bar del boss

    [Header("Sounds")] 
    [SerializeField] private AudioSource AttackAudio;
    [SerializeField] private AudioSource TeleportAudio;
    [SerializeField] private AudioSource DeathVoiceAudio;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        audioHandler = FindObjectOfType<AudioHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isDead)
        {
            animator.SetBool("DistanceAttack", false);
            animator.CrossFade("Idle", 0f);
        }


        if(!playerController.isDead) 
            Attack();
    }

    // Funzione che permette al boss di attaccare
    void Attack()
    {
        if(!isDead) // Se non è morto
            transform.LookAt(player.transform); // guarda sempre il player

        // Distanza tra player e boss
        float distance = Vector3.Distance(transform.position, player.transform.position);
        
        // Se la distanza e maggiore di 4, non sta sparando e non è morto
        /*
        if (distance > 8 && !isShooting && !isDead && !playerController.isDead)
        {
            animator.SetBool("DistanceAttack", true); // Animazione di attacco
            //StartCoroutine(ShootDistance()); // Inzia a sparare
        }*/
        if(!teleported && distance <= 8 && !isDead && !playerController.isDead)
        {
            StartCoroutine(Teleport()); // Si teletrasporta
        }
        else if (distance < 8 || isDead || playerController.isDead)
        {
            animator.SetBool("DistanceAttack", false); // Animazione di attacco
        }
        // Se la distanza e minore o uguale a 4, non sta sparando e non è morto
        else
        {
            animator.SetBool("DistanceAttack", true);
        }
    }

    // Permette l'animazione di attacco e il suo controllo
    public IEnumerator ShootDistance()
    {
        animator.SetBool("DistanceAttack", true); // Animazione di attacco
        isShooting = true; // indica che sta attaccando

        // Istanzia il projectileDistance  
        /*
        GameObject proj = Instantiate(projectileDistance, spawnPointProjectile.transform.position, Quaternion.identity);
        proj.transform.localRotation = transform.rotation; // Prendo la rotazione del boss
        DestroyProj(proj, 2f); // Viene ditrutto dopo 2 secondi*/
        
        yield return new WaitForSeconds(30f); // Aspetta prima di poter attaccare di nuovo
        isShooting = false; // Ora il boss puo tornare ad attaccare

    }

    // Permette l'animazione di teletrasporto e il suo controllo
    public IEnumerator Teleport()
    {
        teleported = true; // Indica che si sta teletrasportando
        isShooting = true; //Blocca gli attacchi
        animator.SetBool("DistanceAttack", false); // Non deve piu attaccare da lontano
        yield return new WaitForSeconds(3.0f); // aspetta
        animator.SetTrigger("Teleport");

        
    }

    // Permette l'animazione di morte e il suo controllo
    public IEnumerator Die()
    {
        isDead = true; // Indica che il boss è morto
        animator.CrossFade("Morte", 0f);
        audioHandler.EnemyKilled.Play();
        DeathVoiceAudio.Play();
        GetComponent<CapsuleCollider>().enabled = false;
        
        yield return new WaitForSeconds(6f); // aspetta 6 secondi

        // Disattiva i gameObject e indica che il boss viene sconfitto
        enabled = false;
        bossHealthBar.SetActive(false);
        gm.gianniSconfitto = true;

    }

    // Funzione che permette al boss di prendere danno
    public void TakeDamage(int damage)
    {
        slider.value -= damage; //prende danno
        if (slider.value > 0)
        {
            audioHandler.EnemyHitted.Play(); 
        }
        else 
        {
            StopAllCoroutines(); // disattiva tutte le coroutine
            StartCoroutine(Die()); // Avvia la coroutine die

        }
    }

    public void DestroyProj(GameObject proj, float time)
    {
        Destroy(proj, time);
    }

    public void ShotMagic()
    {
        // Istanzia il projectileDistance 
        AttackAudio.Play();
        GameObject proj = Instantiate(projectileDistance, spawnPointProjectile.transform.position, Quaternion.identity);
        proj.transform.localRotation = transform.rotation; // Prendo la rotazione del boss
        DestroyProj(proj, 2f); // Viene ditrutto dopo 2 secondi
    }

    public void DoTeleport()
    {
        TeleportAudio.Play();
        var r = Random.Range(1, 4); // esce un numero da 1 a 4

        switch (r)
        {
            case 1:
                if (CurrentPosition != tp1) // Se esce la posizione tp1 e il boss non si trova gia li
                {
                    this.transform.position = tp1; //Teleporta il boss nella posizione tp1
                    CurrentPosition = tp1;
                }
                else
                {
                    this.transform.position = new Vector3(314, 22, -114); // Posizione di default
                    CurrentPosition = transform.position;
                }
                break;
            
            case 2:
                if (CurrentPosition != tp2)
                {
                    this.transform.position = tp2;
                    CurrentPosition = tp2;
                }
                else
                {
                    this.transform.position = new Vector3(314, 22, -114); // Posizione di default
                    CurrentPosition = transform.position;
                }
                break;
            
            case 3:
                if (CurrentPosition != tp3)
                {
                    this.transform.position = tp3;
                    CurrentPosition = tp3;
                }
                else 
                { 
                    this.transform.position = new Vector3(314, 22, -114); // Posizione di default
                    CurrentPosition = transform.position; 
                }
                break;
            
            case 4:
                if (CurrentPosition != tp4)
                {
                    this.transform.position = tp4;
                    CurrentPosition = tp4;
                }
                else 
                { 
                    this.transform.position = new Vector3(314, 22, -114); // Posizione di default
                    CurrentPosition = transform.position; 
                }
                break;
        }
        // Torna a poter attaccare o teletrasportarsi
        isShooting = false;
    }

    public void ResetTeleported()
    {
        teleported = false;
    }
}

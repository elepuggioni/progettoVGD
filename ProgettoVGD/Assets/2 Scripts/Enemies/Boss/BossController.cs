using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    // Posizioni di teletrasporto
    private Vector3 tp1 = new Vector3(276, 20, -156);
    private Vector3 tp2 = new Vector3(327, 20, -99);
    private Vector3 tp3 = new Vector3(328, 20, -143);
    private Vector3 tp4 = new Vector3(275.5f, 20, -91.4f);


    #region Riferiementi 
    private GameObject player;
    public GameManager gm;
    private Animator animator;
    [SerializeField] GameObject boss;
    [SerializeField] GameObject projectileDistance;
    [SerializeField] GameObject spawnPointProjectile;
    [SerializeField] GameObject bossHealthBar;
    #endregion

    private bool isShooting = false; // indica che il boss sta attaccando o si sta teletrasportando
    private bool isImmune = false; // indica che il boss è immune
    private bool isDead = false; //indica che il boss è morto

    public Slider slider; // Lo slider che rappresenta la health bar del boss



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
        if (distance > 4 && !isShooting && !isDead)
        {
            StartCoroutine(ShootDistance()); // Inzia a sparare
        }
        // Se la distanza e minore o uguale a 4, non sta sparando e non è morto
        else if(distance <= 4 && !isShooting && !isDead)
        {
            StartCoroutine(Teleport()); // Si teletrasporta
        }
    }

    // Permette l'animazione di attacco e il suo controllo
    public IEnumerator ShootDistance()
    {
        animator.SetBool("DistanceAttack", true); // Animazione di attacco
        isShooting = true; // indica che sta attaccando

        // Istanzia il projectileDistance  
        GameObject proj = Instantiate(projectileDistance, spawnPointProjectile.transform.position, Quaternion.identity);
        proj.transform.localRotation = transform.rotation; // Prendo la rotazione del boss
        Destroy(proj, 2f); // Viene ditrutto dopo 2 secondi
        
        yield return new WaitForSeconds(2f); // Aspetta prima di poter attaccare di nuovo
        isShooting = false; // Ora il boss puo tornare ad attaccare

    }

    // Permette l'animazione di teletrasporto e il suo controllo
    public IEnumerator Teleport()
    {
        animator.SetBool("DistanceAttack", false); // Non deve piu attaccare da lontano
        isShooting = true; // Indica che si sta teletrasportando
        yield return new WaitForSeconds(1.2f); // aspetta

        var r = Random.Range(1, 4); // esce un numero da 1 a 4

        if (r == 1 && transform.position != tp1) // Se esce la posizione tp1 e il boss non si trova gia li
            this.transform.position = tp1; //Teleporta il boss nella posizione tp1

        else if (r == 2 && transform.position != tp2)
            this.transform.position = tp2;

        else if (r == 3 && transform.position != tp3)
            this.transform.position = tp3;

        else if (r == 4 && transform.position != tp4) 
            this.transform.position = tp4;

        else
            this.transform.position = new Vector3(314, 22, -114); // Posizione di default

        isShooting = false; // Torna a poter attaccare o teletrasportarsi

    }

    // Dopo aver preso danno il boss rimane immune per 1.2 secondi
    public IEnumerator Immunity()
    {
        yield return new WaitForSeconds(0.665f);
        isImmune = false;
    }

    // Permette l'animazione di morte e il suo controllo
    public IEnumerator Die()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Die Layer"), 1); // Vai nell'altro layer dell'animator
        animator.SetTrigger("Die"); // Setta il trigger
        isDead = true; // Indica che il boss è morto

        yield return new WaitForSeconds(6f); // aspetta 6 secondi

        // Disattiva i gameObject e indica che il boss viene sconfitto
        boss.gameObject.SetActive(false); 
        bossHealthBar.SetActive(false);
        gm.gianniSconfitto = true;

    }

    // Funzione che permette al boss di prendere danno
    public void TakeDamage(int damage)
    {
        if (!isImmune) // Se non è immune
        {
            isImmune = true; // diventa immune
            slider.value -= damage; //prende danno
            if (slider.value <= 0) // se perde tutte le vite
            {
                StopAllCoroutines(); // disattiva tutte le coroutine
                StartCoroutine(Die()); // Avvia la coroutine die

            }

            if (this.gameObject.activeSelf) // Se il boss e attivo
                StartCoroutine(Immunity()); // Attiva la coroutine
        }
    }

}

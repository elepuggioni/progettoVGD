using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private Vector3 tp1 = new Vector3(276, 20, -156);
    private Vector3 tp2 = new Vector3(327, 20, -99);
    private Vector3 tp3 = new Vector3(328, 20, -143);
    private Vector3 tp4 = new Vector3(275.5f, 20, -91.4f);

    private GameObject player;
    private Animator animator;

    [SerializeField] GameObject projectileDistance;
    [SerializeField] GameObject spawnPointProjectile;

    private bool isShooting = false;


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

    void Attack()
    {
        transform.LookAt(player.transform);
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > 4 && !isShooting)
        {
            StartCoroutine(ShootDistance());
        }
        else if(distance <= 4 && !isShooting)
        {
            StartCoroutine(Teleport());
        }
    }

    public IEnumerator ShootDistance()
    {
        animator.SetBool("DistanceAttack", true);
        isShooting = true;
        GameObject proj = Instantiate(projectileDistance, spawnPointProjectile.transform.position, Quaternion.identity);
        proj.transform.localRotation = transform.rotation;
        Destroy(proj, 2f);
        yield return new WaitForSeconds(2.8f);
        isShooting = false;

    }

    public IEnumerator Teleport()
    {

        animator.SetBool("DistanceAttack", false);
        isShooting = true;
        yield return new WaitForSeconds(2.267f);

        var r = Random.Range(1, 4);

        if (r == 1 && transform.position != tp1) // Se è uscita la posizione tp1 e il boss non si trova gia li
            this.transform.position = tp1; //Teleporta il boss nella posizione tp1

        else if (r == 2 && transform.position != tp2)
            this.transform.position = tp2;

        else if (r == 3 && transform.position != tp3)
            this.transform.position = tp3;

        else if (r == 4 && transform.position != tp4) 
            this.transform.position = tp4;

        else
            this.transform.position = new Vector3(314, 22, -114); // Posizione di default

        isShooting = false;

    }


}

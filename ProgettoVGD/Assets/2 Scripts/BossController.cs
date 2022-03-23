using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private GameObject player;
    private Animator animator;
    [SerializeField] GameObject projectileDistance;
    [SerializeField] GameObject projectileNear;
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
        float distance = Vector3.Distance(transform.position, player.transform.position);
        transform.LookAt(player.transform);
        if (distance > 4 && !isShooting)
        {
            StartCoroutine(ShootDistance());
        }
        else if (distance < 2.5 && !isShooting)
        {
            StartCoroutine(ShootNear());
        }
    }

    public IEnumerator ShootDistance()
    {

        GameObject proj;
        animator.SetBool("DistanceAttack", true);
        isShooting = true;
        proj = Instantiate(projectileDistance, spawnPointProjectile.transform.position, Quaternion.identity);
        proj.transform.localRotation = transform.rotation;
        Destroy(proj, 2f);
        yield return new WaitForSeconds(2.5f);
        isShooting = false;

    }

    public IEnumerator ShootNear()
    {
        GameObject proj;
        animator.SetBool("DistanceAttack", false);
        isShooting = true;
        StartCoroutine(Wait());
        proj = Instantiate(projectileNear, spawnPointProjectile.transform.position, Quaternion.identity);
        proj.transform.localRotation = transform.rotation;
        Destroy(proj, 1.8f);
        yield return new WaitForSeconds(2f);
        isShooting = false;

    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
    }
}

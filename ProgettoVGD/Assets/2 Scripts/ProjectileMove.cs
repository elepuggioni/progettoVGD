using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour {

	public float speed;
	public float accuracy;

	private GameObject player;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update () {

		if (speed != 0)
		{
			Vector3 directionToPlayer = player.transform.position - transform.position; //direzione verso il player
			Vector3 direction = transform.forward; // direzione attuale del proiettile
 			float turn = 30f; // gradi per secondo
			Vector3 resultingDirection = Vector3.RotateTowards(direction, directionToPlayer, turn * Mathf.Deg2Rad * Time.deltaTime, 1f);
			transform.rotation = Quaternion.LookRotation(resultingDirection); // Crea una rotazione
			transform.position += (transform.forward )  * (speed * Time.deltaTime); // Muovi la posizione verso il forward del proiettile
		}
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
			other.GetComponent<PlayerController>().TakeDamage(1);
        }
    }


}

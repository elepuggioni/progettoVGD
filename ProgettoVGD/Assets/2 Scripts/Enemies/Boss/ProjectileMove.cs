using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour {

	public float speed; // velocita di moviemento
	private GameObject player; //riferiemento del player
	private PlayerController playerController;
	private BossController bossController;

	void Awake () {
		//prendo il player
		player = GameObject.FindGameObjectWithTag("Player Center");
		playerController = player.GetComponentInParent<PlayerController>();
		bossController = FindObjectOfType<BossController>();
	}

	void Update () {

		if (speed != 0)
		{
			Vector3 directionToPlayer = player.transform.position - transform.position; //direzione verso il player
			Vector3 direction = transform.forward; // direzione attuale del proiettile
 			float turn = 10f; // gradi per secondo

			// rotazione verso un certo obiettivo
			Vector3 resultingDirection = Vector3.RotateTowards(direction, directionToPlayer, turn * Mathf.Deg2Rad * Time.deltaTime, 1f);
			// Applica una rotazione per il proiettile verso il player
			transform.rotation = Quaternion.LookRotation(resultingDirection); 

			// Muovi la posizione del proiettile nel verso di sparo
			transform.position += (transform.forward )  * (speed * Time.deltaTime); 
		}
		
	}

    private void OnTriggerEnter(Collider other)
    {
		// Se hitto il player questo prende danno
        if(other.CompareTag("Player"))
        {
	        bossController.DestroyProj(this.gameObject, 0f);
	        playerController.TakeDamage(1);
        }
    }


}

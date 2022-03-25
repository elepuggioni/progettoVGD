using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour {

	public float speed;
	public float accuracy;

	void Start () {	

	}

	void Update () {	

		if (speed != 0)
			transform.position += (transform.forward )  * (speed * Time.deltaTime); // Muovi la posizione verso il forward del proiettile
	}

	


}

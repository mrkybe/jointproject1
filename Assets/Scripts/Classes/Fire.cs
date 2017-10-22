using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	public float speed = 2;
	public GameObject ammo;
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Shoot ();
		}
		
	}

	void FindProjectile() {
		


	}

	void Shoot()
	{
		GameObject bulletClone = Instantiate(ammo, transform.position, transform.rotation);
		Rigidbody rb = bulletClone.GetComponent<Rigidbody>();
		rb.velocity = transform.forward * speed;
	}
}

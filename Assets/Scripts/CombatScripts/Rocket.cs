using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	public GameObject ammo;
	public float amount = 3;
	public float fireRate = 5;
	public float nextFire;

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1") && Time.time > nextFire && amount != 0) 
		{
			nextFire = Time.time + fireRate;
			Instantiate (ammo, transform.position, transform.rotation);
			amount--;
		}
	}
		

	//need to show explosion particle effect.
	void OnCollisionEnter()
	{

	}
}

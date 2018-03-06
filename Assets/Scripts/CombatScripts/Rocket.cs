using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	public GameObject ammo;
	public float amount = 3;
	public float fireRate;
	public float nextFire = 5;

	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Fire1") && Time.time > nextFire && amount != 0) 
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

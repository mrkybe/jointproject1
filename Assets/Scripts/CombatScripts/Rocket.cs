﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	public GameObject ammo;
	public float amount = 3;
	public float fireRate = 5;
	public float nextFire;
	public Transform shotSpawn;
	public AudioClip shootSound;

	private AudioSource source;


	void Awake(){

		source = GetComponent <AudioSource> ();
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1") && Time.time > nextFire && amount != 0) 
		{
			nextFire = Time.time + fireRate;
			Instantiate (ammo, shotSpawn.position, shotSpawn.rotation);
			source.PlayOneShot (shootSound);
			amount--;
		}
	}
}

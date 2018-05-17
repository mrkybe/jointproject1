using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Classes.WorldSingleton;

public class Rocket : MonoBehaviour {

	public GameObject ammo;
	private float amount = 5;
	public float fireRate = 5;
	public float nextFire;
	public Transform shotSpawn;
	public AudioClip shootSound;
	private CombatController manager;
	private AudioSource source;


	void Awake(){

		source = GetComponent <AudioSource> ();
		manager = GameObject.Find ("Overseer").GetComponent<CombatController> ();
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1") && Time.time > nextFire && amount != 0 && manager.playerCanMove.Equals(true)) 
		{
			nextFire = Time.time + fireRate;
			Instantiate (ammo, shotSpawn.position, shotSpawn.rotation);
			source.PlayOneShot (shootSound);
			amount--;
		}
	}
}

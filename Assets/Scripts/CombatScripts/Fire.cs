using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Fire class is responsible for creating and firing a projectile forward when the player hits the fire button
///</summary>
public class Fire : MonoBehaviour {

	public float speed;
	public Transform shotSpawn;
	public float fireRate;
	public GameObject shot;
	private float nextFire = 2;
	public AudioClip shootSound;

	private AudioSource source;


	void Awake(){

		source = GetComponent <AudioSource> ();
	}
		


	void Update ()
	{
		if (Input.GetButton("Fire1") && Time.time > nextFire) 
		{
			source.PlayOneShot (shootSound);
			nextFire = Time.time + fireRate;
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
		}
	}

	public void enemyFire()
	{
		if (Time.time > nextFire) 
		{
			source.PlayOneShot (shootSound);
			nextFire = Time.time + fireRate;
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);

		}

	}

}

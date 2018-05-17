using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Classes.WorldSingleton;
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
	private CombatController manager;
    private Rigidbody myRigidbody;

	void Awake()
	{
		source = GetComponent <AudioSource> ();
		manager = GameObject.Find ("Overseer").GetComponent<CombatController> ();
	    myRigidbody = this.GetComponent<Rigidbody>();
    }
		


	void Update ()
	{
		if (Input.GetButton("Fire1") && Time.time > nextFire && shot != null && manager.playerCanMove.Equals(true)) 
		{
			source.PlayOneShot (shootSound);
			nextFire = Time.time + fireRate;
			GameObject bullet = Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
		    Rigidbody brb = bullet.GetComponent<Rigidbody>();
            bullet.transform.RotateAround(bullet.transform.position, Vector3.up, Random.value * Random.value * 6f - 3f);
            brb.velocity += myRigidbody.velocity;
		}
	}

	public void enemyFire()
	{
		if (Time.time > nextFire) 
		{
			source.PlayOneShot (shootSound);
			nextFire = Time.time + fireRate;
		    GameObject bullet = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
		    bullet.GetComponent<Rigidbody>().velocity += this.GetComponent<Rigidbody>().velocity;

        }

	}

}

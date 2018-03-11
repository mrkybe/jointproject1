using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour {

	public GameObject Player;
	public GameObject DetectionRange;
	public float radius = 10;


	public float speed;
	public Transform shotSpawn;
	public float fireRate;
	public GameObject shot;
	private float nextFire = 2;



	private bool InRange = false;

	void Start ()
	{
		SphereCollider sphereCollider = DetectionRange.GetComponent<SphereCollider> ();
		sphereCollider.radius = radius; 
		Player = GameObject.Find ("Combat_ship_player");
	}





	void Update ()
	{
		if (InRange) {
			gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
//			//gameObject.GetComponent<AI_Enemy> ().enabled = false;
			gameObject.transform.LookAt (Player.transform);
//
//
//			if (Time.time > nextFire) 
//			{
//				nextFire = Time.time + fireRate;
//				Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
//			}



		}
		else {
			//gameObject.GetComponent<AI_Enemy> ().enabled = true;
		}
	}


	public void OnTriggerStay (Collider col)
	{
		Debug.Log (col.name);
		if (col.name == "Combat_ship_player")
		{
			InRange = true; 

	
			//Debug.Log("Player in range");
		}
	}

	public void OnTriggerExit (Collider col)
	{

		InRange = false;
		//Debug.Log("Player is out of range");
	}
		
}
